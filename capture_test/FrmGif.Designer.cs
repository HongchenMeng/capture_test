namespace DevCapture
{
    partial class FrmGif
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGif));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.switchBox1 = new IPlugins.SwitchBox();
            this.btn_copy = new IPlugins.ButtonEx();
            this.btn_save = new IPlugins.ButtonEx();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.Location = new System.Drawing.Point(1, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(363, 227);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // switchBox1
            // 
            this.switchBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.switchBox1.BackColor = System.Drawing.Color.Transparent;
            this.switchBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.switchBox1.ForeColor = System.Drawing.Color.White;
            this.switchBox1.IsNO = true;
            this.switchBox1.Location = new System.Drawing.Point(5, 269);
            this.switchBox1.Name = "switchBox1";
            this.switchBox1.NOColor = System.Drawing.Color.DodgerBlue;
            this.switchBox1.NOText = "高质量";
            this.switchBox1.OFFColor = System.Drawing.Color.Gray;
            this.switchBox1.OFFText = "低质量";
            this.switchBox1.Size = new System.Drawing.Size(100, 25);
            this.switchBox1.TabIndex = 4;
            this.switchBox1.Text = "switchBox1";
            this.switchBox1.SwitchChanged += new System.EventHandler(this.switchBox1_SwitchChanged);
            // 
            // btn_copy
            // 
            this.btn_copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_copy.BackColor = System.Drawing.Color.Transparent;
            this.btn_copy.FlatAppearance.BorderSize = 0;
            this.btn_copy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_copy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_copy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_copy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.btn_copy.Location = new System.Drawing.Point(111, 269);
            this.btn_copy.Name = "btn_copy";
            this.btn_copy.Size = new System.Drawing.Size(75, 25);
            this.btn_copy.TabIndex = 5;
            this.btn_copy.Text = "复制";
            this.btn_copy.UseVisualStyleBackColor = false;
            this.btn_copy.Click += new System.EventHandler(this.btn_copy_Click);
            // 
            // btn_save
            // 
            this.btn_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_save.BackColor = System.Drawing.Color.Transparent;
            this.btn_save.FlatAppearance.BorderSize = 0;
            this.btn_save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.btn_save.Location = new System.Drawing.Point(192, 269);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 25);
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // FrmGif
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(365, 300);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_copy);
            this.Controls.Add(this.switchBox1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGif";
            this.Text = "FrmGif";
            this.Load += new System.EventHandler(this.FrmGif_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private IPlugins.SwitchBox switchBox1;
        private IPlugins.ButtonEx btn_copy;
        private IPlugins.ButtonEx btn_save;
    }
}