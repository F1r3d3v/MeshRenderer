using System.Drawing;
using System.Drawing.Imaging;

namespace GK1_PolygonEditor
{
    internal unsafe class UnsafeBitmap : IDisposable
    {
        private Bitmap? _bitmap;
        private BitmapData? _bmpData;
        private int* _data;

        public int Width => _bitmap!.Width;
        public int Height => _bitmap!.Height;
        public Bitmap Bitmap => _bitmap!;

        public int* Data => _data;

        public UnsafeBitmap(int width, int heigth)
        {
            _bitmap = new Bitmap(width, heigth, PixelFormat.Format32bppArgb);
        }

        private UnsafeBitmap() { }

        public static UnsafeBitmap FromFile(string path)
        {
            UnsafeBitmap bitmap = new UnsafeBitmap();
            bitmap._bitmap = new Bitmap(path);
            return bitmap;
        }

        public void Begin()
        {
            Rectangle rect = new Rectangle(0, 0, _bitmap!.Width, _bitmap.Height);
            _bmpData = _bitmap.LockBits(rect, ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            _data = (int*)_bmpData.Scan0;
        }

        public void End()
        {
            _bitmap!.UnlockBits(_bmpData!);
        }

        public void Resize(int width, int height)
        {
            _bitmap!.Dispose();
            _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }

        public void Clear(Color color)
        {
            int value = color.ToArgb();
            int size = _bitmap!.Width * _bitmap.Height;
            for (int i = 0; i < size; i++)
                *(_data + i) = value;
        }

        public unsafe void SetHorizontalLine(int x1, int x2, int y, Color color)
        {
            int value = color.ToArgb();
            int length = x2 - x1;
            for (int i = 0; i <= length; i++)
                *(_data + y * _bitmap!.Width + x1 + i) = value;
        }

        public unsafe void SetPixel(int x, int y, Color color)
        {
            int value = color.ToArgb();
            *(_data + y * _bitmap!.Width + x) = value;
        }

        public unsafe Color GetPixel(int x, int y)
        {
            int pixel = *(_data + y * _bitmap!.Width + x);
            return Color.FromArgb(pixel);
        }

        public void Dispose() => _bitmap!.Dispose();
    }
}
