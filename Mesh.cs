using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Mesh
    {
        public List<Triangle> Triangles { get; set; } = [];
        public List<Vertex> Vertices { get; set; } = [];
        private Vector3[,] _controlPoints;

        public Mesh(Vector3[,] controlPoints, int subdivisions)
        {
            _controlPoints = controlPoints;
            float step = 1.0f / subdivisions;

            for (int i = 0; i <= subdivisions; i++)
            {
                for (int j = 0; j <= subdivisions; j++)
                {
                    float u = i * step;
                    float v = j * step;

                    Vertices.Add(Projection(u, v));
                }
            }

            for (int i = 0; i < subdivisions; i++)
            {
                for (int j = 0; j < subdivisions; j++)
                {
                    int topLeft = i * (subdivisions + 1) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = (i + 1) * (subdivisions + 1) + j;
                    int bottomRight = bottomLeft + 1;

                    Triangles.Add(new Triangle(Vertices[topLeft], Vertices[bottomLeft], Vertices[topRight]));
                    Triangles.Add(new Triangle(Vertices[topRight], Vertices[bottomLeft], Vertices[bottomRight]));
                }
            }
        }

        public Vertex Projection(float u, float v)
        {
            float[] bernU1 = Bernstein(3, u);
            float[] bernU2 = Bernstein(2, u);
            float[] bernV1 = Bernstein(3, v);
            float[] bernV2 = Bernstein(2, v);

            Vector3 p = CalculatePosition(bernU1, bernV1);
            Vector3 pu = CalculatePu(bernU2, bernV1);
            Vector3 pv = CalculatePv(bernU1, bernV2);
            Vector2 uv = new Vector2(u, v);

            return new Vertex(p, pu, pv, uv);
        }

        private Vector3 CalculatePu(float[] bernU, float[] bernV)
        {
            Vector3 tangentU = Vector3.Zero;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector3 diff = _controlPoints[i + 1, j] - _controlPoints[i, j];
                    tangentU += diff * bernU[i] * bernV[j];
                }
            }
            return 3 * tangentU;
        }

        private Vector3 CalculatePv(float[] bernU, float[] bernV)
        {
            Vector3 tangentV = Vector3.Zero;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector3 diff = _controlPoints[i, j + 1] - _controlPoints[i, j];
                    tangentV += diff * bernU[i] * bernV[j];
                }
            }
            return 3 * tangentV;
        }

        private Vector3 CalculatePosition(float[] bernU, float[] bernV)
        {
            Vector3 pos = Vector3.Zero;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    pos += _controlPoints[i, j] * bernU[i] * bernV[j];
                }
            }

            return pos;
        }

        private static float[] Bernstein(int n, float t)
        {
            float[] bernstein = new float[n + 1];
            float factor = 1 - t;

            bernstein[0] = 1;

            for (int i = 1; i <= n; i++)
            {
                bernstein[i] = bernstein[i - 1] * (t / i) * (n - i + 1);
            }

            float factorPower = 1;
            for (int i = n; i >= 0; i--)
            {
                bernstein[i] *= factorPower;
                factorPower *= factor;
            }

            return bernstein;
        }

    }
}
