using GK1_PolygonEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Texture
    {
        private DirectBitmap _texture;

        public Texture(string path)
        {
            Bitmap m = new Bitmap(path);
            _texture = new DirectBitmap(m.Width, m.Height);
            Graphics g = Graphics.FromImage(_texture.Bitmap);
            g.DrawImage(m, 0, 0);
        }

        public Color Sample(float u, float v)
        {
            u = Math.Clamp(u, 0f, 1f);
            v = Math.Clamp(1.0f - v, 0f, 1f);
            int x = (int)Math.Round(u * (_texture.Width - 1));
            int y = (int)Math.Round(v * (_texture.Height - 1));
            return _texture.GetPixel(x, y);
        }
    }

}
