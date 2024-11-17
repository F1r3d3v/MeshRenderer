using System.Numerics;

namespace GK1_MeshEditor
{
    internal class MeshTransform : Transform
    {
        public List<(Vector3 origP, Vector3 origPu, Vector3 origPv, Vector3 origN)> OriginalVertexData = [];
        private Mesh _mesh;

        public MeshTransform(Mesh mesh)
        {
            _mesh = mesh;
            RefreshMesh();
        }

        public void RefreshMesh()
        {
            OriginalVertexData.Clear();
            foreach (var vertex in _mesh.Vertices)
            {
                OriginalVertexData.Add((vertex.P, vertex.Pu, vertex.Pv, vertex.N));
            }
        }

        public override void ApplyTransformations()
        {
            for (int i = 0; i < _mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _mesh.Vertices[i];
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
        }

        public void ResetMeshToOriginal()
        {
            for (int i = 0; i < _mesh.Vertices.Count; ++i)
            {
                Vertex vertex = _mesh.Vertices[i];
                var (originalPosition, originalPu, originalPv, originalN) = OriginalVertexData[i];
                vertex.P = originalPosition;
                vertex.Pu = originalPu;
                vertex.Pv = originalPv;
                vertex.N = originalN;
            }
        }
    }
}
