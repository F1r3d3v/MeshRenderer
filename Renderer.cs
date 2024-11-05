using GK1_PolygonEditor;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Renderer
    {
        private Control _canvas;
        private UnsafeBitmap _bitmap;
        private BezierSurface _bezierSurface;

        public bool RenderWireframe { get; set; } = true;
        public Texture Texture { get; set; }

        public Renderer(BezierSurface bezierSurface, Control canvas, UnsafeBitmap bitmap)
        {
            _bezierSurface = bezierSurface;
            _canvas = canvas;
            _bitmap = bitmap;
            _canvas.Paint += OnPaint!;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.ScaleTransform(1, -1);
            g.TranslateTransform(_canvas.Width / 2, -_canvas.Height / 2);

            g.Clear(Color.White);

            DrawControlPoints(g);
            if (RenderWireframe)
                DrawWireframe(g);

            e.Graphics.DrawImage(_bitmap.Bitmap, 0, 0);
        }

        public void DrawControlPoints(Graphics g)
        {
            for (int i = 0; i < _bezierSurface.ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < _bezierSurface.ControlPoints.GetLength(1); j++)
                {
                    Vector3 point = _bezierSurface.ControlPoints[i, j];

                    if (j < _bezierSurface.ControlPoints.GetLength(1) - 1)
                    {
                        Vector3 nextPointH = _bezierSurface.ControlPoints[i, j + 1];
                        DrawLine(g, point, nextPointH, Pens.Gray);
                    }

                    if (i < _bezierSurface.ControlPoints.GetLength(0) - 1)
                    {
                        Vector3 nextPointV = _bezierSurface.ControlPoints[i + 1, j];
                        DrawLine(g, point, nextPointV, Pens.Gray);
                    }

                    DrawPoint(g, point, Brushes.Red);
                }
            }
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
                g.FillPolygon(Brushes.Gray, points);
                g.DrawPolygon(Pens.Black, points);
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

        public void Render()
        {
            _canvas.Invalidate();
        }
    }
}
