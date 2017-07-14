using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using DevCapture;
using System.IO;
using Microsoft.Win32;
using IPlugins;
using System.Threading;

namespace DevCaptureTool
{
    public partial class FrmMain : IPlugins.FrmBase
    {
        public FrmMain() {
            InitializeComponent();
            this.Sizeable = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += (s, e) => { e.Cancel = true; if (!m_bInToggle) this.ToggleWindow(); };
            m_info = new Info[]{
                new Info(HOTKEY_NORMAL_ID,0,0,sbox_ctrl_normal,sbox_alt_normal,sbox_shift_normal,tbx_normal,"热键[普通截图]注册失败"),
                new Info(HOTKEY_CLIPBOARD_ID,0,0,sbox_ctrl_clipboard,sbox_alt_clipboard,sbox_shift_clipboard,tbx_clipboard,"热键[从剪切板截图]注册失败"),
                new Info(HOTKEY_GIFTOGGLE_ID,0,0,sbox_ctrl_giftoggle,sbox_alt_giftoggle,sbox_shift_giftoggle,tbx_giftoggle,"热键[Fif录制(开始/暂停)]注册失败"),
                new Info(HOTKEY_GIFSTOP_ID,0,0,sbox_ctrl_gifstop,sbox_alt_gifstop,sbox_shift_gifstop,tbx_gifstop,"热键[Fif录制(结束录制)]注册失败")
            };
        }

        public class Info
        {
            public int ID;
            public uint CtrlKey;
            public uint AuxKey;
            public SwitchBox sbox_Ctrl;
            public SwitchBox sbox_Alt;
            public SwitchBox sbox_Shift;
            public TextBoxCu Tbx_Aux;
            public string ErrorInfo;
            public Info(int id, uint nCtrlKey, uint nAuxKey, SwitchBox ckboxCtrl, SwitchBox ckboxAlt, SwitchBox ckboxShift, TextBoxCu tbxAux, string strError) {
                this.ID = id;
                this.CtrlKey = nCtrlKey;
                this.AuxKey = nAuxKey;
                this.sbox_Ctrl = ckboxCtrl;
                this.sbox_Alt = ckboxAlt;
                this.sbox_Shift = ckboxShift;
                this.Tbx_Aux = tbxAux;
                this.ErrorInfo = strError;
            }
        }

        private const int HOTKEY_NORMAL_ID = 1000;
        private const int HOTKEY_CLIPBOARD_ID = 1001;
        private const int HOTKEY_GIFTOGGLE_ID = 1002;
        private const int HOTKEY_GIFSTOP_ID = 1003;
        private bool m_bAutoRun;
        private bool m_bCaptureCur;

        private bool m_bInToggle;
        private Info[] m_info;
        private FrmCapture m_frmCapture;
        private KeyHook m_keyHook;
        private DateTime m_dtLastDownPrt;

        private void Form2_Load(object sender, EventArgs e) {
            m_dtLastDownPrt = DateTime.Now;
            m_keyHook = new KeyHook();
            m_keyHook.KeyHookEvent += m_keyHook_KeyHookEvent;
            m_keyHook.SetHook();
            contextMenuStrip1.Renderer = new DevCapture.ToolStripRendererEx();
            if (System.IO.Directory.Exists("./Plugins")) {
                string strRet = FrmCapture.LoadPlugins("./Plugins");
                if (strRet != "OK") {
                    MessageBox.Show(strRet, "DevCapture");
                }
            }
            FrmCapture.RecordError += (s, args) => new FrmTextAlert(args.ErrorMessage).Show();
            notifyIcon1.Visible = true;     //托盘来一个气泡提示
            notifyIcon1.ShowBalloonTip(30, "DevCapture", "DevCapture启动运行", ToolTipIcon.Info);
            this.Opacity = 0;
            string strFileName = Application.StartupPath + "/DevCaptureSetting.cfg";
            if (File.Exists(strFileName) && this.LoadSetting(strFileName)) {
                this.BeginInvoke(new MethodInvoker(() => this.Visible = false));
                return;
            }
            this.BeginInvoke(new MethodInvoker(() => {
                Thread.Sleep(500);
                Application.DoEvents();
                this.ToggleWindow();
            }));
        }

        private void m_keyHook_KeyHookEvent(object sender, KeyHookEventArgs e) {
            if (e.KeyCode == (int)Keys.PrintScreen && m_bCaptureCur) {
                if (DateTime.Now.Subtract(m_dtLastDownPrt).TotalMilliseconds > 500)
                    DevCapture.Win32.DrawCurToScreen(MousePosition);       //如果按下不松开会一直触发
                m_dtLastDownPrt = DateTime.Now;
            }
        }

        private void ToggleWindow() {
            new Thread(() => {
                lock (this) m_bInToggle = true;
                float fIncrement = 0.1F;
                float fValue = 1;
                this.Invoke(new MethodInvoker(() => {
                    this.Activate();
                    if (this.Opacity == 1) {
                        fIncrement = -0.1F;
                        fValue = 0;
                    } else this.Visible = true;
                }));
                for (int i = 0; i < 10; i++) {
                    this.Invoke(new MethodInvoker(() => this.Opacity += fIncrement));
                    Thread.Sleep(50);
                }
                this.Invoke(new MethodInvoker(() => {
                    this.Opacity = fValue;
                    this.Visible = fValue == 1;
                }));
                lock (this) m_bInToggle = false;
            }) { IsBackground = true }.Start();
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 35), new Point(0, this.Height), Color.White, Color.FromArgb(230, 230, 230))) {
                e.Graphics.FillRectangle(lgb, 0, 35, this.Width, this.Height - 35);
            }
            base.OnPaint(e);
        }

        //获取辅助按键
        private void textBox_KeyDown(object sender, KeyEventArgs e) {
            if ("None" != e.Modifiers.ToString()) { //禁止输入控制键(非alt ctrl shift...)
                MessageBox.Show("不能有[Ctrl,Alt,Shift]等按键");
                return;
            }
            (sender as TextBoxCu).Text = e.KeyCode.ToString();   //显示点下的按键
        }

        private void btn_settiing_Click(object sender, EventArgs e) {
            bool b = (!sbox_ctrl_normal.IsNO && !sbox_alt_normal.IsNO && !sbox_shift_normal.IsNO)
                || (!sbox_ctrl_clipboard.IsNO && !sbox_alt_clipboard.IsNO && !sbox_shift_clipboard.IsNO)
                || (!sbox_ctrl_giftoggle.IsNO && !sbox_alt_giftoggle.IsNO && !sbox_shift_giftoggle.IsNO)
                || (!sbox_ctrl_gifstop.IsNO && !sbox_alt_gifstop.IsNO && !sbox_shift_gifstop.IsNO);
            if (b) {
                MessageBox.Show("组合键中至少出现[Ctrl,Alt,Shift]中的一个");
                return;
            }
            if (tbx_normal.Text == "" || tbx_clipboard.Text == "" || tbx_giftoggle.Text == "" || tbx_gifstop.Text == "") {
                MessageBox.Show("组合键中不能没有辅助建");
                return;
            }
            m_bAutoRun = ckbox_autorun.Checked;
            m_bCaptureCur = ckbox_capcursor.Checked;
            if (m_bAutoRun) {
                DialogResult dr = MessageBox.Show("若要开机自动运行 确保此路径不会被改变\r\n是否继续?", "询问", MessageBoxButtons.YesNo);
                if (dr != DialogResult.Yes) return;
            }
            foreach (var v in m_info) {
                v.CtrlKey = 0
                    | (v.sbox_Ctrl.IsNO ? Win32.MOD_CONTROL : 0)
                    | (v.sbox_Alt.IsNO ? Win32.MOD_ALT : 0)
                    | (v.sbox_Shift.IsNO ? Win32.MOD_SHIFT : 0);
                v.AuxKey = Convert.ToUInt32((Keys)Enum.Parse(typeof(Keys), v.Tbx_Aux.Text));
                Win32.UnregisterHotKey(this.Handle, v.ID);
                if (!Win32.RegisterHotKey(this.Handle, v.ID, v.CtrlKey, v.AuxKey))   //登记新的热键
                    MessageBox.Show(v.ErrorInfo);
            }

            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
            if (m_bAutoRun) {
                if (regKey == null)
                    regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\");
                regKey.SetValue("DevCapture", Application.ExecutablePath);
            } else {
                if (regKey != null) {
                    if (regKey.GetValue("DevCapture") != null)
                        regKey.DeleteValue("DevCapture");
                }
            }
            regKey.Close();
            FrmCapture.ScreenRecorderInterval = (int)nud_sleep.Value;
            using (FileStream fs = new FileStream("./DevCaptureSetting.cfg", FileMode.Create)) {
                foreach (var v in m_info) {
                    fs.Write(BitConverter.GetBytes(v.CtrlKey), 0, 4);
                    fs.Write(BitConverter.GetBytes(v.AuxKey), 0, 4);
                }
                fs.Write(BitConverter.GetBytes(FrmCapture.ScreenRecorderInterval), 0, 4);
                fs.WriteByte((byte)(m_bAutoRun ? 1 : 0));               //保存是否自起
                fs.WriteByte((byte)(m_bCaptureCur ? 1 : 0));            //保存是否捕获鼠标
                byte[] byText = Encoding.ASCII.GetBytes("\r\nDevCaptureSettingFile by Crystal_lz");
                fs.Write(byText, 0, byText.Length);
            }
            MessageBox.Show("设置完成");
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == Win32.WM_HOTKEY) {
                switch ((int)m.WParam) {
                    case HOTKEY_NORMAL_ID:
                        this.StartCapture(false);
                        break;
                    case HOTKEY_CLIPBOARD_ID:
                        this.StartCapture(true);
                        break;
                    case HOTKEY_GIFTOGGLE_ID:
                        if (FrmCapture.ScreenRecorder == null) {
                            new FrmTextAlert("没有指定录制区域或者窗体").Show();
                            return;
                        }
                        Bitmap bmp = new Bitmap(16, 16);
                        using (Graphics g = Graphics.FromImage(bmp)) {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.DrawIcon(Properties.Resources.Icon, new Rectangle(0, 0, 16, 16));
                            if (FrmCapture.ScreenRecorder.IsStarted) {
                                FrmCapture.ScreenRecorder.Stop();
                                g.FillEllipse(Brushes.Red, new Rectangle(9, 9, 7, 7));
                            } else {
                                FrmCapture.ScreenRecorder.Start();
                                g.FillEllipse(Brushes.Lime, new Rectangle(9, 9, 7, 7));
                            }
                            notifyIcon1.Icon = Icon.FromHandle(bmp.GetHicon());
                        }
                        break;
                    case HOTKEY_GIFSTOP_ID:
                        if (FrmCapture.ScreenRecorder == null) {
                            new FrmTextAlert("没有指定录制区域或者窗体").Show();
                            return;
                        }
                        if (FrmCapture.ScreenRecorder.Frames.Count == 0) {
                            new FrmTextAlert("当前还没有录制数据帧").Show();
                            return;
                        }
                        notifyIcon1.Icon = Properties.Resources.Icon;
                        FrmCapture.ScreenRecorder.Stop();
                        Dictionary<Image, int> dicFrames = new Dictionary<Image, int>();
                        foreach (var v in FrmCapture.ScreenRecorder.Frames) dicFrames.Add(v.Key, v.Value);
                        new FrmGif(dicFrames).Show();
                        FrmCapture.ScreenRecorder.Clear();
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void StartCapture(bool bFromClip) {
            if (m_frmCapture == null || m_frmCapture.IsDisposed)
                m_frmCapture = new FrmCapture(m_bCaptureCur, bFromClip);
            m_frmCapture.Show();
        }

        private bool LoadSetting(string strFileName) {
            int nIndex = 0;
            byte[] byData = File.ReadAllBytes(strFileName);
            foreach (var v in m_info) {
                v.CtrlKey = BitConverter.ToUInt32(byData, nIndex);
                nIndex += 4;
                v.AuxKey = BitConverter.ToUInt32(byData, nIndex);
                nIndex += 4;
                Win32.UnregisterHotKey(this.Handle, v.ID);
                if (!Win32.RegisterHotKey(this.Handle, v.ID, v.CtrlKey, v.AuxKey))   //登记新的热键
                    MessageBox.Show(v.ErrorInfo);
            }
            FrmCapture.ScreenRecorderInterval = BitConverter.ToInt32(byData, nIndex);
            nIndex += 4;
            m_bAutoRun = byData[nIndex++] == 1;
            m_bCaptureCur = byData[nIndex] == 1;
            return true;
        }

        private void ShowSetting() {
            foreach (var v in m_info) {
                v.sbox_Ctrl.IsNO = (v.CtrlKey & Win32.MOD_CONTROL) != 0;
                v.sbox_Alt.IsNO = (v.CtrlKey & Win32.MOD_ALT) != 0;
                v.sbox_Shift.IsNO = (v.CtrlKey & Win32.MOD_SHIFT) != 0;
                v.Tbx_Aux.Text = v.AuxKey == 0 ? "NULL" : ((Keys)v.AuxKey).ToString();
            }
            nud_sleep.Value = FrmCapture.ScreenRecorderInterval;
            ckbox_autorun.Checked = m_bAutoRun;
            ckbox_capcursor.Checked = m_bCaptureCur;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            m_keyHook.UnLoadHook();
            Environment.Exit(0);
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.Visible == true) {
                this.Activate();
                return;
            }
            this.ShowSetting();
            this.ToggleWindow();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (!this.Visible) this.ShowSetting();
            this.ToggleWindow();
        }
    }
}
