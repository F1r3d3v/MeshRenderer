using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Cube : GraphicsObject
    {
        public Vector3 P1 { get; set; }
        public Vector3 P2 { get; set; }
        public Mesh Mesh { get; set; } = new Mesh();

        public Cube() { }

        public Cube(Vector3 p1, Vector3 p2)
        {
            P1 = p1;
            P2 = p2;
            CubeMesh.GenerateMesh(Mesh, p1, p2);
        }

        public override void Draw(Renderer renderer)
        {
            if (Mesh != null)
            {
                if (EditorViewModel.GetInstance().RenderWireframe)
                    renderer.DrawWireframe(Mesh);
                else
                    renderer.DrawMesh(Mesh, false);
            }
        }
    }
}
