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
    public partial class CustomSilder : UserControl
    {

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
            set => Slider.Value = value;
        }
        [Category("Behavior")]
        public int Divider { get; set; } = 1;

        public TrackBar TrackBar => Slider;

        public CustomSilder()
        {
            InitializeComponent();
            var binding = new Binding("Text", Slider, "Value", true, DataSourceUpdateMode.Never);
            binding.Format += (sender, e) => e.Value = (Convert.ToSingle(e.Value!) / Divider).ToString();
            SliderValue.DataBindings.Add(binding);
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
