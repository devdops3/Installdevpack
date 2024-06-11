using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace WebApp
{
    public class SSLPost
    {

        public class CertPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest request, int problem)
            {
                return true;
            }
        }
        //public bool postData(string url, string data2Post, out string responseData, out string errMsg)
        //{
        //    responseData = "";
        //    errMsg = "";
        //    if (data2Post == "")
        //    {
        //        errMsg = "Nothing to post!";
        //        return false;
        //    }
        //    int num = 0x7530;
        //    HttpWebResponse response = null;
        //    try
        //    {
        //        int num3;
        //        ServicePointManager.CertificatePolicy = new CertPolicy();
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.UserAgent = "SinaptIQ WebBot";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.Timeout = num;
        //        request.KeepAlive = true;
        //        request.ContentLength = data2Post.Length;
        //        StringBuilder builder = new StringBuilder();
        //        char[] anyOf = new char[] { '?', '=', '&', '/', ' ' };
        //        for (int i = 0; i < data2Post.Length; i = num3 + 1)
        //        {
        //            num3 = data2Post.IndexOfAny(anyOf, i);
        //            if (num3 == -1)
        //            {
        //                builder.Append(HttpUtility.UrlEncode(data2Post.Substring(i, data2Post.Length - i)));
        //                break;
        //            }
        //            builder.Append(HttpUtility.UrlEncode(data2Post.Substring(i, num3 - i)));
        //            builder.Append(data2Post.Substring(num3, 1));
        //        }
        //        byte[] bytes = null;
        //        if (data2Post != null)
        //        {
        //            bytes = Encoding.UTF8.GetBytes(builder.ToString());
        //            request.ContentLength = bytes.Length;
        //            Stream requestStream = request.GetRequestStream();
        //            requestStream.Write(bytes, 0, bytes.Length);
        //            requestStream.Close();
        //        }
        //        else
        //        {
        //            request.ContentLength = 0L;
        //        }
        //        response = (HttpWebResponse)request.GetResponse();
        //        responseData = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        //        responseData = responseData.Trim();
        //    }
        //    catch (Exception exception)
        //    {
        //        errMsg = exception.Message;
        //        return false;
        //    }
        //    finally
        //    {
        //        if (response != null)
        //        {
        //            response.Close();
        //        }
        //    }
        //    return true;
        //}


        public bool postData(string postURL, string data2Post, out string responseData, out string errMsg)
        {
            responseData = "";
            errMsg = "";


            HttpWebResponse response = null;

            try
            {
                ServicePointManager.CertificatePolicy = new CertPolicy();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);
                request.Method = "POST";
                request.UserAgent = "SinaptIQ WebBot";
                // request.ContentType = "application/xml; charset=utf-8";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;

                if (data2Post != "")
                {
                    //writeLog("data2post:" + data2Post);
                    StreamWriter postwriter = new StreamWriter(request.GetRequestStream());
                    postwriter.Write(data2Post);
                    postwriter.Close();
                }
                else
                {
                    request.ContentLength = 0L;
                }
                response = (HttpWebResponse)request.GetResponse();
                responseData = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                responseData = responseData.Trim();

                //writeLog("RES DATA : " + responseData);
            }
            catch (Exception e)
            {
                responseData = e.Message;
                errMsg = e.Message;
                //writeLog("Error in postData : " + e.ToString());
                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return true;
        }


        public bool postFraud(string backendUrl, string compressEncrypt, out string responseData, out string errMsg)
        {
            errMsg = "";
            SSLPost sslPost = new SSLPost();
            responseData = "";
            bool sendSuccess = false;


            if (!sslPost.postData(backendUrl, compressEncrypt, out responseData, out errMsg))
            {
                //errMsg = "To URL : " + backendUrl + "<br> Error Message : " + errMsg + "<br> Time stamp : " + DateTime.Now.ToString();
            }
            else
            {
                sendSuccess = true;
            }

            return sendSuccess;
            //writeLog("doAPICallUrl end");
        }



        public bool sendRequest(string payload, string paymentURL, out string encResponse, out string err)
        {
            encResponse = "";
            err = "";
            try
            {
                WebRequest objRequest = WebRequest.Create(paymentURL);
                objRequest.Timeout = 600000; //In milliseconds
                objRequest.Method = "POST";
                objRequest.ContentLength = payload.Length;
                objRequest.ContentType = "application/x-www-formurlencoded";
                StreamWriter postWriter = new StreamWriter(objRequest.GetRequestStream());
                postWriter.Write(payload);
                postWriter.Close();
                WebResponse objResponse = objRequest.GetResponse();
                StreamReader sr = new
                StreamReader(objResponse.GetResponseStream());
                encResponse = sr.ReadToEnd();
                sr.Close();
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message.ToString();
                return false;
            }
        }
    }
}