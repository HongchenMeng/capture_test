using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace IPlugins
{
    public class Win32
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x, int y, int cx, int cy, int width, int height);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        public const uint WM_NCHITTEST = 0x0084;

        public const uint WM_GETMINMAXINFO = 0x24;

        public const uint HTCLIENT = 1;
        public const uint HTCAPTION = 2;
        public const uint HTSYSMENU = 3;
        public const uint HTHSCROLL = 6;
        public const uint HTVSCROLL = 7;
        public const uint HTMINBUTTON = 8;
        public const uint HTMAXBUTTON = 9;
        public const uint HTLEFT = 10;
        public const uint HTRIGHT = 11;
        public const uint HTTOP = 12;
        public const uint HTTOPLEFT = 13;
        public const uint HTTOPRIGHT = 14;
        public const uint HTBOTTOM = 15;
        public const uint HTBOTTOMLEFT = 16;
        public const uint HTBOTTOMRIGHT = 17;
        public const uint HTCLOSE = 20;

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;

        public struct POINT
        {
            public int X;
            public int Y;
        }

        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
    }
}
