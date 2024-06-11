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
    public class SendGridEmailService
    {
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion

        public bool SendEmail(SendGridEmailRequest sendGridEmailRequest)
        {
            HttpService _service = new HttpService();

            string emailUrl = ConfigurationManager.AppSettings["SendGridEmailInterfaceApiUrl"].ToString();
            string requestData = JsonConvert.SerializeObject(sendGridEmailRequest);

            log.Info("SendGrid Email Request to SendGridEmailInterface API : " + requestData);

            log.Info("Email sending.....");
            string response = _service.Post(requestData, emailUrl);
            log.Info("Email sent");

            log.Info("SendGrid Email Response Status from SendGridEmailInterface API : " + response);
            return true;
        }
    }

    public class SendGridEmailRequest
    {
        public string ToAddress { get; set; }
        public string FromEmailDisplayName { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
    }
}