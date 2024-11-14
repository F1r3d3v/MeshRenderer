using System.Diagnostics;
using System.Numerics;
using Timer = System.Timers.Timer;

namespace GK1_MeshEditor
{
    public partial class Editor : Form
    {
        PhongShader shader = new PhongShader();
        DeltaTime? deltaTime;

        private Timer animationTimer;
        private Timer refreshTimer;
        private float angle = 0;
        private bool animDirection = true;
        private static TimeSpan counterElapsed = TimeSpan.Zero;
        private static int fpsCounter = 0;
        private static Form? EditorForm;
        private static string Title = "Mesh Editor";
        private static bool IsRunning = true;

        static BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
        static SurfaceTransform surfaceTransform = new SurfaceTransform() { _bezierSurface = bezierSurface };
        static Scene scene = new Scene();
        static Renderer? renderer;
        static DirectBitmap? bmpLive;
        static DirectBitmap? bmpLast;
        static bool Render = true;

        EditorViewModel Model { get; } = EditorViewModel.GetInstance();

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112)
            {
                if (m.WParam == (IntPtr)0xF030 || m.WParam == (IntPtr)0xF120 || m.WParam == (IntPtr)0xF032 || m.WParam == (IntPtr)0xF122)
                {
                    OnResizeBegin(EventArgs.Empty);
                }
            }
            base.WndProc(ref m);
        }

        public Editor()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            Load += (s, e) => WindowState = FormWindowState.Normal;
            Text = Title;
            EditorForm = this;
            Application.ApplicationExit += (s, e) => IsRunning = false;

            surfaceTransform.BezierSurface = bezierSurface;
            scene.graphicsObjects.Add(bezierSurface);
            renderCanvas.Paint += OnPaint!;

            InitBindings();

            animationTimer = new Timer(1000.0 / 60.0);
            animationTimer.Elapsed += Timer_Tick!;

            refreshTimer = new Timer(1000.0 / 60.0);
            refreshTimer.Elapsed += (s, e) => renderCanvas.Render();

            if (pTexture.DefaultTexture == string.Empty) cbTexture.Enabled = false;
            pTexture.TextureChanged += (s, e) => Model.Texture = (cbTexture.Checked) ? new Texture(pTexture.FilePath!) : null;

            if (pNormalMap.DefaultTexture == string.Empty) cbNormalMap.Enabled = false;
            pNormalMap.TextureChanged += (s, e) => Model.NormalMap = (cbNormalMap.Checked) ? new NormalMap(pNormalMap.FilePath!) : null;

            bmpLive = new DirectBitmap(renderCanvas.Width, renderCanvas.Height);
            bmpLast = new DirectBitmap(renderCanvas.Width, renderCanvas.Height);
            renderer = new Renderer(scene, renderCanvas, bmpLive, shader);

            Resize += Editor_Resize!;

            ResizeBegin += (s, e) =>
            {
                lock (EditorViewModel.GetInstance().RenderLock)
                {
                    Render = false;
                }
                Win32.SuspendPainting(renderCanvas.Handle);
            };

            ResizeEnd += (s, e) =>
            {
                lock (EditorViewModel.GetInstance().RenderLock)
                {
                    renderer.Resize(renderCanvas.Width, renderCanvas.Height);
                    Render = true;
                }
                Win32.ResumePainting(renderCanvas.Handle);
            };

            var renderThread = new Thread(new ThreadStart(RenderLoop));
            renderThread.Start();
            refreshTimer.Start();
        }

        FormWindowState LastWindowState = FormWindowState.Normal;
        private void Editor_Resize(object sender, EventArgs e)
        {
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;
                if (WindowState == FormWindowState.Maximized || WindowState == FormWindowState.Normal)
                {
                    lock (EditorViewModel.GetInstance().RenderLock)
                    {
                        renderer!.Resize(renderCanvas.Width, renderCanvas.Height);
                        Render = true;
                    }
                    Win32.ResumePainting(renderCanvas.Handle);
                }
            }
        }

        private static void RenderLoop()
        {
            double maxFPS = 100;
            double minFramePeriodMsec = 1000.0 / maxFPS;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (IsRunning)
            {
                lock (EditorViewModel.GetInstance())
                    EditorViewModel.GetInstance().SetState();

                RenderState s = EditorViewModel.GetInstance().GetState();

                if (s.SurfaceDensity != bezierSurface.SurfaceDensity)
                {
                    Array.Copy(surfaceTransform.OriginalControlPoints, bezierSurface.ControlPoints, 16);
                    bezierSurface.GenerateMesh(s.SurfaceDensity);
                    surfaceTransform.BezierSurface = bezierSurface;
                }

                surfaceTransform.Rotate(s.XRotation, 0, s.ZRotation);
                surfaceTransform.ApplyTransformations();


                lock (EditorViewModel.GetInstance().RenderLock)
                    if (Render)
                    {
                        scene.Render(renderer!);
                    }

                lock (bmpLast!)
                {
                    bmpLast.Dispose();
                    bmpLast = (DirectBitmap)bmpLive!.Clone();
                }

                double msToWait = minFramePeriodMsec - stopwatch.ElapsedMilliseconds;
                if (msToWait > 0)
                    Thread.Sleep((int)msToWait);

                UpdateFPSCounter(stopwatch);
                stopwatch.Restart();
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            lock (bmpLast!)
                e.Graphics.DrawImage(bmpLast!.Bitmap, 0, 0, bmpLast.Width, bmpLast.Height);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            float delta = deltaTime!.GetDeltaTime();
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
            lock (EditorViewModel.GetInstance())
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
            lock (EditorViewModel.GetInstance())
                Model.Texture = (((CheckBox)sender).Checked) ? new Texture(pTexture.FilePath!) : null;
        }

        private void cbNormalMap_CheckedChanged(object sender, EventArgs e)
        {
            lock (EditorViewModel.GetInstance())
                Model.NormalMap = (((CheckBox)sender).Checked) ? new NormalMap(pNormalMap.FilePath!) : null;
        }

        private static void UpdateFPSCounter(Stopwatch sw)
        {
            counterElapsed += sw.Elapsed;
            fpsCounter++;
            if (counterElapsed >= TimeSpan.FromSeconds(1))
            {
                EditorForm!.Invoke(() =>
                    EditorForm.Text = Title + " | FPS: " + fpsCounter.ToString() + " | M: " + (GC.GetTotalMemory(false) / 1048576f).ToString("F") + " MB"
                );
                fpsCounter = 0;
                counterElapsed -= TimeSpan.FromSeconds(1);
            }
        }
    }
}
