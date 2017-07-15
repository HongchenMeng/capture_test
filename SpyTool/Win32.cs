using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace SpyTool
{
    public class Win32
    {
        /// <summary>
        /// 通过窗口句柄获取窗口标题.该函数将指定窗口的标题条文本（如果存在）拷贝到一个缓存区内。
        /// 如果指定的窗口是一个控件，则拷贝控件的文本。
        /// 但是，GetWindowText可能无法获取外部应用程序中控件的文本，获取自绘的控件或者是外部的密码编辑框很有可能会失败。
        /// Int GetWindowText(HWND hWnd,LPTSTR lpString,Int nMaxCount)
        /// IpString：指向接收文本的缓冲区的指针。
        /// </summary>
        /// <param name="hWnd">带文本的窗口或控件的句柄</param>
        /// <param name="byBuffer"></param>
        /// <param name="nMaxCount">指定要保存在缓冲区内的字符的最大个数，其中包含NULL字符。如果文本超过界限，它就被截断。</param>
        /// <returns>如果函数成功，返回值是拷贝的字符串的字符个数，不包括中断的空字符；如果窗口无标题栏或文本，或标题栏为空，或窗口或控制的句柄无效，则返回值为零。函数不能返回在其他应用程序中的编辑控件的文本。</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, byte[] byBuffer, int nMaxCount);

        /// <summary>
        /// 获得指定窗口所属的类的类名。
        /// int GetClassName(HWND hWnd, LPTSTR IpClassName, int nMaxCount)；
        /// IpClassName:指向接收窗口类名字符串的缓冲区的指针。
        /// nMaxCount：指定由参数lpClassName指示的缓冲区的字节数。如果类名字符串大于缓冲区的长度，则多出的部分被截断。
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类</param>
        /// <param name="byBuffer"></param>
        /// <param name="nMaxCount">如果类名字符串大于缓冲区的长度，则多出的部分被截断。</param>
        /// <returns>如果函数成功，返回值为拷贝到指定缓冲区的字符个数：如果函数失败，返回值为0。若想获得更多错误信息，请调用GetLastError函数。</returns>
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, byte[] byBuffer, int nMaxCount);

        /// <summary>
        /// 返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// </summary>
        /// <returns>函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 在窗口列表中寻找与指定条件相符的第一个子窗口 
        /// </summary>
        /// <param name="hWndParent">要查找的子窗口所在的父窗口的句柄（如果设置了hwndParent，则表示从这个hwndParent指向的父窗口中搜索子窗口）。如果hwndParent为 0 ，则函数以桌面窗口为父窗口，查找桌面窗口的所有子窗口。</param>
        /// <param name="hChildAfter">子窗口句柄。查找从在Z序中的下一个子窗口开始。子窗口必须为hwndParent窗口的直接子窗口而非后代窗口。如果HwndChildAfter为NULL，查找从hwndParent的第一个子窗口开始。如果hwndParent 和 hwndChildAfter同时为NULL，则函数查找所有的顶层窗口及消息窗口。</param>
        /// <param name="lpszClass">指向一个指定了类名的空结束字符串，或一个标识类名字符串的成员的指针。如果该参数为一个成员，则它必须为前次调用theGlobaIAddAtom函数产生的全局成员。该成员为16位，必须位于lpClassName的低16位，高位必须为0。</param>
        /// <param name="lpszWindowText">指向一个指定了窗口名（窗口标题）的空结束字符串。如果该参数为 NULL，则为所有窗口全匹配。</param>
        /// <returns>Long，找到的窗口的句柄。如未找到相符窗口，则返回零。会设置GetLastError.如果函数成功，返回值为具有指定类名和窗口名的窗口句柄。如果函数失败，返回值为NULL。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hChildAfter, string lpszClass, string lpszWindowText);

        /// <summary>
        /// 获得包含指定点的窗口的句柄。
        /// HWND WindowFromPoint（POINT Point）；
        /// WindowFromPoint函数不获取隐藏或禁止的窗口句柄，即使点在该窗口内。应用程序应该使用ChildWindowFromPoint函数进行无限制查询，这样就可以获得静态文本控件的句柄。
        /// </summary>
        /// <param name="pt">指定一个被检测的点的POINT结构。</param>
        /// <returns>返回值为包含该点的窗口的句柄。如果包含指定点的窗口不存在，返回值为NULL。如果该点在静态文本控件之上，返回值是在该静态文本控件的下面的窗口的句柄。</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT pt);

        /// <summary>
        /// 通过窗口句柄获取窗口信息
        /// </summary>
        /// <param name="hWnd">窗口的句柄</param>
        /// <param name="lpWindowInfo">指向一个接收信息的 PWINDOWINFO 结构，注意，在调用该函数之前必须设置 cbSize 成员为sizeof(WINDOWINFO)。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hWnd, ref WINDOWINFO lpWindowInfo);

        /// <summary>
        /// 获得指定窗口的有关信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值
        /// LONG GetWindowLong(HWND hWnd,int nlndex);
        /// 如果函数成功，返回值是所需的32位值；如果函数失败，返回值是0。若想获得更多错误信息请调用 GetLastError函数。
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄,它可以是窗口句柄及间接给出的窗口所属的窗口类。</param>
        /// <param name="nIndex">需要获得的相关信息的类型,指定要获得值的大于等于0的值的偏移量。有效值的范围从0到额外窗口内存空间的字节数一4例如，若指定了12位或多于12位的额外类存储空间，则应设为第三个32位整数的索引位8。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// 用于得到被定义的系统数据或者系统配置信息的一个专有名词。
        /// int WINAPI GetSystemMetrics( __in intnIndex);
        /// GetSystemMetrics函数可以获取系统分辨率，但这只是其功能之一，GetSystemMetrics函数只有一个参数，称之为「索引」
        /// 这个索引有75个标识符，通过设置不同的标识符就可以获取系统分辨率、窗体显示区域的宽度和高度、滚动条的宽度和高度。
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// 找出某个窗口的创建者（线程或进程），返回创建者的标志符。
        /// </summary>
        /// <param name="hWnd">被查找窗口的句柄.</param>
        /// <param name="lpDwProcessId">进程号的存放地址（变量地址）</param>
        /// <returns>返回线程号</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd,ref int lpDwProcessId);

        /// <summary>
        /// 该函数将指定的消息发送到一个或多个窗口。此函数为指定的窗口调用窗口程序，直到窗口程序处理完消息再返回。
        /// 而和函数PostMessage不同，PostMessage是将一个消息寄送到一个线程的消息队列后就立即返回。
        /// LRESULT SendMessage（HWND hWnd，UINT Msg，WPARAM wParam，LPARAM IParam）
        /// </summary>
        /// <param name="hWnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口。</param>
        /// <param name="uMsg">指定被发送的消息。</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns>返回值指定消息处理的结果，依赖于所发送的消息。</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 定义一个新的窗口消息，保证该消息在系统范围内是唯一的。通常调用SendMessage或者PostMessage函数时，可以使用该函数返回的消息值。
        /// UINT RegisterWindowMessage( lpString);
        /// </summary>
        /// <param name="lpString">（被注册）消息的名字</param>
        /// <returns>Long，& C000 到 & FFFF之间的一个消息编号。零意味着出错（注册消息失败）</returns>
        [DllImport("user32", EntryPoint = "RegisterWindowMessage")]
        public static extern int RegisterWindowMessage(string lpString);

        /// <summary>
        /// 检索是一个基于先前生成的对象的引用访问的对象请求的接口指针
        /// </summary>
        /// <param name="lResult">由以前的成功调用的LresultFromObject函数返回一个32位值。</param>
        /// <param name="riid">参考要检索的接口标识符。这是IID_IAccessible。</param>
        /// <param name="wParam">更多信息，提供相关的WM_GETOBJECT消息的wParam参数。</param>
        /// <param name="ppvObject">接收接口指针的地址返回给客户端。</param>
        /// <returns></returns>
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
        /// rect结构定义了一个矩形框左上角以及右下角的坐标,并绘制该矩形
        /// left ： 指定矩形框左上角的x坐标
        ///top： 指定矩形框左上角的y坐标
        ///right： 指定矩形框右下角的x坐标
        ///bottom：指定矩形框右下角的y坐标
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
        /// WINDOWINFO结构，用于保存窗口信息。GetWindowInfo API（检索有关指定窗口的信息）的输出参数。
        /// cbSize：结构的大小，以字节为单位。调用者必须设置该成员tosizeof（windowinfo）。
        /// rcWindow：窗口坐标。
        /// rcClient：客户区域的坐标。
        /// dwStyle：窗口样式。一个表的窗口风格，seeWindow Styles。
        /// dwExStyle：扩展窗口样式。一个表的扩展窗口样式，seeextended窗口样式
        /// dwWindowStatus：窗口状态。如果这件isws_activecaption（端口），窗口活动。否则，此成员为零。
        /// cxWindowBorders：窗口边框的宽度，以像素为单位。
        /// cyWindowBorders：窗口边框的高度，以像素为单位。
        /// atomWindowType：窗口类的原子（seeregisterclass）
        /// wCreatorVersion：创建窗口的应用程序的Windows版本。
        /// </summary>
        public struct WINDOWINFO
        {
            /// <summary>
            /// cbSize：结构的大小，以字节为单位。调用者必须设置该成员tosizeof（windowinfo）。
            /// </summary>
            public uint cbSize;

            /// <summary>
            /// rcWindow：窗口坐标。
            /// </summary>
            public RECT rcWindow;

            /// <summary>
            /// rcClient：客户区域的坐标
            /// </summary>
            public RECT rcClient;
            /// <summary>
            /// dwStyle：窗口样式。一个表的窗口风格，seeWindow Styles
            /// </summary>
            public uint dwStyle;
            /// <summary>
            /// dwExStyle：扩展窗口样式。一个表的扩展窗口样式，seeextended窗口样式
            /// </summary>
            public uint dwExStyle;
            /// <summary>
            /// dwWindowStatus：窗口状态。如果这件isws_activecaption（端口），窗口活动。否则，此成员为零。
            /// </summary>
            public uint dwWindowStatus;
            /// <summary>
            /// cxWindowBorders：窗口边框的宽度，以像素为单位
            /// </summary>
            public uint cxWindowBorders;
            /// <summary>
            /// cyWindowBorders：窗口边框的高度，以像素为单位。
            /// </summary>
            public uint cyWindowBorders;
            /// <summary>
            /// atomWindowType：窗口类的原子（seeregisterclass）
            /// </summary>
            public short atomWindowType;
            /// <summary>
            /// wCreatorVersion：创建窗口的应用程序的Windows版本。
            /// </summary>
            public short wCreatorVersion;
        }
        /// <summary>
        /// 得到的HTML文档
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <returns></returns>
        public static mshtml.IHTMLDocument2 GetHtmlDocument(IntPtr hwnd) {
            System.Object domObject = new System.Object();
            System.Guid guidIEDocument2 = new Guid();
            int WM_HTML_GETOBJECT = Win32.RegisterWindowMessage("WM_Html_GETOBJECT");
            int W = Win32.SendMessage(hwnd, (uint)WM_HTML_GETOBJECT, IntPtr.Zero, IntPtr.Zero);
            int lreturn = Win32.ObjectFromLresult(W, ref guidIEDocument2, 0, ref domObject);
            mshtml.IHTMLDocument2 doc = (mshtml.IHTMLDocument2)domObject;
            return doc;
        }
        /// <summary>
        /// 获取桌面矩形
        /// </summary>
        /// <returns></returns>
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
