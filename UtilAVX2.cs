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
            Vector256<float> interpPx = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.P.X)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.P.X))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.P.X))
            );

            Vector256<float> interpPy = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.P.Y)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.P.Y))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.P.Y))
            );

            Vector256<float> interpPz = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.P.Z)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.P.Z))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.P.Z))
            );

            // Interpolate Pu
            Vector256<float> interpPuX = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pu.X)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pu.X))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pu.X))
            );

            Vector256<float> interpPuY = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pu.Y)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pu.Y))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pu.Y))
            );

            Vector256<float> interpPuZ = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pu.Z)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pu.Z))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pu.Z))
            );

            // Interpolate Pv
            Vector256<float> interpPvX = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pv.X)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pv.X))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pv.X))
            );

            Vector256<float> interpPvY = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pv.Y)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pv.Y))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pv.Y))
            );

            Vector256<float> interpPvZ = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.Pv.Z)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.Pv.Z))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pv.Z))
            );

            // Interpolate N
            Vector256<float> interpNX = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.N.X)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.N.X))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.N.X))
            );

            Vector256<float> interpNY = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.N.Y)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.N.Y))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.Pv.Y))
            );

            Vector256<float> interpNZ = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.N.Z)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.N.Z))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.N.Z))
            );

            // Interpolate UV
            Vector256<float> interpUVx = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.UV.X)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.UV.X))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.UV.X))
            );

            Vector256<float> interpUVy = Avx.Add(
                Avx.Add(
                    Avx.Multiply(v.X, Vector256.Create(tri.V1.UV.Y)),
                    Avx.Multiply(v.Y, Vector256.Create(tri.V2.UV.Y))
                ),
                Avx.Multiply(v.Z, Vector256.Create(tri.V3.UV.Y))
            );

            return new AVX2Vertex(
                interpPx, interpPy, interpPz,
                interpPuX, interpPuY, interpPuZ,
                interpPvX, interpPvY, interpPvZ,
                interpNX, interpNY, interpNZ,
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
            public Vector256<byte> R;
            public Vector256<byte> G;
            public Vector256<byte> B;

            public AVX2Color(Vector256<byte> r, Vector256<byte> g, Vector256<byte> b)
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

        public static Vector256<float> AVX2Pow(Vector256<float> b, int size)
        {
            for (int i = 0; i < size; i++)
            {
                b = Vector256.Multiply(b, b);
            }

            return b;
        }
    }
}
