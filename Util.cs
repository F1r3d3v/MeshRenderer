using System.Numerics;

namespace GK1_MeshEditor
{
    internal static class Util
    {
        // TODO: Cache independent variables for given triangle
        public static Vector3 CartesianToBaricentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            if (Math.Abs(b.X - c.X) < 1e-5 && Math.Abs(b.Y - c.Y) < 1e-5) return new Vector3(1.0f / 3.0f, 1.0f / 3.0f, 1.0f / 3.0f);
            Vector2 v0 = b - a, v1 = c - a, v2 = p - a;
            float invDen = 1 / (v0.X * v1.Y - v1.X * v0.Y);
            float v = (v2.X * v1.Y - v1.X * v2.Y) * invDen;
            float w = (v0.X * v2.Y - v2.X * v0.Y) * invDen;
            float u = 1.0f - v - w;

            return new Vector3(u, v, w);
        }

        public static Vertex InterpolateVertex(Triangle tri, Vector3 baricentricCoords)
        {
            float u = baricentricCoords.X;
            float v = baricentricCoords.Y;
            float w = baricentricCoords.Z;
            
            Vector3 interpolatedP = u * tri.V1.P + v * tri.V2.P + w * tri.V3.P;
            Vector3 interpolatedPu = u * tri.V1.Pu + v * tri.V2.Pu + w * tri.V3.Pu;
            Vector3 interpolatedPv = u * tri.V1.Pv + v * tri.V2.Pv + w * tri.V3.Pv;
            Vector2 interpolatedUV = u * tri.V1.UV + v * tri.V2.UV + w * tri.V3.UV;

            return new Vertex(interpolatedP, interpolatedPu, interpolatedPv, interpolatedUV);
        }

        public static void AssignVectorToMatrix(ref Matrix4x4 m, Vector3 v, int col)
        {
            m[0, col] = v.X;
            m[1, col] = v.Y;
            m[2, col] = v.Z;
            m[3, col] = 0.0f;
        }

        public static bool CloseTo(this Vector3 a, Vector3 b, double eps)
        {
            return Math.Abs(a.X - b.X) < eps && Math.Abs(a.Y - b.Y) < eps && Math.Abs(a.Z - b.Z) < eps;
        }

        public static Matrix4x4 GetReverseZ()
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, 1, 0, 0,
                                 0, 0, -1, 0,
                                 0, 0, 1, 0);
        }
    }
}
