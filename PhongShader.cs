using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;

namespace GK1_MeshEditor
{
    internal class PhongShader : IShader
    {
        private Vector3 _viewDirection { get; set; } = new Vector3(0, 0, 1);

        public Color CalculateColor(Vertex vertex)
        {
            var state = EditorViewModel.GetInstance().GetState();
            if (vertex.P.CloseTo(state.LightPosition, 1e-5)) return state.SurfaceColor;

            Vector3 lightDirection = Vector3.Normalize(state.LightPosition - vertex.P);
            Color color = state.SurfaceColor;
            Vector3 normal = vertex.N;

            if (state.Texture != null)
                color = state.Texture.Sample(vertex.UV.X, vertex.UV.Y);

            if (state.NormalMap != null)
            {
                Vector3 offset = state.NormalMap.Sample(vertex.UV.X, vertex.UV.Y);
                Matrix4x4 matrix = Matrix4x4.Identity;
                Util.AssignVectorToMatrix(ref matrix, vertex.Pu, 0);
                Util.AssignVectorToMatrix(ref matrix, vertex.Pv, 1);
                Util.AssignVectorToMatrix(ref matrix, vertex.N, 2);

                normal = Vector3.Normalize(Vector3.TransformNormal(offset, Matrix4x4.Transpose(matrix)));
            }

            Vector3 lightColor = new Vector3(state.LightColor.R, state.LightColor.G, state.LightColor.B) / 255.0f;
            Vector3 surfaceColor = new Vector3(color.R, color.G, color.B) / 255.0f;
            Vector3 reflection = 2 * Vector3.Dot(normal, lightDirection) * normal - lightDirection;
            reflection = Vector3.Normalize(reflection);

            float cos1 = Math.Max(Vector3.Dot(normal, lightDirection), 0);
            float cos2 = Math.Max(Vector3.Dot(_viewDirection, reflection), 0);
            Vector3 intensity = (lightColor * surfaceColor) * (state.CoefKd * cos1 + state.CoefKs * MathF.Pow(cos2, state.CoefM));

            Vector3 finalColor = Vector3.Min(intensity, Vector3.One) * 255;
            return Color.FromArgb((int)finalColor.X, (int)finalColor.Y, (int)finalColor.Z);
        }

        public UtilAVX2.AVX2Color CalculateColorSimd(UtilAVX2.AVX2Vertex vertex)
        {
            var state = EditorViewModel.GetInstance().GetState();

            Vector256<float> one = Vector256.Create(1.0f);
            Vector256<float> zero = Vector256.Create(0.0f);

            Vector256<float> lightPosX = Vector256.Create(state.LightPosition.X);
            Vector256<float> lightPosY = Vector256.Create(state.LightPosition.Y);
            Vector256<float> lightPosZ = Vector256.Create(state.LightPosition.Z);

            Vector256<float> lightColorR = Vector256.Create(state.LightColor.R / 255.0f);
            Vector256<float> lightColorG = Vector256.Create(state.LightColor.G / 255.0f);
            Vector256<float> lightColorB = Vector256.Create(state.LightColor.B / 255.0f);

            Vector256<float> surfaceColorR = Vector256.Create(state.SurfaceColor.R / 255.0f);
            Vector256<float> surfaceColorG = Vector256.Create(state.SurfaceColor.G / 255.0f);
            Vector256<float> surfaceColorB = Vector256.Create(state.SurfaceColor.B / 255.0f);

            UtilAVX2.AVX2Vector3 viewDir = new UtilAVX2.AVX2Vector3(_viewDirection);

            if (state.Texture != null)
            {
                UtilAVX2.AVX2Color sampledColor = state.Texture.SampleSimd(vertex.UVx, vertex.UVy);
                surfaceColorR = Avx2.ConvertToVector256Single(sampledColor.R) / 255.0f;
                surfaceColorG = Avx2.ConvertToVector256Single(sampledColor.G) / 255.0f;
                surfaceColorB = Avx2.ConvertToVector256Single(sampledColor.B) / 255.0f;
            }

            Vector256<float> lightDirX = lightPosX - vertex.Px;
            Vector256<float> lightDirY = lightPosY - vertex.Py;
            Vector256<float> lightDirZ = lightPosZ - vertex.Pz;
            UtilAVX2.AVX2Vector3 lightDir = new UtilAVX2.AVX2Vector3(lightDirX, lightDirY, lightDirZ);
            lightDir.Normalize();

            UtilAVX2.AVX2Vector3 normal = new UtilAVX2.AVX2Vector3(vertex.NX, vertex.NY, vertex.NZ);

            //if (state.NormalMap != null)
            //{
            //    UtilAVX2.AVX2Color normalSample = state.NormalMap.SampleSimd(vertex.UVx, vertex.UVy);
            //    normalX = Avx.Subtract(Avx.Multiply(normalSample.R, two), one);
            //    normalY = Avx.Subtract(Avx.Multiply(normalSample.G, two), one);
            //    normalZ = Avx.Subtract(Avx.Multiply(normalSample.B, two), one);
            //}

            Vector256<float> dotNL = Vector256.Max(normal.X * lightDir.X + normal.Y * lightDir.Y + normal.Z * lightDir.Z, zero);

            Vector256<float> reflectionX = 2 * normal.X * dotNL - lightDir.X;
            Vector256<float> reflectionY = 2 * normal.Y * dotNL - lightDir.Y;
            Vector256<float> reflectionZ = 2 * normal.Z * dotNL - lightDir.Z;
            UtilAVX2.AVX2Vector3 reflection = new UtilAVX2.AVX2Vector3(reflectionX, reflectionY, reflectionZ);
            reflection.Normalize();

            Vector256<float> dotVR = Vector256.Max(viewDir.X * reflection.X + viewDir.Y * reflection.Y + viewDir.Z * reflection.Z, zero);
            Vector256<float> dotVRPower = UtilAVX2.AVX2Pow(dotVR, state.CoefM);
            

            Vector256<float> colorFactor = (state.CoefKd * dotNL + state.CoefKs * dotVRPower);
            Vector256<float> colorR = Vector256.Min((lightColorR * surfaceColorR) * colorFactor, one);
            Vector256<float> colorG = Vector256.Min((lightColorG * surfaceColorG) * colorFactor, one);
            Vector256<float> colorB = Vector256.Min((lightColorB * surfaceColorB) * colorFactor, one);

            Vector256<int> finalR = Avx2.ConvertToVector256Int32(colorR * 255.0f);
            Vector256<int> finalG = Avx2.ConvertToVector256Int32(colorG * 255.0f);
            Vector256<int> finalB = Avx2.ConvertToVector256Int32(colorB * 255.0f);

            return new UtilAVX2.AVX2Color(finalR, finalG, finalB);
        }
    }
}
