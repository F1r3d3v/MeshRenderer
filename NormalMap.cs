using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class NormalMap
    {
        private Bitmap _normalMap;

        public NormalMap(Bitmap normalMap)
        {
            _normalMap = normalMap;
        }

        public Vector3 Sample(float u, float v)
        {
            int x = (int)(u * (_normalMap.Width - 1));
            int y = (int)(v * (_normalMap.Height - 1));
            Color color = _normalMap.GetPixel(x, y);

            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float Nx = 2 * r - 1;
            float Ny = 2 * g - 1;
            float Nz = 2 * b - 1;

            return new Vector3(Nx, Ny, Nz);
        }
    }
}
