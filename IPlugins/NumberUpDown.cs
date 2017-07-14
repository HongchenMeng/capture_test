using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    public class NumberUpDown : Control
    {
        public NumberUpDown() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            m_tbx = new TextBox();
            m_tbx.Multiline = true;
            m_tbx.BorderStyle = BorderStyle.None;
            this.BackColor = Color.Transparent;
            m_tbx.Text = "50";
            m_tbx.GotFocus += (s, e) => { m_bFocus = true; this.Invalidate(); };
            m_tbx.LostFocus += (s, e) => { m_bFocus = false; this.Invalidate(); };
            m_tbx.KeyDown += (s, e) => this.OnKeyDown(e);
            m_tbx.KeyPress += (s, e) => this.OnKeyPress(e);
            m_tbx.KeyUp += (s, e) => this.OnKeyUp(e);
            m_tbx.TextChanged += (s, e) => this.OnValueChanged(e);
            m_tbx.BackColorChanged += (s, e) => m_tbx.BackColor = Color.White;
            this.Controls.Add(m_tbx);
        }

        private double _Value = 50;

        public double Value {
            get { return _Value; }
            set {
                if (value == _Value) return;
                this.SetValue(value);
            }
        }

        private double _MinValue;

        public double MinValue {
            get { return _MinValue; }
            set {
                if (value == _MinValue) return;
                if (value >= this._MaxValue) throw new ArgumentException("The [MinValue] must be less than [MaxValue]");
                _MinValue = value;
                if (this._MinValue > this._Value) this.Value = this._MinValue;
            }
        }

        private double _MaxValue = 100;

        public double MaxValue {
            get { return _MaxValue; }
            set {
                if (value == _MaxValue) return;
                if (value <= this._MinValue) throw new ArgumentException("The [MaxValue] must be more than [MinValue]");
                _MaxValue = value;
                if (this._MaxValue < this._Value) this.Value = this._MaxValue;
            }
        }

        private double _Increment = 1;

        public double Increment {
            get { return _Increment; }
            set {
                if (value < 0) throw new ArgumentException("The [Increment] must be more than zero");
                if (value == _Increment) return;
                _Increment = value;
            }
        }

        private int _DecimalPlaces;

        public int DecimalPlaces {
            get { return _DecimalPlaces; }
            set {
                if (value < 0) value = 0;
                if (value == _DecimalPlaces) return;
                _DecimalPlaces = value;
                m_tbx.Text = this._Value.ToString("F" + this._DecimalPlaces);
            }
        }

        public override Color BackColor {
            get { return Color.Transparent; }
            set { }
        }

        public override Color ForeColor {
            get { return base.ForeColor; }
            set {
                if (base.ForeColor == value) return;
                base.ForeColor = value;
                m_tbx.ForeColor = value;
            }
        }

        private TextBox m_tbx;
        private bool m_bFocus;
        private Rectangle m_rectUp;
        private Rectangle m_rectDown;
        private bool m_bMouseOnUp;
        private bool m_bMouseOnDown;

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e) {
            if (this.ValueChanged != null) this.ValueChanged(this, e);
        }

        protected override void OnResize(EventArgs e) {
            m_tbx.Left = m_tbx.Top = 3;
            m_tbx.Width = this.Width - 18;
            m_tbx.Height = this.Height - 6;
            m_rectUp = new Rectangle(this.Width - 15, 0, 15, this.Height / 2);
            m_rectDown = new Rectangle(this.Width - 15, this.Height / 2, 15, this.Height / 2);
            m_tbx.Validating += m_tbx_Validating;
            base.OnResize(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (m_rectUp.Contains(e.Location)) {
                if (!m_bMouseOnUp) {
                    m_bMouseOnUp = true;
                    this.Invalidate();
                }
            } else {
                if (m_bMouseOnUp) {
                    m_bMouseOnUp = false;
                    this.Invalidate();
                }
            }
            if (m_rectDown.Contains(e.Location)) {
                if (!m_bMouseOnDown) {
                    m_bMouseOnDown = true;
                    this.Invalidate();
                }
            } else {
                if (m_bMouseOnDown) {
                    m_bMouseOnDown = false;
                    this.Invalidate();
                }
            }
            base.OnMouseMove(e);
        }

        private void m_tbx_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            double dValue = this._Value;
            try {
                dValue = double.Parse(m_tbx.Text);
            } catch {
                m_tbx.Text = dValue.ToString("F" + this._DecimalPlaces);
                return;
            }
            this.Value = dValue;
        }

        private void SetValue(double dValue) {
            if (dValue < this._MinValue) dValue = this._MinValue;
            if (dValue > this._MaxValue) dValue = this._MaxValue;
            if (dValue != this._Value) {
                m_tbx.Text = dValue.ToString("F" + this._DecimalPlaces);
                this._Value = dValue;
                this.OnValueChanged(EventArgs.Empty);
            }

        }

        protected override void OnMouseLeave(EventArgs e) {
            m_bMouseOnUp = m_bMouseOnDown = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnClick(EventArgs e) {
            if (m_bMouseOnUp) this.Value += this._Increment;
            if (m_bMouseOnDown) this.Value -= this._Increment;
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            using (SolidBrush sb = new SolidBrush(Color.White)) {
                g.FillPath(
                    sb,
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 5)
                    );
            }
            g.Clip = new Region(new Rectangle(this.Width - 15, 0, 15, this.Height));
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 1), new Point(0, this.Height),
               Color.White, Color.LightGray)) {
                g.FillPath(
                    lgb,
                    RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 1), 5)
                    );
            }
            g.ResetClip();
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 1), new Point(0, this.Height),
               Color.LightGray, Color.White))
            using (Pen p = new Pen(lgb)) {
                g.DrawPath(p, RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 1, this.Width - 1, this.Height - 2), 5));
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 0), new Point(0, this.Height - 1),
               (m_bFocus || this.Focused) ? Color.DodgerBlue : Color.Gray, Color.FromArgb(20, 0, 0, 0)))
            using (Pen p = new Pen(lgb)) {
                g.DrawPath(p, RenderHelper.CreateRoundedRectanglePath(new Rectangle(0, 0, this.Width - 1, this.Height - 2), 5));
            }
            using (Pen p = new Pen(Color.FromArgb(200, 200, 200))) {
                g.DrawLine(p, this.Width - 15, 1, this.Width - 15, this.Height - 2);
                p.Color = Color.FromArgb(180, 180, 180);
                g.DrawLine(p, this.Width - 15, this.Height / 2, this.Width, this.Height / 2);
            }

            g.SmoothingMode = SmoothingMode.None;
            g.FillPolygon(m_bMouseOnUp ? Brushes.DodgerBlue : Brushes.Gray, new Point[]{
                new Point(this.Width - 12,9),
                new Point(this.Width - 4,9),
                new Point(this.Width - 8,4)
            });
            g.FillPolygon(m_bMouseOnDown ? Brushes.DodgerBlue : Brushes.Gray, new Point[]{
                new Point(this.Width - 11,14),
                new Point(this.Width - 4,14),
                new Point(this.Width - 8,18)
            });
            base.OnPaint(e);
        }
    }
}
