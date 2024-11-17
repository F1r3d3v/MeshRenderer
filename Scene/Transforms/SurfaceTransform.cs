using System.Numerics;

namespace GK1_MeshEditor
{
    internal class SurfaceTransform : MeshTransform
    {
        private BezierSurface _bezierSurface;
        public Vector3[,] OriginalControlPoints = new Vector3[4, 4];

        public SurfaceTransform(BezierSurface bezierSurface) : base(bezierSurface.Mesh)
        {
            _bezierSurface = bezierSurface;
            Array.Copy(bezierSurface.ControlPoints, OriginalControlPoints, 16);
        }

        public void RefreshSurface()
        {
            Array.Copy(_bezierSurface.ControlPoints, OriginalControlPoints, 16);
            RefreshMesh();
        }

        public override void ApplyTransformations()
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

            base.ApplyTransformations();
        }

        public void ResetSurfaceToOriginal()
        {
            Array.Copy(OriginalControlPoints, _bezierSurface.ControlPoints, 16);
            ResetMeshToOriginal();
        }
    }
}
