using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaweiH5
{
    public partial class HoyoLabForm : Form
    {

        private Timer timer;

        public HoyoLabForm()
        {
            InitializeComponent();
            LoadWebViewAndStartCookieCheck();
        }

        private async void LoadWebViewAndStartCookieCheck()
        {
            await InitializeWebView();
            StartCookieCheck();
        }

        private async Task InitializeWebView()
        {
            await hoyoverseWebview.EnsureCoreWebView2Async();
            await ClearCookies();
        }

        private async Task ClearCookies()
        {
            var cookieManager = hoyoverseWebview.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync("https://account.hoyoverse.com");

            foreach (var cookie in cookies)
            {
                cookieManager.DeleteCookie(cookie);
            }
        }

        private void StartCookieCheck()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 second interval
            timer.Tick += (sender, e) => getCookie();
            timer.Start();
        }

        private async void getCookie()
        {
            var cookieManager = hoyoverseWebview.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync("https://account.hoyoverse.com");
            StringBuilder cookieString = new StringBuilder();

            foreach (var cookie in cookies)
            {
                cookieString.Append($"{cookie.Name}={cookie.Value};");
            }

            string cookieData = cookieString.ToString();
            if (cookieData.Contains("ltuid"))
            {
                ((Form1)Owner).textBox1.ResetText();
                ((Form1)Owner).textBox1.AppendText(cookieData);
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}