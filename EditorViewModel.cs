using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GK1_MeshEditor
{
    internal sealed class EditorViewModel : INotifyPropertyChanged
    {
        private static EditorViewModel? _instance;

        public static EditorViewModel GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EditorViewModel();
            }
            return _instance;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            lock (this)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }

        public object RenderLock = new object();

        private Vector3 _lightPosition = new Vector3(0, 0, 150);
        public Vector3 LightPosition
        {
            get => _lightPosition;
            set
            {
                if (!SetField(ref _lightPosition, value)) return;
            }
        }
        public Color LightColor { get; set; } = Color.White;

        private int _surfaceDensity = 20;
        public int SurfaceDensity
        {
            get => _surfaceDensity;
            set
            {
                if (!SetField(ref _surfaceDensity, value)) return;
            }
        }

        private float _xRotation = 0;
        public float XRotation
        {
            get => _xRotation;
            set
            {
                if (!SetField(ref _xRotation, value)) return;
            }
        }

        private float _zRotation = 0;
        public float ZRotation
        {
            get => _zRotation;
            set
            {
                if (!SetField(ref _zRotation, value)) return;
            }
        }

        private bool _renderWireframe = true;
        public bool RenderWireframe
        {
            get => _renderWireframe;
            set
            {
                if (!SetField(ref _renderWireframe, value)) return;
            }
        }

        private float _coefKd = 0.5f;
        public float CoefKd
        {
            get => _coefKd;
            set
            {
                if (!SetField(ref _coefKd, value)) return;
            }
        }

        private float _coefKs = 0.5f;
        public float CoefKs
        {
            get => _coefKs;
            set
            {
                if (!SetField(ref _coefKs, value)) return;
            }
        }

        private int _coefM = 10;
        public int CoefM
        {
            get => _coefM;
            set
            {
                if (!SetField(ref _coefM, value)) return;
            }
        }

        private bool _isAnimationPlaying = false;
        public bool IsAnimationPlaying
        {
            get => _isAnimationPlaying;
            set
            {
                if (!SetField(ref _isAnimationPlaying, value)) return;
            }
        }

        public Color SurfaceColor = Color.Gray;
        public Texture? Texture;
        public NormalMap? NormalMap;

        private RenderState _state;
        public void SetState() => _state = new RenderState(_instance!);
        public RenderState GetState() => _state;
    }

    internal struct RenderState(EditorViewModel m)
    {
        public int SurfaceDensity = m.SurfaceDensity;
        public float ZRotation = m.ZRotation;
        public float XRotation = m.XRotation;
        public float CoefKd = m.CoefKd;
        public float CoefKs = m.CoefKs;
        public int CoefM = m.CoefM;
        public Vector3 LightPosition = m.LightPosition;
        public Color LightColor = m.LightColor;
        public Color SurfaceColor = m.SurfaceColor;
        public Texture? Texture = m.Texture;
        public NormalMap? NormalMap = m.NormalMap;
    }
}
