using System.Numerics;

namespace GK1_MeshEditor
{
    internal class BezierSurface : GraphicsObject
    {
        public Vector3[,] ControlPoints { get; set; } = new Vector3[4,4];
        public Mesh? Mesh { get; set; }

        public int SurfaceDensity { get; private set; }

        public BezierSurface() { }

        public BezierSurface(Vector3[,] controlPoints)
        {
            if (controlPoints.GetLength(0) != 4 || controlPoints.GetLength(1) != 4)
                throw new InvalidDataException("Control points must be a 4x4 array.");

            ControlPoints = controlPoints;
        }

        public void GenerateMesh(int subdivisions)
        {
            if (subdivisions > 0)
            {
                SurfaceDensity = subdivisions;
                Mesh = new Mesh(ControlPoints, subdivisions);
            }
        }

        public static BezierSurface LoadFromFile(string fileName)
        {
            BezierSurface surface = new BezierSurface();
            int count = 0;
            string[]? cords;

            using var stream = new StreamReader(fileName);
            while (!stream.EndOfStream)
            {
                cords = stream.ReadLine()?.Split(' ');
                if (cords == null) break;
                if (count > 15) throw new InvalidDataException("Control points must be a 4x4 array.");

                float X = float.Parse(cords[0]);
                float Y = float.Parse(cords[1]);
                float Z = float.Parse(cords[2]);

                Vector3 p = new Vector3(X, Y, Z);
                int i = count / 4;
                int j = count % 4;

                surface.ControlPoints[j, i] = p;
                count++;
            }

            return surface;
        }

        public override void Draw(Renderer renderer)
        {
            if (Mesh != null)
            {
                //if (EditorViewModel.GetInstance().RenderWireframe)
                if (false)
                    renderer.DrawWireframe(Mesh);
                else
                    renderer.DrawMesh(Mesh);
            }

            DrawControlPoints(renderer);
        }

        private void DrawControlPoints(Renderer renderer)
        {
            Pen p = new Pen(Color.Red, 2);
            for (int i = 0; i < ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < ControlPoints.GetLength(1); j++)
                {
                    Vector3 point = ControlPoints[i, j];

                    if (j < ControlPoints.GetLength(1) - 1)
                    {
                        Vector3 nextPointH = ControlPoints[i, j + 1];
                        renderer.DrawLine(point, nextPointH, p);
                    }

                    if (i < ControlPoints.GetLength(0) - 1)
                    {
                        Vector3 nextPointV = ControlPoints[i + 1, j];
                        renderer.DrawLine(point, nextPointV, p);
                    }

                    renderer.DrawPoint(point, Brushes.Green);
                }
            }
            p.Dispose();
        }
    }
}
