using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace DevCaptureTool
{
    public class Win32
    {
        /// <summary>
        /// 注册全局热键.定义一个系统范围的热键。
        /// 函数原型：BOOL RegisterHotKey（HWND hWnd，int id，UINT fsModifiers，UINT vk）；
        /// </summary>
        /// <param name="hWnd">接收热键产生WM_HOTKEY消息的窗口句柄。若该参数NULL，传递给调用线程的WM_HOTKEY消息必须在消息循环中进行处理。</param>
        /// <param name="id">定义热键的标识符。调用线程中的其他热键，不能使用同样的标识符。应用程序必须定义一个0X0000-0xBFFF范围的值。一个共享的动态链接库（DLL）必须定义一个范围为0xC000-0xFFFF的值(GlobalAddAtom函数返回该范围）。为了避免与其他动态链接库定义的热键冲突，一个DLL必须使用GlobalAddAtom函数获得热键的标识符。</param>
        /// <param name="fsModifiers">定义为了产生WM_HOTKEY消息而必须与由nVirtKey参数定义的键一起按下的键。</param>
        /// <param name="vk">定义热键的虚拟键码。</param>
        /// <returns>若函数调用成功，返回一个非0值。若函数调用失败，则返回值为0</returns>
        [DllImport("user32.dll")]//注册全局热键
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        /// <summary>
        /// 卸载全局热键.释放调用线程先前登记的热键
        /// </summary>
        /// <param name="hWnd">与被释放的热键相关的窗口句柄。若热键不与窗口相关，则该参数为NULL。</param>
        /// <param name="id">定义被释放的热键的标识符。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]//卸载全局热键
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const uint WM_HOTKEY = 0x312;
        public const uint MOD_ALT = 0x1;
        public const uint MOD_CONTROL = 0x2;
        public const uint MOD_SHIFT = 0x4;
    }
}
