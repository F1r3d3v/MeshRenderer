using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Reflector : ILightSource
    {
        public Vector3 Position {  get; set; }
        public Vector3 Direction { get; set; }
        public Color Color { get; set; }

        public Reflector(Vector3 pos, Vector3 dir, Color col)
        {
            Position = pos;
            Direction = dir;
            Color = col;
        }
    }
}
