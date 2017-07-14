using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DevCapture
{
    [Designer(typeof(CaptureToolbarDesigner)), DefaultEvent("ToolButtonClick")]
    public partial class CaptureToolbar : UserControl
    {
        public CaptureToolbar() {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        public event EventHandler ToolButtonClick;

        public virtual string GetSelectBtnName() {
            foreach (Control ctrl in this.Controls) {
                if (ctrl is CaptureToolButton && ((CaptureToolButton)ctrl).IsSelected) return ctrl.Name; 
            }
            return null;
        }

        public virtual void ClearSelect() {
            foreach (Control ctrl in this.Controls) {
                if (ctrl is CaptureToolButton) ((CaptureToolButton)ctrl).IsSelected = false;
            }
        }

        protected virtual void OnToolButtonClick(object sender, EventArgs e) {
            if (this.ToolButtonClick != null) this.ToolButtonClick(sender, e);
        }

        protected override void OnCreateControl() {
            //Win32.SetWindowRgn(this.Handle, Win32.CreateRoundRectRgn(0, 0, this.Width + 1, this.Height + 1, 5, 5), true);
            foreach (Control ctrl in this.Controls) {
                if (ctrl is CaptureToolButton) {
                    ctrl.Click += (s, e) => this.OnToolButtonClick(s, e);
                }
            }
            base.OnCreateControl();
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            RenderHelper.RenderBackground(g, this.ClientRectangle, Properties.Resources.back_toolbar);
            //g.DrawRectangle(Pens.DodgerBlue, 0, 0, this.Width - 1, this.Height - 1);
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
            Size sz = this.InitSize();
            base.SetBoundsCore(x, y, sz.Width, sz.Height, specified);
        }
        /// <summary>
        /// 确定工具栏大小 以及将工具栏内的按钮按照left排序并固定在一条上
        /// </summary>
        /// <returns>工具栏大小</returns>
        private Size InitSize() {
            if (this.Controls.Count == 0) return new Size(75, 30);

            int nTop = 4;
            int nLeft = 4;
            int nHightMax = 0;
            int flag = this.Controls.Count;
            int len = flag;
            int[] indexs = new int[len];

            for (int i = 0; i < len; i++) indexs[i] = i;
            while (flag != 0) {
                len = flag;
                flag = 0;
                for (int i = 1; i < len; i++) {
                    if (this.Controls[indexs[i - 1]].Height > nHightMax)
                        nHightMax = this.Controls[indexs[i - 1]].Height;
                    if (this.Controls[indexs[i - 1]].Left > this.Controls[indexs[i]].Left) {
                        int temp = indexs[i - 1];
                        indexs[i - 1] = indexs[i];
                        indexs[i] = temp;
                        flag = i;
                    }
                }
            }
            nHightMax += 6;
            this.Controls[indexs[0]].Left = nLeft;
            this.Controls[indexs[0]].Top = nTop;
            len = this.Controls.Count;
            for (int i = 1; i < len; i++) {
                this.Controls[indexs[i]].Left = this.Controls[indexs[i - 1]].Right + 6;
                this.Controls[indexs[i]].Top = (nHightMax - this.Controls[indexs[i]].Height) >> 1;
            }
            return new Size(this.Controls[indexs[len - 1]].Right + 4, nHightMax);
        }
    }
}
