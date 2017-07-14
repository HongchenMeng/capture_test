using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Reflection;

namespace DevCapture
{
    public partial class FrmCapture : Form
    {
        public FrmCapture(bool bCaptureCursor, bool bFromClipboard) {
            InitializeComponent();
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            m_rectScreen = Win32.GetDesktopRect();
            this.Location = m_rectScreen.Location;
            this.Size = m_rectScreen.Size;

            imageCroppingBox1.IsDrawMagnifier = true;
            Image imgScreen = this.GetFullScreen(bCaptureCursor, bFromClipboard);
            this.imageCroppingBox1.Image = imgScreen;
            this.imageCroppingBox1.Dock = DockStyle.Fill;
            this.imageCroppingBox1.KeyUp += (s, e) => { if (e.KeyCode == Keys.ControlKey)m_bAltDown = m_bCtrlDown = false; };
        }

        private static int _ScreenRecorderInterval;

        public static int ScreenRecorderInterval {
            get { return _ScreenRecorderInterval; }
            set { _ScreenRecorderInterval = value; }
        }

        private static ScreenRecorder _ScreenRecorder;

        public static ScreenRecorder ScreenRecorder {
            get { return _ScreenRecorder; }
        }

        private Point m_ptLastMouseDown;        //鼠标上一次点击位置
        private Point m_ptCurrent;              //鼠标当前位置
        private Rectangle m_rect;               //自动框选时候的区域
        private bool m_bDrawInfo;               //当前是否需要绘制被框选窗体的文字信息

        private bool m_bGetVisable = true;      //是否只框选可见窗体
        private bool m_bGetTransparent;         //是否框选透明窗体
        private bool m_bSpyWeb = true;          //时候spy webbrowser控件
        private bool m_bCtrlDown;               //ctrl键是否被点下
        private bool m_bAltDown;                //alt键是否被点下

        private Rectangle m_rectScreen;         //屏幕的矩形区域

        private string m_strShow;               //自动框选时 所需要绘制的文字信息

        private IntPtr m_hWnd;                  //自动框选是 被框选窗体的句柄

        private Image m_imgMosaic;              //马赛克图像
        private SolidBrush m_sbFill;            //FillXXX 所需要的画刷
        private TextureBrush m_tbMosaic;        //马赛克画刷
        private Image m_imgCurrentLayer;        //后期绘制时候 mousemove 中临时绘制的图像
        private Image m_imgLastLayer;           //上一次的历史记录

        private bool m_bDrawEffect;             //mousedown 中表示为是否进行后期绘制
        private List<Image> m_layer = new List<Image>();    //历史记录
        //缓存的插件
        private static Dictionary<string, List<IPlugins.IFilter>> m_dic_plugin = new Dictionary<string, List<IPlugins.IFilter>>();

        public static event ScreenRecorder.RecordErrorEventHandler RecordError;

        private void FrmCaption_Load(object sender, EventArgs e) {
            m_sbFill = new SolidBrush(Color.Red);

            panel1.Parent = imageCroppingBox1;
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
            panel1.Cursor = Cursors.Default;
            panel1.Visible = false;
            this.captureToolbar1.Parent = imageCroppingBox1;
            this.captureToolbar1.Visible = false;
            this.captureToolbar1.Cursor = Cursors.Default;
            colorBox1.ColorChanged += (s, ex) => {
                sizeTrackBar1.Color = colorBox1.Color;
                textBox1.ForeColor = colorBox1.Color;
                m_sbFill.Color = colorBox1.Color;
            };

            contextMenuStrip1.Renderer = new ToolStripRendererEx();
            textBox1.Visible = false;
            textBox1.TextChanged += (s, ex) => this.SetTextBoxSize();
            textBox1.Validating += textBox1_Validating;

            linkLabel1.Location = new Point(163, 5);
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.Click += (s, ex) => {
                FontDialog fd = new FontDialog();
                fd.Font = textBox1.Font;
                if (fd.ShowDialog() != DialogResult.OK) return;
                textBox1.Font = fd.Font;
                this.SetTextBoxSize();
            };
            linkLabel1.Visible = false;
            //将插件绑定到右键菜单上面
            if (m_dic_plugin.Count > 0) {
                tmsi_plugin.DropDownItems.Clear();
                foreach (var v in m_dic_plugin) {
                    ToolStripMenuItem item = null;
                    if (v.Value.Count == 1) {                   //如果一个dll中只有一个插件则不设立子菜单
                        item = new ToolStripMenuItem(v.Value[0].GetPluginName(), v.Value[0].GetPluginIcon());
                        item.Tag = v.Value[0];
                        item.Click += item_Click;
                    } else {                                    //否则建立子菜单 并以dll名作为分组名
                        item = new ToolStripMenuItem(v.Key);
                        foreach (var i in v.Value) {
                            ToolStripMenuItem subItem = new ToolStripMenuItem(i.GetPluginName(), i.GetPluginIcon());
                            subItem.Tag = i;
                            subItem.Click += item_Click;
                            item.DropDownItems.Add(subItem);
                        }
                    }
                    tmsi_plugin.DropDownItems.Add(item);
                }
            }
        }
        //调用插件
        private void item_Click(object sender, EventArgs e) {
            try {
                var result = ((sender as ToolStripMenuItem).Tag as IPlugins.IFilter).ExecFilter(m_imgLastLayer.Clone() as Bitmap);
                if (result.IsClose) this.Close();
                if (!result.IsModified) return;
                m_imgCurrentLayer = result.ResultImage;
                imageCroppingBox1.Invalidate(imageCroppingBox1.SelectedRectangle);
                this.SetHistoryLayer();
                imageCroppingBox1.IsLockSelected = true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        //绘图时候选择文字工具 文本框离开焦点的时候将文本绘制上去
        private void textBox1_Validating(object sender, CancelEventArgs e) {
            if (textBox1.Text.Trim() == string.Empty) return;
            using (Graphics g = Graphics.FromImage(m_imgCurrentLayer)) {
                Brush brush = m_sbFill;
                if (ckbox_mosaic.Checked) brush = m_tbMosaic;
                g.DrawImage(m_imgLastLayer, 0, 0);
                g.DrawString(
                    textBox1.Text,
                    textBox1.Font,
                    brush,
                    textBox1.Left - imageCroppingBox1.SelectedRectangle.Left,
                    textBox1.Top - imageCroppingBox1.SelectedRectangle.Top
                    );
            }
            this.SetHistoryLayer();
            imageCroppingBox1.Invalidate(imageCroppingBox1.SelectedRectangle);
            textBox1.Visible = false;
            textBox1.Text = string.Empty;
        }

        private void _ScreenRecorder_RecordError(object sender, RecordErrorEventArgs e) {
            if (FrmCapture.RecordError != null) FrmCapture.RecordError(FrmCapture.ScreenRecorder, e);
        }
        /// <summary>
        /// 获取桌面图片
        /// </summary>
        /// <param name="bCaptureCursor">是否捕获鼠标</param>
        /// <param name="bFromClipboard">是否从剪切板获取图像</param>
        /// <returns>获取到的图像</returns>
        private Image GetFullScreen(bool bCaptureCursor, bool bFromClipboard) {
            if (bCaptureCursor) Win32.DrawCurToScreen(MousePosition);
            Bitmap bmp = new Bitmap(m_rectScreen.Width, m_rectScreen.Height);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.CopyFromScreen(m_rectScreen.X, m_rectScreen.Y, 0, 0, bmp.Size);
                if (bFromClipboard) {
                    Image img = Clipboard.GetImage();
                    if (img != null) {
                        Point pt = new Point((this.Width - img.Width) / 2, (this.Height - img.Height) / 2);
                        Rectangle rectScreen = Screen.PrimaryScreen.Bounds;
                        if (img.Width <= rectScreen.Width && img.Height <= rectScreen.Height)
                            pt = new Point(rectScreen.Left + (rectScreen.Width - img.Width) / 2, rectScreen.Top + (rectScreen.Height - img.Height) / 2);
                        g.DrawImage(img, pt);
                    }
                }
            }
            return bmp;
        }

        private void imageCroppingBox1_MouseDown(object sender, MouseEventArgs e) {
            m_ptLastMouseDown = e.Location;
            if (e.Button != MouseButtons.Left) return;                  //禁止右键的时候触发
            if (!imageCroppingBox1.IsLockSelected) {
                this.captureToolbar1.Visible = false;
            }
            if (m_bCtrlDown && !imageCroppingBox1.IsSelected) {
                this.Close();
                string strFileName = Application.StartupPath + "/SpyTool.exe";
                if (System.IO.File.Exists(strFileName)) {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = strFileName;
                    p.StartInfo.Arguments = String.Format("{0} {1} {2}", m_hWnd, m_bGetVisable, m_bGetTransparent);
                    p.Start();
                    p.WaitForInputIdle();
                    Win32.SetForegroundWindow(p.MainWindowHandle);
                } else
                    new FrmTextAlert("没有发现[SpyTool.exe]").Show();
            }
            if (m_bAltDown && !imageCroppingBox1.IsSelected) {
                if (FrmCapture._ScreenRecorder != null) FrmCapture._ScreenRecorder.Dispose();
                FrmCapture._ScreenRecorder = new ScreenRecorder(m_hWnd, FrmCapture._ScreenRecorderInterval);
                FrmCapture._ScreenRecorder.RecordError += _ScreenRecorder_RecordError;
                new FrmRectAlert(m_rect, "Gif Window").Show();
                this.Close();
            }
            //如果已经锁定了选取 并且 鼠标点下的位置在选取内 而且工具条上有选择工具 那么则表示可能需要绘制了 如：矩形框 箭头 等
            if (imageCroppingBox1.IsLockSelected && imageCroppingBox1.SelectedRectangle.Contains(e.Location) && captureToolbar1.GetSelectBtnName() != null) {
                if (captureToolbar1.GetSelectBtnName() == "btn_text") { //如果选择的是文字工具 那么特殊处理
                    textBox1.Location = e.Location;                     //将文本框位置设置为鼠标点下的位子
                    textBox1.Visible = true;                            //显示文本框以便输入文字
                    textBox1.Focus();                                   //获得焦点
                    return;                                             //特殊处理的角色 直接返回
                }
                m_bDrawEffect = true;                                   //设置标识 在MouseMove用于判断当前的操作是否是后期的绘制
                Cursor.Clip = imageCroppingBox1.SelectedRectangle;      //设置鼠标的活动区域
            }
        }

        private void imageCroppingBox1_MouseUp(object sender, MouseEventArgs e) {
            m_bDrawEffect = false;                                      //无论如何 鼠标抬起的时候 后期绘制的标识都应该标记为false
            Cursor.Clip = Rectangle.Empty;                              //无论如何 鼠标抬起的时候 应该取消鼠标的活动限制
            if (e.Button == MouseButtons.Right) {
                if (imageCroppingBox1.IsSelected) {                     //如果是右键抬起并且有选择区域的情况 则有可能是右键菜单 或者 取消选择
                    if (imageCroppingBox1.SelectedRectangle.Contains(e.Location)) {
                        contextMenuStrip1.Show(e.Location);             //弹出右键菜单
                        return;
                    }
                    imageCroppingBox1.Clear();                          //取消选择
                    m_layer.Clear();                                    //同时情况历史图层
                } else
                    this.Close();                                       //如果没有选择区域且右键抬起 则直接关闭窗体
                this.captureToolbar1.Visible = false;                   //如果代码走到这里 则表示取消了选择 那么工具条应该被隐藏
                captureToolbar1.ClearSelect();                          //清空工具条上的工具选择
                panel1.Visible = false;                                 //配置面板也应该被关闭
                m_imgLastLayer = m_imgCurrentLayer = null;              //临时图层也应该被清空
            }
            //如果是左键抬起 并且 抬起时候的位置和鼠标点下的位置一样 并且 还没有选择区域的情况下 则赋值为自动框选出来的区域(自动框选时)
            if (e.Button == MouseButtons.Left && (m_ptLastMouseDown == e.Location) && !imageCroppingBox1.IsSelected) {
                imageCroppingBox1.SelectedRectangle = m_rect;
            }
            if (imageCroppingBox1.IsLockSelected) {                     //如果是鼠标抬起 并且 已经锁定了区域的情况下 则有可能在进行后期绘制
                //如果工具条上有选择工具 那么就可以确定是在进行后期的绘制   当然如果选择的是文字工具 那么忽略 因为特殊处理的 其他则设置历史图层
                if (captureToolbar1.GetSelectBtnName() != null && captureToolbar1.GetSelectBtnName() != "btn_text") this.SetHistoryLayer();
            } else if (imageCroppingBox1.IsSelected) {                  //如果没有进入上面的判断 则判断当前有没有选取 可能鼠标抬起是确认选取
                this.SetToolBarLocation();                              //如果有选取 那么应该设置工具条的位置并且显示 以便后期绘制
                this.captureToolbar1.Visible = true;
                if (m_imgLastLayer != null) m_imgLastLayer.Dispose();
                m_imgLastLayer = imageCroppingBox1.GetSelectedImage();                          //默认图层为选取区域的图像
                if (m_imgCurrentLayer != null) m_imgCurrentLayer.Dispose();
                m_imgCurrentLayer = new Bitmap(m_imgLastLayer.Width, m_imgLastLayer.Height);    //绘制面板图层置空
                if (m_imgMosaic != null) m_imgMosaic.Dispose();
                m_imgMosaic = ImageHelper.Mosaic(m_imgLastLayer, 10);                           //设置当前区域的马赛克图像
                if (m_tbMosaic != null) m_tbMosaic.Dispose();
                m_tbMosaic = new TextureBrush(m_imgMosaic);                                     //设置马赛克画刷
            }
        }

        private void imageCroppingBox1_MouseMove(object sender, MouseEventArgs e) {
            if (m_ptCurrent == e.Location) return;
            m_ptCurrent = e.Location;
            if (imageCroppingBox1.IsLockSelected) {                     //如果已经锁定了选取 则有可能是正在进行后期绘制
                imageCroppingBox1.Cursor = imageCroppingBox1.SelectedRectangle.Contains(e.Location) ? Cursors.Cross : Cursors.Default;
                if (m_bDrawEffect) this.DrawEffects();                  //如果说在mousedown中设置了绘制标识 那么则表示是在进行后期绘制
                else if (imageCroppingBox1.SelectedRectangle.Contains(e.Location))              //如果没有设置绘制标识 判断鼠标是否在选取内
                    imageCroppingBox1.Invalidate(imageCroppingBox1.SelectedRectangle);          //重绘选取 以便在paint绘制画笔大小预览(圆和矩形的那个)
                return;
            }//如果已经锁定选取 那么直接return掉
            //在mousemove中还有可能是正在根据鼠标位置自动框选窗体 以下代码为获取窗体信息
            string strText = string.Empty;
            string strClassName = string.Empty;
            m_bDrawInfo = !(imageCroppingBox1.IsSelected || e.Button != MouseButtons.None);
            if (!m_bDrawInfo) return;                                   //如果已经有选取 或者移动过程中有鼠标被点下 都不需要获取窗体信息了
            IntPtr hWnd = Win32.GetWindowFromPoint(e.Location.X, e.Location.Y, this.Handle, m_bGetVisable, m_bGetTransparent);
            m_hWnd = hWnd;                                                      //更具鼠标位置获取窗体句柄
            strClassName = Win32.GetClassName(hWnd);
            strText = Win32.GetWindowText(hWnd);
            Win32.RECT rect = new Win32.RECT();
            Win32.GetWindowRect(hWnd, ref rect);                                //获取窗体大小
            m_rect = rect.ToRectangle();
            if (m_bSpyWeb && strClassName.ToLower() == "internet explorer_server") {        //如果说当前窗体是webbrowser控件 并且有设置进行spy
                var element = Win32.GetHtmlDocument(hWnd).elementFromPoint(e.Location.X - rect.Left, e.Location.Y - rect.Top);
                if (element != null) {
                    var p = ((mshtml.IHTMLElement2)element).getBoundingClientRect();        //获取当前鼠标下的html元素信息
                    m_rect.X = rect.Left + p.left;
                    m_rect.Y = rect.Top + p.top;
                    m_rect.Width = p.right - p.left;
                    m_rect.Height = p.bottom - p.top;
                    string strInner = element.innerText == null ? "null" : element.innerText.Trim().Split('\n')[0];
                    strText = string.Format("<{0}>innerText:[{1}]", element.tagName, strInner + (strInner.Length == 1 ? "" : "..."));
                    if (strText.Length > 100) strText = strText.Substring(0, 100) + "...]";
                }
            }
            m_strShow = string.Format("TEXT:{0}\r\nHWND:0x{1} [{2}]", strText, hWnd.ToString("X").PadLeft(8, '0'), strClassName);
            if (m_rectScreen.X < 0) m_rect.X -= m_rectScreen.X;     //判断一下屏幕的坐标是否是小于0的 如果是 则在绘制区域的时候需要加上这个差值
            if (m_rectScreen.Y < 0) m_rect.Y -= m_rectScreen.Y;     //多显示器屏幕坐标可能是负数{如(-1920,0)两个显示器 主显示器在右边} 在绘制的时候窗体内部坐标是0开始的 需要转换
            m_rect.Intersect(imageCroppingBox1.DisplayRectangle);   //确定出来的区域和截图窗体的区域做一个交集 (不能让区域超出屏幕啊 比如屏幕边缘有个窗体 那么框选出来的区域是超出屏幕的)
            imageCroppingBox1.Invalidate();
        }

        private void imageCroppingBox1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (m_layer.Count > 0)                                  //绘制最后一张图层
                g.DrawImage(m_layer[m_layer.Count - 1], imageCroppingBox1.SelectedRectangle);
            if (m_imgCurrentLayer != null)                          //当前正在mousemove中进行绘制的临时图层
                g.DrawImage(m_imgCurrentLayer, imageCroppingBox1.SelectedRectangle.Location);
            if (imageCroppingBox1.Cursor == Cursors.Cross) {        //如果鼠标是十字架 说明正在后期绘制或者准备后期绘制
                using (Pen p = new Pen(colorBox1.Color)) {
                    Rectangle rect = new Rectangle(m_ptCurrent.X - sizeTrackBar1.Value / 2, m_ptCurrent.Y - sizeTrackBar1.Value / 2, sizeTrackBar1.Value, sizeTrackBar1.Value);
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(50, 0, 0, 0))) {
                        g.DrawRectangle(p, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        g.DrawEllipse(p, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                    }//绘制画笔大小预览(圆和矩形的那个)
                    //以下根据工具条上选择的工具 确定是否需要绘制辅助线条 主要用于马赛克时候让用户知道绘制的区域 非马赛克也无所谓 反正都会被挡住
                    if (m_bDrawEffect) {                            //如果mousedown中标识了 后期绘制
                        rect = new Rectangle(                       //鼠标点下到当前位置的矩形区域
                            m_ptCurrent.X < m_ptLastMouseDown.X ? m_ptCurrent.X : m_ptLastMouseDown.X,
                            m_ptCurrent.Y < m_ptLastMouseDown.Y ? m_ptCurrent.Y : m_ptLastMouseDown.Y,
                            Math.Abs(m_ptLastMouseDown.X - m_ptCurrent.X),
                            Math.Abs(m_ptLastMouseDown.Y - m_ptCurrent.Y));
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                        p.DashPattern = new float[] { 5, 5 };
                        switch (captureToolbar1.GetSelectBtnName()) {
                            case "btn_rect":
                                g.DrawRectangle(p, rect);
                                break;
                            case "btn_elips":
                                g.DrawRectangle(p, rect);           //被绘制的圆形的外切矩形
                                g.DrawEllipse(p, rect);
                                break;
                            case "btn_arrow":
                                g.DrawLine(p, m_ptLastMouseDown, m_ptCurrent);
                                break;
                        }
                    }
                }
            }
            if (!m_bDrawInfo) return;                               //如果在mousemove中被标识为true 则表示还出于自动框选窗体 那么进行文字信息绘制
            imageCroppingBox1.PreViewRectangle = m_rect;
            Size sz = g.MeasureString(m_strShow, this.Font).ToSize();
            Point pt = new Point(m_rect.X + sz.Width > this.Width ? this.Width - sz.Width : m_rect.X, m_rect.Y - sz.Height - 5);
            if (pt.Y < 0) pt.Y = 5;
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(125, 0, 0, 0))) {
                g.FillRectangle(sb, new Rectangle(pt, sz));
            }
            g.DrawString(m_strShow, this.Font, Brushes.White, pt);
        }
        //一些快捷键
        private void imageCroppingBox1_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.W:                                    //微距离移动鼠标
                    Win32.SetCursorPos(m_ptCurrent.X, m_ptCurrent.Y - 1);//可以不用判断 Y < 0
                    break;
                case Keys.A:
                    Win32.SetCursorPos(m_ptCurrent.X - 1, m_ptCurrent.Y);
                    break;
                case Keys.S:
                    Win32.SetCursorPos(m_ptCurrent.X, m_ptCurrent.Y + 1);
                    break;
                case Keys.D:
                    Win32.SetCursorPos(m_ptCurrent.X + 1, m_ptCurrent.Y);
                    break;
                case Keys.V:
                    m_bGetVisable = !m_bGetVisable;             //自动框选时候是只否获取可见窗体
                    new FrmTextAlert("是否只获取可见窗体:" + m_bGetVisable).Show(this);
                    break;
                case Keys.T:
                    m_bGetTransparent = !m_bGetTransparent;     //自动框选时候是否获取透明窗体
                    new FrmTextAlert("是否获取透明窗体:" + m_bGetTransparent).Show(this);
                    break;
                case Keys.H:
                    m_bSpyWeb = !m_bSpyWeb;                     //自动框选时候是否对webbrowser进行spy
                    new FrmTextAlert("Spy WebBrowser:" + m_bSpyWeb).Show(this);
                    break;
                case Keys.ControlKey:
                    m_bCtrlDown = true;
                    break;
                case Keys.Menu:
                    m_bAltDown = true;
                    break;
            }
            Console.WriteLine(e.KeyCode);
        }

        private void captureToolbar1_ToolButtonClick(object sender, EventArgs e) {
            this.OnToolBarClick((sender as Control).Name);
        }

        private void imageCroppingBox1_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (imageCroppingBox1.SelectedRectangle.Size == Size.Empty) return;
            Clipboard.SetImage(m_imgLastLayer);
            this.Close();
        }

        private void tmsi_comm_Click(object sender, EventArgs e) {
            this.OnToolBarClick((sender as ToolStripMenuItem).Tag.ToString());
        }

        private void tmsi_toggle_Click(object sender, EventArgs e) {
            captureToolbar1.Visible = !captureToolbar1.Visible;
            panel1.Visible = captureToolbar1.Visible && captureToolbar1.GetSelectBtnName() != null;
        }

        private void tmsi_gif_Click(object sender, EventArgs e) {
            if (FrmCapture._ScreenRecorder != null) FrmCapture._ScreenRecorder.Dispose();
            FrmCapture._ScreenRecorder = new ScreenRecorder(this.RectangleToScreen(imageCroppingBox1.SelectedRectangle), FrmCapture._ScreenRecorderInterval);
            FrmCapture._ScreenRecorder.RecordError += _ScreenRecorder_RecordError;
            new FrmRectAlert(imageCroppingBox1.SelectedRectangle, "Gif Rectangle").Show();
            this.Close();
        }
        /// <summary>
        /// 工具栏被点下的时候
        /// </summary>
        /// <param name="strCtrlName">被点下的工具的控件名字</param>
        private void OnToolBarClick(string strCtrlName) {
            switch (strCtrlName) {
                case "btn_close": this.Close(); break;
                case "btn_ok":
                    Clipboard.SetImage(m_imgLastLayer);
                    this.Close();
                    break;
                case "btn_out":
                    new FrmOut(m_imgLastLayer).Show();          //弹出选取
                    this.Close();
                    break;
                case "btn_cancel":
                    if (textBox1.Visible) {                     //如果撤销的时候正则使用文字工具 则清理掉
                        textBox1.Text = string.Empty;
                        textBox1.Visible = false;
                        break;
                    }
                    if (m_layer.Count > 0) {                    //如果存在历史图层
                        m_layer.RemoveAt(m_layer.Count - 1);    //则干掉最后一层
                        if (m_imgCurrentLayer != null) m_imgCurrentLayer.Dispose();
                        m_imgCurrentLayer = new Bitmap(m_imgLastLayer.Width, m_imgLastLayer.Height);        //当前正在绘制的图层清理掉
                        if (m_imgLastLayer != null) m_imgLastLayer.Dispose();
                        if (m_layer.Count > 0) {                //如果干点最后一层还存在图层 则设置
                            m_imgLastLayer = m_layer[m_layer.Count - 1].Clone() as Bitmap;
                        } else {                                //否则回到刚选择好选取的时候
                            m_imgLastLayer = imageCroppingBox1.GetSelectedImage();
                            imageCroppingBox1.IsLockSelected = captureToolbar1.GetSelectBtnName() != null;
                        }
                        imageCroppingBox1.Invalidate(imageCroppingBox1.SelectedRectangle);
                    } else {                                    //如果已经不存在历史记录了 则直接撤销选取 重新选择区域
                        imageCroppingBox1.Clear();
                        captureToolbar1.ClearSelect();
                        captureToolbar1.Visible = false;
                        panel1.Visible = false;
                    }
                    break;
                case "btn_save":
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "*.png|*.png|*.jpg|*.jpg|*.bmp|*.bmp|*.gif|*.gif|*.tiff|*.tiff";
                    sfd.FileName = "DevCap_"/*Developer Capture*/ + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                    System.Drawing.Imaging.ImageFormat imgf = System.Drawing.Imaging.ImageFormat.Png;
                    if (sfd.ShowDialog() == DialogResult.OK) {
                        switch (sfd.FilterIndex) {
                            case 2: imgf = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                            case 3: imgf = System.Drawing.Imaging.ImageFormat.Bmp; break;
                            case 4: imgf = System.Drawing.Imaging.ImageFormat.Gif; break;
                            case 5: imgf = System.Drawing.Imaging.ImageFormat.Tiff; break;
                        }
                        using (System.IO.Stream stream = new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create)) {
                            m_imgLastLayer.Save(stream, imgf);
                            //如果使用此工具或者此代码 请保留此代码 好歹让原著装个逼啊 开源需要动力
                            byte[] byString = Encoding.ASCII.GetBytes("\0\0\r\nBy->Crystal_lz");
                            stream.Write(byString, 0, byString.Length); ;
                        }
                        this.Close();
                    }
                    break;
                case "btn_rect":
                case "btn_elips":
                case "btn_arrow":
                case "btn_brush":
                case "btn_text":
                    if (captureToolbar1.GetSelectBtnName() == null) {                       //如果没有工具被选择 那么配置面板信息隐藏
                        if (m_layer.Count == 0) imageCroppingBox1.IsLockSelected = false;   //如果没有历史图层 则取消选取的锁定
                        panel1.Visible = false;
                    } else {
                        imageCroppingBox1.IsLockSelected = true;                            //否则锁定选取 并且显示配置面板
                        panel1.Visible = true;
                        if (strCtrlName == "btn_arrow") {
                            rdbtn_draw.Text = "箭头";
                            rdbtn_fill.Text = "直线";
                        } else {
                            rdbtn_draw.Text = "绘制";
                            rdbtn_fill.Text = "填充";
                        }
                        //如果是文字工具被选择 则显示[选择字体]并隐藏 (绘制 填充) 控件
                        rdbtn_draw.Visible = rdbtn_fill.Visible = !(linkLabel1.Visible = strCtrlName == "btn_text");
                        this.SetToolBarLocation();
                    }
                    break;
            }
        }
        //设置工具条位置
        private void SetToolBarLocation() {
            this.captureToolbar1.Location = new Point(imageCroppingBox1.SelectedRectangle.Left, imageCroppingBox1.SelectedRectangle.Bottom + 5);
            int nBottom = captureToolbar1.Bottom + (panel1.Visible ? panel1.Height + 5 : 0);
            if (this.captureToolbar1.Right > this.Width) {
                this.captureToolbar1.Left = this.Width - this.captureToolbar1.Width;
            }
            if (nBottom > this.Height) {
                this.captureToolbar1.Top = this.imageCroppingBox1.SelectedRectangle.Top - 5 - this.captureToolbar1.Height - (panel1.Visible ? panel1.Height + 5 : 5);
            }
            if (this.captureToolbar1.Top < 0) {
                this.captureToolbar1.Top = this.imageCroppingBox1.SelectedRectangle.Top + 5;
            }
            if (panel1.Visible)
                panel1.Left = captureToolbar1.Left;
            panel1.Top = captureToolbar1.Bottom + 5;
        }
        //后期绘制
        private void DrawEffects() {
            //绘制的起点坐标(相对于选取内的坐标)
            Point ptStart = new Point(m_ptLastMouseDown.X < m_ptCurrent.X ? m_ptLastMouseDown.X : m_ptCurrent.X, m_ptLastMouseDown.Y < m_ptCurrent.Y ? m_ptLastMouseDown.Y : m_ptCurrent.Y);
            ptStart = (Point)((Size)ptStart - (Size)imageCroppingBox1.SelectedRectangle.Location);
            Size sz = new Size(Math.Abs(m_ptCurrent.X - m_ptLastMouseDown.X), Math.Abs(m_ptCurrent.Y - m_ptLastMouseDown.Y));
            Brush brush = m_sbFill;                                 //默认为填充画笔 否则使用马赛克画刷
            if (ckbox_mosaic.Checked) brush = m_tbMosaic;
            using (Pen p = new Pen(brush, sizeTrackBar1.Value))     //根据画刷设置画笔
            using (Graphics g = Graphics.FromImage(m_imgCurrentLayer)) {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                switch (captureToolbar1.GetSelectBtnName()) {
                    case "btn_rect":
                        g.Clear(Color.Transparent);                 //清空上一次绘制
                        if (rdbtn_fill.Checked)                     //如果选择的是 填充 则直接使用画刷 否则画笔
                            g.FillRectangle(brush, new Rectangle(ptStart, sz));
                        else
                            g.DrawRectangle(p, new Rectangle(ptStart, sz));
                        break;
                    case "btn_elips":
                        g.Clear(Color.Transparent);
                        if (rdbtn_fill.Checked)
                            g.FillEllipse(brush, new Rectangle(ptStart, sz));
                        else
                            g.DrawEllipse(p, new Rectangle(ptStart, sz));
                        break;
                    case "btn_arrow":
                        g.Clear(Color.Transparent);
                        if (rdbtn_draw.Checked)                     //箭头还是直线 工具条被选择为箭头工具时候 (绘制 填充) 被改为 (箭头 直线)
                            p.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5, true);
                        g.DrawLine(p, (Point)((Size)m_ptLastMouseDown - (Size)imageCroppingBox1.SelectedRectangle.Location), (Point)((Size)m_ptCurrent - (Size)imageCroppingBox1.SelectedRectangle.Location));
                        break;
                    case "btn_brush":                               //如果是画线 则就不需要g.Clear(Color.Transparent)来清空上一次才绘制了
                        p.StartCap = p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.DrawLine(p, (Point)((Size)m_ptLastMouseDown - (Size)imageCroppingBox1.SelectedRectangle.Location), (Point)((Size)m_ptCurrent - (Size)imageCroppingBox1.SelectedRectangle.Location));
                        m_ptLastMouseDown = m_ptCurrent;            //重置上一次点击位置 以便下一次进入此代码使用
                        break;
                }
                imageCroppingBox1.Invalidate(imageCroppingBox1.SelectedRectangle);
            }
        }
        //设置历史图层
        private void SetHistoryLayer() {
            using (Graphics g = Graphics.FromImage(m_imgLastLayer)) {
                g.DrawImage(m_imgCurrentLayer, 0, 0);               //先将临时绘制的图层绘制到LastLayer中
            }
            m_layer.Add(m_imgLastLayer);                            //然后保存本次图层
            m_imgLastLayer = m_imgLastLayer.Clone() as Bitmap;      //克隆一份作为下一次的背景图层
            m_imgCurrentLayer = m_imgLastLayer.Clone() as Bitmap;   //临时绘制图层也得清理掉
            if (m_imgMosaic != null) m_imgMosaic.Dispose();
            m_imgMosaic = ImageHelper.Mosaic(m_imgLastLayer, 10);   //
            if (m_tbMosaic != null) m_tbMosaic.Dispose();
            m_tbMosaic = new TextureBrush(m_imgMosaic);             //设置马赛克画刷
        }

        private void SetTextBoxSize() {
            Size se = TextRenderer.MeasureText(textBox1.Text, textBox1.Font);
            textBox1.Size = se.IsEmpty ? new Size(50, textBox1.Font.Height) : se;
        }


        public static string LoadPlugins(string strPath) {
            if (!System.IO.Directory.Exists(strPath))
                return "不存在路径:\r\n" + strPath;
            string strRet = string.Empty;
            foreach (var v in System.IO.Directory.GetFiles(strPath, "*.dll")) {
                try {
                    List<IPlugins.IFilter> lst = FrmCapture.GetInterface(v);
                    if (lst.Count == 0) continue;
                    if (lst != null) m_dic_plugin.Add(System.IO.Path.GetFileNameWithoutExtension(v), lst);
                } catch (Exception ex) {
                    strRet += v + "\r\n" + ex.Message + "\r\n";
                }
            }
            return strRet == string.Empty ? "OK" : strRet;
        }

        public static List<IPlugins.IFilter> GetInterface(string strFileName) {
            List<IPlugins.IFilter> lst = new List<IPlugins.IFilter>();
            Assembly asm = Assembly.LoadFile(System.IO.Path.GetFullPath(strFileName));
            foreach (var t in asm.GetTypes()) {
                if (t.GetInterface("IFilter") != null) {
                    var i = (IPlugins.IFilter)Activator.CreateInstance(t);
                    i.InitPlugin(Application.StartupPath);
                    lst.Add(i);
                }
            }
            return lst;
        }
    }
}
