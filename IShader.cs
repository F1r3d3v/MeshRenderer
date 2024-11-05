using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal interface IShader
    {
        public Color CalculateColor(Vertex vertex);
    }
}
