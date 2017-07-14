using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Drawing2D;

namespace DevCapture
{
    [DefaultEvent("ColorChanged")]
    public class ColorBox : Control
    {
        public ColorBox() {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Color = Color.Red;
        }

        private int m_nIndexMOuseON;
        private bool m_bMoveTrack;
        private bool m_bMouseOnResult;
        private Rectangle m_rectTemp;
        private Rectangle m_rectTrack = new Rectangle(2, 34, 150, 8);
        private Rectangle m_rectResult = new Rectangle(2, 2, 28, 28);
        private Rectangle[] m_rectColors;
        private Color[] m_colors = new Color[]{
            Color.Black,Color.DimGray,Color.DarkRed,Color.DarkGoldenrod,
            Color.DarkGreen,Color.DarkBlue,Color.DarkViolet,Color.DarkCyan,
            Color.White,Color.DarkGray,Color.Red,Color.Yellow,
            Color.LightGreen,Color.Blue,Color.Fuchsia,Color.Cyan
        };

        public event EventHandler ColorChanged;

        private Color _Color;

        public Color Color {
            get { return _Color; }
            set {
                if (_Color.ToArgb() == value.ToArgb()) return;
                _Color = value;
                this._SourceColor = Color.FromArgb(255, value.R, value.G, value.B);
                this._Alpha = value.A;
                this.OnColorChanged(new EventArgs());
                this.Invalidate();
            }
        }

        private Color _SourceColor;

        public Color SourceColor {
            get { return _SourceColor; }
        }

        private byte _Alpha;

        public byte Alpha {
            get { return _Alpha; }
            set {
                if (value == _Alpha) return;
                _Alpha = value;
                this._Color = Color.FromArgb(value, this._Color);
                this.Invalidate();
            }
        }

        protected virtual void OnColorChanged(EventArgs e) {
            if (this.ColorChanged != null)
                this.ColorChanged(this, e);
        }

        protected override void OnCreateControl() {
            m_rectColors = new Rectangle[16];
            for (int i = 0; i < 8; i++) {
                m_rectColors[i] = new Rectangle(i * 15 + 34, 2, 13, 13);
            }
            for (int i = 8; i < 16; i++) {
                m_rectColors[i] = new Rectangle((i - 8) * 15 + 34, 17, 13, 13);
            }
            base.OnCreateControl();
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Y < m_rectTrack.Bottom && e.Y > m_rectTrack.Top) {
                this.SetAlpha(e.Location.X);
                m_bMoveTrack = true;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            m_bMoveTrack = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            if (m_rectResult.Contains(e.Location)) {
                if (m_rectTemp != m_rectResult) {
                    m_rectTemp = m_rectResult;
                    m_bMouseOnResult = true;
                    this.Invalidate();
                }
            } else {
                m_bMouseOnResult = false;
                for (int i = 0, len = m_rectColors.Length; i < len; i++) {
                    if (m_rectColors[i].Contains(e.Location)) {
                        if (m_rectTemp != m_rectColors[i]) {
                            m_rectTemp = m_rectColors[i];
                            m_nIndexMOuseON = i;
                            this.Invalidate();
                        }
                        base.OnMouseMove(e);
                        return;
                    }
                }
                if (!m_rectTemp.IsEmpty) {
                    m_nIndexMOuseON = -1;
                    m_rectTemp = Rectangle.Empty;
                    this.Invalidate();
                }
            }
            if (m_bMoveTrack) {
                this.SetAlpha(e.Location.X);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            m_rectTemp = Rectangle.Empty;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e) {
            if (m_bMouseOnResult) {
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK) {
                    this.Color = cd.Color;
                }
            } else {
                if (m_nIndexMOuseON != -1) this.Color = m_colors[m_nIndexMOuseON];
            }
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.DrawImage(Properties.Resources.alpha_back, m_rectResult.X + 2, m_rectResult.Y + 2, 24, 24);
            g.DrawRectangle(Pens.White, m_rectResult.X, m_rectResult.Y, m_rectResult.Width - 1, m_rectResult.Height - 1);
            using (SolidBrush sb = new SolidBrush(Color.White)) {
                for (int i = 0; i < 16; i++) {
                    sb.Color = m_colors[i];
                    g.FillRectangle(sb, m_rectColors[i]);
                    g.DrawRectangle(Pens.White, m_rectColors[i].X, m_rectColors[i].Y, m_rectColors[i].Width - 1, m_rectColors[i].Height - 1);
                }
                sb.Color = this._Color;
                g.FillRectangle(sb, 4, 4, 24, 24);
            }
            if (!m_rectTemp.IsEmpty) g.DrawRectangle(Pens.Cyan, m_rectTemp.X, m_rectTemp.Y, m_rectTemp.Width - 1, m_rectTemp.Height - 1);
            using (LinearGradientBrush lgb = new LinearGradientBrush(new Point(m_rectTrack.Left + 1, m_rectTrack.Top + 1), new Point(m_rectTrack.Right - 1, m_rectTrack.Top + 1), Color.Transparent, this._SourceColor)) {
                g.FillRectangle(lgb, m_rectTrack);
            }
            g.DrawRectangle(Pens.White, m_rectTrack.Left, m_rectTrack.Top, m_rectTrack.Width - 1, m_rectTrack.Height - 1);
            int x = (int)(((double)this._Alpha / 255) * (m_rectTrack.Width - 2) + m_rectTrack.Left);
            if (x < m_rectTrack.Left + 1) x = m_rectTrack.Left + 1;
            g.FillPolygon(Brushes.White, new Point[]{
                    new Point(x,m_rectTrack.Top),
                    new Point(x - 2,m_rectTrack.Top - 3),
                    new Point(x + 3,m_rectTrack.Top - 3)}
                );
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
            width = 154;
            height = 45;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void SetAlpha(int x) {
            if (x <= m_rectTrack.Left) x = m_rectTrack.Left + 1;
            else if (x >= m_rectTrack.Right) x = m_rectTrack.Right - 1;
            this.Alpha = (byte)((((double)x - m_rectTrack.Left - 1) / (m_rectTrack.Width - 2)) * 255);
        }
    }
}
