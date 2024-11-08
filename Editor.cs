using GK1_MeshEditor.CustomControls;
using GK1_PolygonEditor;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace GK1_MeshEditor
{
    public partial class Editor : Form
    {
        private BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
        private SurfaceTransform surfaceTransform = new SurfaceTransform();
        UnsafeBitmap bitmap;
        Renderer renderer;
        Scene scene;
        PhongShader shader = new PhongShader();
        DeltaTime deltaTime;
        EditorViewModel Model { get; } = EditorViewModel.GetInstance();

        private Timer timer;
        private float angle = 0;
        private bool animDirection = true;

        public Editor()
        {
            InitializeComponent();

            surfaceTransform.BezierSurface = bezierSurface;

            bitmap = new UnsafeBitmap(renderCanvas.Width, renderCanvas.Height);
            scene = new Scene();
            scene.graphicsObjects.Add(bezierSurface);

            renderer = new Renderer(scene, renderCanvas, bitmap);

            timer = new Timer(16.6);
            timer.Elapsed += Timer_Tick!;

            shader.BezierSurface = bezierSurface;
            renderer.Shader = shader;

            Model.DensityChanged += Model_DensityChanged;
            Model.RotationChanged += Model_RotationChanged;
            Model.PropertyChanged += (s, e) => renderer.RenderScene();
            InitBindings();
        }

        private void Model_RotationChanged()
        {
            surfaceTransform.Rotate(Model.XRotation, 0, Model.ZRotation);
            surfaceTransform.ApplyTransformations();
        }

        private void Model_DensityChanged()
        {
            Array.Copy(surfaceTransform.OriginalControlPoints, bezierSurface.ControlPoints, 16);
            bezierSurface.GenerateMesh(Model.SurfaceDensity);
            surfaceTransform.BezierSurface = bezierSurface;
            Model_RotationChanged();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float delta = deltaTime.GetDeltaTime();
            //Console.WriteLine(delta);
            angle += ((animDirection) ? 1 : -1) * MathF.PI/2 * delta;
            float turnGap = 25.0f;

            if (angle > 6 * Math.PI) animDirection = false;
            else if (angle < 0) animDirection = true;

            Vector3 lightPos = Model.LightPosition;
            lightPos.X = (turnGap * angle) * (float)Math.Cos(angle);
            lightPos.Y = (turnGap * angle) * (float)Math.Sin(angle);
            //Console.WriteLine(lightPos);
            Model.LightPosition = lightPos;
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
            binding.Parse += (sender, e) => e.Value = (int)e.Value! / 100.0f;
            binding.Format += (sender, e) => e.Value = Convert.ToInt32(((float)e.Value!) * 100);
            csCoefKd.TrackBar.DataBindings.Add(binding);

            binding = new Binding("Value", Model, "CoefKs", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Parse += (sender, e) => e.Value = (int)e.Value! / 100.0f;
            binding.Format += (sender, e) => e.Value = Convert.ToInt32(((float)e.Value!) * 100);
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
                timer.Start();
            }
            else timer.Stop();
        }

        private void bSurfaceColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.FullOpen = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bezierSurface.Color = dlg.Color;
                renderer.RenderScene();
            }
        }
    }
}
