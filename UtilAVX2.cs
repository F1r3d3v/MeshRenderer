using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal static class UtilAVX2
    {
        public struct AVX2Vertex
        {
            public Vector256<float> Px;
            public Vector256<float> Py;
            public Vector256<float> Pz;

            public Vector256<float> PuX;
            public Vector256<float> PuY;
            public Vector256<float> PuZ;

            public Vector256<float> PvX;
            public Vector256<float> PvY;
            public Vector256<float> PvZ;

            public Vector256<float> NX;
            public Vector256<float> NY;
            public Vector256<float> NZ;

            public Vector256<float> UVx;
            public Vector256<float> UVy;

            public AVX2Vertex(Vector256<float> px, Vector256<float> py, Vector256<float> pz,
                              Vector256<float> puX, Vector256<float> puY, Vector256<float> puZ,
                              Vector256<float> pvX, Vector256<float> pvY, Vector256<float> pvZ,
                              Vector256<float> nX, Vector256<float> nY, Vector256<float> nZ,
                              Vector256<float> uvx, Vector256<float> uvy)
            {
                Px = px;
                Py = py;
                Pz = pz;

                PuX = puX;
                PuY = puY;
                PuZ = puZ;

                PvX = pvX;
                PvY = pvY;
                PvZ = pvZ;

                NX = nX;
                NY = nY;
                NZ = nZ;

                UVx = uvx;
                UVy = uvy;
            }
        }

        public static AVX2Vertex InterpolateVertexVec(Triangle tri, AVX2Vector3 v)
        {
            // Interpolate position P
            Vector256<float> interpPx = v.X * tri.V1.P.X + v.Y * tri.V2.P.X + v.Z * tri.V3.P.X;
            Vector256<float> interpPy = v.X * tri.V1.P.Y + v.Y * tri.V2.P.Y + v.Z * tri.V3.P.Y;
            Vector256<float> interpPz = v.X * tri.V1.P.Z + v.Y * tri.V2.P.Z + v.Z * tri.V3.P.Z;

            // Interpolate Pu
            Vector256<float> interpPuX = v.X * tri.V1.Pu.X + v.Y * tri.V2.Pu.X + v.Z * tri.V3.Pu.X;
            Vector256<float> interpPuY = v.X * tri.V1.Pu.Y + v.Y * tri.V2.Pu.Y + v.Z * tri.V3.Pu.Y;
            Vector256<float> interpPuZ = v.X * tri.V1.Pu.Z + v.Y * tri.V2.Pu.Z + v.Z * tri.V3.Pu.Z;
            AVX2Vector3 interpPu = new AVX2Vector3(interpPuX, interpPuY, interpPuZ);
            interpPu.Normalize();

            // Interpolate Pv
            Vector256<float> interpPvX = v.X * tri.V1.Pv.X + v.Y * tri.V2.Pv.X + v.Z * tri.V3.Pv.X;
            Vector256<float> interpPvY = v.X * tri.V1.Pv.Y + v.Y * tri.V2.Pv.Y + v.Z * tri.V3.Pv.Y;
            Vector256<float> interpPvZ = v.X * tri.V1.Pv.Z + v.Y * tri.V2.Pv.Z + v.Z * tri.V3.Pv.Z;
            AVX2Vector3 interpPv = new AVX2Vector3(interpPvX, interpPvY, interpPvZ);
            interpPu.Normalize();

            // Interpolate N
            Vector256<float> interpNX = v.X * tri.V1.N.X + v.Y * tri.V2.N.X + v.Z * tri.V3.N.X;
            Vector256<float> interpNY = v.X * tri.V1.N.Y + v.Y * tri.V2.N.Y + v.Z * tri.V3.N.Y;
            Vector256<float> interpNZ = v.X * tri.V1.N.Z + v.Y * tri.V2.N.Z + v.Z * tri.V3.N.Z;
            AVX2Vector3 interpN = new AVX2Vector3(interpNX, interpNY, interpNZ);
            interpPu.Normalize();

            // Interpolate UV
            Vector256<float> interpUVx = v.X * tri.V1.UV.X + v.Y * tri.V2.UV.X + v.Z * tri.V3.UV.X;
            Vector256<float> interpUVy = v.X * tri.V1.UV.Y + v.Y * tri.V2.UV.Y + v.Z * tri.V3.UV.Y;

            return new AVX2Vertex(
                interpPx, interpPy, interpPz,
                interpPu.X, interpPu.Y, interpPu.Z,
                interpPv.X, interpPv.Y, interpPv.Z,
                interpN.X, interpN.Y, interpN.Z,
                interpUVx, interpUVy
            );
        }


        public struct AVX2Vector3
        {
            public Vector256<float> X;
            public Vector256<float> Y;
            public Vector256<float> Z;

            public AVX2Vector3(Vector256<float> x, Vector256<float> y, Vector256<float> z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public AVX2Vector3(Vector3 v)
            {
                X = Vector256.Create(v.X);
                Y = Vector256.Create(v.Y);
                Z = Vector256.Create(v.Z);
            }

            public AVX2Vector3(float x, float y, float z)
            {
                X = Vector256.Create(x);
                Y = Vector256.Create(y);
                Z = Vector256.Create(z);
            }


            public void Normalize()
            {
                Vector256<float> Length = Avx.Sqrt(Avx.Add(Avx.Multiply(X, X), Avx.Add(Avx.Multiply(Y, Y), Avx.Multiply(Z, Z))));
                X = Avx.Divide(X, Length);
                Y = Avx.Divide(Y, Length);
                Z = Avx.Divide(Z, Length);
            }
        }

        public static AVX2Vector3 CartesianToBaricentricCachedAVX2(Vector256<float> pX,
                                                                  Vector256<float> pY,
                                                                  Vector2 a,
                                                                  Vector2 v0,
                                                                  Vector2 v1,
                                                                  float invDen)
        {
            Vector256<float> ax = Vector256.Create(a.X);
            Vector256<float> ay = Vector256.Create(a.Y);

            // Compute v2 = p - a
            Vector256<float> v2X = Avx.Subtract(pX, ax);
            Vector256<float> v2Y = Avx.Subtract(pY, ay);

            // Compute v = (v2.X * v1.Y - v1.X * v2.Y) * invDen
            Vector256<float> v1YVec = Vector256.Create(v1.Y);
            Vector256<float> v1XVec = Vector256.Create(v1.X);
            Vector256<float> invDenVec = Vector256.Create(invDen);

            Vector256<float> vCross = Avx.Subtract(
                Avx.Multiply(v2X, v1YVec),
                Avx.Multiply(v1XVec, v2Y)
            );
            Vector256<float> v = Avx.Multiply(vCross, invDenVec);

            // Compute w = (v0.X * v2.Y - v2.X * v0.Y) * invDen
            Vector256<float> v0XVec = Vector256.Create(v0.X);
            Vector256<float> v0YVec = Vector256.Create(v0.Y);

            Vector256<float> wCross = Avx.Subtract(
                Avx.Multiply(v0XVec, v2Y),
                Avx.Multiply(v2X, v0YVec)
            );
            Vector256<float> w = Avx.Multiply(wCross, invDenVec);

            // Compute u = 1.0f - v - w
            Vector256<float> ones = Vector256.Create(1.0f);
            Vector256<float> u = Avx.Subtract(Avx.Subtract(ones, v), w);

            return new AVX2Vector3(u, v, w);
        }

        public struct AVX2Color
        {
            public Vector256<int> R;
            public Vector256<int> G;
            public Vector256<int> B;

            public AVX2Color(Vector256<int> r, Vector256<int> g, Vector256<int> b)
            {
                R = r;
                G = g;
                B = b;
            }

            public Color GetColor(int lane)
            {
                return Color.FromArgb(
                    R.GetElement(lane),
                    G.GetElement(lane),
                    B.GetElement(lane));
            }
        }

        public static Vector256<float> AVX2Pow(Vector256<float> a, int size)
        {
            Vector256<float> b = a;
            for (int i = 0; i < size; i++)
                b = Vector256.Multiply(b, a);

            return b;
        }
    }
}
