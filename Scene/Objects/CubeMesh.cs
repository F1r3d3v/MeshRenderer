using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class CubeMesh
    {
        public static void GenerateMesh(Mesh mesh, Vector3 p1, Vector3 p2)
        {
            mesh.Vertices.Clear();
            mesh.Triangles.Clear();

            var vertices = new List<Vertex>
            {
                // Front face
                new Vertex(new Vector3(p1.X, p1.Y, p2.Z), Vector3.UnitX, Vector3.UnitY, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p1.X, p2.Y, p2.Z), Vector3.UnitX, Vector3.UnitY, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p2.X, p2.Y, p2.Z), Vector3.UnitX, Vector3.UnitY, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p2.X, p1.Y, p2.Z), Vector3.UnitX, Vector3.UnitY, new Vector2(1, 0)), // Bottom right

                // Back face
                new Vertex(new Vector3(p2.X, p1.Y, p1.Z), -Vector3.UnitX, Vector3.UnitY, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p2.X, p2.Y, p1.Z), -Vector3.UnitX, Vector3.UnitY, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p1.X, p2.Y, p1.Z), -Vector3.UnitX, Vector3.UnitY, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p1.X, p1.Y, p1.Z), -Vector3.UnitX, Vector3.UnitY, new Vector2(1, 0)), // Bottom right

                // Left face
                new Vertex(new Vector3(p1.X, p1.Y, p1.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p1.X, p2.Y, p1.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p1.X, p2.Y, p2.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p1.X, p1.Y, p2.Z), Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)), // Bottom right

                // Right face
                new Vertex(new Vector3(p2.X, p1.Y, p2.Z), -Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p2.X, p2.Y, p2.Z), -Vector3.UnitZ, Vector3.UnitY, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p2.X, p2.Y, p1.Z), -Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p2.X, p1.Y, p1.Z), -Vector3.UnitZ, Vector3.UnitY, new Vector2(1, 0)), // Bottom right

                // Top face
                new Vertex(new Vector3(p1.X, p2.Y, p2.Z), Vector3.UnitX, -Vector3.UnitZ, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p1.X, p2.Y, p1.Z), Vector3.UnitX, -Vector3.UnitZ, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p2.X, p2.Y, p1.Z), Vector3.UnitX, -Vector3.UnitZ, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p2.X, p2.Y, p2.Z), Vector3.UnitX, -Vector3.UnitZ, new Vector2(1, 0)), // Bottom right

                // Bottom face
                new Vertex(new Vector3(p1.X, p1.Y, p1.Z), Vector3.UnitX, Vector3.UnitZ, new Vector2(0, 0)), // Bottom left
                new Vertex(new Vector3(p1.X, p1.Y, p2.Z), Vector3.UnitX, Vector3.UnitZ, new Vector2(0, 1)), // Top left
                new Vertex(new Vector3(p2.X, p1.Y, p2.Z), Vector3.UnitX, Vector3.UnitZ, new Vector2(1, 1)), // Top right
                new Vertex(new Vector3(p2.X, p1.Y, p1.Z), Vector3.UnitX, Vector3.UnitZ, new Vector2(1, 0)), // Bottom right
            };

            mesh.Vertices.AddRange(vertices);

            for (int i = 0; i < 6; i++)
            {
                int bottomLeft = i * 4;
                int topLeft = bottomLeft + 1;
                int topRight = bottomLeft + 2;
                int bottomRight = bottomLeft + 3;

                mesh.Triangles.Add(new Triangle(mesh.Vertices[bottomLeft], mesh.Vertices[topLeft], mesh.Vertices[bottomRight]));
                mesh.Triangles.Add(new Triangle(mesh.Vertices[topRight], mesh.Vertices[topLeft], mesh.Vertices[bottomRight]));
            }
        }
    }
}
