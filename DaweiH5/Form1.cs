using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DaweiH5.TaskExecutor;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DaweiH5
{
    public partial class Form1 : Form
    {
        private string game_biz = "hk4e_cn";
        private string lang = "zh-cn";
        private string taskIdentifier = "e20241004rolewarm";

        public Form1()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            comboBox1.Items.AddRange(new object[]
            {
                "加急订单",
                "和声的回响"
            });
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "加急订单":
                    taskIdentifier = "e20241004rolewarm";
                    break;
                case "和声的回响":
                    taskIdentifier = "e20241005ost";
                    break;
                default:
                    taskIdentifier = "e20241004rolewarm";
                    break;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string cookie = textBox1.Text;
            var executor = new TaskExecutorMerlin(game_biz, lang, cookie);
            executor.taskIdentifier = taskIdentifier;
            await executor.ExecuteTasksAsync();
        }

        private void qrButton_Click(object sender, EventArgs e)
        {
            using (var qrForm = new QRForm())
            {
                qrForm.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var hoYoLabForm = new HoyoLabForm())
            {
                hoYoLabForm.ShowDialog(this);
            }
        }
    }
}