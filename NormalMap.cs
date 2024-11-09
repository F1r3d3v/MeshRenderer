using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class NormalMap(string path)
    {
        private Texture _normalMap = new Texture(path);

        public Vector3 Sample(float u, float v)
        {
            Color color = _normalMap.Sample(u, v);

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
