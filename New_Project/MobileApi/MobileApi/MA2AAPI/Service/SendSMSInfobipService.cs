using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net;
using Newtonsoft.Json;

namespace MA2AAPI.Service
{
    public class SendSMSInfobipService
    {
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion

        public bool SendSMS(SendSmsInfobipRequest sendSmsInfobipRequest)
        {
            HttpService _service = new HttpService();

            string smsUrl = ConfigurationManager.AppSettings["SendSmsInfobipInterfaceApiUrl"].ToString();
            string requestData = JsonConvert.SerializeObject(sendSmsInfobipRequest);

            log.Info("Send SMS Request to SendSmsInfobipInterface API : " + requestData);

            log.Info("SMS sending.....");
            string response = _service.Post(requestData, smsUrl);
            log.Info("SMS sent");

            log.Info("Send SMS Response Status from SendSmsInfobipInterface API : " + response);
            return true;
        }
    }

    public class SendSmsInfobipRequest
    {
        public string ToMobile { get; set; }
        public string Message { get; set; }
    }
}