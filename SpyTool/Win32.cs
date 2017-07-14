using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace SpyTool
{
    public class Win32
    {
        /// <summary>
        /// 通过窗口句柄获取窗口标题
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="byBuffer"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, byte[] byBuffer, int nMaxCount);

        /// <summary>
        /// 通过窗口句柄获取窗口类型
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="byBuffer"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, byte[] byBuffer, int nMaxCount);

        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndParent">父窗口句柄</param>
        /// <param name="hChildAfter">子窗口句柄</param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindowText"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hChildAfter, string lpszClass, string lpszWindowText);

        /// <summary>
        /// 窗口坐标
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT pt);
        
        /// <summary>
        /// 通过窗口句柄获取窗口信息
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpWindowInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hWnd, ref WINDOWINFO lpWindowInfo);
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd,ref int lpDwProcessId);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32", EntryPoint = "RegisterWindowMessage")]
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
        public const long WS_VISIBLE = 0x10000000;
        public const long WS_EX_TRANSPARENT = 0x20;
        public const long GWL_HWNDPARENT = -8;

        public const uint WM_GETICON = 0x7F;
        public const int ICON_SMALL = 0;

        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;

        /// <summary>
        /// 坐标结构，X,Y
        /// </summary>
        public struct POINT {
            public int X;
            public int Y;
        }

        /// <summary>
        /// 矩形结构，左，上，右，底
        /// 初始化矩形
        /// </summary>
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public System.Drawing.Rectangle ToRectangle() {
                return new System.Drawing.Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
        }
        /// <summary>
        /// 窗口信息结构：
        /// 
        /// </summary>
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public short atomWindowType;
            public short wCreatorVersion;
        }

        public static mshtml.IHTMLDocument2 GetHtmlDocument(IntPtr hwnd) {
            System.Object domObject = new System.Object();
            System.Guid guidIEDocument2 = new Guid();
            int WM_HTML_GETOBJECT = Win32.RegisterWindowMessage("WM_Html_GETOBJECT");
            int W = Win32.SendMessage(hwnd, (uint)WM_HTML_GETOBJECT, IntPtr.Zero, IntPtr.Zero);
            int lreturn = Win32.ObjectFromLresult(W, ref guidIEDocument2, 0, ref domObject);
            mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)domObject;
            return doc;
        }

        public static System.Drawing.Rectangle GetDesktopRect() {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            rect.X = Win32.GetSystemMetrics(Win32.SM_XVIRTUALSCREEN);
            rect.Y = Win32.GetSystemMetrics(Win32.SM_YVIRTUALSCREEN);
            rect.Width = Win32.GetSystemMetrics(Win32.SM_CXVIRTUALSCREEN);
            rect.Height = Win32.GetSystemMetrics(Win32.SM_CYVIRTUALSCREEN);
            return rect;
        }
    }
}
