using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace SpyTool
{
    public partial class FrmSpy : Form
    {
        public FrmSpy(IntPtr hWnd, bool bVisableOnly, bool bTransparent) {
            InitializeComponent();
            m_hWndSelect = hWnd;
            chbox_visable.Checked = bVisableOnly;
            chbox_tran.Checked = bTransparent;
            this.Text = "SpyTool";
            this.StartPosition = FormStartPosition.CenterScreen;
            picbox_refresh.Click += (s, e) => this.LoadTreeWnd(m_hWndSelect);
        }

        private IntPtr m_hWndTemp;                          //临时句柄
        private IntPtr m_hWndSelect;                        //选中的句柄
        private Color m_clrTemp;                            //TreeNode的ForeColor临时保存变量
        private TreeNode m_nodeSelect;                      //选中的节点
        private List<int> m_lst_error_pid;                  //有些进程无法获取到MainModule 将id保存
        private Dictionary<int, Process> m_dic_process;     //进程列表
        private Dictionary<IntPtr, TreeNode> m_dic_node;    //所有树节点
        private byte[] m_byTextBuffer;                      //GetWindowText GetClassName 需要的缓存
        private FrmShowRect m_frmRect;                      //用于显示选框的窗体

        private void Form1_Load(object sender, EventArgs e) {
            m_byTextBuffer = new byte[256];
            m_lst_error_pid = new List<int>();
            m_dic_node = new Dictionary<IntPtr, TreeNode>();
            m_dic_process = new Dictionary<int, Process>();
            tree_wnd.ImageList = imglst_icon;
            cmbox_search.DisplayMember = "Text";            //使用TreeMode的Text作为显示
            m_frmRect = new FrmShowRect();
            //chbox_visable.Checked = true;                   //默认只抓去可视窗体
            this.LoadTreeWnd(m_hWndSelect);                 //加载节点
        }

        public void LoadTreeWnd(IntPtr hWndSelect) {        //加载树 并选中某个句柄节点
            cmbox_search.Items.Clear();
            m_dic_node.Clear();
            m_dic_process.Clear();
            m_lst_error_pid.Clear();
            foreach (var p in Process.GetProcesses()) {     //获取进程快照
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                m_dic_process.Add(p.Id, p);
            }
            tree_wnd.Nodes.Clear();
            TreeNode node = this.GetNodeFromWindowInfo(this.GetWindowInfo(Win32.GetDesktopWindow()), true);
            tree_wnd.Nodes.Add(node);                       //桌面作为根节点
            this.LoadTreeWnd(IntPtr.Zero, IntPtr.Zero, node, true); //递归节点
            if (m_dic_node.ContainsKey(hWndSelect)) {       //若是存在选中的节点
                tree_wnd.SelectedNode = m_dic_node[hWndSelect];
                m_dic_node[hWndSelect].ForeColor = Color.Blue;
                m_nodeSelect = m_dic_node[hWndSelect];
            }
            node.Expand();                                  //展开桌面节点
        }

        public void LoadTreeWnd(IntPtr hParent, IntPtr hAfter, TreeNode treeNode, bool bTopWindow) {
            while ((hAfter = Win32.FindWindowEx(hParent, hAfter, null, null)) != IntPtr.Zero) {
                WindowInfo wi = this.GetWindowInfo(hAfter);
                if (chbox_visable.Checked && (wi.WndInfo.dwStyle & Win32.WS_VISIBLE) == 0) continue;
                if (!chbox_tran.Checked && (wi.WndInfo.dwExStyle & Win32.WS_EX_TRANSPARENT) != 0) continue;
                TreeNode node = this.GetNodeFromWindowInfo(wi, bTopWindow);
                if (wi.Process == null) wi.Process = ((WindowInfo)treeNode.Tag).Process;
                if ((wi.WndInfo.dwStyle & Win32.WS_VISIBLE) == 0) {
                    node.ForeColor = Color.Gray;            //非可视窗体 使用灰色
                } else if ((wi.WndInfo.dwExStyle & Win32.WS_EX_TRANSPARENT) != 0) {
                    node.ForeColor = Color.Red;             //透明窗体 使用红色
                }
                treeNode.Nodes.Add(node);
                LoadTreeWnd(hAfter, IntPtr.Zero, node, false);
            }
        }

        private WindowInfo GetWindowInfo(IntPtr hWnd) {
            WindowInfo windowInfo = new WindowInfo(hWnd);
            Win32.WINDOWINFO wi = new Win32.WINDOWINFO();
            Win32.GetWindowInfo(hWnd, ref wi);
            //GetWindowInfo返回的ExStyle貌似有些问题
            wi.dwExStyle = (uint)Win32.GetWindowLong(hWnd, Win32.GWL_EXSTYLE);
            int len = Win32.GetClassName(hWnd, m_byTextBuffer, m_byTextBuffer.Length);
            windowInfo.ClassName = Encoding.Default.GetString(m_byTextBuffer, 0, len > m_byTextBuffer.Length ? m_byTextBuffer.Length : len);
            len = Win32.GetWindowText(hWnd, m_byTextBuffer, m_byTextBuffer.Length);
            windowInfo.WindowText = Encoding.Default.GetString(m_byTextBuffer, 0, len > m_byTextBuffer.Length ? m_byTextBuffer.Length : len);
            windowInfo.WndInfo = wi;
            return windowInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wi"></param>
        /// <param name="bTopWindow">是否是顶级窗体</param>
        /// <returns></returns>
        private TreeNode GetNodeFromWindowInfo(WindowInfo wi, bool bTopWindow) {
            int pid = 0;
            Win32.GetWindowThreadProcessId(wi.Hwnd, ref pid);
            if (m_dic_process.ContainsKey(pid)) wi.Process = m_dic_process[pid];
            TreeNode node = new TreeNode(wi.Hwnd.ToString("X").PadLeft(8, '0') + "|" + wi.WindowText + "|" + wi.ClassName);
            node.ImageKey = "none";
            foreach (var v in imglst_icon.Images.Keys) {        //若ImageList中存在对应Class的图标
                if (wi.ClassName.ToLower().Contains(v)) {
                    node.ImageKey = v;
                    break;
                }
            }
            if (node.ImageKey == "none") {                      //若不存在图标 则尝试直接去获取窗体图标
                IntPtr pi = (IntPtr)Win32.SendMessage(wi.Hwnd, Win32.WM_GETICON, (IntPtr)Win32.ICON_SMALL, IntPtr.Zero);
                if (pi != IntPtr.Zero) {
                    imglst_icon.Images.Add(wi.Hwnd.ToString(), Icon.FromHandle(pi));
                    node.ImageKey = wi.Hwnd.ToString();
                }
            }
            //若依然未获取到图标 并且窗口是顶级窗体 尝试从可执行文件获取图标
            if (node.ImageKey == "none" && bTopWindow && !m_lst_error_pid.Contains(pid)) {
                if (m_dic_process.ContainsKey(pid)) {
                    try {
                        string strKey = "P" + pid.ToString("X");
                        if (!imglst_icon.Images.ContainsKey(strKey)) {
                            Icon i = Icon.ExtractAssociatedIcon(m_dic_process[pid].Modules[0].FileName);
                            imglst_icon.Images.Add(strKey, i);
                        }
                        node.ImageKey = strKey;
                    } catch { m_lst_error_pid.Add(pid); }
                }
            }
            if (wi.ClassName.ToLower().Contains("internet explorer_server")) {
                wi.Document = Win32.GetHtmlDocument(wi.Hwnd);
            }
            m_dic_node.Add(wi.Hwnd, node);
            cmbox_search.Items.Add(node);
            node.SelectedImageKey = node.ImageKey;
            node.Tag = wi;
            return node;
        }

        private void tree_wnd_AfterSelect(object sender, TreeViewEventArgs e) {
            //try {
            WindowInfo wi = (WindowInfo)e.Node.Tag;
            ucWindowInfo1.SetShow(wi);
            m_hWndSelect = wi.Hwnd;
            //} catch { }
        }
        //模糊查询
        private void cmbox_search_TextUpdate(object sender, EventArgs e) {
            try {
                string strText = cmbox_search.Text;
                cmbox_search.Items.Clear();
                cmbox_search.SelectionStart = strText.Length;
                this.Cursor = Cursors.Default;
                List<TreeNode> lst = new List<TreeNode>();
                foreach (var v in m_dic_node.Values) {
                    if (v.Text.ToLower().Contains(strText)) lst.Add(v);
                }
                cmbox_search.Items.AddRange(lst.ToArray());
                cmbox_search.DroppedDown = lst.Count != 0;
                //cmbox_search.DroppedDown = lst.Count != 0;
            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cmbox_search_SelectedIndexChanged(object sender, EventArgs e) {
            if (cmbox_search.SelectedItem != null) {
                this.SelectNode((TreeNode)cmbox_search.SelectedItem);
            }
        }

        private void picbox_spy_MouseDown(object sender, MouseEventArgs e) {
            m_frmRect.Show();
            timer1.Enabled = true;
        }

        private void picbox_spy_MouseUp(object sender, MouseEventArgs e) {
            m_frmRect.Hide();
            timer1.Enabled = false;
            if (m_dic_node.ContainsKey(m_hWndTemp)) this.SelectNode(m_dic_node[m_hWndTemp]);
            else this.LoadTreeWnd(m_hWndTemp);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            Win32.POINT pt = new Win32.POINT();
            pt.X = MousePosition.X;
            pt.Y = MousePosition.Y;
            IntPtr hWnd = Win32.WindowFromPoint(pt);
            if (hWnd == m_hWndTemp) return;
            Console.WriteLine(hWnd + "\t" + MousePosition.ToString());
            m_hWndTemp = hWnd;
            WindowInfo wi = this.GetWindowInfo(hWnd);
            ucWindowInfo1.SetShow(wi, true);
            m_frmRect.AnimateSelect(wi.WndInfo.rcWindow.ToRectangle());
        }

        private void SelectNode(TreeNode node) {
            if (m_nodeSelect != null) {
                m_nodeSelect.ForeColor = m_clrTemp;
            }
            m_nodeSelect = node;
            m_clrTemp = node.ForeColor;
            tree_wnd.SelectedNode = node;
            node.ForeColor = Color.Blue;
            m_hWndSelect = ((WindowInfo)node.Tag).Hwnd;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            new FrmAbout().ShowDialog();
        }
    }
}
