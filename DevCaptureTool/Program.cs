using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace DevCaptureTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            bool bCreateNew = false;
            using (Mutex mutext = new Mutex(true, "DevCapture_By_Crystal", out bCreateNew)) {
                if (bCreateNew) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMain());
                } else {
                    new DevCapture.FrmTextAlert("已经运行了一个实例").ShowDialog();
                }
            }
        }
    }
}
