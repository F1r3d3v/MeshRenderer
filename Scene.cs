using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Scene
    {
        public List<GraphicsObject> graphicsObjects { get; private set; } = [];
        public Color ClearColor { get; set; } = Color.White;
        public void Render(Renderer renderer)
        {
            renderer.Clear(ClearColor);
            foreach (GraphicsObject obj in graphicsObjects)
            {
                renderer.DrawObject(obj);
            }
        }
    }
}
