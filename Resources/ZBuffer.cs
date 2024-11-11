using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor.Resources
{
    internal class ZBuffer
    {
        float[,] zBufferMap;
        int Width;
        int Height;

        public ZBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            zBufferMap = new float[width, height];
            for (int i = 0; i < width * height; i++)
                zBufferMap[i % width, i / width] = float.MaxValue;
        }

        public void Clear()
        {
            for (int i = 0; i < Width * Height; i++)
                zBufferMap[i % Width, i / Width] = float.MaxValue;
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            zBufferMap = new float[width, height];
            for (int i = 0; i < width * height; i++)
                zBufferMap[i % width, i / width] = float.MaxValue;
        }

        public float this[int x, int y]
        {
            get
            {
                if (x > Width - 1 || x < 0 || y < 0 || y > Height - 1) return float.MinValue;
                return zBufferMap[x, y];
            }
            set
            {
                zBufferMap[x, y] = value;
            }
        }
    }
}
