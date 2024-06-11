﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using log4net;

namespace MA2AAPI.Service
{
    public class HttpService
    {
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion

        public string Post(string json, string url)
        {
            var responseData = string.Empty;

            try
            {
                var requestTimeout = ConfigurationManager.AppSettings["PortalApiTimeOut"].ToString();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = int.Parse(requestTimeout);
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)3072;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (httpResponse.StatusCode.Equals(HttpStatusCode.OK) || httpResponse.StatusCode.Equals(HttpStatusCode.Created))
                    {
                        responseData = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
                    }
                    else if (httpResponse.StatusCode.Equals(HttpStatusCode.GatewayTimeout))
                    {
                        responseData = "Time-out.";
                    }
                    else if (httpResponse.StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        responseData = "Internal server error.";
                    }
                    else if (httpResponse.StatusCode.Equals(HttpStatusCode.NotFound))
                    {
                        responseData = "Method not found.";
                    }
                    else
                    {
                        responseData = "System error.";
                    }
                }
            }
            catch (Exception ex)
            {
                this.writeLog("Exception occur when request to Service :" + ex.Message);
                responseData = ex.Message;

            }

            return responseData;
        }
    }
}