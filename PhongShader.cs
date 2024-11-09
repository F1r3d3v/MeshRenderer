using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class PhongShader : IShader
    {
        private Vector3 _viewDirection { get; set; } = new Vector3(0, 0, 1);
        public BezierSurface BezierSurface { get; set; }

        private EditorViewModel Model = EditorViewModel.GetInstance();

        public Color CalculateColor(Vertex vertex)
        {
            Vector3 lightDirection = Vector3.Normalize(Model.GetState().LightPosition - vertex.P);
            Color color = BezierSurface.Color;
            Vector3 normal = vertex.N;

            if (BezierSurface.Texture != null)
                color = BezierSurface.Texture.Sample(vertex.UV.X, vertex.UV.Y);

            if (BezierSurface.NormalMap != null)
            {
                Vector3 offset = BezierSurface.NormalMap.Sample(vertex.UV.X, vertex.UV.Y);
                Matrix4x4 matrix = Matrix4x4.Identity;
                Util.AssignVectorToMatrix(ref matrix, vertex.Pu, 0);
                Util.AssignVectorToMatrix(ref matrix, vertex.Pv, 1);
                Util.AssignVectorToMatrix(ref matrix, vertex.N, 2);

                normal = Vector3.Normalize(Vector3.TransformNormal(offset, matrix));
            }

            int[] finalColor = new int[3];
            for (int i = 0; i < finalColor.Length; i++)
            {
                float lightColor = ((Model.GetState().LightColor.ToArgb() >> 8 * i) & 0xFF) / 255.0f;
                float surfaceColor = ((color.ToArgb() >> 8 * i) & 0xFF) / 255.0f;

                Vector3 reflection = 2 * Vector3.Dot(normal, lightDirection) * normal - lightDirection;
                reflection = Vector3.Normalize(reflection);

                float cos1 = Math.Max(Vector3.Dot(normal, lightDirection), 0);
                float cos2 = Math.Max(Vector3.Dot(_viewDirection, reflection), 0);
                float intensity = Math.Min((lightColor * surfaceColor) * (Model.GetState().CoefKd * cos1 + Model.GetState().CoefKs * MathF.Pow(cos2, Model.GetState().CoefM)), 1.0f);
                finalColor[i] = (int)(intensity * 255);
            }

            return Color.FromArgb(finalColor[2], finalColor[1], finalColor[0]);
        }
    }
}
