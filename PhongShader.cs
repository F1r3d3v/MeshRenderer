using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

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

            int[] finalColor = new int[3];
            for (int i = 0; i < finalColor.Length; i++)
            {
                float lightColor = ((state.LightColor.ToArgb() >> 8 * i) & 0xFF) / 255.0f;
                float surfaceColor = ((color.ToArgb() >> 8 * i) & 0xFF) / 255.0f;

                Vector3 reflection = 2 * Vector3.Dot(normal, lightDirection) * normal - lightDirection;
                reflection = Vector3.Normalize(reflection);

                float cos1 = Math.Max(Vector3.Dot(normal, lightDirection), 0);
                float cos2 = Math.Max(Vector3.Dot(_viewDirection, reflection), 0);
                float intensity = Math.Min((lightColor * surfaceColor) * (state.CoefKd * cos1 + state.CoefKs * MathF.Pow(cos2, state.CoefM)), 1.0f);
                finalColor[i] = (int)(intensity * 255);
            }

            return Color.FromArgb(finalColor[2], finalColor[1], finalColor[0]);
        }
    }
}
