using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace DevCaptureTool
{
    public class Win32
    {
        [DllImport("user32.dll")]//注册全局热键
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]//卸载全局热键
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const uint WM_HOTKEY = 0x312;
        public const uint MOD_ALT = 0x1;
        public const uint MOD_CONTROL = 0x2;
        public const uint MOD_SHIFT = 0x4;
    }
}
