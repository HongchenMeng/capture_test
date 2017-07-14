using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DevCapture
{
    public class ScreenRecorder : IDisposable
    {
        private IntPtr m_hWnd;
        private Rectangle m_rect;
        private int m_nSleep;

        private bool _IsStarted;

        public bool IsStarted {
            get { return _IsStarted; }
            set { _IsStarted = value; }
        }

        private string m_strLastHash;

        public delegate void RecordErrorEventHandler(object sender, RecordErrorEventArgs e);
        public event RecordErrorEventHandler RecordError;

        private Dictionary<Image, int> _Frames = new Dictionary<Image, int>();
        /// <summary>
        /// 获取已经录制的帧以及对应延时
        /// </summary>
        [Description("已经录制的帧以及对应延时")]
        public Dictionary<Image, int> Frames {
            get { return _Frames; }
        }

        private bool _DrawCursor = true;
        /// <summary>
        /// 或者或者设置是否绘制鼠标的位置
        /// 注意：并不是录制鼠标 而是在录像中用一个光点代替鼠标
        /// </summary>
        [Description("是否绘制鼠标的位置")]
        public bool DrawCursor {
            get { return _DrawCursor; }
            set { _DrawCursor = value; }
        }

        private int _CursorWidth = 15;
        /// <summary>
        /// 获取或者设置鼠标光点的直径 当DrawCursor=true时有意义
        /// </summary>
        [Description("需要绘制的鼠标的大小 当DrawCursor=true时有意义")]
        public int CursorWidth {
            get { return _CursorWidth; }
            set {
                if (value < 10) throw new Exception("不能低于10个像素");
                if (value > 100) throw new Exception("不能高于100个像素");
                _CursorWidth = value;
            }
        }
        protected virtual void OnRecordError(RecordErrorEventArgs e) {
            if (this.RecordError != null) this.RecordError(this, e);
        }
        /// <summary>
        /// 构造函数(录制指定窗体)
        /// </summary>
        /// <param name="hWnd">需要录制的窗体句柄</param>
        /// <param name="nSleep">延时</param>
        public ScreenRecorder(IntPtr hWnd, int nSleep) {
            m_hWnd = hWnd;
            m_nSleep = nSleep;
        }
        /// <summary>
        /// 构造函数(录制指定区域)
        /// </summary>
        /// <param name="rect">需要录制的区域</param>
        /// <param name="nSleep">延时</param>
        public ScreenRecorder(Rectangle rect, int nSleep) {
            m_rect = rect;
            m_nSleep = nSleep;
        }
        /// <summary>
        /// 开始录制
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Start() {
            if (this._IsStarted) return false;
            this._IsStarted = true;
            int nSleep = m_nSleep;
            Win32.RECT rect = new Win32.RECT();
            new Thread(() => {
                while (_IsStarted) {
                    if (m_hWnd != IntPtr.Zero) {    //如果指定了句柄 录制该窗体区域
                        if (Win32.GetWindowRect(m_hWnd, ref rect)) m_rect = rect.ToRectangle();
                        else {
                            _IsStarted = false;
                            lock (this._Frames) {
                                if (this._Frames.Count == 0)
                                    this.OnRecordError(new RecordErrorEventArgs("[" + m_hWnd.ToString("X8") + "]" + new Win32Exception(Marshal.GetLastWin32Error()).Message));
                            }
                            break;
                        }
                    }
                    Image img = this.GetScreen(m_rect);
                    string strHash = this.GetImageHash(img);
                    if (strHash == m_strLastHash) {
                        nSleep += m_nSleep;         //如果和上一帧数据重复 则不添加帧 多添加一轮延时
                    } else {
                        if (_IsStarted) lock (this._Frames) this._Frames.Add(img, nSleep);
                        nSleep = m_nSleep;
                        m_strLastHash = strHash;
                    }
                    Thread.Sleep(m_nSleep);
                }
            }) { IsBackground = true }.Start();
            return true;
        }
        /// <summary>
        /// 暂停录制
        /// </summary>
        public void Stop() {
            this._IsStarted = false;
        }
        /// <summary>
        /// 情况当前已经录制的帧
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Clear() {
            if (this._IsStarted) return false;
            lock (this._Frames) this._Frames.Clear();
            m_strLastHash = string.Empty;
            GC.Collect();
            return true;
        }

        private Image GetScreen(Rectangle rect) {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.CopyFromScreen(rect.Left, rect.Y, 0, 0, rect.Size);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (this._DrawCursor) {
                    Point pt = System.Windows.Forms.Control.MousePosition;
                    pt.X -= rect.X + this._CursorWidth / 2;
                    pt.Y -= rect.Y + this._CursorWidth / 2;
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0))) {
                        g.FillEllipse(sb, pt.X, pt.Y, this._CursorWidth, this._CursorWidth);
                        sb.Color = Color.FromArgb(125, 255, 255, 255);
                        g.FillEllipse(sb, pt.X + 2, pt.Y + 2, this._CursorWidth - 4, this._CursorWidth - 4);
                    }
                }
            }
            return bmp;
        }

        private string GetImageHash(Image img) {
            using (MemoryStream ms = new MemoryStream()) {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byImg = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(byImg, 0, byImg.Length);
                return BitConverter.ToString(System.Security.Cryptography.MD5.Create().ComputeHash(byImg)).Replace("-", "");
            }
        }

        public void Dispose() {
            this.Clear();
        }
    }

    public class RecordErrorEventArgs : EventArgs
    {
        private string _ErrorMessage;

        public string ErrorMessage {
            get { return _ErrorMessage; }
        }

        public RecordErrorEventArgs(string strErrorMessage) {
            this._ErrorMessage = strErrorMessage;
        }
    }
}
