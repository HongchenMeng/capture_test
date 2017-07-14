namespace DevCapture
{
    partial class FrmCapture
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmsi_cancel = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsi_out = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsi_save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tmsi_close = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsi_ok = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tmsi_toggle = new System.Windows.Forms.ToolStripMenuItem();
            this.tmsi_plugin = new System.Windows.Forms.ToolStripMenuItem();
            this.无ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rdbtn_draw = new System.Windows.Forms.RadioButton();
            this.rdbtn_fill = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sizeTrackBar1 = new DevCapture.SizeTrackBar();
            this.ckbox_mosaic = new System.Windows.Forms.CheckBox();
            this.colorBox1 = new DevCapture.ColorBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.captureToolbar1 = new DevCapture.CaptureToolbar();
            this.imageCroppingBox1 = new DevCapture.ImageCroppingBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tmsi_gif = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tmsi_cancel,
            this.tmsi_out,
            this.tmsi_save,
            this.toolStripSeparator2,
            this.tmsi_close,
            this.tmsi_ok,
            this.toolStripSeparator3,
            this.tmsi_toggle,
            this.tmsi_plugin,
            this.toolStripSeparator1,
            this.tmsi_gif});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 220);
            // 
            // tmsi_cancel
            // 
            this.tmsi_cancel.Image = global::DevCapture.Properties.Resources.btn_cancel;
            this.tmsi_cancel.Name = "tmsi_cancel";
            this.tmsi_cancel.Size = new System.Drawing.Size(182, 22);
            this.tmsi_cancel.Tag = "btn_cancel";
            this.tmsi_cancel.Text = "撤销操作";
            this.tmsi_cancel.Click += new System.EventHandler(this.tmsi_comm_Click);
            // 
            // tmsi_out
            // 
            this.tmsi_out.Image = global::DevCapture.Properties.Resources.btn_out;
            this.tmsi_out.Name = "tmsi_out";
            this.tmsi_out.Size = new System.Drawing.Size(182, 22);
            this.tmsi_out.Tag = "btn_out";
            this.tmsi_out.Text = "弹出选区";
            this.tmsi_out.Click += new System.EventHandler(this.tmsi_comm_Click);
            // 
            // tmsi_save
            // 
            this.tmsi_save.Image = global::DevCapture.Properties.Resources.btn_save;
            this.tmsi_save.Name = "tmsi_save";
            this.tmsi_save.Size = new System.Drawing.Size(182, 22);
            this.tmsi_save.Tag = "btn_save";
            this.tmsi_save.Text = "保存";
            this.tmsi_save.Click += new System.EventHandler(this.tmsi_comm_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // tmsi_close
            // 
            this.tmsi_close.Image = global::DevCapture.Properties.Resources.btn_close;
            this.tmsi_close.Name = "tmsi_close";
            this.tmsi_close.Size = new System.Drawing.Size(182, 22);
            this.tmsi_close.Tag = "btn_close";
            this.tmsi_close.Text = "关闭";
            this.tmsi_close.Click += new System.EventHandler(this.tmsi_comm_Click);
            // 
            // tmsi_ok
            // 
            this.tmsi_ok.Image = global::DevCapture.Properties.Resources.btn_ok;
            this.tmsi_ok.Name = "tmsi_ok";
            this.tmsi_ok.Size = new System.Drawing.Size(182, 22);
            this.tmsi_ok.Tag = "btn_ok";
            this.tmsi_ok.Text = "确定";
            this.tmsi_ok.Click += new System.EventHandler(this.tmsi_comm_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(179, 6);
            // 
            // tmsi_toggle
            // 
            this.tmsi_toggle.Name = "tmsi_toggle";
            this.tmsi_toggle.Size = new System.Drawing.Size(182, 22);
            this.tmsi_toggle.Text = "隐藏/显示工具条";
            this.tmsi_toggle.Click += new System.EventHandler(this.tmsi_toggle_Click);
            // 
            // tmsi_plugin
            // 
            this.tmsi_plugin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.无ToolStripMenuItem});
            this.tmsi_plugin.Name = "tmsi_plugin";
            this.tmsi_plugin.Size = new System.Drawing.Size(182, 22);
            this.tmsi_plugin.Text = "插件";
            // 
            // 无ToolStripMenuItem
            // 
            this.无ToolStripMenuItem.Enabled = false;
            this.无ToolStripMenuItem.Name = "无ToolStripMenuItem";
            this.无ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.无ToolStripMenuItem.Text = "无";
            // 
            // rdbtn_draw
            // 
            this.rdbtn_draw.AutoSize = true;
            this.rdbtn_draw.BackColor = System.Drawing.Color.Transparent;
            this.rdbtn_draw.Checked = true;
            this.rdbtn_draw.ForeColor = System.Drawing.Color.White;
            this.rdbtn_draw.Location = new System.Drawing.Point(163, 3);
            this.rdbtn_draw.Name = "rdbtn_draw";
            this.rdbtn_draw.Size = new System.Drawing.Size(49, 17);
            this.rdbtn_draw.TabIndex = 3;
            this.rdbtn_draw.TabStop = true;
            this.rdbtn_draw.Text = "绘制";
            this.rdbtn_draw.UseVisualStyleBackColor = false;
            // 
            // rdbtn_fill
            // 
            this.rdbtn_fill.AutoSize = true;
            this.rdbtn_fill.BackColor = System.Drawing.Color.Transparent;
            this.rdbtn_fill.ForeColor = System.Drawing.Color.White;
            this.rdbtn_fill.Location = new System.Drawing.Point(218, 3);
            this.rdbtn_fill.Name = "rdbtn_fill";
            this.rdbtn_fill.Size = new System.Drawing.Size(49, 17);
            this.rdbtn_fill.TabIndex = 4;
            this.rdbtn_fill.Text = "填充";
            this.rdbtn_fill.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.sizeTrackBar1);
            this.panel1.Controls.Add(this.ckbox_mosaic);
            this.panel1.Controls.Add(this.colorBox1);
            this.panel1.Controls.Add(this.rdbtn_draw);
            this.panel1.Controls.Add(this.rdbtn_fill);
            this.panel1.Location = new System.Drawing.Point(42, 130);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 49);
            this.panel1.TabIndex = 6;
            // 
            // sizeTrackBar1
            // 
            this.sizeTrackBar1.BackColor = System.Drawing.Color.Transparent;
            this.sizeTrackBar1.Color = System.Drawing.Color.Red;
            this.sizeTrackBar1.Location = new System.Drawing.Point(163, 27);
            this.sizeTrackBar1.MaxValue = 30;
            this.sizeTrackBar1.MinValue = 1;
            this.sizeTrackBar1.Name = "sizeTrackBar1";
            this.sizeTrackBar1.Size = new System.Drawing.Size(182, 19);
            this.sizeTrackBar1.TabIndex = 7;
            this.sizeTrackBar1.Text = "sizeTrackBar1";
            this.sizeTrackBar1.Value = 1;
            // 
            // ckbox_mosaic
            // 
            this.ckbox_mosaic.AutoSize = true;
            this.ckbox_mosaic.BackColor = System.Drawing.Color.Transparent;
            this.ckbox_mosaic.ForeColor = System.Drawing.Color.White;
            this.ckbox_mosaic.Location = new System.Drawing.Point(273, 4);
            this.ckbox_mosaic.Name = "ckbox_mosaic";
            this.ckbox_mosaic.Size = new System.Drawing.Size(62, 17);
            this.ckbox_mosaic.TabIndex = 6;
            this.ckbox_mosaic.Text = "马赛克";
            this.ckbox_mosaic.UseVisualStyleBackColor = false;
            // 
            // colorBox1
            // 
            this.colorBox1.Alpha = ((byte)(255));
            this.colorBox1.BackColor = System.Drawing.Color.Transparent;
            this.colorBox1.Color = System.Drawing.Color.Red;
            this.colorBox1.Location = new System.Drawing.Point(3, 3);
            this.colorBox1.Name = "colorBox1";
            this.colorBox1.Size = new System.Drawing.Size(154, 45);
            this.colorBox1.TabIndex = 2;
            this.colorBox1.Text = "colorBox1";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(45, 185);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 7;
            // 
            // captureToolbar1
            // 
            this.captureToolbar1.BackColor = System.Drawing.Color.Transparent;
            this.captureToolbar1.Location = new System.Drawing.Point(42, 97);
            this.captureToolbar1.Name = "captureToolbar1";
            this.captureToolbar1.Size = new System.Drawing.Size(348, 27);
            this.captureToolbar1.TabIndex = 1;
            this.captureToolbar1.ToolButtonClick += new System.EventHandler(this.captureToolbar1_ToolButtonClick);
            // 
            // imageCroppingBox1
            // 
            this.imageCroppingBox1.BackColor = System.Drawing.Color.Black;
            this.imageCroppingBox1.Image = null;
            this.imageCroppingBox1.IsDrawMagnifier = false;
            this.imageCroppingBox1.IsLockSelected = false;
            this.imageCroppingBox1.IsSetClip = true;
            this.imageCroppingBox1.Location = new System.Drawing.Point(12, 12);
            this.imageCroppingBox1.MaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.imageCroppingBox1.Name = "imageCroppingBox1";
            this.imageCroppingBox1.PreViewRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.imageCroppingBox1.SelectedRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.imageCroppingBox1.Size = new System.Drawing.Size(517, 388);
            this.imageCroppingBox1.TabIndex = 0;
            this.imageCroppingBox1.Text = "imageCroppingBox1";
            this.imageCroppingBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageCroppingBox1_Paint);
            this.imageCroppingBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imageCroppingBox1_KeyDown);
            this.imageCroppingBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imageCroppingBox1_MouseDoubleClick);
            this.imageCroppingBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageCroppingBox1_MouseDown);
            this.imageCroppingBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageCroppingBox1_MouseMove);
            this.imageCroppingBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageCroppingBox1_MouseUp);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkColor = System.Drawing.Color.White;
            this.linkLabel1.Location = new System.Drawing.Point(212, 27);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(55, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "选择字体";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // tmsi_gif
            // 
            this.tmsi_gif.Name = "tmsi_gif";
            this.tmsi_gif.Size = new System.Drawing.Size(182, 22);
            this.tmsi_gif.Text = "设置为GIF录制区域";
            this.tmsi_gif.Click += new System.EventHandler(this.tmsi_gif_Click);
            // 
            // FrmCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 412);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.captureToolbar1);
            this.Controls.Add(this.imageCroppingBox1);
            this.Name = "FrmCapture";
            this.Text = "FrmCaption";
            this.Load += new System.EventHandler(this.FrmCaption_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageCroppingBox imageCroppingBox1;
        private CaptureToolbar captureToolbar1;
        private ColorBox colorBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tmsi_cancel;
        private System.Windows.Forms.ToolStripMenuItem tmsi_out;
        private System.Windows.Forms.ToolStripMenuItem tmsi_save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tmsi_close;
        private System.Windows.Forms.ToolStripMenuItem tmsi_ok;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tmsi_plugin;
        private System.Windows.Forms.ToolStripMenuItem 无ToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdbtn_draw;
        private System.Windows.Forms.RadioButton rdbtn_fill;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ckbox_mosaic;
        private SizeTrackBar sizeTrackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem tmsi_toggle;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tmsi_gif;

    }
}