using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace IPlugins
{
    [DefaultEvent("ValueChanged")]
    public class TrackBox : Control
    {
        public TrackBox() {
            this.SetStyle(ControlStyles.Opaque, false);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.BackColor = Color.Transparent;
        }

        private int _Value = 50;

        public int Value {
            get { return _Value; }
            set {
                if (value < _MinValue) value = _MinValue;
                if (value > _MaxValue) value = _MaxValue;
                if (value == _Value) return;
                _Value = value;
                this.Invalidate();
                this.OnValueChanged(new EventArgs());
            }
        }

        private int _MinValue = 0;

        public int MinValue {
            get { return _MinValue; }
            set {
                if (_MinValue >= _MaxValue) throw new ArgumentException("the [MinValue] must be less than [MaxValue]");
                if (value == _MinValue) return;
                _MinValue = value;
                this.Invalidate();
            }
        }

        private int _MaxValue = 100;

        public int MaxValue {
            get { return _MaxValue; }
            set {
                if (value <= _MinValue) throw new ArgumentException("the [MaxValue] must be more than [MinValue]");
                if (value == _MaxValue) return;
                _MaxValue = value;
                this.Invalidate();
            }
        }

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged(EventArgs e) {
            if (this.ValueChanged != null) this.ValueChanged(this, e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            this.Focus();
            this.SetValue(e.X);
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) this.SetValue(e.X);
            base.OnMouseMove(e);
        }

        //override proce

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            switch (keyData) {
                case Keys.Down:
                case Keys.Left: this.Value--; return true;
                case Keys.Up:
                case Keys.Right: this.Value++; return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //protected override bool ProcessDialogKey(Keys keyData) {
        //    switch (keyData) {
        //        case Keys.Down:
        //        case Keys.Left: this.Value--; break;
        //        case Keys.Up:
        //        case Keys.Right: this.Value++; break;
        //    }
        //    return base.ProcessDialogKey(keyData);
        //}

        //protected override void OnKeyDown(KeyEventArgs e) {
        //    switch (e.KeyCode) {
        //        case Keys.Down:
        //        case Keys.Left: this.Value--; break;
        //        case Keys.Up:
        //        case Keys.Right: this.Value++; break;
        //    }
        //    base.OnKeyDown(e);
        //}

        protected override void OnPaint(PaintEventArgs pevent) {
            Graphics g = pevent.Graphics;

            g.FillRectangle(Brushes.DodgerBlue, 7, 7, this.Width - 14, 3);

            g.SmoothingMode = SmoothingMode.HighQuality;
            int nLeft = (int)((this.Width - 14) * (double)this._Value / (this._MaxValue - this._MinValue));
            if (nLeft < 0) nLeft = 0;
            if (nLeft >= this.Width - 16) nLeft = this.Width - 16;
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 0), new Point(0, this.Height),
               Color.White, Color.FromArgb(240, 240, 240))) {
                g.FillEllipse(lgb, new Rectangle(nLeft, 0, 15, 15));
            }
            using (LinearGradientBrush lgb = new LinearGradientBrush(
               new Point(0, 0), new Point(0, this.Height),
               Color.FromArgb(200, 200, 200), Color.FromArgb(150, 150, 150))) {
                g.DrawEllipse(new Pen(lgb), new Rectangle(nLeft, 0, 15, 15));
            }
            base.OnPaint(pevent);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
            height = 16;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void SetValue(int x) {
            int nTemp = (int)(this._MinValue + ((double)this._MaxValue - this._MinValue) * (x - 7) / (this.Width - 14));
            if (nTemp < this._MinValue) nTemp = this._MinValue;
            if (nTemp > this._MaxValue) nTemp = this._MaxValue;
            if (nTemp == this._Value) return;
            this._Value = nTemp;
            this.Invalidate();
            this.OnValueChanged(new EventArgs());
        }
    }
}
