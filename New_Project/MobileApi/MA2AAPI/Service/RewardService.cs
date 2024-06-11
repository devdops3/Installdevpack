using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace MA2AAPI.Service
{
    public class RewardService
    {
        private readonly MA2AAPI.PushNotiWCF.ServiceClient _pushNotiWCF;
        private readonly MA2AAPI.MA2AAPIWCF.ServiceClient _agentWCF;
        private readonly object balanceLock = new object();

        public RewardService() 
        {
            _pushNotiWCF = new MA2AAPI.PushNotiWCF.ServiceClient();
            _agentWCF = new MA2AAPI.MA2AAPIWCF.ServiceClient();
        }
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion

        public void PushNotification(string messageId, string agentUserId, double rewardAmount, int agentId, string deviceInfo, string deviceToken)
        {
            var notiType = ConfigurationManager.AppSettings["RewardNotiType"].ToString();
            var notiTitle = ConfigurationManager.AppSettings["RewardNotiTitle"].ToString();
            var notiMessage = string.Format(ConfigurationManager.AppSettings["RewardNotiMessage"].ToString(), Convert.ToInt32(rewardAmount));
            var errorMessage = string.Empty;
            var ds = new DataSet();
            try
            {
                if (_agentWCF.pushNotification(notiTitle, notiMessage, "NearMe", DateTime.Now.ToString(), notiType, out errorMessage, out ds))
                {
                    var notiIDstr = ds.Tables[0].Rows[0].ItemArray[0];
                    int id = Convert.ToInt32(notiIDstr);
                    string agentUserNotiId = string.Empty;
                    writeLog(messageId + "Pushed Noti to Agent: " + agentId + ", NotiId: " + id);

                    if (_agentWCF.AddNotiProfile(id, notiType, agentUserId, out errorMessage))
                    {
                        if (_agentWCF.AddAgentNotiList(agentUserId, id, out errorMessage, out agentUserNotiId))
                        {
                            if (deviceInfo.StartsWith("iOS"))
                            {
                                writeLog(messageId + "Device Info is  " + deviceInfo + "PushToApple For agent user  " + agentUserId);
                                _pushNotiWCF.PushToApple(deviceToken, notiMessage, 0, out errorMessage);
                            }
                            else
                            {
                                writeLog(messageId + "Device Info is  " + deviceInfo + "PushToAndroid For agent user  " + agentUserId);
                                _pushNotiWCF.PushToAndroid(deviceToken, notiMessage, notiTitle, agentUserNotiId, out errorMessage);
                            }
                        }
                    }
                }
                else
                {
                    writeLog(messageId + "No agent user in the agent :" + agentId);
                }
            }
            catch (Exception ex)
            {
                writeLog("Exception error occurred at PushNotification: " + ex.Message);
            }

        }

        public bool UpdateRewardWinner(string messageId, string rewardId, string agentcode, string agentUserId, int agentId, double rewardAmount, out double availableBalance)
        {
            var errorMessage = string.Empty;
            availableBalance = 0;
            try
            {
                writeLog(messageId + "In UpdateRewardWinner method : AgentId : " + agentId);
                if (!_agentWCF.UpdateRewardWinner(rewardId, agentcode, agentUserId, agentId, rewardAmount, out availableBalance, out errorMessage))
                {
                    writeLog(messageId + "Error in UpdateRewardWinner : AgentUserId : " + agentUserId + " | AgentId : " + agentId + " | errorMessage : " + errorMessage);
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                writeLog(messageId + "Exception error occurred in UpdateRewardWinner : " + ex.Message);
                return false;
            }
        }

        public double GetRemainingBalance(string messageId, DataRow reward, DataTable totalUsedRewardAmountTable)
        {
            writeLog(messageId + "In GetRemainingBalance method");
            var totalUsedRewardAmount = 0.00;
            var totalUsedRewardAmountStr = totalUsedRewardAmountTable.Rows[0]["TotalUsedRewardAmount"].ToString();
            if ((totalUsedRewardAmountTable.Rows.Count > 0) && !string.IsNullOrEmpty(totalUsedRewardAmountStr)) totalUsedRewardAmount = Convert.ToDouble(totalUsedRewardAmountStr);
            var totalRewardBalance = Convert.ToDouble(reward["TotalBalance"].ToString());
            var availableRewardBalance = totalRewardBalance - totalUsedRewardAmount;
            writeLog(messageId + "AvailableRewardBalance : " + availableRewardBalance);
            return availableRewardBalance;

        }

        public double GetRandomRewardAmount(List<string> rewardAmountList)
        {
            var random = new Random();
            int index = random.Next(rewardAmountList.Count);
            var randomRewardAmount = Convert.ToDouble(rewardAmountList[index]);
            return randomRewardAmount;
        }

        public List<string> GetRewardAmountList(string messageId, DataRow rewardRow)
        {
            var rewardAmountstr = rewardRow["RewardAmount"].ToString();
            writeLog(messageId + "Reward amount list : " + rewardAmountstr);
            return rewardAmountstr.Split(',').ToList();
        }

        public void CheckThresholdBalance(DataRow reward, string rewardId, string messageId)
        {
            writeLog(messageId + "In CheckThresholdBalance");
            var errorMessage = string.Empty;
            var totalRewardBalance = Convert.ToDouble(reward["TotalBalance"].ToString());
            var totalUsedRewardBalanceStr = _agentWCF.GetSumUsedRewardBalance(rewardId);
            var totalUsedRewardBalance = (!string.IsNullOrEmpty(totalUsedRewardBalanceStr)) ? Convert.ToDouble(totalUsedRewardBalanceStr) : 0;
            var rewardBalance = totalRewardBalance - totalUsedRewardBalance;
            var thresholdBalance = Convert.ToDouble(reward["ThresholdAlertBalance"]);
            var thresholdAlertEmail = reward["ThresholdAlertEmail"].ToString();
            var configKey = ConfigurationManager.AppSettings["RewardSysConfigKey"].ToString();
            var increment = ConfigurationManager.AppSettings["RewardEmailAlertTimeInterval"].ToString();
            if ((rewardBalance <= thresholdBalance) && _agentWCF.UpdateRewardSysConfigByKey(configKey, Convert.ToInt32(increment), out errorMessage))
            {
                Task.Factory.StartNew(() => SendAlertEmail(rewardBalance, thresholdBalance, thresholdAlertEmail));
            }
        }

        public void SendAlertEmail(double rewardBalance, double thresholdBalance, string emails)
        {
            writeLog("In SendAlertEmail");
            var fromEmail = ConfigurationManager.AppSettings["fromEmail"].ToString();
            var emailSubject = string.Format(ConfigurationManager.AppSettings["RewardEmailSubject"].ToString(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:sstt"));
            var emailDisplayName = ConfigurationManager.AppSettings["RewardEmailDisplayName"].ToString();
            var templateName = ConfigurationManager.AppSettings["RewardEmailTemplateName"].ToString();
            var emailAddresses = emails.Split(',').ToArray();
            var emailBody = _agentWCF.GetEmailTemplateByName(templateName);
            emailBody = ReplaceWithRelatedName(rewardBalance, thresholdBalance, emailBody);

            SendGridEmailService sendGridEmailService = new SendGridEmailService();

            foreach (var address in emailAddresses)
            {
                SendGridEmailRequest sendGridEmailRequest = new SendGridEmailRequest
                {
                    FromEmailDisplayName = emailDisplayName,
                    Subject = emailSubject,
                    MessageBody = emailBody,
                    ToAddress = address
                };

                if (!sendGridEmailService.SendEmail(sendGridEmailRequest))
                {
                    writeLog("Unsucessful sending alert email : " + address);
                }
            }
        }

        public string ReplaceWithRelatedName(double rewardBalance, double thresholdBalance, string emailBody)
        {
            var recepientUserName = ConfigurationManager.AppSettings["RewardRecepientUserName"].ToString();
            emailBody = emailBody.Replace("[[username]]", recepientUserName);
            emailBody = emailBody.Replace("[[rewardBalance]]", (rewardBalance <= 0) ? "0" : rewardBalance.ToString("#,##"));
            emailBody = emailBody.Replace("[[thresholdBalance]]", (thresholdBalance <= 0) ? "0" : thresholdBalance.ToString("#,##"));
            return emailBody;
        }
    }
}