using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevCapture
{
    public class Win32
    {
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, byte[] byBuffer, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, byte[] byBuffer, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hChildAfter, string lpszClass, string lpszWindowText);
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out PCURSORINFO pci);
        [DllImport("user32.dll", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);

        [DllImport("OLEACC.DLL", EntryPoint = "ObjectFromLresult")]
        public static extern int ObjectFromLresult(
            int lResult,
            ref System.Guid riid,
            int wParam,
            [MarshalAs(UnmanagedType.Interface), System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out]ref System.Object ppvObject
        );

        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const long WS_POPUP = 0x80000000;
        public const long WS_VISIBLE = 0x10000000;
        public const long WS_EX_TRANSPARENT = 0x20;

        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;

        public struct LPPOINT
        {
            public int X;
            public int Y;
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public System.Drawing.Rectangle ToRectangle() {
                return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
            }

            public bool Contains(int x, int y) {
                return (x >= Left && x < Right) && (y >= Top && y < Bottom);
            }
        }

        public struct PCURSORINFO
        {
            public int cbSize;
            public int flag;
            public IntPtr hCursor;
            public LPPOINT ptScreenPos;
        }

        private static byte[] m_byBuffer = new byte[256];
        /// <summary>
        /// 根据鼠标位置获取窗口句柄
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="hWndExcept">需要忽略的窗口句柄 通常该值为调用者窗口句柄</param>
        /// <param name="bVisbleOnly">是否只获取可见窗口</param>
        /// <param name="bTransparent">是否获取透明窗口</param>
        /// <returns></returns>
        public static IntPtr GetWindowFromPoint(int x, int y, IntPtr hWndExcept, bool bVisbleOnly, bool bTransparent) {
            IntPtr hParent = IntPtr.Zero;
            IntPtr hAfter = IntPtr.Zero;
            RECT rect = new RECT();
            while ((hAfter = FindWindowEx(hParent, hAfter, null, null)) != IntPtr.Zero) {
                //如果获取出来的窗口为需要忽略的窗口 跳过
                if (hAfter == hWndExcept) continue;
                //如果只需要获取可见窗口 而获取出来的为不可见窗口 跳过
                if (bVisbleOnly && (Win32.GetWindowLong(hAfter, GWL_STYLE) & WS_VISIBLE) == 0) continue;
                //如果不获取透明窗口 而获取出来的窗口为透明 跳过
                if (!bTransparent && (Win32.GetWindowLong(hAfter, GWL_EXSTYLE) & WS_EX_TRANSPARENT) != 0) continue;
                //这个类的窗体可以忽略掉 一般这种窗体都是隐藏的 比如开始菜单这些 但是确实有visible属性
                string strClassName = Win32.GetClassName(hAfter);
                if (strClassName == "Windows.UI.Core.CoreWindow") {
                    if ((GetWindowLong(hAfter, GWL_STYLE) & WS_POPUP) != 0) continue;
                    continue;
                }
                GetWindowRect(hAfter, ref rect);
                if (rect.Contains(x, y)) {
                    hParent = hAfter;
                    hAfter = IntPtr.Zero;
                }
            }
            return hParent;
        }

        public static string GetWindowText(IntPtr hWnd) {
            int len = Win32.GetWindowText(hWnd, m_byBuffer, m_byBuffer.Length);
            return Encoding.Default.GetString(m_byBuffer, 0, len);
        }

        public static string GetClassName(IntPtr hWnd) {
            int len = Win32.GetClassName(hWnd, m_byBuffer, m_byBuffer.Length);
            return Encoding.Default.GetString(m_byBuffer, 0, len);
        }
        /// <summary>
        /// 获取整个屏幕的矩形区域
        /// </summary>
        /// <returns>矩形区域</returns>
        public static Rectangle GetDesktopRect() {
            Rectangle rect = new Rectangle();
            rect.X = Win32.GetSystemMetrics(Win32.SM_XVIRTUALSCREEN);
            rect.Y = Win32.GetSystemMetrics(Win32.SM_YVIRTUALSCREEN);
            rect.Width = Win32.GetSystemMetrics(Win32.SM_CXVIRTUALSCREEN);
            rect.Height = Win32.GetSystemMetrics(Win32.SM_CYVIRTUALSCREEN);
            return rect;
        }
        /// <summary>
        /// 根据句柄获取webbrowser控件
        /// </summary>
        /// <param name="hwnd">webbrowser控件句柄</param>
        /// <returns>Document对象</returns>
        public static mshtml.IHTMLDocument2 GetHtmlDocument(IntPtr hwnd) {
            System.Object domObject = new System.Object();
            System.Guid guidIEDocument2 = new Guid();
            int WM_HTML_GETOBJECT = Win32.RegisterWindowMessage("WM_Html_GETOBJECT");
            int W = Win32.SendMessage(hwnd, (uint)WM_HTML_GETOBJECT, IntPtr.Zero, IntPtr.Zero);
            int lreturn = Win32.ObjectFromLresult(W, ref guidIEDocument2, 0, ref domObject);
            mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)domObject;
            return doc;
        }

        //在桌面绘制鼠标
        public static Rectangle DrawCurToScreen(Point ptMousePosition) {
            //如果直接将捕获当的鼠标画在bmp上 光标不会反色 指针边框也很浓 也就是说
            //尽管bmp上绘制了图像 绘制鼠标的时候还是以黑色作为鼠标的背景 然后在将混合好的鼠标绘制到图像 会很别扭
            //所以 干脆直接在桌面把鼠标绘制出来再截取桌面
            Rectangle rect = Win32.GetDesktopRect();
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {   //传入0默认就是桌面 Win32.GetDesktopWindow()也可以
                Win32.PCURSORINFO pci;
                pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Win32.PCURSORINFO));
                Win32.GetCursorInfo(out pci);
                if (pci.hCursor != IntPtr.Zero) {
                    Cursor cur = new Cursor(pci.hCursor);
                    Rectangle rect_cur = new Rectangle((Point)((Size)ptMousePosition - (Size)cur.HotSpot), cur.Size);
                    g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size);
                    //g.CopyFromScreen(rect_cur.Location, rect_cur.Location, rect_cur.Size); //在桌面绘制鼠标前 先在桌面绘制一下当前的桌面图像
                    //如果不绘制当前桌面 那么cur.Draw的时候会是用历史桌面的快照 进行鼠标的混合 那么到时候混出现底色(测试中就是这样的)
                    cur.Draw(g, rect_cur);
                    return rect_cur;
                }
                return Rectangle.Empty;
            }
        }
    }
}
