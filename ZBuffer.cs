namespace GK1_MeshEditor
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
                zBufferMap[i % width, i / width] = float.MinValue;
        }

        public void Clear()
        {
            for (int i = 0; i < Width * Height; i++)
                zBufferMap[i % Width, i / Width] = float.MinValue;
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;
            zBufferMap = new float[width, height];
            for (int i = 0; i < width * height; i++)
                zBufferMap[i % width, i / width] = float.MinValue;
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
                if (x > Width - 1 || x < 0 || y < 0 || y > Height - 1) return;
                zBufferMap[x, y] = value;
            }
        }
    }
}
