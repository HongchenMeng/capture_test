namespace SpyTool
{
    partial class FrmSpy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSpy));
            this.imglst_icon = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbox_tran = new System.Windows.Forms.CheckBox();
            this.chbox_visable = new System.Windows.Forms.CheckBox();
            this.picbox_refresh = new System.Windows.Forms.PictureBox();
            this.picbox_spy = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tree_wnd = new System.Windows.Forms.TreeView();
            this.cmbox_search = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ucWindowInfo1 = new SpyTool.UcWindowInfo();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_spy)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imglst_icon
            // 
            this.imglst_icon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglst_icon.ImageStream")));
            this.imglst_icon.TransparentColor = System.Drawing.Color.Transparent;
            this.imglst_icon.Images.SetKeyName(0, "button");
            this.imglst_icon.Images.SetKeyName(1, "combobox");
            this.imglst_icon.Images.SetKeyName(2, "edit");
            this.imglst_icon.Images.SetKeyName(3, "internet explorer_server");
            this.imglst_icon.Images.SetKeyName(4, "none");
            this.imglst_icon.Images.SetKeyName(5, "progress");
            this.imglst_icon.Images.SetKeyName(6, "richedit");
            this.imglst_icon.Images.SetKeyName(7, "scrollbar");
            this.imglst_icon.Images.SetKeyName(8, "static");
            this.imglst_icon.Images.SetKeyName(9, "statusbar");
            this.imglst_icon.Images.SetKeyName(10, "datetimepick");
            this.imglst_icon.Images.SetKeyName(11, "syslink");
            this.imglst_icon.Images.SetKeyName(12, "listview");
            this.imglst_icon.Images.SetKeyName(13, "tabcontrol");
            this.imglst_icon.Images.SetKeyName(14, "systreeview");
            this.imglst_icon.Images.SetKeyName(15, "toolbar");
            this.imglst_icon.Images.SetKeyName(16, "trackbar");
            this.imglst_icon.Images.SetKeyName(17, "windowsforms10.window");
            this.imglst_icon.Images.SetKeyName(18, "#32769");
            this.imglst_icon.Images.SetKeyName(19, "commandbar");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.chbox_tran);
            this.panel1.Controls.Add(this.chbox_visable);
            this.panel1.Controls.Add(this.picbox_refresh);
            this.panel1.Controls.Add(this.picbox_spy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 33);
            this.panel1.TabIndex = 1;
            // 
            // chbox_tran
            // 
            this.chbox_tran.AutoSize = true;
            this.chbox_tran.Location = new System.Drawing.Point(157, 9);
            this.chbox_tran.Name = "chbox_tran";
            this.chbox_tran.Size = new System.Drawing.Size(83, 17);
            this.chbox_tran.TabIndex = 3;
            this.chbox_tran.Text = "Transparent";
            this.chbox_tran.UseVisualStyleBackColor = true;
            // 
            // chbox_visable
            // 
            this.chbox_visable.AutoSize = true;
            this.chbox_visable.Location = new System.Drawing.Point(61, 9);
            this.chbox_visable.Name = "chbox_visable";
            this.chbox_visable.Size = new System.Drawing.Size(81, 17);
            this.chbox_visable.TabIndex = 2;
            this.chbox_visable.Text = "VisableOnly";
            this.chbox_visable.UseVisualStyleBackColor = true;
            // 
            // picbox_refresh
            // 
            this.picbox_refresh.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_refresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picbox_refresh.Image = global::SpyTool.Properties.Resources.refresh;
            this.picbox_refresh.Location = new System.Drawing.Point(32, 4);
            this.picbox_refresh.Name = "picbox_refresh";
            this.picbox_refresh.Size = new System.Drawing.Size(23, 23);
            this.picbox_refresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picbox_refresh.TabIndex = 1;
            this.picbox_refresh.TabStop = false;
            // 
            // picbox_spy
            // 
            this.picbox_spy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picbox_spy.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picbox_spy.Image = global::SpyTool.Properties.Resources.spy;
            this.picbox_spy.Location = new System.Drawing.Point(3, 4);
            this.picbox_spy.Name = "picbox_spy";
            this.picbox_spy.Size = new System.Drawing.Size(23, 23);
            this.picbox_spy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picbox_spy.TabIndex = 0;
            this.picbox_spy.TabStop = false;
            this.picbox_spy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picbox_spy_MouseDown);
            this.picbox_spy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picbox_spy_MouseUp);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tree_wnd);
            this.splitContainer1.Panel1.Controls.Add(this.cmbox_search);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucWindowInfo1);
            this.splitContainer1.Size = new System.Drawing.Size(733, 555);
            this.splitContainer1.SplitterDistance = 351;
            this.splitContainer1.TabIndex = 3;
            // 
            // tree_wnd
            // 
            this.tree_wnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_wnd.Location = new System.Drawing.Point(0, 21);
            this.tree_wnd.Name = "tree_wnd";
            this.tree_wnd.Size = new System.Drawing.Size(351, 534);
            this.tree_wnd.TabIndex = 2;
            this.tree_wnd.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_wnd_AfterSelect);
            // 
            // cmbox_search
            // 
            this.cmbox_search.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbox_search.FormattingEnabled = true;
            this.cmbox_search.Location = new System.Drawing.Point(0, 0);
            this.cmbox_search.Name = "cmbox_search";
            this.cmbox_search.Size = new System.Drawing.Size(351, 21);
            this.cmbox_search.TabIndex = 1;
            this.cmbox_search.SelectedIndexChanged += new System.EventHandler(this.cmbox_search_SelectedIndexChanged);
            this.cmbox_search.TextUpdate += new System.EventHandler(this.cmbox_search_TextUpdate);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(246, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "&About";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ucWindowInfo1
            // 
            this.ucWindowInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucWindowInfo1.Location = new System.Drawing.Point(0, 0);
            this.ucWindowInfo1.Name = "ucWindowInfo1";
            this.ucWindowInfo1.Size = new System.Drawing.Size(378, 555);
            this.ucWindowInfo1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 588);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbox_spy)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imglst_icon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UcWindowInfo ucWindowInfo1;
        private System.Windows.Forms.PictureBox picbox_refresh;
        private System.Windows.Forms.PictureBox picbox_spy;
        private System.Windows.Forms.CheckBox chbox_tran;
        private System.Windows.Forms.CheckBox chbox_visable;
        private System.Windows.Forms.TreeView tree_wnd;
        private System.Windows.Forms.ComboBox cmbox_search;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LinkLabel linkLabel1;

    }
}

