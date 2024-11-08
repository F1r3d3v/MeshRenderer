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
        private Bitmap _texture;

        public Texture(Bitmap texture)
        {
            _texture = texture;
        }

        public Color Sample(float u, float v)
        {
            int x = (int)(u * (_texture.Width - 1));
            int y = (int)(v * (_texture.Height - 1));
            return _texture.GetPixel(x, y);
        }
    }

}
