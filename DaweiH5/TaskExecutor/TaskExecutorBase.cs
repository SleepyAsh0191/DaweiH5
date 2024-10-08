using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaweiH5.TaskExecutor
{
    public abstract class TaskExecutorBase
    {
        protected string game_biz;
        protected string lang;
        protected string cookie;
        public string taskIdentifier { get; set; }

        protected TaskExecutorBase(string game_biz, string lang, string cookie)
        {
            this.game_biz = game_biz;
            this.lang = lang;
            this.cookie = cookie;
        }

        public abstract Task ExecuteTasksAsync();

        protected void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
