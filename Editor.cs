using GK1_MeshEditor.CustomControls;
using GK1_PolygonEditor;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace GK1_MeshEditor
{
    public partial class Editor : Form
    {
        static BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
        static SurfaceTransform surfaceTransform = new SurfaceTransform();
        PhongShader shader = new PhongShader();
        DeltaTime deltaTime;

        private Timer animationTimer;
        private Timer refreshTimer;
        private float angle = 0;
        private bool animDirection = true;

        static Scene scene = new Scene();
        static Renderer? renderer;
        static DirectBitmap? bmpLive;
        static DirectBitmap? bmpLast;

        EditorViewModel Model { get; } = EditorViewModel.GetInstance();

        public Editor()
        {
            InitializeComponent();

            surfaceTransform.BezierSurface = bezierSurface;
            scene.graphicsObjects.Add(bezierSurface);
            renderCanvas.Paint += OnPaint!;
            Model.DensityChanged += Model_DensityChanged;
            InitBindings();

            animationTimer = new Timer(1000.0 / 60.0);
            animationTimer.Elapsed += Timer_Tick!;

            refreshTimer = new Timer(1000.0 / 60.0);
            refreshTimer.Elapsed += (s, e) => renderCanvas.Invalidate();

            shader.BezierSurface = bezierSurface;

            bmpLive = new DirectBitmap(renderCanvas.Width, renderCanvas.Height);
            bmpLast = new DirectBitmap(renderCanvas.Width, renderCanvas.Height);
            renderer = new Renderer(scene, renderCanvas, bmpLive, bmpLast);
            renderer.Shader = shader;

            var renderThread = new Thread(new ThreadStart(RenderLoop));
            renderThread.Start();
            refreshTimer.Start();
        }

        private static void RenderLoop()
        {
            double maxFPS = 60;
            double minFramePeriodMsec = 1000.0 / maxFPS;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                lock (EditorViewModel.GetInstance())
                    EditorViewModel.GetInstance().SetState();

                RenderState s = EditorViewModel.GetInstance().GetState();

                Array.Copy(surfaceTransform.OriginalControlPoints, bezierSurface.ControlPoints, 16);
                bezierSurface.GenerateMesh(s.SurfaceDensity);
                surfaceTransform.BezierSurface = bezierSurface;

                surfaceTransform.Rotate(s.XRotation, 0, s.ZRotation);
                surfaceTransform.ApplyTransformations();
                scene.Render(renderer!);
                renderer!.DrawPoint(EditorViewModel.GetInstance().LightPosition, Brushes.Black);

                lock (bmpLast!)
                {
                    bmpLast.Dispose();
                    bmpLast = (DirectBitmap)bmpLive!.Clone();
                }

                double msToWait = minFramePeriodMsec - stopwatch.ElapsedMilliseconds;
                if (msToWait > 0)
                    Thread.Sleep((int)msToWait);
                stopwatch.Restart();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            lock (bmpLast!)
                e.Graphics.DrawImage(bmpLast!.Bitmap, 0, 0);
        }

        private void Model_DensityChanged()
        {
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float delta = deltaTime.GetDeltaTime();
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

            binding = new Binding("Value", Model, "LightPosition", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = new Vector3(Model.LightPosition.X, Model.LightPosition.Y, Convert.ToSingle(e.Value!));
            binding.Format += (sender, e) => e.Value = Convert.ToInt32(((Vector3)e.Value!).Z);
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

            binding = new Binding("Text", Model, "IsAnimationPlaying", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = (bool)e.Value! ? "Stop animation" : "Start animation";
            bAnimation.DataBindings.Add(binding);
        }

        private void bAnimation_Click(object sender, EventArgs e)
        {
            Model.IsAnimationPlaying = !Model.IsAnimationPlaying;
            if (Model.IsAnimationPlaying)
            {
                deltaTime = DeltaTime.CreatePoint();
                animationTimer.Start();
            }
            else
                animationTimer.Stop();
        }

        private void bSurfaceColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.FullOpen = true;
            dlg.Color = bezierSurface.Color;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bezierSurface.Color = dlg.Color;
            }
        }

        private void bLightColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.FullOpen = true;
            dlg.Color = Model.LightColor;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Model.LightColor = dlg.Color;
            }
        }

        private void cbTexture_CheckedChanged(object sender, EventArgs e)
        {
            bezierSurface.Texture = (((CheckBox)sender).Checked) ? new Texture(pTexture.FilePath!) : null;
        }

        private void cbNormalMap_CheckedChanged(object sender, EventArgs e)
        {
            bezierSurface.NormalMap = (((CheckBox)sender).Checked) ? new NormalMap(pNormalMap.FilePath!) : null;
        }
    }
}
