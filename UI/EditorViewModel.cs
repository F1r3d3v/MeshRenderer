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

        public delegate void ZPlaneEventHandler(object sender, float z);
        public event ZPlaneEventHandler? ZPlaneChanged;

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

        private Color _lightColor = Color.White;
        public Color LightColor
        {
            get => _lightColor;
            set
            {
                if (!SetField(ref _lightColor, value)) return;
            }
        }

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

        private bool _renderControlPoints = true;
        public bool RenderControlPoints
        {
            get => _renderControlPoints;
            set
            {
                if (!SetField(ref _renderControlPoints, value)) return;
            }
        }

        private bool _mainLightEnabled = true;
        public bool MainLightEnabled
        {
            get => _mainLightEnabled;
            set
            {
                if (!SetField(ref _mainLightEnabled, value)) return;
            }
        }

        private bool _reflectorsEnabled = false;
        public bool ReflectorsEnabled
        {
            get => _reflectorsEnabled;
            set
            {
                if (!SetField(ref _reflectorsEnabled, value)) return;
            }
        }

        private int _reflectorsFocus = 1;
        public int ReflectorsFocus
        {
            get => _reflectorsFocus;
            set
            {
                if (!SetField(ref _reflectorsFocus, value)) return;
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

        private float _zPlane = 300;
        public float ZPlane
        {
            get => _zPlane;
            set
            {
                if (!SetField(ref _zPlane, value)) return;
                LightPosition.Z = value;
                ZPlaneChanged?.Invoke(this, value);
            }
        }

        public Vector3 LightPosition = new Vector3(0, 0, 300);

        private bool _isAnimationPlaying = false;
        public bool IsAnimationPlaying
        {
            get => _isAnimationPlaying;
            set
            {
                if (!SetField(ref _isAnimationPlaying, value)) return;
            }
        }

        private Color _surfaceColor = Color.White;
        public Color SurfaceColor
        {
            get => _surfaceColor;
            set
            {
                if (!SetField(ref _surfaceColor, value)) return;
            }
        }

        private Texture? _texture;
        public Texture? Texture
        {
            get => _texture;
            set
            {
                if (!SetField(ref _texture, value)) return;
            }
        }

        public NormalMap? _normalMap;
        public NormalMap? NormalMap
        {
            get => _normalMap;
            set
            {
                if (!SetField(ref _normalMap, value)) return;
            }
        }

        private RenderState _state;
        public RenderState SetState() => _state = new RenderState(_instance!);
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
        public int ReflectorsFocus = m.ReflectorsFocus;
        public Vector3 LightPosition = m.LightPosition;
        public Color LightColor = m.LightColor;
        public Color SurfaceColor = m.SurfaceColor;
        public Texture? Texture = m.Texture;
        public NormalMap? NormalMap = m.NormalMap;
    }
}
