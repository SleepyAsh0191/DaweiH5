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
        public HoyoLabForm()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var cookieManager = hoyoverseWebview.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync("https://account.hoyoverse.com");
            StringBuilder cookieString = new StringBuilder();

            foreach (var cookie in cookies)
            {
                cookieString.AppendLine($"{cookie.Name}: {cookie.Value}");
            }

            MessageBox.Show(cookieString.ToString());
        }
    }
}
