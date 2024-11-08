using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1_MeshEditor.CustomControls
{
    public partial class CustomSilder : UserControl, INotifyPropertyChanged
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

        [Category("Appearance")]
        public string SliderText
        {
            get => SliderName.Text; set => SliderName.Text = value;
        }

        [Category("Behavior")]
        public int MinValue
        {
            get => Slider.Minimum; set => Slider.Minimum = value;
        }

        [Category("Behavior")]
        public int MaxValue
        {
            get => Slider.Maximum; set => Slider.Maximum = value;
        }

        [Category("Behavior")]
        public int Value
        {
            get => Slider.Value;
            set
            {
                Slider.Value = value;
            }
        }
        [Category("Behavior")]
        public int Divider { get; set; } = 1;

        public TrackBar TrackBar => Slider;

        private float _realValue;
        public float RealValue
        {
            get => _realValue;
            set
            {
                if (!SetField(ref _realValue, value)) return;
            }
        }

        public CustomSilder()
        {
            InitializeComponent();
            var binding = new Binding("Text", Slider, "Value");
            binding.Format += (sender, e) => e.Value = (Convert.ToSingle(e.Value!) / Divider).ToString();
            SliderValue.DataBindings.Add(binding);

            binding = new Binding("RealValue", Slider, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += (sender, e) => e.Value = Convert.ToSingle(e.Value!) / Divider;
            DataBindings.Add(binding);
        }

        private void CustomSilder_Load(object sender, EventArgs e)
        {
            foreach (Binding binding in Slider.DataBindings)
            {
                binding.WriteValue();
            }
        }
    }
}
