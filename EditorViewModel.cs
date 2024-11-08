using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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

        public event Action? DensityChanged;
        public event Action? RotationChanged;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private Vector3 _lightPosition;
        public Vector3 LightPosition
        {
            get => _lightPosition;
            set
            {
                if (!SetField(ref _lightPosition, value)) return;
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
                DensityChanged?.Invoke();
            }
        }

        private float _xRotation;
        public float XRotation
        {
            get => _xRotation;
            set
            {
                if (!SetField(ref _xRotation, value)) return;
                RotationChanged?.Invoke();
            }
        }

        private float _zRotation;
        public float ZRotation
        {
            get => _zRotation;
            set
            {
                if (!SetField(ref _zRotation, value)) return;
                RotationChanged?.Invoke();
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

        private float _coefKd;
        public float CoefKd
        {
            get => _coefKd;
            set
            {
                if (!SetField(ref _coefKd, value)) return;
            }
        }

        private float _coefKs;
        public float CoefKs
        {
            get => _coefKs;
            set
            {
                if (!SetField(ref _coefKs, value)) return;
            }
        }

        private float _coefM;
        public float CoefM
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
    }
}
