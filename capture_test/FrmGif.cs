using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevCapture
{
    public partial class FrmGif : IPlugins.FrmBase
    {
        public FrmGif(Dictionary<Image, int> dicFrames) {
            InitializeComponent();

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Sizeable = false;
            m_dicFrames = dicFrames;
            m_size = Size.Empty;
            foreach (var v in m_dicFrames) {
                m_size = v.Key.Size;
                break;
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.Width = this.ClientSize.Width;
            if ((m_size.Width + 2) > pictureBox1.Width) pictureBox1.Width = m_size.Width + 2;
            if ((m_size.Height + 2) > pictureBox1.Height) pictureBox1.Height = m_size.Height + 2;
            this.Size = new Size(pictureBox1.Width + 2, pictureBox1.Height + 75);
        }

        private Size m_size;
        private Dictionary<Image, int> m_dicFrames;

        private void FrmGif_Load(object sender, EventArgs e) {
            this.BeginInvoke(new MethodInvoker(() => this.switchBox1.IsNO = false));
        }

        private void btn_copy_Click(object sender, EventArgs e) {
            if (!System.IO.Directory.Exists("./temp")) System.IO.Directory.CreateDirectory("./temp");
            string strTempFile = "./temp/DevCap_"/*Developer Capture*/ + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".gif";
            strTempFile = System.IO.Path.GetFullPath(strTempFile);
            pictureBox1.Image.Save(strTempFile);
            var i = new DataObject();
            byte[] byData = Encoding.UTF8.GetBytes("<QQRichEditFormat><Info version=\"1001\"></Info><EditElement type=\"1\" filepath=\"" + strTempFile + "\" shortcut=\"\"></EditElement><EditElement type=\"0\"><![CDATA[]]></EditElement></QQRichEditFormat>");
            i.SetData("QQ_Unicode_RichEdit_Format", new System.IO.MemoryStream(byData));
            i.SetData("QQ_RichEdit_Format", new System.IO.MemoryStream(byData));
            i.SetData("FileDrop", new string[] { strTempFile });
            i.SetData("FileNameW", new string[] { strTempFile });
            i.SetData("FileName", new string[] { strTempFile });
            i.SetData("DeviceIndependentBitmap", pictureBox1.Image);
            Clipboard.SetDataObject(i, true);
        }

        private void btn_save_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.gif|*.gif";
            sfd.FileName = "DevCap_"/*Developer Capture*/ + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".gif";
            if (sfd.ShowDialog() != DialogResult.OK) return;
            pictureBox1.Image.Save(sfd.FileName);
            this.Close();
        }

        private void switchBox1_SwitchChanged(object sender, EventArgs e) {
            pictureBox1.Image = null;
            btn_copy.Enabled = btn_save.Enabled = switchBox1.Enabled = false;
            new System.Threading.Thread(() => {
                GIFCreator gc = new GIFCreator(m_size.Width, m_size.Height, switchBox1.IsNO ? GIFCreator.GIFColorDepth.Depth8Bit : GIFCreator.GIFColorDepth.Depth4Bit);
                int i = 0;
                DateTime dt = DateTime.Now;
                foreach (var v in m_dicFrames) {
                    gc.AddFrame(v.Key, v.Value);
                    this.BeginInvoke(new MethodInvoker(() => this.Text = "处理帧 -> " + (++i) + "/" + m_dicFrames.Count + "[" + DateTime.Now.Subtract(dt).TotalSeconds.ToString("F3") + "]"));
                    Application.DoEvents();
                }
                pictureBox1.Invoke(new MethodInvoker(() => pictureBox1.Image = gc.GetGifImage()));
                int nSize = gc.Size;
                double dSize = nSize;
                string strUnit = "b";
                if (dSize / 1024 >= 1) {
                    dSize /= 1024;
                    strUnit = "kb";
                }
                if (dSize / 1024 >= 1) {
                    dSize /= 1024;
                    strUnit = "mb";
                }
                this.BeginInvoke(new MethodInvoker(() => {
                    this.Text += "(" + dSize.ToString("F2") + strUnit + "," + nSize + "b)";
                    btn_copy.Enabled = btn_save.Enabled = switchBox1.Enabled = true;
                }));
            }) { IsBackground = true }.Start();
        }
    }
}
