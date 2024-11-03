using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Triangle
    {
        public Vertex V1 { get; }
        public Vertex V2 { get; }
        public Vertex V3 { get; }

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
    }
}
