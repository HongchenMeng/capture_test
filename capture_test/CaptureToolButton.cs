using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace DevCapture
{
    [Designer(typeof(CaptureToolButtonDesigner))]
    public class CaptureToolButton : Control
    {
        public CaptureToolButton() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.BackColor = Color.Transparent;
        }

        private Image _Image;
        public Image Image {
            get { return _Image; }
            set {
                _Image = value;
                this.Invalidate();
            }
        }

        private bool _IsCheckButton;
        public bool IsCheckButton {
            get { return _IsCheckButton; }
            set {
                _IsCheckButton = value;
                if (!_IsCheckButton) this._IsRadioButton = false;
            }
        }

        private bool _IsRadioButton;
        public bool IsRadioButton {
            get { return _IsRadioButton; }
            set {
                _IsRadioButton = value;
                if (_IsRadioButton) this._IsCheckButton = true;
            }
        }

        private bool _IsSelected;
        public bool IsSelected {
            get { return _IsSelected; }
            set {
                if (value == _IsSelected) return;
                _IsSelected = value;
                this.Invalidate();
            }
        }

        public override string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                Size se = TextRenderer.MeasureText(this.Text, this.Font);
                this.Width = se.Width + 21;
            }
        }

        private bool m_bMouseEnter;

        protected override void OnMouseEnter(EventArgs e) {
            m_bMouseEnter = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            m_bMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e) {
            if (this._IsCheckButton) {
                if (this._IsSelected) {
                    if (!this._IsRadioButton) {
                        this._IsSelected = false;
                        this.Invalidate();
                    }
                } else {
                    this._IsSelected = true; this.Invalidate();
                    for (int i = 0, len = this.Parent.Controls.Count; i < len; i++) {
                        if (this.Parent.Controls[i] is CaptureToolButton && this.Parent.Controls[i] != this) {
                            if (((CaptureToolButton)(this.Parent.Controls[i])).IsSelected)
                                ((CaptureToolButton)(this.Parent.Controls[i])).IsSelected = false;
                        }
                    }
                }
            }
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e) {
            this.OnClick(e);
            base.OnDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            if (m_bMouseEnter) {
                //g.FillRectangle(Brushes.LightGray, this.ClientRectangle);
                g.DrawRectangle(Pens.DodgerBlue, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            if (this._Image == null) 
                g.DrawImage(global::DevCapture.Properties.Resources.src_image_none, new Rectangle(2, 2, 17, 17));
            else
                g.DrawImage(this._Image, new Rectangle(2, 2, 17, 17));
            using (SolidBrush sb = new SolidBrush(this.ForeColor)) {
                g.DrawString(this.Text, this.Font, sb, 21, (this.Height - this.Font.Height) / 2);
            }
            if (this._IsSelected)
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
            base.SetBoundsCore(x, y, TextRenderer.MeasureText(this.Text, this.Font).Width + 21, 21, specified);
        }
    }
}
