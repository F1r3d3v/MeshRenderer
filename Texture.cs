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
        private Bitmap? _normalMap;

        public Texture(Bitmap texture, Bitmap? normalMap = null)
        {
            _texture = texture;
            _normalMap = normalMap;
        }

        public Vector3 GetNormal(float u, float v)
        {
            int x = (int)(u * (_texture.Width - 1));
            int y = (int)(v * (_texture.Height - 1));
            Color color = _normalMap?.GetPixel(x, y) ?? Color.FromArgb(128, 128, 255);

            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float Nx = 2 * r - 1;
            float Ny = 2 * g - 1;
            float Nz = 2 * b - 1;

            return new Vector3(Nx, Ny, Nz);
        }

        public Color Sample(float u, float v)
        {
            int x = (int)(u * (_texture.Width - 1));
            int y = (int)(v * (_texture.Height - 1));
            return _texture.GetPixel(x, y);
        }
    }

}
