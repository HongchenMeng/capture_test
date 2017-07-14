using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IPlugins
{
    public class ResultInfo : IDisposable
    {
        private Image _ResultImage;

        public Image ResultImage {
            get { return _ResultImage; }
        }

        private bool _IsModified;

        public bool IsModified {
            get { return _IsModified; }
        }

        private bool _IsClose;

        public bool IsClose {
            get { return _IsClose; }
        }

        public ResultInfo(Image imgResult, bool bModified, bool bClose) {
            this._ResultImage = imgResult;
            this._IsModified = bModified;
            this._IsClose = bClose;
        }

        public void Dispose() {
            if (this._ResultImage != null) this._ResultImage.Dispose();
        }
    }
}
