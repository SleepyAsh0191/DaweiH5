using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using QRCoder;
using System.Windows.Forms;
using System.Linq;

namespace WindowsFormsApp1
{
    public partial class QRForm : Form
    {

        private readonly string qrLoginUrl = "https://passport-api.mihoyo.com/account/ma-cn-passport/web/createQRLogin";
        private readonly string qrStatusUrl = "https://passport-api.miyoushe.com/account/ma-cn-passport/web/queryQRLoginStatus";
        private CancellationTokenSource cancellationTokenSource;
        private bool scannedAlertShown = false;

        public QRForm()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {

        }

        private async void QRForm_Load(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(qrLoginUrl);
                var request = new RestRequest(qrLoginUrl, Method.Post);
                request.AddHeader("x-rpc-app_id", "cie2gjc0sg00");
                request.AddHeader("x-rpc-device_id", "c0570f46-ced6-4a17-1e51-0682e1c2162a");

                // 发送请求
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    MessageBox.Show(response.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var jsonResponse = JObject.Parse(response.Content);
                if (jsonResponse["retcode"]?.ToString() != "0")
                {
                    MessageBox.Show($"Error: retcode is {jsonResponse["retcode"]}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 获取二维码URL并生成二维码
                var qrUrl = jsonResponse["data"]?["url"]?.ToString();
                var ticket = jsonResponse["data"]?["ticket"]?.ToString();
                if (!string.IsNullOrEmpty(qrUrl) && !string.IsNullOrEmpty(ticket))
                {
                    using (var qrGenerator = new QRCodeGenerator())
                    {
                        var qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
                        var qrCode = new QRCode(qrCodeData);

                        // Adjust bitmap size to fit QRPic dimensions
                        using (var qrBitmap = qrCode.GetGraphic(4))
                        {
                            var resizedBitmap = new Bitmap(qrBitmap, QRPic.Width, QRPic.Height);
                            QRPic.Image = resizedBitmap;
                        }
                    }

                    // 启动查询二维码扫描状态的任务
                    await QueryQRLoginStatusAsync(ticket, cancellationTokenSource.Token);
                }
                else
                {
                    MessageBox.Show("QR URL or ticket not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task QueryQRLoginStatusAsync(string ticket, CancellationToken cancellationToken)
        {
            try
            {
                var client = new RestClient(qrStatusUrl);
                while (!cancellationToken.IsCancellationRequested)
                {
                    var request = new RestRequest(qrStatusUrl, Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("x-rpc-app_id", "cie2gjc0sg00");
                    request.AddHeader("x-rpc-device_id", "c0570f46-ced6-4a17-1e51-0682e1c2162a");
                    request.AddJsonBody(new { ticket });

                    var response = await client.ExecuteAsync(request, cancellationToken);
                    if (response.IsSuccessful)
                    {
                        var jsonResponse = JObject.Parse(response.Content);
                        if (jsonResponse["retcode"]?.ToString() == "0")
                        {
                            var status = jsonResponse["data"]?["status"]?.ToString();
                            switch (status)
                            {
                                case "Created":
                                    break;
                                case "Scanned":
                                    break;
                                case "Confirmed":
                                    var userInfo = jsonResponse["data"]?["user_info"]?.ToString();
                                    var cookies = response.Headers.Where(h => h.Name == "Set-Cookie").Select(h => h.Value.ToString());
                                    var allCookies = string.Join("; ", cookies);

                                    if (((Form1)Owner)?.textBox1 != null)
                                    {
                                        ((Form1)Owner).textBox1.ResetText();
                                        ((Form1)Owner).textBox1.ReadOnly = false;

                                        // 将 cookies 字符串按 `;` 分割
                                        var cookieParts = allCookies.Split(';');

                                        // 使用 LINQ 过滤掉包含不需要字段的部分
                                        var filteredCookies = cookieParts
                                            .Where(part => !part.Trim().StartsWith("Path", StringComparison.OrdinalIgnoreCase) &&
                                                           !part.Trim().StartsWith("Domain", StringComparison.OrdinalIgnoreCase) &&
                                                           !part.Trim().StartsWith("Max-Age", StringComparison.OrdinalIgnoreCase) &&
                                                           !part.Trim().StartsWith("HttpOnly", StringComparison.OrdinalIgnoreCase) &&
                                                           !part.Trim().StartsWith("Secure", StringComparison.OrdinalIgnoreCase))
                                            .Select(part => part.Trim());

                                        // 将过滤后的 cookies 组合回一个字符串
                                        string cleanedCookies = string.Join("; ", filteredCookies);



                                        ((Form1)Owner).textBox1.AppendText($"{cleanedCookies}");
                                    }
                                    MessageBox.Show($"登录完毕，以下是用户信息 .\nUser Info: {userInfo}", "已确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                    return;
                                default:
                                    break;
                            }
                        }
                        else if (jsonResponse["retcode"]?.ToString() == "-3501")
                        {
                            MessageBox.Show("二维码过期，请重新生成", "过期", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (jsonResponse["retcode"]?.ToString() == "-3505")
                        {
                            MessageBox.Show("用户已经取消扫码", "取消", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    await Task.Delay(500, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // 窗口关闭时任务取消，不需要处理此异常
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
            base.OnFormClosing(e);
        }
    }
}