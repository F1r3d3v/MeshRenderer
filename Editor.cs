using GK1_MeshEditor.CustomControls;
using GK1_PolygonEditor;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GK1_MeshEditor
{
    public partial class Editor : Form, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
        private SurfaceTransform surfaceTransform = new SurfaceTransform();
        UnsafeBitmap bitmap;
        Renderer renderer;

        public bool IsAnimationPlaying { get; private set; } = false;

        private Vector3 _lightPosition = Vector3.Zero;
        public Vector3 LightPosition
        {
            get => _lightPosition;
            set
            {
                if (!SetField(ref _lightPosition, value)) return;
                renderer.Render();
            }
        }
        public Color LightColor { get; private set; } = Color.White;

        private int _surfaceDensity;
        public int SurfaceDensity
        {
            get => _surfaceDensity;
            set
            {
                if (!SetField(ref _surfaceDensity, value)) return;
                Array.Copy(surfaceTransform.OriginalControlPoints, bezierSurface.ControlPoints, 16);
                bezierSurface.GenerateMesh(value);
                surfaceTransform.BezierSurface = bezierSurface;
                surfaceTransform.Rotate(_xRotation, 0, _zRotation);
                surfaceTransform.ApplyTransformations();
                renderer.Render();
            }
        }

        private float _xRotation;
        public float XRotation
        {
            get => _surfaceDensity;
            set
            {
                if (!SetField(ref _xRotation, value)) return;
                surfaceTransform.Rotate(value, 0, _zRotation);
                surfaceTransform.ApplyTransformations();
                renderer.Render();
            }
        }

        private float _zRotation;
        public float ZRotation
        {
            get => _surfaceDensity;
            set
            {
                if (!SetField(ref _zRotation, value)) return;
                surfaceTransform.Rotate(_xRotation, 0, value);
                surfaceTransform.ApplyTransformations();
                renderer.Render();
            }
        }

        public Editor()
        {
            InitializeComponent();

            surfaceTransform.BezierSurface = bezierSurface;

            bitmap = new UnsafeBitmap(renderCanvas.Width, renderCanvas.Height);
            renderer = new Renderer(bezierSurface, renderCanvas, bitmap);

            InitBindings();
        }


        private void InitBindings()
        {
            Binding binding;
            binding = new Binding("SurfaceDensity", csDensity.TrackBar, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            DataBindings.Add(binding);

            binding = new Binding("XRotation", csXRotation.TrackBar, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = (int)e.Value! * (float)(Math.PI / 180);
            DataBindings.Add(binding);

            binding = new Binding("ZRotation", csZRotation.TrackBar, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = (int)e.Value! * (float)(Math.PI / 180);
            DataBindings.Add(binding);

            binding = new Binding("Checked", renderer, "RenderWireframe", true, DataSourceUpdateMode.OnPropertyChanged);
            cbWireframe.DataBindings.Add(binding);

            binding = new Binding("LightPosition", csLightZPlane.TrackBar, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = new Vector3(_lightPosition.X, _lightPosition.Y, Convert.ToSingle(e.Value!));
            DataBindings.Add(binding);
        }
    }
}
