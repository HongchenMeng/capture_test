using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DevCapture
{
    /// <summary>
    /// valuechanged
    /// </summary>
    public class SizeTrackBar : Control
    {
        public SizeTrackBar() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        private int _Value = 1;

        public int Value {
            get { return _Value; }
            set {
                if (value < _MinValue || value > _MaxValue)
                    throw new ArgumentOutOfRangeException("the [Value] must be between the [MinValue] and [MaxValue]");
                if (value == _Value) return;
                _Value = value;
                this.Invalidate();
                this.OnValueChanged(new EventArgs());
            }
        }

        private int _MinValue = 1;

        public int MinValue {
            get { return _MinValue; }
            set {
                if (_MinValue >= _MaxValue) throw new ArgumentException("the [MinValue] must be less than [MaxValue]");
                if (value == _MinValue) return;
                _MinValue = value;
                this.Invalidate();
            }
        }

        private int _MaxValue = 30;

        public int MaxValue {
            get { return _MaxValue; }
            set {
                if (value <= _MinValue) throw new ArgumentException("the [MaxValue] must be more than [MinValue]");
                if (value == _MaxValue) return;
                _MaxValue = value;
                this.Invalidate();
            }
        }

        private Color _Color = Color.Red;

        public Color Color {
            get { return _Color; }
            set {
                if (value == _Color) return;
                _Color = value;
                this.Invalidate();
            }
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e){
            if(this.ValueChanged != null) this.ValueChanged(this,e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            int nTemp = (int)(this._MinValue + ((double)this._MaxValue - this._MinValue) * e.X / this.Width);
            if (nTemp < this._MinValue) nTemp = this._MinValue;
            if (nTemp > this._MaxValue) nTemp = this._MaxValue;
            if (nTemp == this._Value) {
                base.OnMouseDown(e);
                return;
            }
            this._Value = nTemp;
            this.Invalidate();
            base.OnMouseDown(e);
            this.OnValueChanged(new EventArgs());
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                int nTemp = (int)(this._MinValue + ((double)this._MaxValue - this._MinValue) * e.X / this.Width);
                if (nTemp < this._MinValue) nTemp = this._MinValue;
                if (nTemp > this._MaxValue) nTemp = this._MaxValue;
                if (nTemp == this._Value) {
                    base.OnMouseMove(e);
                    return;
                }
                this._Value = nTemp;
                this.Invalidate();
            }
            base.OnMouseMove(e);
            this.OnValueChanged(new EventArgs());
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(new Point[]{
                new Point(0,this.Height - 1),
                new Point(this.Width,0),
                new Point(this.Width,this.Height)
            });
            using (SolidBrush sb = new SolidBrush(this._Color)) {
                g.FillPath(sb, gp);
            }
            g.DrawString("Size", this.Font, Brushes.White, 0, 0);
            using (Pen p = new Pen(Color.FromArgb(255 - this._Color.R, 255 - this._Color.G, 255 - this._Color.B))) {
                int nLeft = (int)((double)this.Width * (this._Value - this.MinValue) / (this._MaxValue - this._MinValue));
                if (nLeft >= this.Width) nLeft = this.Width - 1;
                else if (nLeft < 0) nLeft = 0;
                g.DrawLine(p, nLeft, 0, nLeft, this.Height);
            }
            base.OnPaint(e);
        }
    }
}
