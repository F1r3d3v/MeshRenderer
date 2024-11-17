using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GK1_MeshEditor.CustomControls
{
    public partial class TexturePicker : UserControl, INotifyPropertyChanged
    {
        public delegate void TexturePickerEventHandler(object sender, string? path);
        public event TexturePickerEventHandler? TextureChanged;

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

        [Category("Appearance")]
        public string ButtonText
        {
            get => button1.Text; set => button1.Text = value;
        }

        private string? _filePath;
        public string? FilePath
        {
            get => _filePath; 
            set
            {
                if (!SetField(ref _filePath, value)) return;
            }
        }

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
