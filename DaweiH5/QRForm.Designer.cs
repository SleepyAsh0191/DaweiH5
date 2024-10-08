namespace DaweiH5
{
    partial class QRForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.QRPic = new System.Windows.Forms.PictureBox();
            this.refreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.QRPic)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(502, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "请使用“米游社”或者任意米哈游游戏App扫码";
            // 
            // QRPic
            // 
            this.QRPic.Location = new System.Drawing.Point(82, 104);
            this.QRPic.Margin = new System.Windows.Forms.Padding(6);
            this.QRPic.Name = "QRPic";
            this.QRPic.Size = new System.Drawing.Size(500, 500);
            this.QRPic.TabIndex = 1;
            this.QRPic.TabStop = false;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(90, 614);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(6);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(484, 46);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "刷新";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // QRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 714);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.QRPic);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "QRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "米游社二维码登录";
            this.Load += new System.EventHandler(this.QRForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QRPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox QRPic;
        private System.Windows.Forms.Button refreshButton;
    }
}