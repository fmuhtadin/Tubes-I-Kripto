namespace WindowsFormsApplication1
{
    partial class Steganography
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Steganography));
            this.groupCover = new System.Windows.Forms.GroupBox();
            this.btnBrowseCover = new System.Windows.Forms.Button();
            this.txtCover = new System.Windows.Forms.TextBox();
            this.groupHidden = new System.Windows.Forms.GroupBox();
            this.btnBrowseHidden = new System.Windows.Forms.Button();
            this.txtHidden = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.groupBit = new System.Windows.Forms.GroupBox();
            this.radio2Bit = new System.Windows.Forms.RadioButton();
            this.radio1Bit = new System.Windows.Forms.RadioButton();
            this.groupEncrypt = new System.Windows.Forms.GroupBox();
            this.radioNoEncrypt = new System.Windows.Forms.RadioButton();
            this.radioEncrypt = new System.Windows.Forms.RadioButton();
            this.btnDecode = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rtxtPSNR = new System.Windows.Forms.RichTextBox();
            this.btnEncode = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.btnLoadOri = new System.Windows.Forms.Button();
            this.btnLoadRes = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.slNotification = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupCover.SuspendLayout();
            this.groupHidden.SuspendLayout();
            this.groupBit.SuspendLayout();
            this.groupEncrypt.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupCover
            // 
            this.groupCover.Controls.Add(this.btnBrowseCover);
            this.groupCover.Controls.Add(this.txtCover);
            this.groupCover.Location = new System.Drawing.Point(12, 12);
            this.groupCover.Name = "groupCover";
            this.groupCover.Size = new System.Drawing.Size(434, 50);
            this.groupCover.TabIndex = 0;
            this.groupCover.TabStop = false;
            this.groupCover.Text = "Cover Object";
            // 
            // btnBrowseCover
            // 
            this.btnBrowseCover.Location = new System.Drawing.Point(352, 16);
            this.btnBrowseCover.Name = "btnBrowseCover";
            this.btnBrowseCover.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseCover.TabIndex = 1;
            this.btnBrowseCover.Text = "Browse";
            this.btnBrowseCover.UseVisualStyleBackColor = true;
            this.btnBrowseCover.Click += new System.EventHandler(this.btnBrowseCover_Click);
            // 
            // txtCover
            // 
            this.txtCover.Location = new System.Drawing.Point(6, 19);
            this.txtCover.Name = "txtCover";
            this.txtCover.Size = new System.Drawing.Size(340, 20);
            this.txtCover.TabIndex = 0;
            // 
            // groupHidden
            // 
            this.groupHidden.Controls.Add(this.btnBrowseHidden);
            this.groupHidden.Controls.Add(this.txtHidden);
            this.groupHidden.Location = new System.Drawing.Point(12, 69);
            this.groupHidden.Name = "groupHidden";
            this.groupHidden.Size = new System.Drawing.Size(434, 50);
            this.groupHidden.TabIndex = 1;
            this.groupHidden.TabStop = false;
            this.groupHidden.Text = "Hidden Object";
            // 
            // btnBrowseHidden
            // 
            this.btnBrowseHidden.Location = new System.Drawing.Point(352, 16);
            this.btnBrowseHidden.Name = "btnBrowseHidden";
            this.btnBrowseHidden.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseHidden.TabIndex = 1;
            this.btnBrowseHidden.Text = "Browse";
            this.btnBrowseHidden.UseVisualStyleBackColor = true;
            this.btnBrowseHidden.Click += new System.EventHandler(this.btnBrowseHidden_Click);
            // 
            // txtHidden
            // 
            this.txtHidden.Location = new System.Drawing.Point(6, 19);
            this.txtHidden.Name = "txtHidden";
            this.txtHidden.Size = new System.Drawing.Size(340, 20);
            this.txtHidden.TabIndex = 0;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(6, 18);
            this.txtKey.MaxLength = 25;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(180, 20);
            this.txtKey.TabIndex = 3;
            // 
            // groupBit
            // 
            this.groupBit.Controls.Add(this.radio2Bit);
            this.groupBit.Controls.Add(this.radio1Bit);
            this.groupBit.Location = new System.Drawing.Point(216, 124);
            this.groupBit.Name = "groupBit";
            this.groupBit.Size = new System.Drawing.Size(112, 50);
            this.groupBit.TabIndex = 4;
            this.groupBit.TabStop = false;
            this.groupBit.Text = "Modified LSB";
            // 
            // radio2Bit
            // 
            this.radio2Bit.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.radio2Bit.AutoSize = true;
            this.radio2Bit.Location = new System.Drawing.Point(58, 19);
            this.radio2Bit.Name = "radio2Bit";
            this.radio2Bit.Size = new System.Drawing.Size(46, 17);
            this.radio2Bit.TabIndex = 1;
            this.radio2Bit.Text = "2-Bit";
            this.radio2Bit.UseVisualStyleBackColor = true;
            // 
            // radio1Bit
            // 
            this.radio1Bit.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.radio1Bit.AutoSize = true;
            this.radio1Bit.Checked = true;
            this.radio1Bit.Location = new System.Drawing.Point(6, 19);
            this.radio1Bit.Name = "radio1Bit";
            this.radio1Bit.Size = new System.Drawing.Size(46, 17);
            this.radio1Bit.TabIndex = 0;
            this.radio1Bit.TabStop = true;
            this.radio1Bit.Text = "1-Bit";
            this.radio1Bit.UseVisualStyleBackColor = true;
            // 
            // groupEncrypt
            // 
            this.groupEncrypt.Controls.Add(this.radioNoEncrypt);
            this.groupEncrypt.Controls.Add(this.radioEncrypt);
            this.groupEncrypt.Location = new System.Drawing.Point(334, 124);
            this.groupEncrypt.Name = "groupEncrypt";
            this.groupEncrypt.Size = new System.Drawing.Size(112, 50);
            this.groupEncrypt.TabIndex = 5;
            this.groupEncrypt.TabStop = false;
            this.groupEncrypt.Text = "Encryption";
            // 
            // radioNoEncrypt
            // 
            this.radioNoEncrypt.AutoSize = true;
            this.radioNoEncrypt.Location = new System.Drawing.Point(55, 19);
            this.radioNoEncrypt.Name = "radioNoEncrypt";
            this.radioNoEncrypt.Size = new System.Drawing.Size(39, 17);
            this.radioNoEncrypt.TabIndex = 1;
            this.radioNoEncrypt.Text = "No";
            this.radioNoEncrypt.UseVisualStyleBackColor = true;
            // 
            // radioEncrypt
            // 
            this.radioEncrypt.AutoSize = true;
            this.radioEncrypt.Checked = true;
            this.radioEncrypt.Location = new System.Drawing.Point(6, 19);
            this.radioEncrypt.Name = "radioEncrypt";
            this.radioEncrypt.Size = new System.Drawing.Size(43, 17);
            this.radioEncrypt.TabIndex = 0;
            this.radioEncrypt.TabStop = true;
            this.radioEncrypt.Text = "Yes";
            this.radioEncrypt.UseVisualStyleBackColor = true;
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(334, 186);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(112, 78);
            this.btnDecode.TabIndex = 7;
            this.btnDecode.Text = "Decode";
            this.btnDecode.UseVisualStyleBackColor = true;
            this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rtxtPSNR);
            this.groupBox5.Location = new System.Drawing.Point(12, 180);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(198, 84);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PSNR";
            // 
            // rtxtPSNR
            // 
            this.rtxtPSNR.Location = new System.Drawing.Point(6, 19);
            this.rtxtPSNR.Name = "rtxtPSNR";
            this.rtxtPSNR.ReadOnly = true;
            this.rtxtPSNR.Size = new System.Drawing.Size(180, 59);
            this.rtxtPSNR.TabIndex = 0;
            this.rtxtPSNR.Text = "";
            // 
            // btnEncode
            // 
            this.btnEncode.AccessibleDescription = "";
            this.btnEncode.Location = new System.Drawing.Point(216, 186);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(112, 78);
            this.btnEncode.TabIndex = 6;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(457, 12);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(244, 233);
            this.axWindowsMediaPlayer1.TabIndex = 9;
            // 
            // btnLoadOri
            // 
            this.btnLoadOri.Location = new System.Drawing.Point(476, 250);
            this.btnLoadOri.Name = "btnLoadOri";
            this.btnLoadOri.Size = new System.Drawing.Size(100, 23);
            this.btnLoadOri.TabIndex = 11;
            this.btnLoadOri.Text = "Load Original";
            this.btnLoadOri.UseVisualStyleBackColor = true;
            this.btnLoadOri.Click += new System.EventHandler(this.btnLoadOri_Click);
            // 
            // btnLoadRes
            // 
            this.btnLoadRes.Location = new System.Drawing.Point(582, 250);
            this.btnLoadRes.Name = "btnLoadRes";
            this.btnLoadRes.Size = new System.Drawing.Size(100, 23);
            this.btnLoadRes.TabIndex = 12;
            this.btnLoadRes.Text = "Load Result";
            this.btnLoadRes.UseVisualStyleBackColor = true;
            this.btnLoadRes.Click += new System.EventHandler(this.btnLoadRes_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtKey);
            this.groupBox1.Location = new System.Drawing.Point(12, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(198, 50);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Key";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slNotification});
            this.statusStrip1.Location = new System.Drawing.Point(0, 302);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(712, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // slNotification
            // 
            this.slNotification.Name = "slNotification";
            this.slNotification.Size = new System.Drawing.Size(0, 17);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 270);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Steganography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 324);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.btnLoadRes);
            this.Controls.Add(this.btnLoadOri);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnDecode);
            this.Controls.Add(this.groupEncrypt);
            this.Controls.Add(this.groupBit);
            this.Controls.Add(this.groupHidden);
            this.Controls.Add(this.groupCover);
            this.Name = "Steganography";
            this.Text = "Steganograph with AVI";
            this.groupCover.ResumeLayout(false);
            this.groupCover.PerformLayout();
            this.groupHidden.ResumeLayout(false);
            this.groupHidden.PerformLayout();
            this.groupBit.ResumeLayout(false);
            this.groupBit.PerformLayout();
            this.groupEncrypt.ResumeLayout(false);
            this.groupEncrypt.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupCover;
        private System.Windows.Forms.Button btnBrowseCover;
        private System.Windows.Forms.TextBox txtCover;
        private System.Windows.Forms.GroupBox groupHidden;
        private System.Windows.Forms.Button btnBrowseHidden;
        private System.Windows.Forms.TextBox txtHidden;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.GroupBox groupBit;
        private System.Windows.Forms.RadioButton radio2Bit;
        private System.Windows.Forms.RadioButton radio1Bit;
        private System.Windows.Forms.GroupBox groupEncrypt;
        private System.Windows.Forms.Button btnDecode;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox rtxtPSNR;
        private System.Windows.Forms.Button btnEncode;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button btnLoadOri;
        private System.Windows.Forms.Button btnLoadRes;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioNoEncrypt;
        private System.Windows.Forms.RadioButton radioEncrypt;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel slNotification;
        private System.Windows.Forms.Button btnClear;

    }
}

