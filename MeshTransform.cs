using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class MeshTransform
    {
        private Mesh _mesh;
        private Matrix4x4 _transform;

        public Mesh Mesh
        {
            get { return _mesh; }
            set
            {
                _mesh = value;
                _originalVertexData.Clear();
                foreach (var vertex in _mesh.Vertices)
                {
                    _originalVertexData.Add((vertex.P, vertex.Pu, vertex.Pv, vertex.N));
                }
            }
        }

        private List<(Vector3 origP, Vector3 origPu, Vector3 origPv, Vector3 origN)> _originalVertexData = [];

        public MeshTransform(Mesh mesh)
        {
            _mesh = mesh;
            _transform = Matrix4x4.Identity;

            foreach (var vertex in _mesh.Vertices)
            {
                _originalVertexData.Add((vertex.P, vertex.Pu, vertex.Pv, vertex.N));
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

            temp = Matrix4x4.Multiply(temp, rotationXMatrix);
            temp = Matrix4x4.Multiply(temp, rotationYMatrix);
            temp = Matrix4x4.Multiply(temp, rotationZMatrix);
            _transform = Matrix4x4.Multiply(_transform, temp);
        }

        public void ApplyTransformations()
        {
            for (int i = 0; i < _mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _mesh.Vertices[i];
                var (originalPosition, originalPu, originalPv, originalN) = _originalVertexData[i];

                Vector3 transformedPosition = Vector3.Transform(originalPosition, _transform);
                Vector3 transformedPu = Vector3.TransformNormal(originalPu, _transform);
                Vector3 transformedPv = Vector3.TransformNormal(originalPv, _transform);
                Vector3 transformedN = Vector3.TransformNormal(originalN, _transform);

                vertex.P = transformedPosition;
                vertex.Pu = Vector3.Normalize(transformedPu);
                vertex.Pv = Vector3.Normalize(transformedPv);
                vertex.N = Vector3.Normalize(transformedN);
            }
        }

        public void ResetTransform()
        {
            _transform = Matrix4x4.Identity;
        }

        public void ResetVerticesToOriginal()
        {
            for (int i = 0; i < _mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _mesh.Vertices[i];
                var (originalPosition, originalPu, originalPv, originalN) = _originalVertexData[i];
                vertex.P = originalPosition;
                vertex.Pu = originalPu;
                vertex.Pv = originalPv;
                vertex.N = originalN;
            }

            ResetTransform();
        }
    }
}
