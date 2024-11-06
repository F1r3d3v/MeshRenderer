using GK1_PolygonEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace GK1_MeshEditor
{
    internal class Renderer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private Control _canvas;
        private UnsafeBitmap _bitmap;
        private BezierSurface _bezierSurface;

        private bool _renderWireframe = true;
        public bool RenderWireframe
        {
            get => _renderWireframe;
            set
            {
                if (!SetField(ref _renderWireframe, value)) return;
                Render();
            }
        }

        public Renderer(BezierSurface bezierSurface, Control canvas, UnsafeBitmap bitmap)
        {
            _bezierSurface = bezierSurface;
            _canvas = canvas;
            _bitmap = bitmap;
            _canvas.Paint += OnPaint!;
            _canvas.Resize += OnResize!;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.ScaleTransform(1, -1);
            g.TranslateTransform(_canvas.Width / 2, -_canvas.Height / 2);

            g.Clear(Color.White);

            if (RenderWireframe)
                DrawWireframe(g);
            else
                DrawMesh(g);

            DrawControlPoints(g);

            e.Graphics.DrawImage(_bitmap.Bitmap, 0, 0);
        }

        private void OnResize(object sender, EventArgs e)
        {
            if (_canvas.Width == 0 && _canvas.Height == 0) return;
            _bitmap.Resize(_canvas.Width, _canvas.Height);
            Render();
        }

        public void DrawControlPoints(Graphics g)
        {
            Pen p = new Pen(Color.Red, 1);
            for (int i = 0; i < _bezierSurface.ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < _bezierSurface.ControlPoints.GetLength(1); j++)
                {
                    Vector3 point = _bezierSurface.ControlPoints[i, j];

                    if (j < _bezierSurface.ControlPoints.GetLength(1) - 1)
                    {
                        Vector3 nextPointH = _bezierSurface.ControlPoints[i, j + 1];
                        DrawLine(g, point, nextPointH, p);
                    }

                    if (i < _bezierSurface.ControlPoints.GetLength(0) - 1)
                    {
                        Vector3 nextPointV = _bezierSurface.ControlPoints[i + 1, j];
                        DrawLine(g, point, nextPointV, p);
                    }

                    DrawPoint(g, point, Brushes.Green);
                }
            }
            p.Dispose();
        }

        public void DrawWireframe(Graphics g)
        {
            if (_bezierSurface.Mesh == null) return;
            foreach (Triangle triangle in _bezierSurface.Mesh.Triangles)
            {
                Point[] points = {
                    new Point((int)triangle.V1.P.X, (int)triangle.V1.P.Y),
                    new Point((int)triangle.V2.P.X, (int)triangle.V2.P.Y),
                    new Point((int)triangle.V3.P.X, (int)triangle.V3.P.Y)
                };
                g.DrawPolygon(Pens.Black, points);
            }
        }

        public void DrawMesh(Graphics g)
        {
            if (_bezierSurface.Mesh == null) return;
            foreach (Triangle triangle in _bezierSurface.Mesh.Triangles)
            {
                Fill(new List<Vector3>() { triangle.V1.P, triangle.V2.P, triangle.V3.P }, Color.Gray, g);
            }
        }

        private void DrawPoint(Graphics g, Vector3 point, Brush brush)
        {
            int size = 10;
            int x = (int)point.X - size / 2;
            int y = (int)point.Y - size / 2;

            g.FillEllipse(brush, x, y, size, size);
        }

        private void DrawLine(Graphics g, Vector3 p1, Vector3 p2, Pen pen)
        {
            g.DrawLine(pen, new PointF(p1.X, p1.Y), new PointF(p2.X, p2.Y));
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
        private void Fill(List<Vector3> verts, Color color, Graphics g)
        {
            int mod(int x, int m) => (x % m + m) % m;

            List<AETStruct> AET = [];
            Pen pen = new Pen(color, 1);

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
                    Point start = new Point((int)(AET[i].x), scanline - 1);
                    Point end = new Point((int)(AET[i + 1].x), scanline - 1);
                    g.DrawLine(pen, start, end);
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    var edge = AET[i];
                    edge.x += edge.slope;
                    AET[i] = edge;
                }
            }

            pen.Dispose();
        }

        public void Render()
        {
            _canvas.Invalidate();
        }
    }
}
