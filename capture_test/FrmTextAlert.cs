using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevCapture
{
    public partial class FrmTextAlert : Form
    {
        public FrmTextAlert(string strMessage)
            : this(strMessage, null) {

        }

        public FrmTextAlert(string strMessage, Form frm) {
            InitializeComponent();
            m_frm = frm;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0;
            this.BackColor = Color.Black;
            this.TopMost = true;
            label1.Text = strMessage;
            label1.Location = new Point(5, 5);
            label1.ForeColor = Color.White;
            label1.Font = new Font(label1.Font.FontFamily, 14, FontStyle.Bold);
            this.Width = label1.Right + 5;
            this.Height = label1.Bottom + 5;

            if (frm != null) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(m_frm.Left + (m_frm.Width - this.Width) / 2, m_frm.Top + (m_frm.Height - this.Height) / 2);
            } else
                this.StartPosition = FormStartPosition.CenterScreen;
        }

        private Form m_frm;

        private void FrmShowAlert_Load(object sender, EventArgs e) {
            new System.Threading.Thread(() => {
                try {
                    for (int i = 0; i < 10; i++) {
                        this.Invoke(new MethodInvoker(() => this.Opacity += 0.1));
                        System.Threading.Thread.Sleep(20);
                    }
                    System.Threading.Thread.Sleep(1000);
                    for (int i = 0; i < 10; i++) {
                        this.Invoke(new MethodInvoker(() => this.Opacity -= 0.1));
                        System.Threading.Thread.Sleep(20);
                    }
                    this.Invoke(new MethodInvoker(() => this.Close()));
                } catch { }//在执行此代码时如果父窗体关闭 此窗体也会释放 Invoke会报错
            }) { IsBackground = true }.Start();
        }
    }
}
