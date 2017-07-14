using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SpyTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] strArgs) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IntPtr hWnd = IntPtr.Zero;
            bool bVisable = true;
            bool bTransparent = false;
            for (int i = 0; i < strArgs.Length && i < 3; i++) {
                switch (i) {
                    case 0: try { hWnd = (IntPtr)int.Parse(strArgs[0]); } catch { } break;
                    case 1: try { bVisable = Convert.ToBoolean(strArgs[1]); } catch { } break;
                    case 2: try { bTransparent = Convert.ToBoolean(strArgs[2]); } catch { } break;
                }
            }
            Application.Run(new FrmSpy(hWnd, bVisable, bTransparent));
        }
    }
}
