using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal interface ILightSource
    {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
    }
}
