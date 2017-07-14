using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace SpyTool
{
    public partial class UcWindowInfo : UserControl
    {
        public UcWindowInfo() {
            InitializeComponent();
            m_dic_style = new Dictionary<uint, string>();
            m_dic_style.Add(0x00000000, "WS_OVERLAPPED");
            m_dic_style.Add(0x80000000, "WS_POPUP");
            m_dic_style.Add(0x40000000, "WS_CHILD");
            m_dic_style.Add(0x20000000, "WS_MINIMIZE");
            m_dic_style.Add(0x10000000, "WS_VISIBLE");
            m_dic_style.Add(0x08000000, "WS_DISABLED");
            m_dic_style.Add(0x04000000, "WS_CLIPSIBLINGS");
            m_dic_style.Add(0x02000000, "WS_CLIPCHILDREN");
            m_dic_style.Add(0x01000000, "WS_MAXIMIZE");
            m_dic_style.Add(0x00800000, "WS_BORDER");
            m_dic_style.Add(0x00400000, "WS_DLGFRAME");
            m_dic_style.Add(0x00200000, "WS_VSCROLL");
            m_dic_style.Add(0x00100000, "WS_HSCROLL");
            m_dic_style.Add(0x00080000, "WS_SYSMENU");
            m_dic_style.Add(0x00040000, "WS_THICKFRAME");
            m_dic_style.Add(0x00020000, "WS_GROUP|WS_MINIMIZEBOX");
            m_dic_style.Add(0x00010000, "WS_TABSTOP|WS_MAXIMIZEBOX");
            m_dic_exstyle = new Dictionary<uint, string>();
            m_dic_exstyle.Add(0x00000001, "WS_EX_DLGMODALFRAME");
            m_dic_exstyle.Add(0x00000004, "WS_EX_NOPARENTNOTIFY");
            m_dic_exstyle.Add(0x00000008, "WS_EX_TOPMOST");
            m_dic_exstyle.Add(0x00000010, "WS_EX_ACCEPTFILES");
            m_dic_exstyle.Add(0x00000020, "WS_EX_TRANSPARENT");
            m_dic_exstyle.Add(0x00000040, "WS_EX_MDICHILD");
            m_dic_exstyle.Add(0x00000080, "WS_EX_TOOLWINDOW");
            m_dic_exstyle.Add(0x00000100, "WS_EX_WINDOWEDGE");
            m_dic_exstyle.Add(0x00000200, "WS_EX_CLIENTEDGE|WS_EX_RTLREADING");
            m_dic_exstyle.Add(0x00000400, "WS_EX_CONTEXTHELP");
            m_dic_exstyle.Add(0x00001000, "WS_EX_RIGHT");
            m_dic_exstyle.Add(0x00000000, "WS_EX_LEFT|WS_EX_LTRREADING|WS_EX_RIGHTSCROLLBAR");
            m_dic_exstyle.Add(0x00004000, "WS_EX_LEFTSCROLLBAR");
            m_dic_exstyle.Add(0x00010000, "WS_EX_CONTROLPARENT");
            m_dic_exstyle.Add(0x00020000, "WS_EX_STATICEDGE");
            m_dic_exstyle.Add(0x00040000, "WS_EX_APPWINDOW");
            m_dic_exstyle.Add(0x00080000, "WS_EX_LAYERED");
            m_dic_exstyle.Add(0x00100000, "WS_EX_NOINHERITLAYOUT");
            m_dic_exstyle.Add(0x00200000, "WS_EX_NOREDIRECTIONBITMAP");
            m_dic_exstyle.Add(0x00400000, "WS_EX_LAYOUTRTL");
            m_dic_exstyle.Add(0x02000000, "WS_EX_COMPOSITED");
            m_dic_exstyle.Add(0x08000000, "WS_EX_NOACTIVATE");
            tab_info.TabPages.Remove(tp_web);
        }

        private Dictionary<uint, string> m_dic_style;
        private Dictionary<uint, string> m_dic_exstyle;
        private System.Diagnostics.Process m_process;

        public void SetShow(WindowInfo wi, bool bJustWindowInfo = false) {
            tbx_class.Text = wi.ClassName;
            tbx_size.Text = wi.WndInfo.cbSize.ToString();
            tbx_rcwindow.Text = wi.WndInfo.rcWindow.ToRectangle().ToString();
            tbx_rcclient.Text = wi.WndInfo.rcClient.ToRectangle().ToString();
            tbx_windowstatus.Text = wi.WndInfo.dwWindowStatus.ToString();
            tbx_cxwindowborders.Text = wi.WndInfo.cxWindowBorders.ToString();
            tbx_cywindowborders.Text = wi.WndInfo.cyWindowBorders.ToString();
            tbx_atomwindowtype.Text = wi.WndInfo.atomWindowType.ToString();
            tbx_creatorversion.Text = "0x" + wi.WndInfo.wCreatorVersion.ToString("X").PadLeft(4, '0');
            tbx_style.Text = "0x" + wi.WndInfo.dwStyle.ToString("X").PadLeft(8, '0') + "\r\n--------\r\n";
            foreach (var v in m_dic_style) {
                if ((wi.WndInfo.dwStyle & v.Key) == v.Key) {
                    tbx_style.Text += "0x" + v.Key.ToString("X").PadLeft(8, '0') + " " + v.Value + "\r\n";
                }
            }
            tbx_exstyle.Text = "0x" + wi.WndInfo.dwExStyle.ToString("X").PadLeft(8, '0') + "\r\n--------\r\n";
            foreach (var v in m_dic_exstyle) {
                if ((wi.WndInfo.dwExStyle & v.Key) == v.Key) {
                    tbx_exstyle.Text += "0x" + v.Key.ToString("X").PadLeft(8, '0') + " " + v.Value + "\r\n";
                }
            }
            tbx_text.Text = wi.WindowText;
            if (bJustWindowInfo) return;
            if (wi.Document == null) {
                tab_info.TabPages.Remove(tp_web);
            } else {
                if (!tab_info.TabPages.Contains(tp_web)) tab_info.TabPages.Add(tp_web);
                this.ShowWeb(wi.Document);
            }
            if (wi.Process == null) {
                web_process.DocumentText = "Can not get process";
                m_process = null;
            } else if (wi.Process != m_process) {
                this.ShowProcess(wi.Process);
                m_process = wi.Process;
                try {
                    lv_modules.Items.Clear();
                    this.ShowModules(wi.Process.Modules);
                } catch { lv_modules.Items.Add(new ListViewItem(new string[] { "-1", "Can not access modules" }) { ForeColor = Color.Red }); }
            }
        }

        private void ShowProcess(Process p) {
            int nTemp = 0;
            string[] strClrs = new string[] { "#FFFFAA", "#AAFFFF" };
            string strHtml = "<html><head><style>*{font-family:'consolas' 'courier new'} table{font-size:14px} td{padding:5px}</style></head><body><center><h1>FileInfo</h1>";
            strHtml += "<table  border='0' cellspacing='1' style='background-color:#00FF00;width:%100'>";
            try {
                strHtml += "<tr style='background-color:#FFFFAA'><td style='width:100px;'>File</td><td>" + this.StringToHtml(p.MainModule.FileVersionInfo.FileName) + "</td></tr>";
                strHtml += "<tr style='background-color:#AAFFFF'><td style='width:100px;'>Version</td><td>" + this.StringToHtml(p.MainModule.FileVersionInfo.FileVersion) + "</td></tr>";
                strHtml += "<tr style='background-color:#FFFFAA'><td style='width:100px;'>Description</td><td>" + this.StringToHtml(p.MainModule.FileVersionInfo.FileDescription) + "</td></tr>";
                strHtml += "<tr style='background-color:#AAFFFF'><td style='width:100px;'>Product</td><td>" + this.StringToHtml(p.MainModule.FileVersionInfo.ProductName) + "</td></tr>";
                strHtml += "<tr style='background-color:#FFFFAA'><td style='width:100px;'>Language</td><td>" + this.StringToHtml(p.MainModule.FileVersionInfo.Language) + "</td></tr>";
            } catch {
                strHtml += "<tr><td>Can not access [MainModule]</td></tr>";
            }
            strHtml += "</table><h1>StartInfo</h1><table  border='0' cellspacing='1' style='background-color:#00FF00;width:%100'>";
            foreach (var v in p.StartInfo.GetType().GetProperties()) {
                try {
                    object objValue = v.GetValue(p.StartInfo, null);
                    string strValue = objValue == null ? "null" : objValue.ToString();
                    if (strValue == "System.String[]") strValue = "[" + string.Join(",", objValue as string[]) + "]";
                    if (strValue.StartsWith("System.")) continue;
                    strHtml += "<tr style='background-color:" + strClrs[nTemp++ % 2] + "'><td style='width:100px;'>" + v.Name + "</td><td>" + this.StringToHtml(strValue) + "</td></tr>";
                } catch { }
            }
            strHtml += "</table><h1>Other</h1><table  border='0' cellspacing='1' style='background-color:#00FF00;width:%100'>";
            foreach (var v in p.GetType().GetProperties()) {
                try {
                    object objValue = v.GetValue(p, null);
                    string strValue = objValue == null ? "null" : objValue.ToString();
                    if (strValue == "System.String[]") strValue = "[" + string.Join(",", objValue as string[]) + "]";
                    if (strValue.StartsWith("System.")) continue;
                    strHtml += "<tr style='background-color:" + strClrs[nTemp++ % 2] + "'><td style='width:100px;'>" + v.Name + "</td><td>" + this.StringToHtml(strValue) + "</td></tr>";
                } catch { }
            }
            strHtml += "</table></center></body></html>";
            web_process.DocumentText = strHtml;
        }

        private void ShowModules(ProcessModuleCollection modules) {
            lv_modules.Items.Clear();
            int index = 0;
            foreach (ProcessModule m in modules) {
                ListViewItem item = new ListViewItem((++index).ToString());
                item.SubItems.Add(m.FileName);
                item.SubItems.Add(m.FileVersionInfo.FileVersion);
                item.SubItems.Add(m.FileVersionInfo.FileDescription);
                item.SubItems.Add(m.FileVersionInfo.ProductName);
                item.SubItems.Add(m.FileVersionInfo.Language);
                item.BackColor = (index % 2 == 0) ? Color.FromArgb(255, 255, 255, 200) : Color.FromArgb(255, 200, 255, 255);
                lv_modules.Items.Add(item);
            }
        }

        private void ShowWeb(mshtml.IHTMLDocument2 doc) {
            tbx_title.Text = doc.title;
            tbx_url.Text = doc.url;
            tbx_refer.Text = doc.referrer;
            tbx_pagesize.Text = doc.fileSize;
            tbx_charset.Text = doc.defaultCharset;
            tbx_createddate.Text = doc.fileCreatedDate;
            tbx_modifieddate.Text = doc.fileModifiedDate;
            tbx_updatedate.Text = doc.fileUpdatedDate;
            tbx_cookie.Text = doc.cookie != null ? string.Join(";\r\n", doc.cookie.Replace("; ", ";").Split(';')) : "";
            var e = ((mshtml.IHTMLDocument3)doc).documentElement;
            tbx_html.Text = e != null ? e.innerHTML : "";
        }

        private string StringToHtml(string strText) {
            return strText.Replace(" ", "&nbsp;")
                            .Replace("<", "&lt;")
                            .Replace(">", "&gt;");
        }
    }
}
