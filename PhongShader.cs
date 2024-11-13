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
            Vector256<float> two = Vector256.Create(2.0f);
            Vector256<float> twofiftyfive = Vector256.Create(255.0f);

            // Light position as SIMD vector
            Vector256<float> lightPosX = Vector256.Create(state.LightPosition.X);
            Vector256<float> lightPosY = Vector256.Create(state.LightPosition.Y);
            Vector256<float> lightPosZ = Vector256.Create(state.LightPosition.Z);

            // Light color as SIMD vector
            Vector256<float> lightColorR = Vector256.Create(state.LightColor.R / 255.0f);
            Vector256<float> lightColorG = Vector256.Create(state.LightColor.G / 255.0f);
            Vector256<float> lightColorB = Vector256.Create(state.LightColor.B / 255.0f);

            // Surface color, default or from texture
            Vector256<float> surfaceColorR = Vector256.Create(state.SurfaceColor.R / 255.0f);
            Vector256<float> surfaceColorG = Vector256.Create(state.SurfaceColor.G / 255.0f);
            Vector256<float> surfaceColorB = Vector256.Create(state.SurfaceColor.B / 255.0f);

            // Sample texture if available
            if (state.Texture != null)
            {
                UtilAVX2.AVX2Color sampledColor = state.Texture.SampleSimd(vertex.UVx, vertex.UVy);
                surfaceColorR = Vector256.Divide(sampledColor.R.AsSingle(), twofiftyfive);
                surfaceColorG = Vector256.Divide(sampledColor.R.AsSingle(), twofiftyfive);
                surfaceColorB = Vector256.Divide(sampledColor.R.AsSingle(), twofiftyfive);
            }

            // Calculate light direction and normalize
            Vector256<float> lightDirX = Avx.Subtract(lightPosX, vertex.Px);
            Vector256<float> lightDirY = Avx.Subtract(lightPosY, vertex.Py);
            Vector256<float> lightDirZ = Avx.Subtract(lightPosZ, vertex.Pz);
            Vector256<float> lightDirLength = Avx.Sqrt(Avx.Add(Avx.Multiply(lightDirX, lightDirX), Avx.Add(Avx.Multiply(lightDirY, lightDirY), Avx.Multiply(lightDirZ, lightDirZ))));
            lightDirX = Avx.Divide(lightDirX, lightDirLength);
            lightDirY = Avx.Divide(lightDirY, lightDirLength);
            lightDirZ = Avx.Divide(lightDirZ, lightDirLength);

            // Normal vector from normal map if available
            Vector256<float> normalX = vertex.NX;
            Vector256<float> normalY = vertex.NY;
            Vector256<float> normalZ = vertex.NZ;

            //if (state.NormalMap != null)
            //{
            //    UtilAVX2.AVX2Color normalSample = state.NormalMap.SampleSimd(vertex.UVx, vertex.UVy);
            //    normalX = Avx.Subtract(Avx.Multiply(normalSample.R, two), one);
            //    normalY = Avx.Subtract(Avx.Multiply(normalSample.G, two), one);
            //    normalZ = Avx.Subtract(Avx.Multiply(normalSample.B, two), one);
            //}

            // Diffuse lighting: max(0, normal • lightDir)
            Vector256<float> dotNL = Avx.Max(Avx.Add(Avx.Multiply(normalX, lightDirX), Avx.Add(Avx.Multiply(normalY, lightDirY), Avx.Multiply(normalZ, lightDirZ))), zero);

            Vector256<float> diffuseR = Avx.Multiply(Avx.Multiply(lightColorR, surfaceColorR), dotNL);
            Vector256<float> diffuseG = Avx.Multiply(Avx.Multiply(lightColorG, surfaceColorG), dotNL);
            Vector256<float> diffuseB = Avx.Multiply(Avx.Multiply(lightColorB, surfaceColorB), dotNL);

            // Reflection vector
            Vector256<float> reflectionX = Avx.Subtract(Avx.Multiply(two, Avx.Multiply(normalX, dotNL)), lightDirX);
            Vector256<float> reflectionY = Avx.Subtract(Avx.Multiply(two, Avx.Multiply(normalY, dotNL)), lightDirY);
            Vector256<float> reflectionZ = Avx.Subtract(Avx.Multiply(two, Avx.Multiply(normalZ, dotNL)), lightDirZ);

            // View direction (assumed constant)
            Vector256<float> viewDirX = Vector256.Create(_viewDirection.X);
            Vector256<float> viewDirY = Vector256.Create(_viewDirection.Y);
            Vector256<float> viewDirZ = Vector256.Create(_viewDirection.Z);

            // Specular lighting: max(0, viewDir • reflection)^m
            Vector256<float> dotVR = Avx.Max(Avx.Add(Avx.Multiply(viewDirX, reflectionX), Avx.Add(Avx.Multiply(viewDirY, reflectionY), Avx.Multiply(viewDirZ, reflectionZ))), zero);
            Vector256<float> specularFactor = UtilAVX2.AVX2Pow(dotVR, state.CoefM);

            Vector256<float> specularR = Avx.Multiply(Avx.Multiply(lightColorR, specularFactor), Vector256.Create(state.CoefKs));
            Vector256<float> specularG = Avx.Multiply(Avx.Multiply(lightColorG, specularFactor), Vector256.Create(state.CoefKs));
            Vector256<float> specularB = Avx.Multiply(Avx.Multiply(lightColorB, specularFactor), Vector256.Create(state.CoefKs));

            // Combine diffuse and specular components, clamp to [0, 1], then scale to [0, 255]
            Vector256<byte> finalR = Avx.Multiply(Avx.Min(Avx.Add(diffuseR, specularR), one), Vector256.Create(255.0f)).AsByte();
            Vector256<byte> finalG = Avx.Multiply(Avx.Min(Avx.Add(diffuseG, specularG), one), Vector256.Create(255.0f)).AsByte();
            Vector256<byte> finalB = Avx.Multiply(Avx.Min(Avx.Add(diffuseB, specularB), one), Vector256.Create(255.0f)).AsByte();

            // Return the final color as a SimdColor struct
            return new UtilAVX2.AVX2Color(finalR, finalG, finalB);
        }
    }
}
