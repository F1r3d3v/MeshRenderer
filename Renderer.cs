using GK1_PolygonEditor;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace GK1_MeshEditor
{
    internal class Renderer : IDisposable
    {
        private Control _canvas;
        private DirectBitmap _bitmap;
        private DirectBitmap _bitmapLast;
        private Scene _scene;
        private Graphics _graphics;
        public Vector3 LightSource { get; set; }
        public IShader Shader { get; set; }

        public Renderer(Scene scene, Control canvas, DirectBitmap bitmap, DirectBitmap bitmapLast)
        {
            _scene = scene;
            _canvas = canvas;
            _bitmap = bitmap;
            _bitmapLast = bitmapLast;
            //_canvas.Resize += OnResize!;
            _graphics = Graphics.FromImage(_bitmap.Bitmap);
            _graphics.ScaleTransform(1, -1);
            _graphics.TranslateTransform(_canvas.Width / 2, -_canvas.Height / 2);
        }

        public void Clear(Color c)
        {
            _graphics.Clear(c);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (_canvas.Width == 0 && _canvas.Height == 0) return;
            lock (_bitmapLast)
            {
                _bitmap.Dispose();
                _bitmap = new DirectBitmap(_canvas.Width, _canvas.Height);
            }
            _graphics.Dispose();
            _graphics = Graphics.FromImage(_bitmap.Bitmap);
            _graphics.ScaleTransform(1, -1);
            _graphics.TranslateTransform(_canvas.Width / 2, -_canvas.Height / 2);
            //RenderScene();
        }

        public void DrawWireframe(Mesh mesh)
        {
            foreach (Triangle triangle in mesh.Triangles)
            {
                PointF[] points = {
                    new PointF(triangle.V1.P.X, triangle.V1.P.Y),
                    new PointF(triangle.V2.P.X, triangle.V2.P.Y),
                    new PointF(triangle.V3.P.X, triangle.V3.P.Y)
                };
                _graphics.DrawPolygon(Pens.Black, points);
            }
        }

        public void DrawMesh(Mesh mesh)
        {
            foreach (Triangle triangle in mesh.Triangles)
            {
                Fill(triangle);
            }
        }

        public void DrawPoint(Vector3 point, Brush brush)
        {
            int size = 10;
            point.X -= size / 2;
            point.Y -= size / 2;

            _graphics.FillEllipse(brush, point.X, point.Y, size, size);
        }

        public void DrawLine(Vector3 p1, Vector3 p2, Pen pen)
        {
            PointF point1 = new PointF(p1.X, p1.Y);
            PointF point2 = new PointF(p2.X, p2.Y);
            _graphics.DrawLine(pen, point1, point2);
        }

        private struct AETStruct
        {
            public Vector3 V1, V2;
            public int ymax;
            public float x, slope;

            public AETStruct(Vector3 v1, Vector3 v2)
            {
                V1 = v1;
                V2 = v2;
                ymax = (int)Math.Max(V1.Y, V2.Y);
                x = (v1.Y <= v2.Y) ? v1.X : v2.X;
                slope = (v1.Y != v2.Y) ? (v1.X - v2.X) / (v1.Y - v2.Y) : 0;
            }

            public override bool Equals(object? obj)
            {
                return obj is AETStruct @struct &&
                       V1.Equals(@struct.V1) &&
                       V2.Equals(@struct.V2);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(V1, V2);
            }
        }
        private void Fill(Triangle tri)
        {
            int mod(int x, int m) => (x % m + m) % m;

            List<AETStruct> AET = [];
            List<Vector3> verts = new List<Vector3>() { tri.V1.P, tri.V2.P, tri.V3.P };
            int vertCount = verts.Count;
            int[] ind = Enumerable.Range(0, vertCount).ToArray();
            Array.Sort(ind, (x, y) => verts[x].Y.CompareTo(verts[y].Y));

            int ymin = (int)verts[ind[0]].Y;
            int ymax = (int)verts[ind[vertCount - 1]].Y;

            int k = 0;

            for (int scanline = ymin; scanline <= ymax + 1; ++scanline)
            {
                while (k < vertCount && (int)Math.Round(verts[ind[k]].Y) == scanline - 1)
                {
                    int i = ind[k++];
                    Vector3 curr = verts[i];

                    Vector3 last = verts[mod(i - 1, vertCount)];
                    AETStruct s = new AETStruct(last, curr);
                    if (last.Y >= curr.Y)
                        AET.Add(s);
                    else
                        AET.Remove(s);

                    Vector3 next = verts[mod(i + 1, vertCount)];
                    s = new AETStruct(curr, next);
                    if (next.Y >= curr.Y)
                        AET.Add(s);
                    else
                        AET.Remove(s);

                }

                AET.Sort((x, y) => x.x.CompareTo(y.x));

                for (int i = 0; i < AET.Count - 1; i += 2)
                {
                    int p1 = (int)Math.Ceiling(AET[i].x);
                    int p2 = (int)Math.Floor(AET[i + 1].x);
                    int y = scanline - 1;
                    for (int j = p1; j <= p2; j++)
                    {
                        int point_x = (int)(j + _canvas.Width / 2.0f);
                        int point_y = (int)(_canvas.Height / 2.0f - y);

                        Vector2 p = new Vector2(j, y);
                        Vector2 v1 = new Vector2(verts[0].X, verts[0].Y);
                        Vector2 v2 = new Vector2(verts[1].X, verts[1].Y);
                        Vector2 v3 = new Vector2(verts[2].X, verts[2].Y);
                        Vector3 b = Util.CartesianToBaricentric(p, v1, v2, v3);

                        Vertex v = Util.InterpolateVertex(tri, b);
                        Color c = Shader.CalculateColor(v);

                        DrawPixel(point_x, point_y, c);
                    }
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    var edge = AET[i];
                    edge.x += edge.slope;
                    AET[i] = edge;
                }
            }
        }

        private void DrawPixel(int x, int y, Color color)
        {
            if (x >= 0 && y >= 0 && x < _bitmap.Width && y < _bitmap.Height)
            {
                _bitmap.SetPixel(x, y, color);
            }
        }

        //public void RenderScene() => _canvas.Invalidate();

        public void Dispose()
        {
            _graphics.Dispose();
        }
    }
}
