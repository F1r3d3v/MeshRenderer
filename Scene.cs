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
            Stopwatch sw = Stopwatch.StartNew();
            renderer.Clear(ClearColor);
            foreach (GraphicsObject obj in graphicsObjects)
            {
                obj.Draw(renderer);
            }
            sw.Stop();
            Console.WriteLine(1000.0f/sw.ElapsedMilliseconds);
        }
    }
}
