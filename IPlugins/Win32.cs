using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace IPlugins
{
    public class Win32
    {
        /// <summary>
        /// 设置了一个窗口的区域.只有被包含在这个区域内的地方才会被重绘,而不包含在区域内的其他区域系统将不会显示
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <param name="hRgn">指向的区域.函数起作用后将把窗体变成这个区域的形状.如果这个参数是空值,窗体区域也会被设置成空值,也就是什么也看不到.</param>
        /// <param name="bRedraw">用于设置 当函数起作用后,窗体是不是该重绘一次. true 则重绘,false 则相反,如果你的窗体是可见的,通常建议设置为 true.</param>
        /// <returns>如果函数执行成功,就返回非零的数字.如果失败,就返回零.</returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);
        /// <summary>
        /// 创建的一个带圆角的矩形区域
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x, int y, int cx, int cy, int width, int height);
        /// <summary>
        /// 返回hWnd参数所指定的窗口的设备环境获得的设备环境覆盖了整个窗口（包括非客户区），
        /// 例如标题栏、菜单、滚动条，以及边框。这使得程序能够在非客户区域实现自定义图形，
        /// 例如自定义标题或者边框。当不再需要该设备环境时，需要调用ReleaseDC函数释放设备环境。
        /// 注意，该函数只获得通用设备环境，该设备环境的任何属性改变都不会反映到窗口的私有或者类设备环境中（如果窗口有的话）!
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        /// <summary>
        /// 释放设备上下文环境（DC）供其他应用程序使用。
        /// 函数的效果与设备上下文环境类型有关。它只释放公用的和设备上下文环境，对于类或私有的则无效
        /// 函数原型：int ReleaseDC(HWND hWnd, HDC hdc)；
        /// </summary>
        /// <param name="hWnd">指向要释放的设备上下文环境所在的窗口的句柄</param>
        /// <param name="hDC">指向要释放的设备上下文环境的句柄</param>
        /// <returns>返回值说明了设备上下文环境是否释放；如果释放成功，则返回值为1；如果没有释放成功，则返回值为0</returns>
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        /// <summary>
        /// 从当前线程中的窗口释放鼠标捕获，并恢复通常的鼠标输入处理。
        /// 捕获鼠标的窗口接收所有的鼠标输入（无论光标的位置在哪里），除非点击鼠标键时，光标热点在另一个线程的窗口中。
        /// 备注：应用程序在调用函数SetCaPture之后调用此函数。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        /// <summary>
        /// 将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。
        /// 而和函数PostMessage不同，PostMessage是将一个消息寄送到一个线程的消息队列后就立即返回。
        /// LRESULT SendMessage（HWND hWnd，UINT Msg，WPARAM wParam，LPARAM IParam）
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="wMsg">指定被发送的消息。</param>
        /// <param name="wParam">指定附加的消息特定信息。</param>
        /// <param name="lParam">指定附加的消息特定信息。</param>
        /// <returns></returns>
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
        /// <summary>
        /// 坐标x,y
        /// </summary>
        public struct POINT
        {
            public int X;
            public int Y;
        }
        /// <summary>
        /// 为了控制窗口的大小，在窗口初始化时，需要用到MINMAXINFO结构体。
        ///  ptMaxSize：  设置窗口最大化时的宽度、高度
        ///  ptMaxPosition： 设置窗口最大化时x坐标、y坐标
        ///  ptMinTrackSize： 设置窗口最小宽度、高度
        ///  ptMaxTrackSize：设置窗口最大宽度、高度
        /// </summary>
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
