using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            set 
            {
                Slider.Value = value;
                SliderValue.Text = ((double)value / Divider).ToString();
            }
        }
        [Category("Behavior")]
        public int Divider { get; set; } = 1;


        public CustomSilder()
        {
            InitializeComponent();
        }

        public virtual void Slider_Scroll(object sender, EventArgs e) => SliderValue.Text = ((double)Value / Divider).ToString();
    }
}
