using System;
using System.Collections.Generic;
using System.Text;

using IPlugins;

namespace Gray
{
    public class PluginBinary : IFilter
    {
        public string GetPluginName() {
            return "二值化";
        }

        public System.Drawing.Image GetPluginIcon() {
            return Properties.Resources.icon_binary;
        }

        public void InitPlugin(string strStarPath) {

        }

        public ResultInfo ExecFilter(System.Drawing.Image imgSrc) {
            Form1 frm = new Form1(imgSrc);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                return new ResultInfo(frm.ResultImage, true, false);
            }
            return new ResultInfo(null, false, false);
        }
    }
}
