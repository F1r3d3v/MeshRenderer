using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GK1_MeshEditor.CustomControls
{
    public partial class TexturePicker : UserControl
    {
        public delegate void TexturePickerEventHandler(object sender, string? path);
        public event TexturePickerEventHandler? TextureChanged;

        [Category("Appearance")]
        public string ButtonText
        {
            get => button1.Text; set => button1.Text = value;
        }
        public string? FilePath { get; private set; }

        private string _default = String.Empty;
        [Category("Behavior")]
        public string DefaultTexture { 
            get => _default;
            set
            {
                _default = value;
                string path = Path.Combine(Application.StartupPath, $"Resources\\{value}");
                if (File.Exists(path))
                {
                    FilePath = path;
                    pictureBox1.Image = Bitmap.FromFile(path);
                }
            }
        }

        public TexturePicker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.InitialDirectory = Path.Combine(Application.StartupPath, "Resources");
            ofd.Title = button1.Text;
            if (ofd.ShowDialog() == DialogResult.OK )
            {
                FilePath = ofd.FileName;
                pictureBox1.Image = Bitmap.FromFile(FilePath);
                TextureChanged?.Invoke(this, FilePath);
            }
        }
    }
}
