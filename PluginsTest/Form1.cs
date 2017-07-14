using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PluginsTest
{
    public partial class Form1 : Form
    {
        public Form1() {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private Image m_img;
        private IPlugins.IFilter m_i = new Gray.PluginBinary();

        private void Form1_Load(object sender, EventArgs e) {
            m_img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(m_img)) {
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.Location, Point.Empty, m_img.Size);
            }
            pictureBox1.Image = m_img;
        }

        private void button1_Click(object sender, EventArgs e) {
            var result = m_i.ExecFilter(m_img as Image);
            if (result.IsClose)this.Close();
            if (!result.IsModified) return;
            pictureBox1.Image = result.ResultImage;
        }
    }
}
