using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using log4net;

namespace MA2AAPI
{
    public class S3FileService
    {
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion
        public string GetDataFromS3(string keyName)
        {
            string responseData = string.Empty;
            try
            {
                string url = ConfigurationManager.AppSettings["GetPhotoS3Url"];
                var requestTimeout = ConfigurationManager.AppSettings["ApiTimeout"].ToString();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

                HttpWebRequest request = WebRequest.Create(url + "?photoUrl=" + keyName) as HttpWebRequest;

                request.Method = "GET";
                request.ContentType = "application/json";

                using (HttpWebResponse output = request.GetResponse() as HttpWebResponse)
                {
                    if (output.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        responseData = new StreamReader(output.GetResponseStream()).ReadToEnd();
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
                    {
                        responseData = "Time-out.";
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        responseData = "Internal server error.";
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.NotFound))
                    {
                        responseData = "Method not found.";
                    }
                    else
                    {
                        responseData = "System error.";
                    }
                }
            }
            catch (Exception e)
            {
                responseData = e.Message;
            }
            return responseData;


        }

        public string GetImageUrlFromS3(string keyName)
        {
            string responseData = string.Empty;
            try
            {
                string url = ConfigurationManager.AppSettings["ImageRetrieveUrl"];
                
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

                HttpWebRequest request = WebRequest.Create(url + "?keyName=" + keyName) as HttpWebRequest;

                request.Method = "GET";
                request.ContentType = "application/json";

                using (HttpWebResponse output = request.GetResponse() as HttpWebResponse)
                {
                    if (output.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        responseData = new StreamReader(output.GetResponseStream()).ReadToEnd();
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
                    {
                        responseData = "Time-out.";
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        responseData = "Internal server error.";
                    }
                    else if (output.StatusCode.Equals(HttpStatusCode.NotFound))
                    {
                        responseData = "Method not found.";
                    }
                    else
                    {
                        responseData = "System error.";
                    }
                }
            }
            catch (Exception e)
            {
                responseData = e.Message;
            }
            return responseData;


        }
    }
}