namespace DaweiH5
{
    partial class HoyoLabForm
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
            this.hoyoverseWebview = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.cookieLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.hoyoverseWebview)).BeginInit();
            this.SuspendLayout();
            // 
            // hoyoverseWebview
            // 
            this.hoyoverseWebview.AllowExternalDrop = true;
            this.hoyoverseWebview.CreationProperties = null;
            this.hoyoverseWebview.DefaultBackgroundColor = System.Drawing.Color.White;
            this.hoyoverseWebview.Location = new System.Drawing.Point(6, 32);
            this.hoyoverseWebview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.hoyoverseWebview.Name = "hoyoverseWebview";
            this.hoyoverseWebview.Size = new System.Drawing.Size(762, 482);
            this.hoyoverseWebview.Source = new System.Uri("https://account.hoyoverse.com/login-platform/index.html?token_type=6&client_type=" +
        "4&app_id=ce1tbuwb00zk&game_biz=hk4e_global&lang=zh-cn&theme=light-hk4e&ux_mode=p" +
        "opup&iframe_level=1#/password-login", System.UriKind.Absolute);
            this.hoyoverseWebview.TabIndex = 0;
            this.hoyoverseWebview.ZoomFactor = 1D;
            // 
            // cookieLabel
            // 
            this.cookieLabel.AutoSize = true;
            this.cookieLabel.Location = new System.Drawing.Point(6, 520);
            this.cookieLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cookieLabel.Name = "cookieLabel";
            this.cookieLabel.Size = new System.Drawing.Size(113, 12);
            this.cookieLabel.TabIndex = 1;
            this.cookieLabel.Text = "Cookie会在这里显示";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请在此登录框中输入账密登录（通过人机验证后没反应为正常现象）";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(608, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "test获取Cookie";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HoyoLabForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 550);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cookieLabel);
            this.Controls.Add(this.hoyoverseWebview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HoyoLabForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HoyoLabForm";
            ((System.ComponentModel.ISupportInitialize)(this.hoyoverseWebview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 hoyoverseWebview;
        private System.Windows.Forms.Label cookieLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}