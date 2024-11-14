using System.Runtime.InteropServices;

namespace GK1_PolygonEditor
{
    unsafe class Canvas : Panel
    {
        private byte* m_surfaceData;
        private int m_stride;
        private int m_width, m_height;
        private IntPtr m_hdcWindow;
        private IntPtr m_surfaceDC;
        private IntPtr m_surfaceBitmap;
        private IntPtr m_oldObject;

        public byte* Data { get => m_surfaceData; }

        public Canvas() : base()
        {
            Resize += Canvas_Resize;
        }

        protected override void WndProc(ref Message m)
        {
            // Process WM_PAINT ourself.
            if (m.Msg == 0x000F)
            {
                PAINTSTRUCT ps;
                Clear(Color.White);
                FillRectangle(Width - 100, Height - 100, 100, 100, Color.Black);

                IntPtr hdc = BeginPaint(Handle, out ps);
                BitBlt(m_hdcWindow, 0, 0, m_width, m_height, m_surfaceDC, 0, 0, RasterOperations.SRCCOPY);
                EndPaint(Handle, ref ps); base.WndProc(ref m);
            }
            // Process WM_ERASEBKGND to prevent flickering.
            else if (m.Msg == 0x0014)
                m.Result = (IntPtr)1;
            else
                base.WndProc(ref m);
        }

        private void Canvas_Resize(object? sender, EventArgs e)
        {
            m_width = Width;
            m_height = Height;
            m_stride = (32 * m_width + 31) / 32 * 4; // Calculate the stride.
            CreateSurface(m_width, m_height);
        }

        public void Render()
        {
            Invalidate(ClientRectangle); // Invalidate the only visible area.
        }
        private void CreateSurface(int width, int height)
        {
            BITMAPINFO bi = new BITMAPINFO();

            m_hdcWindow = GetDC(Handle);
            m_surfaceDC = CreateCompatibleDC(m_hdcWindow);

            bi.bmiHeader.biSize = (uint)Marshal.SizeOf<BITMAPINFOHEADER>();
            bi.bmiHeader.biWidth = width;
            bi.bmiHeader.biHeight = -height;
            bi.bmiHeader.biPlanes = 1;
            bi.bmiHeader.biBitCount = 32;
            bi.bmiHeader.biCompression = BitmapCompressionMode.BI_RGB; // No compression
            bi.bmiHeader.biSizeImage = (uint)(width * height * 4);
            bi.bmiHeader.biXPelsPerMeter = 0;
            bi.bmiHeader.biYPelsPerMeter = 0;
            bi.bmiHeader.biClrUsed = 0;
            bi.bmiHeader.biClrImportant = 0;

            IntPtr ppvBits;
            m_surfaceBitmap = CreateDIBSection(m_surfaceDC, ref bi, DIB_RGB_COLORS, out ppvBits, IntPtr.Zero, 0);

            m_surfaceData = (byte*)ppvBits.ToPointer();

            m_oldObject = SelectObject(m_surfaceDC, m_surfaceBitmap);
        }
        private void Clear(Color color)
        {
            int argb = color.ToArgb();
            for (int i = 0; i < m_stride * m_height; i += 4)
                *(int*)(m_surfaceData + i) = argb;
        }
        private void FillRectangle(int x0, int y0, int width, int height, Color color)
        {
            int argb = color.ToArgb();
            for (int y = y0; y < y0 + height; y++)
                for (int x = x0; x < x0 + width; x++)
                    SetPixel(x, y, argb);
        }

        private void SetPixel(int x, int y, int argb)
        {
            m_surfaceData[y * m_stride + 4 * x + 0] = (byte)((argb >> 0) & 0x000000FF);
            m_surfaceData[y * m_stride + 4 * x + 1] = (byte)((argb >> 8) & 0x000000FF);
            m_surfaceData[y * m_stride + 4 * x + 2] = (byte)((argb >> 16) & 0x000000FF);
            m_surfaceData[y * m_stride + 4 * x + 3] = (byte)((argb >> 24) & 0x000000FF);
        }
        private enum RasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000
        }
        private enum BitmapCompressionMode : uint
        {
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            BI_JPEG = 4,
            BI_PNG = 5
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public BitmapCompressionMode biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }
        [StructLayoutAttribute(LayoutKind.Sequential)]
        private struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            public int[] bmiColors;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public RECT rcPaint;
            public bool fRestore;
            public bool fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
        }
        private const int DIB_RGB_COLORS = 0;
        private const int DIB_PAL_COLORS = 1;

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperations dwRop);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hNewObj);

        [DllImport("user32.dll")]
        static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);
    }
}
