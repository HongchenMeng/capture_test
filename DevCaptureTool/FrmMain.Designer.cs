namespace DevCaptureTool
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.labelEx1 = new IPlugins.LabelEx();
            this.sbox_ctrl_normal = new IPlugins.SwitchBox();
            this.sbox_alt_normal = new IPlugins.SwitchBox();
            this.sbox_shift_normal = new IPlugins.SwitchBox();
            this.tbx_normal = new IPlugins.TextBoxCu();
            this.tbx_clipboard = new IPlugins.TextBoxCu();
            this.sbox_shift_clipboard = new IPlugins.SwitchBox();
            this.sbox_alt_clipboard = new IPlugins.SwitchBox();
            this.sbox_ctrl_clipboard = new IPlugins.SwitchBox();
            this.labelEx2 = new IPlugins.LabelEx();
            this.tbx_giftoggle = new IPlugins.TextBoxCu();
            this.sbox_shift_giftoggle = new IPlugins.SwitchBox();
            this.sbox_alt_giftoggle = new IPlugins.SwitchBox();
            this.sbox_ctrl_giftoggle = new IPlugins.SwitchBox();
            this.labelEx3 = new IPlugins.LabelEx();
            this.tbx_gifstop = new IPlugins.TextBoxCu();
            this.sbox_shift_gifstop = new IPlugins.SwitchBox();
            this.sbox_alt_gifstop = new IPlugins.SwitchBox();
            this.sbox_ctrl_gifstop = new IPlugins.SwitchBox();
            this.labelEx4 = new IPlugins.LabelEx();
            this.btn_settiong = new IPlugins.ButtonEx();
            this.ckbox_autorun = new IPlugins.CheckBoxEx();
            this.ckbox_capcursor = new IPlugins.CheckBoxEx();
            this.nud_sleep = new IPlugins.NumberUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelEx1
            // 
            this.labelEx1.BackColor = System.Drawing.Color.Transparent;
            this.labelEx1.ForeColor = System.Drawing.Color.Gray;
            this.labelEx1.Location = new System.Drawing.Point(37, 35);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(336, 18);
            this.labelEx1.TabIndex = 0;
            this.labelEx1.Text = "普通截图";
            // 
            // sbox_ctrl_normal
            // 
            this.sbox_ctrl_normal.BackColor = System.Drawing.Color.Transparent;
            this.sbox_ctrl_normal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_ctrl_normal.ForeColor = System.Drawing.Color.White;
            this.sbox_ctrl_normal.IsNO = false;
            this.sbox_ctrl_normal.Location = new System.Drawing.Point(40, 56);
            this.sbox_ctrl_normal.Name = "sbox_ctrl_normal";
            this.sbox_ctrl_normal.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_ctrl_normal.NOText = "Ctrl";
            this.sbox_ctrl_normal.OFFColor = System.Drawing.Color.Gray;
            this.sbox_ctrl_normal.OFFText = "Ctrl";
            this.sbox_ctrl_normal.Size = new System.Drawing.Size(80, 23);
            this.sbox_ctrl_normal.TabIndex = 1;
            this.sbox_ctrl_normal.Text = "switchBox1";
            // 
            // sbox_alt_normal
            // 
            this.sbox_alt_normal.BackColor = System.Drawing.Color.Transparent;
            this.sbox_alt_normal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_alt_normal.ForeColor = System.Drawing.Color.White;
            this.sbox_alt_normal.IsNO = false;
            this.sbox_alt_normal.Location = new System.Drawing.Point(126, 56);
            this.sbox_alt_normal.Name = "sbox_alt_normal";
            this.sbox_alt_normal.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_alt_normal.NOText = "Alt";
            this.sbox_alt_normal.OFFColor = System.Drawing.Color.Gray;
            this.sbox_alt_normal.OFFText = "Alt";
            this.sbox_alt_normal.Size = new System.Drawing.Size(80, 23);
            this.sbox_alt_normal.TabIndex = 2;
            this.sbox_alt_normal.Text = "switchBox2";
            // 
            // sbox_shift_normal
            // 
            this.sbox_shift_normal.BackColor = System.Drawing.Color.Transparent;
            this.sbox_shift_normal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_shift_normal.ForeColor = System.Drawing.Color.White;
            this.sbox_shift_normal.IsNO = false;
            this.sbox_shift_normal.Location = new System.Drawing.Point(212, 56);
            this.sbox_shift_normal.Name = "sbox_shift_normal";
            this.sbox_shift_normal.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_shift_normal.NOText = "Shift";
            this.sbox_shift_normal.OFFColor = System.Drawing.Color.Gray;
            this.sbox_shift_normal.OFFText = "Shift";
            this.sbox_shift_normal.Size = new System.Drawing.Size(80, 23);
            this.sbox_shift_normal.TabIndex = 3;
            this.sbox_shift_normal.Text = "switchBox3";
            // 
            // tbx_normal
            // 
            this.tbx_normal.ForeColor = System.Drawing.Color.DimGray;
            this.tbx_normal.Location = new System.Drawing.Point(298, 56);
            this.tbx_normal.Name = "tbx_normal";
            this.tbx_normal.ReadOnly = true;
            this.tbx_normal.Size = new System.Drawing.Size(75, 23);
            this.tbx_normal.TabIndex = 4;
            this.tbx_normal.Text = "NULL";
            this.tbx_normal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // tbx_clipboard
            // 
            this.tbx_clipboard.ForeColor = System.Drawing.Color.DimGray;
            this.tbx_clipboard.Location = new System.Drawing.Point(298, 108);
            this.tbx_clipboard.Name = "tbx_clipboard";
            this.tbx_clipboard.ReadOnly = true;
            this.tbx_clipboard.Size = new System.Drawing.Size(75, 23);
            this.tbx_clipboard.TabIndex = 9;
            this.tbx_clipboard.Text = "NULL";
            this.tbx_clipboard.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // sbox_shift_clipboard
            // 
            this.sbox_shift_clipboard.BackColor = System.Drawing.Color.Transparent;
            this.sbox_shift_clipboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_shift_clipboard.ForeColor = System.Drawing.Color.White;
            this.sbox_shift_clipboard.IsNO = false;
            this.sbox_shift_clipboard.Location = new System.Drawing.Point(212, 108);
            this.sbox_shift_clipboard.Name = "sbox_shift_clipboard";
            this.sbox_shift_clipboard.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_shift_clipboard.NOText = "Shift";
            this.sbox_shift_clipboard.OFFColor = System.Drawing.Color.Gray;
            this.sbox_shift_clipboard.OFFText = "Shift";
            this.sbox_shift_clipboard.Size = new System.Drawing.Size(80, 23);
            this.sbox_shift_clipboard.TabIndex = 8;
            this.sbox_shift_clipboard.Text = "switchBox4";
            // 
            // sbox_alt_clipboard
            // 
            this.sbox_alt_clipboard.BackColor = System.Drawing.Color.Transparent;
            this.sbox_alt_clipboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_alt_clipboard.ForeColor = System.Drawing.Color.White;
            this.sbox_alt_clipboard.IsNO = false;
            this.sbox_alt_clipboard.Location = new System.Drawing.Point(126, 108);
            this.sbox_alt_clipboard.Name = "sbox_alt_clipboard";
            this.sbox_alt_clipboard.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_alt_clipboard.NOText = "Alt";
            this.sbox_alt_clipboard.OFFColor = System.Drawing.Color.Gray;
            this.sbox_alt_clipboard.OFFText = "Alt";
            this.sbox_alt_clipboard.Size = new System.Drawing.Size(80, 23);
            this.sbox_alt_clipboard.TabIndex = 7;
            this.sbox_alt_clipboard.Text = "switchBox5";
            // 
            // sbox_ctrl_clipboard
            // 
            this.sbox_ctrl_clipboard.BackColor = System.Drawing.Color.Transparent;
            this.sbox_ctrl_clipboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_ctrl_clipboard.ForeColor = System.Drawing.Color.White;
            this.sbox_ctrl_clipboard.IsNO = false;
            this.sbox_ctrl_clipboard.Location = new System.Drawing.Point(40, 108);
            this.sbox_ctrl_clipboard.Name = "sbox_ctrl_clipboard";
            this.sbox_ctrl_clipboard.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_ctrl_clipboard.NOText = "Ctrl";
            this.sbox_ctrl_clipboard.OFFColor = System.Drawing.Color.Gray;
            this.sbox_ctrl_clipboard.OFFText = "Ctrl";
            this.sbox_ctrl_clipboard.Size = new System.Drawing.Size(80, 23);
            this.sbox_ctrl_clipboard.TabIndex = 6;
            this.sbox_ctrl_clipboard.Text = "switchBox6";
            // 
            // labelEx2
            // 
            this.labelEx2.BackColor = System.Drawing.Color.Transparent;
            this.labelEx2.ForeColor = System.Drawing.Color.Gray;
            this.labelEx2.Location = new System.Drawing.Point(37, 87);
            this.labelEx2.Name = "labelEx2";
            this.labelEx2.Size = new System.Drawing.Size(336, 18);
            this.labelEx2.TabIndex = 5;
            this.labelEx2.Text = "从剪切板截图";
            // 
            // tbx_giftoggle
            // 
            this.tbx_giftoggle.ForeColor = System.Drawing.Color.DimGray;
            this.tbx_giftoggle.Location = new System.Drawing.Point(298, 160);
            this.tbx_giftoggle.Name = "tbx_giftoggle";
            this.tbx_giftoggle.ReadOnly = true;
            this.tbx_giftoggle.Size = new System.Drawing.Size(75, 23);
            this.tbx_giftoggle.TabIndex = 14;
            this.tbx_giftoggle.Text = "NULL";
            this.tbx_giftoggle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // sbox_shift_giftoggle
            // 
            this.sbox_shift_giftoggle.BackColor = System.Drawing.Color.Transparent;
            this.sbox_shift_giftoggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_shift_giftoggle.ForeColor = System.Drawing.Color.White;
            this.sbox_shift_giftoggle.IsNO = false;
            this.sbox_shift_giftoggle.Location = new System.Drawing.Point(212, 160);
            this.sbox_shift_giftoggle.Name = "sbox_shift_giftoggle";
            this.sbox_shift_giftoggle.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_shift_giftoggle.NOText = "Shift";
            this.sbox_shift_giftoggle.OFFColor = System.Drawing.Color.Gray;
            this.sbox_shift_giftoggle.OFFText = "Shift";
            this.sbox_shift_giftoggle.Size = new System.Drawing.Size(80, 23);
            this.sbox_shift_giftoggle.TabIndex = 13;
            this.sbox_shift_giftoggle.Text = "switchBox7";
            // 
            // sbox_alt_giftoggle
            // 
            this.sbox_alt_giftoggle.BackColor = System.Drawing.Color.Transparent;
            this.sbox_alt_giftoggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_alt_giftoggle.ForeColor = System.Drawing.Color.White;
            this.sbox_alt_giftoggle.IsNO = false;
            this.sbox_alt_giftoggle.Location = new System.Drawing.Point(126, 160);
            this.sbox_alt_giftoggle.Name = "sbox_alt_giftoggle";
            this.sbox_alt_giftoggle.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_alt_giftoggle.NOText = "Alt";
            this.sbox_alt_giftoggle.OFFColor = System.Drawing.Color.Gray;
            this.sbox_alt_giftoggle.OFFText = "Alt";
            this.sbox_alt_giftoggle.Size = new System.Drawing.Size(80, 23);
            this.sbox_alt_giftoggle.TabIndex = 12;
            this.sbox_alt_giftoggle.Text = "switchBox8";
            // 
            // sbox_ctrl_giftoggle
            // 
            this.sbox_ctrl_giftoggle.BackColor = System.Drawing.Color.Transparent;
            this.sbox_ctrl_giftoggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_ctrl_giftoggle.ForeColor = System.Drawing.Color.White;
            this.sbox_ctrl_giftoggle.IsNO = false;
            this.sbox_ctrl_giftoggle.Location = new System.Drawing.Point(40, 160);
            this.sbox_ctrl_giftoggle.Name = "sbox_ctrl_giftoggle";
            this.sbox_ctrl_giftoggle.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_ctrl_giftoggle.NOText = "Ctrl";
            this.sbox_ctrl_giftoggle.OFFColor = System.Drawing.Color.Gray;
            this.sbox_ctrl_giftoggle.OFFText = "Ctrl";
            this.sbox_ctrl_giftoggle.Size = new System.Drawing.Size(80, 23);
            this.sbox_ctrl_giftoggle.TabIndex = 11;
            this.sbox_ctrl_giftoggle.Text = "switchBox9";
            // 
            // labelEx3
            // 
            this.labelEx3.BackColor = System.Drawing.Color.Transparent;
            this.labelEx3.ForeColor = System.Drawing.Color.Gray;
            this.labelEx3.Location = new System.Drawing.Point(37, 139);
            this.labelEx3.Name = "labelEx3";
            this.labelEx3.Size = new System.Drawing.Size(336, 18);
            this.labelEx3.TabIndex = 10;
            this.labelEx3.Text = "GIF录制(开始/暂停)";
            // 
            // tbx_gifstop
            // 
            this.tbx_gifstop.ForeColor = System.Drawing.Color.DimGray;
            this.tbx_gifstop.Location = new System.Drawing.Point(298, 212);
            this.tbx_gifstop.Name = "tbx_gifstop";
            this.tbx_gifstop.ReadOnly = true;
            this.tbx_gifstop.Size = new System.Drawing.Size(75, 23);
            this.tbx_gifstop.TabIndex = 19;
            this.tbx_gifstop.Text = "NULL";
            this.tbx_gifstop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            // 
            // sbox_shift_gifstop
            // 
            this.sbox_shift_gifstop.BackColor = System.Drawing.Color.Transparent;
            this.sbox_shift_gifstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_shift_gifstop.ForeColor = System.Drawing.Color.White;
            this.sbox_shift_gifstop.IsNO = false;
            this.sbox_shift_gifstop.Location = new System.Drawing.Point(212, 212);
            this.sbox_shift_gifstop.Name = "sbox_shift_gifstop";
            this.sbox_shift_gifstop.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_shift_gifstop.NOText = "Shift";
            this.sbox_shift_gifstop.OFFColor = System.Drawing.Color.Gray;
            this.sbox_shift_gifstop.OFFText = "Shift";
            this.sbox_shift_gifstop.Size = new System.Drawing.Size(80, 23);
            this.sbox_shift_gifstop.TabIndex = 18;
            this.sbox_shift_gifstop.Text = "switchBox10";
            // 
            // sbox_alt_gifstop
            // 
            this.sbox_alt_gifstop.BackColor = System.Drawing.Color.Transparent;
            this.sbox_alt_gifstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_alt_gifstop.ForeColor = System.Drawing.Color.White;
            this.sbox_alt_gifstop.IsNO = false;
            this.sbox_alt_gifstop.Location = new System.Drawing.Point(126, 212);
            this.sbox_alt_gifstop.Name = "sbox_alt_gifstop";
            this.sbox_alt_gifstop.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_alt_gifstop.NOText = "Alt";
            this.sbox_alt_gifstop.OFFColor = System.Drawing.Color.Gray;
            this.sbox_alt_gifstop.OFFText = "Alt";
            this.sbox_alt_gifstop.Size = new System.Drawing.Size(80, 23);
            this.sbox_alt_gifstop.TabIndex = 17;
            this.sbox_alt_gifstop.Text = "switchBox11";
            // 
            // sbox_ctrl_gifstop
            // 
            this.sbox_ctrl_gifstop.BackColor = System.Drawing.Color.Transparent;
            this.sbox_ctrl_gifstop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sbox_ctrl_gifstop.ForeColor = System.Drawing.Color.White;
            this.sbox_ctrl_gifstop.IsNO = false;
            this.sbox_ctrl_gifstop.Location = new System.Drawing.Point(40, 212);
            this.sbox_ctrl_gifstop.Name = "sbox_ctrl_gifstop";
            this.sbox_ctrl_gifstop.NOColor = System.Drawing.Color.DodgerBlue;
            this.sbox_ctrl_gifstop.NOText = "Ctrl";
            this.sbox_ctrl_gifstop.OFFColor = System.Drawing.Color.Gray;
            this.sbox_ctrl_gifstop.OFFText = "Ctrl";
            this.sbox_ctrl_gifstop.Size = new System.Drawing.Size(80, 23);
            this.sbox_ctrl_gifstop.TabIndex = 16;
            this.sbox_ctrl_gifstop.Text = "switchBox12";
            // 
            // labelEx4
            // 
            this.labelEx4.BackColor = System.Drawing.Color.Transparent;
            this.labelEx4.ForeColor = System.Drawing.Color.Gray;
            this.labelEx4.Location = new System.Drawing.Point(37, 191);
            this.labelEx4.Name = "labelEx4";
            this.labelEx4.Size = new System.Drawing.Size(336, 18);
            this.labelEx4.TabIndex = 15;
            this.labelEx4.Text = "GIF录制(结束)";
            // 
            // btn_settiong
            // 
            this.btn_settiong.BackColor = System.Drawing.Color.Transparent;
            this.btn_settiong.FlatAppearance.BorderSize = 0;
            this.btn_settiong.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_settiong.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_settiong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_settiong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.btn_settiong.Location = new System.Drawing.Point(212, 241);
            this.btn_settiong.Name = "btn_settiong";
            this.btn_settiong.Size = new System.Drawing.Size(161, 46);
            this.btn_settiong.TabIndex = 20;
            this.btn_settiong.Text = "设置";
            this.btn_settiong.UseVisualStyleBackColor = false;
            this.btn_settiong.Click += new System.EventHandler(this.btn_settiing_Click);
            // 
            // ckbox_autorun
            // 
            this.ckbox_autorun.AutoSize = true;
            this.ckbox_autorun.BackColor = System.Drawing.Color.Transparent;
            this.ckbox_autorun.ForeColor = System.Drawing.Color.Gray;
            this.ckbox_autorun.Location = new System.Drawing.Point(40, 270);
            this.ckbox_autorun.Name = "ckbox_autorun";
            this.ckbox_autorun.Size = new System.Drawing.Size(74, 17);
            this.ckbox_autorun.TabIndex = 21;
            this.ckbox_autorun.Text = "开机启动";
            this.ckbox_autorun.UseVisualStyleBackColor = true;
            // 
            // ckbox_capcursor
            // 
            this.ckbox_capcursor.AutoSize = true;
            this.ckbox_capcursor.BackColor = System.Drawing.Color.Transparent;
            this.ckbox_capcursor.ForeColor = System.Drawing.Color.Gray;
            this.ckbox_capcursor.Location = new System.Drawing.Point(126, 270);
            this.ckbox_capcursor.Name = "ckbox_capcursor";
            this.ckbox_capcursor.Size = new System.Drawing.Size(74, 17);
            this.ckbox_capcursor.TabIndex = 22;
            this.ckbox_capcursor.Text = "捕获鼠标";
            this.ckbox_capcursor.UseVisualStyleBackColor = false;
            // 
            // nud_sleep
            // 
            this.nud_sleep.DecimalPlaces = 0;
            this.nud_sleep.ForeColor = System.Drawing.Color.Gray;
            this.nud_sleep.Increment = 1D;
            this.nud_sleep.Location = new System.Drawing.Point(126, 241);
            this.nud_sleep.MaxValue = 10000D;
            this.nud_sleep.MinValue = 100D;
            this.nud_sleep.Name = "nud_sleep";
            this.nud_sleep.Size = new System.Drawing.Size(80, 23);
            this.nud_sleep.TabIndex = 23;
            this.nud_sleep.Text = "numberUpDown1";
            this.nud_sleep.Value = 200D;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(37, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "GIF录制间隔";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ScreenCapture";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 54);
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(410, 300);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nud_sleep);
            this.Controls.Add(this.ckbox_capcursor);
            this.Controls.Add(this.ckbox_autorun);
            this.Controls.Add(this.btn_settiong);
            this.Controls.Add(this.tbx_gifstop);
            this.Controls.Add(this.sbox_shift_gifstop);
            this.Controls.Add(this.sbox_alt_gifstop);
            this.Controls.Add(this.sbox_ctrl_gifstop);
            this.Controls.Add(this.labelEx4);
            this.Controls.Add(this.tbx_giftoggle);
            this.Controls.Add(this.sbox_shift_giftoggle);
            this.Controls.Add(this.sbox_alt_giftoggle);
            this.Controls.Add(this.sbox_ctrl_giftoggle);
            this.Controls.Add(this.labelEx3);
            this.Controls.Add(this.tbx_clipboard);
            this.Controls.Add(this.sbox_shift_clipboard);
            this.Controls.Add(this.sbox_alt_clipboard);
            this.Controls.Add(this.sbox_ctrl_clipboard);
            this.Controls.Add(this.labelEx2);
            this.Controls.Add(this.tbx_normal);
            this.Controls.Add(this.sbox_shift_normal);
            this.Controls.Add(this.sbox_alt_normal);
            this.Controls.Add(this.sbox_ctrl_normal);
            this.Controls.Add(this.labelEx1);
            this.Name = "Form2";
            this.Text = "DevCapture By Crystal_lz";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IPlugins.LabelEx labelEx1;
        private IPlugins.SwitchBox sbox_ctrl_normal;
        private IPlugins.SwitchBox sbox_alt_normal;
        private IPlugins.SwitchBox sbox_shift_normal;
        private IPlugins.TextBoxCu tbx_normal;
        private IPlugins.TextBoxCu tbx_clipboard;
        private IPlugins.SwitchBox sbox_shift_clipboard;
        private IPlugins.SwitchBox sbox_alt_clipboard;
        private IPlugins.SwitchBox sbox_ctrl_clipboard;
        private IPlugins.LabelEx labelEx2;
        private IPlugins.TextBoxCu tbx_giftoggle;
        private IPlugins.SwitchBox sbox_shift_giftoggle;
        private IPlugins.SwitchBox sbox_alt_giftoggle;
        private IPlugins.SwitchBox sbox_ctrl_giftoggle;
        private IPlugins.LabelEx labelEx3;
        private IPlugins.TextBoxCu tbx_gifstop;
        private IPlugins.SwitchBox sbox_shift_gifstop;
        private IPlugins.SwitchBox sbox_alt_gifstop;
        private IPlugins.SwitchBox sbox_ctrl_gifstop;
        private IPlugins.LabelEx labelEx4;
        private IPlugins.ButtonEx btn_settiong;
        private IPlugins.CheckBoxEx ckbox_autorun;
        private IPlugins.CheckBoxEx ckbox_capcursor;
        private IPlugins.NumberUpDown nud_sleep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;



    }
}