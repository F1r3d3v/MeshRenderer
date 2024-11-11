using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class SurfaceTransform
    {
        private BezierSurface _bezierSurface;
        private Matrix4x4 _transform;
        public List<(Vector3 origP, Vector3 origPu, Vector3 origPv, Vector3 origN)> OriginalVertexData = [];
        public Vector3[,] OriginalControlPoints = new Vector3[4, 4];

        public SurfaceTransform()
        {
            _transform = Matrix4x4.Identity;
            _transform.M33 = 1;
            _transform.M43 = 0;
        }

        public BezierSurface BezierSurface
        {
            get { return _bezierSurface; }
            set
            {
                _bezierSurface = value;
                Array.Copy(value.ControlPoints, OriginalControlPoints, 16);
                OriginalVertexData.Clear();

                if (_bezierSurface.Mesh == null) return;
                foreach (var vertex in _bezierSurface.Mesh.Vertices)
                {
                    OriginalVertexData.Add((vertex.P, vertex.Pu, vertex.Pv, vertex.N));
                }
            }
        }

        public void Translate(float tx, float ty, float tz)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(tx, ty, tz);
            _transform = Matrix4x4.Multiply(_transform, translationMatrix);
        }

        public void Scale(float sx, float sy, float sz)
        {
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(sx, sy, sz);
            _transform = Matrix4x4.Multiply(_transform, scaleMatrix);
        }

        public void Rotate(float rx, float ry, float rz)
        {
            Matrix4x4 rotationXMatrix = Matrix4x4.CreateRotationX(rx);
            Matrix4x4 rotationYMatrix = Matrix4x4.CreateRotationY(ry);
            Matrix4x4 rotationZMatrix = Matrix4x4.CreateRotationZ(rz);

            Matrix4x4 temp = Matrix4x4.Identity;
            temp = Matrix4x4.Multiply(rotationXMatrix, rotationYMatrix);
            temp = Matrix4x4.Multiply(temp, rotationZMatrix);
            _transform = Matrix4x4.Multiply(_transform, temp);
        }

        public void ApplyTransformations()
        {
            for (int i = 0; i < _bezierSurface.ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < _bezierSurface.ControlPoints.GetLength(1); j++)
                {
                    Vector3 originalPosition = OriginalControlPoints[i, j];
                    Vector3 transformedPosition = Vector3.Transform(originalPosition, _transform);
                    _bezierSurface.ControlPoints[i, j] = transformedPosition;
                }
            }

            if (_bezierSurface.Mesh == null) return;
            for (int i = 0; i < _bezierSurface.Mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _bezierSurface.Mesh.Vertices[i];
                var (originalPosition, originalPu, originalPv, originalN) = OriginalVertexData[i];

                Vector3 transformedPosition = Vector3.Transform(originalPosition, _transform);
                Vector3 transformedPu = Vector3.TransformNormal(originalPu, _transform);
                Vector3 transformedPv = Vector3.TransformNormal(originalPv, _transform);
                Vector3 transformedN = Vector3.TransformNormal(originalN, _transform);
                
                vertex.P = transformedPosition;
                vertex.Pu = Vector3.Normalize(transformedPu);
                vertex.Pv = Vector3.Normalize(transformedPv);
                vertex.N = Vector3.Normalize(transformedN);
            }

            ResetTransform();
        }

        public void ResetTransform()
        {
            _transform = Matrix4x4.Identity;
            _transform.M33 = 1;
            _transform.M43 = 0;
        }

        public void ResetSurfaceToOriginal()
        {
            Array.Copy(OriginalControlPoints, _bezierSurface.ControlPoints, 16);
            if (_bezierSurface.Mesh == null) return;
            for (int i = 0; i < _bezierSurface.Mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _bezierSurface.Mesh.Vertices[i];
                var (originalPosition, originalPu, originalPv, originalN) = OriginalVertexData[i];
                vertex.P = originalPosition;
                vertex.Pu = originalPu;
                vertex.Pv = originalPv;
                vertex.N = originalN;
            }

            ResetTransform();
        }
    }
}
