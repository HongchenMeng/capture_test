using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace DevCaptureTool
{
    public class KeyHook
    {
        #region Win32

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc hProc, IntPtr hMod, int dwThreadId);
        [DllImport("user32.dll")]
        public static extern int CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("kernel32.dll")]//获取模块句柄  
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public struct KeyInfoStruct
        {
            public int vkCode;        //按键键码
            public int scanCode;
            public int flags;       //键盘是否按下的标志
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;      //钩子类型 全局钩子
        private const int WM_KEYUP = 0x101;     //按键抬起
        private const int WM_KEYDOWN = 0x100;       //按键按下

        #endregion

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void KeyHookHanlder(object sender, KeyHookEventArgs e);
        public event KeyHookHanlder KeyHookEvent;

        private IntPtr m_hHook;
        private GCHandle gc;

        protected virtual void OnKeyHookEvent(KeyHookEventArgs e) {
            if (KeyHookEvent != null) this.KeyHookEvent(this, e);
        }

        private int KeyHookProcedure(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && KeyHookEvent != null) {
                KeyInfoStruct inputInfo = (KeyInfoStruct)Marshal.PtrToStructure(lParam, typeof(KeyInfoStruct));
                if (wParam == (IntPtr)WM_KEYDOWN) {//如果按键按下
                    this.OnKeyHookEvent(new KeyHookEventArgs(inputInfo.vkCode));
                }
            }
            return CallNextHookEx(m_hHook, nCode, wParam, lParam);//继续传递消息
        }

        public IntPtr SetHook() {
            if (m_hHook == IntPtr.Zero) {
                HookProc keyCallBack = new HookProc(KeyHookProcedure);
                m_hHook = SetWindowsHookEx(
                    WH_KEYBOARD_LL, keyCallBack,
                    GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                gc = GCHandle.Alloc(keyCallBack);               //反正被回收期回收
            }
            return m_hHook;
        }

        public IntPtr UnLoadHook() {
            if (m_hHook != IntPtr.Zero) {
                if (UnhookWindowsHookEx(m_hHook))
                    m_hHook = IntPtr.Zero;
            }
            return m_hHook;
        }
    }

    public class KeyHookEventArgs : EventArgs
    {
        private int keyCode;
        public int KeyCode {
            get { return keyCode; }
        }

        public KeyHookEventArgs(int code) {
            this.keyCode = code;
        }
    }
}
