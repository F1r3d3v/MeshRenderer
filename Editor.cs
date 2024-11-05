using GK1_MeshEditor.CustomControls;
using GK1_PolygonEditor;

namespace GK1_MeshEditor
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            BezierSurface bezierSurface = BezierSurface.LoadFromFile("Resources/control_points.txt");
            bezierSurface.GenerateMesh(2);
            UnsafeBitmap bitmap = new UnsafeBitmap(renderCanvas.Width, renderCanvas.Height);
            Renderer renderer = new Renderer(bezierSurface, renderCanvas, bitmap);
            renderer.Render();
        }
    }
}
