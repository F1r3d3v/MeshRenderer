using System.Numerics;
using Timer = System.Timers.Timer;

namespace GK1_MeshEditor
{
    public partial class Editor : Form
    {
        DeltaTime? deltaTime;

        private Timer animationTimer;
        private float angle = 0;
        private bool animDirection = true;

        private float cubeXangle = 0;
        private float cubeZangle = 0;

        static BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
        private SurfaceTransform surfaceTransform = new SurfaceTransform(bezierSurface);

        static Cube cube = new Cube(new Vector3(-200, -200, -200), new Vector3(200, 200, 200));
        private MeshTransform cubeTransform = new MeshTransform(cube.Mesh);

        private PhongShader shader;
        private Scene scene;
        Renderer? renderer;
        DirectBitmap? bmp;

        EditorViewModel Model { get; } = EditorViewModel.GetInstance();

        public Editor()
        {
            InitializeComponent();

            scene = new Scene();
            shader = new PhongShader(scene);

            scene.graphicsObjects.Add(cube);
            scene.graphicsObjects.Add(bezierSurface);
            Vector3 pos1 = new Vector3(-500, 500, Model.ZPlane);
            Vector3 dir1 = Vector3.Normalize(pos1);
            Vector3 pos2 = new Vector3(-500, -500, Model.ZPlane);
            Vector3 dir2 = Vector3.Normalize(pos2);
            Vector3 pos3 = new Vector3(500, -500, Model.ZPlane);
            Vector3 dir3 = Vector3.Normalize(pos3);
            scene.LightSources.Add(new Reflector(pos1, dir1, Color.FromArgb(255, 0, 0)));
            scene.LightSources.Add(new Reflector(pos2, dir2, Color.FromArgb(0, 255, 0)));
            scene.LightSources.Add(new Reflector(pos3, dir3, Color.FromArgb(0, 0, 255)));
            scene.LightSources.ForEach(light => Model.ZPlaneChanged += (s, e) => {
                var p = light.Position;
                p.Z = e;
                light.Position = p;
            });


            renderCanvas.Paint += OnPaint!;

            bmp = new DirectBitmap(renderCanvas.Width, renderCanvas.Height);
            renderer = new Renderer(scene, renderCanvas, bmp, shader);

            animationTimer = new Timer(1000.0 / 60.0);
            animationTimer.Elapsed += Timer_Tick!;

            pTexture.TextureChanged += (s, e) => Model.Texture = (cbTexture.Checked) ? new Texture(pTexture.FilePath!) : null;
            pNormalMap.TextureChanged += (s, e) => Model.NormalMap = (cbNormalMap.Checked) ? new NormalMap(pNormalMap.FilePath!) : null;

            Model.PropertyChanged += (s, e) => renderer.RenderScene();

            InitBindings();

            deltaTime = DeltaTime.CreatePoint();
            animationTimer.Start();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            RenderState s;
            lock (EditorViewModel.GetInstance())
                s = EditorViewModel.GetInstance().SetState();

            if (s.SurfaceDensity != bezierSurface.SurfaceDensity)
            {
                surfaceTransform.ResetSurfaceToOriginal();
                bezierSurface.GenerateMesh(s.SurfaceDensity);
                surfaceTransform.RefreshMesh();
            }

            surfaceTransform.Rotate(s.XRotation, 0, s.ZRotation);
            surfaceTransform.ApplyTransformations();
            surfaceTransform.ResetTransform();

            cubeTransform.Rotate(cubeXangle, 0, cubeZangle);
            cubeTransform.ApplyTransformations();
            cubeTransform.ResetTransform();

            scene.Render(renderer!);

            e.Graphics.DrawImageUnscaled(bmp!.Bitmap, 0, 0, bmp.Width, bmp.Height);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float delta = deltaTime!.GetDeltaTime();

            if (Model.IsAnimationPlaying)
            {
                angle += ((animDirection) ? 1 : -1) * MathF.PI / 2 * delta;
                float turnGap = 25.0f;

                if (angle > 6 * Math.PI) animDirection = false;
                else if (angle < 0) animDirection = true;

                lock (EditorViewModel.GetInstance())
                {
                    Vector3 lightPos = Model.LightPosition;
                    lightPos.X = (turnGap * angle) * (float)Math.Cos(angle);
                    lightPos.Y = (turnGap * angle) * (float)Math.Sin(angle);
                    Model.LightPosition = lightPos;
                }
            }

            cubeXangle += delta * MathF.PI / 4;
            cubeZangle += delta * MathF.PI / 8;
            renderer!.RenderScene();
        }

        private void InitBindings()
        {
            Binding binding;
            binding = new Binding("Value", Model, "SurfaceDensity", true, DataSourceUpdateMode.OnPropertyChanged);
            csDensity.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "XRotation", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = (int)e.Value! * (float)(Math.PI / 180);
            binding.Format += (sender, e) => e.Value = Convert.ToInt32((float)e.Value! * (float)(180 / Math.PI));
            csXRotation.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "ZRotation", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = (int)e.Value! * (float)(Math.PI / 180);
            binding.Format += (sender, e) => e.Value = Convert.ToInt32((float)e.Value! * (float)(180 / Math.PI));
            csZRotation.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Checked", Model, "RenderWireframe", true, DataSourceUpdateMode.OnPropertyChanged);
            cbWireframe.DataBindings.Add(binding);

            binding = new Binding("Checked", Model, "RenderControlPoints", true, DataSourceUpdateMode.OnPropertyChanged);
            cbControlPoints.DataBindings.Add(binding);

            binding = new Binding("Checked", Model, "MainLightEnabled", true, DataSourceUpdateMode.OnPropertyChanged);
            cbLightToggle.DataBindings.Add(binding);

            binding = new Binding("Checked", Model, "ReflectorsEnabled", true, DataSourceUpdateMode.OnPropertyChanged);
            cbReflectorsToggle.DataBindings.Add(binding);

            binding = new Binding("Enabled", pTexture, "FilePath", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = ((string)e.Value!) != String.Empty;
            cbTexture.DataBindings.Add(binding);

            binding = new Binding("Enabled", pNormalMap, "FilePath", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = ((string)e.Value!) != String.Empty;
            cbNormalMap.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "ZPlane", true, DataSourceUpdateMode.OnPropertyChanged);
            csLightZPlane.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "CoefKd", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = (int)e.Value! / Convert.ToSingle(csCoefKd.Divider);
            binding.Format += (sender, e) => e.Value = Convert.ToInt32(((float)e.Value!) * csCoefKd.Divider);
            csCoefKd.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "CoefKs", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = (int)e.Value! / Convert.ToSingle(csCoefKs.Divider);
            binding.Format += (sender, e) => e.Value = Convert.ToInt32(((float)e.Value!) * csCoefKs.Divider);
            csCoefKs.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "CoefM", true, DataSourceUpdateMode.OnPropertyChanged);
            csCoefM.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "ReflectorsFocus", true, DataSourceUpdateMode.OnPropertyChanged);
            csReflectorFocus.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Text", Model, "IsAnimationPlaying", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = (bool)e.Value! ? "Stop animation" : "Start animation";
            bAnimation.DataBindings.Add(binding);
        }

        private void bAnimation_Click(object sender, EventArgs e)
        {
            lock (EditorViewModel.GetInstance())
                Model.IsAnimationPlaying = !Model.IsAnimationPlaying;
        }

        private void bSurfaceColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.FullOpen = true;
            dlg.Color = Model.SurfaceColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lock (EditorViewModel.GetInstance())
                    Model.SurfaceColor = dlg.Color;
            }
        }

        private void bLightColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.FullOpen = true;
            dlg.Color = Model.LightColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lock (EditorViewModel.GetInstance())
                    Model.LightColor = dlg.Color;
            }
        }

        private void cbTexture_CheckedChanged(object sender, EventArgs e)
        {
            Model.Texture = (((CheckBox)sender).Checked) ? new Texture(pTexture.FilePath!) : null;
            renderer!.RenderScene();
        }

        private void cbNormalMap_CheckedChanged(object sender, EventArgs e)
        {
            Model.NormalMap = (((CheckBox)sender).Checked) ? new NormalMap(pNormalMap.FilePath!) : null;
            renderer!.RenderScene();
        }
    }
}
