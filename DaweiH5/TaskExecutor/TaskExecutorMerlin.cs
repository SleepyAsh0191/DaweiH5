﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaweiH5.TaskExecutor
{
    public class TaskExecutorMerlin : TaskExecutorBase
    {
        private readonly string initialTaskUrlTemplate = "https://hk4e-api.mihoyo.com/event/merlin_v2/v3/flow/run/hk4e_cn/{0}/1?game_biz={1}";
        private readonly string doTaskUrlTemplate = "https://hk4e-api.mihoyo.com/event/merlin_v2/v3/flow/run/hk4e_cn/{0}/2?game_biz={1}&lang=zh-cn";
        private readonly string loginUrl = "https://api-takumi.mihoyo.com/common/badge/v1/login/account";

        public TaskExecutorMerlin(string game_biz, string lang, string cookie) : base(game_biz, lang, cookie) { }

        public override async Task ExecuteTasksAsync()
        {
            try
            {
                // 首先获取 uid 和 region
                var client = new RestClient("https://passport-api.mihoyo.com/binding/api/getUserGameRolesByCookieToken?game_biz=" + game_biz);
                var request = new RestRequest();
                request.AddHeader("Cookie", cookie);

                var roleResponse = await client.ExecuteAsync(request);
                if (!roleResponse.IsSuccessful)
                {
                    ShowErrorMessage("获取角色失败，请检查是否绑定米游社");
                    return;
                }

                var roleJson = JObject.Parse(roleResponse.Content);
                if (roleJson["retcode"]?.ToString() != "0")
                {
                    ShowErrorMessage($"错误 {roleJson["retcode"]}");
                    return;
                }

                var gameRole = roleJson["data"]?["list"]?.FirstOrDefault(role => role["is_chosen"]?.ToObject<bool>() == true);
                if (gameRole == null)
                {
                    gameRole = roleJson["data"]?["list"]?.First();
                }

                var region = gameRole?["region"]?.ToString();
                var gameUid = gameRole?["game_uid"]?.ToString();

                if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(gameUid))
                {
                    ShowErrorMessage("获取 region 与 UID 失败，请检查cookie");
                    return;
                }

                // 首先访问登录接口从 set-cookie 中获取 e_hk4e_token
                client = new RestClient(loginUrl);
                request = new RestRequest(loginUrl, Method.Post);
                request.AddHeader("Cookie", cookie);
                request.AddJsonBody(new { region, uid = gameUid, game_biz, lang });
                var loginResponse = await client.ExecuteAsync(request);
                if (loginResponse.IsSuccessful)
                {
                    var eHk4eToken = loginResponse.Headers
                        .Where(h => h.Name == "Set-Cookie" && h.Value.ToString().Contains("e_hk4e_token"))
                        .Select(h => h.Value.ToString().Split(';').FirstOrDefault(part => part.Contains("e_hk4e_token")))
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(eHk4eToken))
                    {
                        cookie += $"; {eHk4eToken}";
                    }
                }
                else
                {
                    ShowErrorMessage("获取 e_hk4e_token 失败，请检查cookie");
                    return;
                }

                // 继续进行任务的处理
                string initialTaskUrl = string.Format(initialTaskUrlTemplate, taskIdentifier, game_biz);
                client = new RestClient(initialTaskUrl);
                request = new RestRequest(initialTaskUrl, Method.Get);
                request.AddHeader("Cookie", cookie);

                // 发送请求
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    ShowErrorMessage(response.ErrorMessage);
                    return;
                }

                var jsonResponse = JObject.Parse(response.Content);
                if (jsonResponse["retcode"]?.ToString() != "0")
                {
                    ShowErrorMessage($"错误码: {jsonResponse["retcode"]}， 错误信息：{jsonResponse["message"]}");
                    return;
                }

                // 如果 retcode 为 0，获取所有 task_id
                var taskIds = jsonResponse["data"]?["tasks"]?["data"]?
                    .Select(task => task["task_id"]?.ToString())
                    .Where(id => !string.IsNullOrEmpty(id))
                    .ToList();

                if (taskIds != null && taskIds.Count > 0)
                {
                    foreach (var taskId in taskIds)
                    {
                        if (!await DoTask(int.Parse(taskId)))
                        {
                            MessageBox.Show("执行任务失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    MessageBox.Show("执行完毕，请检查《原神》游戏内邮箱\n如果游戏内无邮件请检查网页是否为兑换码领取或分享领取，本程序已为您跳过大部分繁琐流程", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("找不到任务清单，可能是活动已经结束", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private async Task<bool> DoTask(int taskId)
        {
            string doTaskUrl = string.Format(doTaskUrlTemplate, taskIdentifier, game_biz);
            var client = new RestClient(doTaskUrl);
            var request = new RestRequest(doTaskUrl, Method.Post);
            request.AddHeader("Cookie", cookie);
            request.AddJsonBody(new { task_id = taskId });

            try
            {
                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    ShowErrorMessage(response.ErrorMessage);
                    return false;
                }

                var jsonResponse = JObject.Parse(response.Content);
                var retcode = jsonResponse["retcode"]?.ToString();
                var message = jsonResponse["message"]?.ToString();
                if (retcode != "0" && retcode != "2007" && retcode != "2004")
                {
                    ShowErrorMessage($"错误：{message}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                return false;
            }
        }
    }
}
