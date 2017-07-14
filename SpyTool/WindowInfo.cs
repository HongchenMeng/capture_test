using System;
using System.Collections.Generic;
using System.Text;

namespace SpyTool
{
    /// <summary>
    /// 窗口信息
    /// </summary>
    public class WindowInfo
    {

        private IntPtr _Hwnd;
        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr Hwnd {
            get { return _Hwnd; }
            set { _Hwnd = value; }
        }

        private string _WindowText;
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string WindowText {
            get { return _WindowText; }
            set { _WindowText = value; }
        }

        private string _ClassName;
        /// <summary>
        /// 窗口类型名
        /// </summary>
        public string ClassName {
            get { return _ClassName; }
            set { _ClassName = value; }
        }

        private Win32.WINDOWINFO _WndInfo;

        public Win32.WINDOWINFO WndInfo {
            get { return _WndInfo; }
            set { _WndInfo = value; }
        }

        private mshtml.IHTMLDocument2 _Document;

        public mshtml.IHTMLDocument2 Document {
            get { return _Document; }
            set { _Document = value; }
        }

        private System.Diagnostics.Process _Process;

        public System.Diagnostics.Process Process {
            get { return _Process; }
            set { _Process = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        public WindowInfo(IntPtr hWnd) {
            this._Hwnd = hWnd;
        }
    }
}
