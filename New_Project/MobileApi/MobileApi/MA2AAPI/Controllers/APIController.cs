using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using log4net;
using A2AAPI.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;
using System.Text;
using System.Collections;
using System.Data;
using System.Configuration;
using MA2AAPI.Constants;
using MA2AAPI.Service;

namespace MA2AAPI.Controllers
{
    public class APIController : Controller
    {
        private readonly MA2AAPIWCF.ServiceClient _agentWCF = new MA2AAPIWCF.ServiceClient();
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ApiService _apiService;
        string errMsg = "";

        public APIController()
        {
            _apiService = new ApiService();
        }

        public void writeLog(string logstring)
        {
            Logger.writeLog(logstring, ref log);
        }

        public static string maskSensitiveData(string value)
        {
            string regularExpressionPattern = @"<Password>(.*?)<\/Password>";
            Regex regex = new Regex(regularExpressionPattern, RegexOptions.Singleline);
            MatchCollection collection = regex.Matches(value);
            if (collection.Count > 0)
            {
                Match m = collection[0];
                var stripped = m.Groups[1].Value;
                if (!string.IsNullOrEmpty(stripped))
                {
                    return value.Replace(stripped, "XXXX-XXXX-XXXX");
                }

            }
            return "";
        }

        public bool CheckUserToken(string reqXml, out string code, out string descp)
        {
            string whitelistusername = ConfigurationManager.AppSettings["WhiteListUserName"].ToString();
            string whitelistpassword = ConfigurationManager.AppSettings["WhiteListPassword"].ToString();
            string version = string.Empty;
            string email = string.Empty;
            string loginID = string.Empty;
            string password = string.Empty;
            string userID = string.Empty;
            string Usertoken = string.Empty;
            string DB_token = string.Empty;
            string AgentUserID = string.Empty;
            bool result = true;
            code = string.Empty;
            descp = string.Empty;

            Hashtable ht = Utils.getHTableFromXML(reqXml);

            if (ht.ContainsKey("Version"))
            {
                version = ht["Version"].ToString();
            }
            if (ht.ContainsKey("Email"))
            {
                email = ht["Email"].ToString();
            }
            if (ht.ContainsKey("LoginID"))
            {
                loginID = ht["LoginID"].ToString();
            }
            if (ht.ContainsKey("Password"))
            {
                password = ht["Password"].ToString();
            }
            if (ht.ContainsKey("UserID"))
            {

                userID = ht["UserID"].ToString();
            }
            if (ht.ContainsKey("Token"))
            {
                Usertoken = ht["Token"].ToString();
            }

            if (version == "3.0")
            {
                DataSet ds = null;
                bool flg = false;
                string errorMessage = string.Empty;

                if (email != "" && !email.Contains(".com"))
                {
                    AgentUserID = email;
                }
                else if (loginID != "")
                {
                    AgentUserID = loginID;
                }
                else if (userID != "")
                {
                    AgentUserID = userID;
                }
                if (String.IsNullOrEmpty(AgentUserID))
                {
                    code = "00";
                    descp = "Success";
                    writeLog(descp + errorMessage);
                    return true;
                }

                flg = _agentWCF.GetToken(AgentUserID, out errorMessage, out ds);

                if (flg)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DB_token = ds.Tables[0].Rows[0]["Token"].ToString();

                        if (version == "3.0" && (AgentUserID != whitelistusername && password != whitelistpassword))
                        {
                            if (DB_token != Usertoken)
                            {
                                code = "401";
                                descp = "Login Expired.";
                                writeLog(descp + errMsg);
                                return false;
                            }
                        }
                    }

                }
            }

            return result;
        }

        public string Index()
        {
            //A2A_process a = new A2A_process();
            //var ab = a.GetAstrologerList("<GetAstrologerList><Version>3.0</Version><TimeStamp>202004061300222050</TimeStamp><Email>09972603712</Email><Password>P+WMJV/b3sknf3YCwH/IJ+ERqjowOTk3MjYwMzcxMg==</Password><taxID>0000000000098</taxID><ServiceId>0</ServiceId><LoginType>POS</LoginType><DeviceUID>357693595673247</DeviceUID><Token>9d7676d2-2081-4dba-8095-0488a0c69f4f</Token></GetAstrologerList> ");
            //var ab = a.AcceptenceListReq("<AcceptenceListReq><Version>3.0</Version><TimeStamp>202003050932483740</TimeStamp><MessageID>257d3d1e-a35b-43ab-a48f-78a1f7117b87</MessageID><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><TxnPerPage>10</TxnPerPage><PageNo>1</PageNo><LoginType>MS</LoginType><DeviceUID>352631025654893</DeviceUID><Token>f125d0ea-01ea-44a9-9467-8aa8eb99e7fc</Token></AcceptenceListReq> ");
            //var ab = a.InsertAgentRequest("<InsertAgentRequest><Version>3.0</Version><TimeStamp>201910311344364000</TimeStamp><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><Amount></Amount><Quantity>100</Quantity><RequestType>Paper Roll</RequestType><RequireDate>2020-02-10</RequireDate>  <LoginType>MS</LoginType><DeviceUID>353793985785243</DeviceUID><Latitude>16.84575</Latitude><Longitude>96.13048</Longitude><Length>100</Length><Token>b734b910-e8e6-47f3-9ae7-9075f4e299c6</Token></InsertAgentRequest>");
            //var ab = a.LoginReq("<LoginReq><Version>3.0</Version><AppVersion>3.1.1</AppVersion><DeviceInfo>HUAWEI,ANE-LX2,4.4.23+,8.0.0</DeviceInfo><TimeStamp>201902182219133130</TimeStamp><LoginType>MS</LoginType><Email>pai111</Email><Password>3mvuhLILmGeKs09z1IAcqnXDCRBwYWkxMTE=</Password><MessageID>" + DateTime.Now + "</MessageID><DeviceUID>353695333677244</DeviceUID><DeviceToken>dPqYTrPb3PY:APA91bGm8uPGEXjAo4DL1bU1laVKPCb4qC0YwO-upaDghj5j0quBMGy7d-wvbcqGDu5qByBjioAkZlQsxQUs39BJvOM0Jl5uDGk_oYBLSMG2ZOx75_EndmgNB3t7Ic4TS78ehK3BLPgA</DeviceToken></LoginReq> ");
            //var aa = a.TxnListReq("<TxnListReq><Version>3.0</Version><TimeStamp>201901171158306000</TimeStamp><MessageID>9f6cd19b-a855-4a41-902a-bb13a8c330ef</MessageID><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><TxnPerPage>10</TxnPerPage><PageNo>1</PageNo><DeviceUID>353790086780243</DeviceUID><Token>f978ae99-da10-402b-8dc1-d9e22d7864f1</Token></TxnListReq>");
            //var aaa = a.InquiryReq("<InquiryReq><Version>3.0</Version><TimeStamp>202004061609357650</TimeStamp><MessageID>c1061878-9bd4-45e8-9da8-2af9a9cd1341</MessageID><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><RequestedBy>09692922675</RequestedBy><LoginType>MS</LoginType><TaxID>0000000000098</TaxID><Ref1></Ref1><Ref2></Ref2><Ref3></Ref3><Ref4></Ref4><Ref5></Ref5><Amount>5000</Amount><TopupType>S</TopupType><IsQR></IsQR><DeviceUID>353798985780243</DeviceUID><Token>c08a398a-610b-4a0c-9b51-886863928dae</Token></InquiryReq> ");
            //var aaa = a.ConfirmReq("<ConfirmReq><Version>3.0</Version><TimeStamp>202003281121584380</TimeStamp><MessageID>" + Guid.NewGuid() + "</MessageID><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><TaxID>0000000000098</TaxID><Ref1>0/Pick a Baby Naming</Ref1><Ref2>33/ဆရာမေဒၚေခ်ာေဆြဇင္သင္/MGMG</Ref2><Ref3>Male/09774941523/Yangon/4:00 AM</Ref3><Ref4>15,16</Ref4><Ref5>Y/1368/TaKu/LaSan/1/Sat</Ref5><Ref6>NearMeTesting</Ref6><Amount>1000</Amount><TopupType>S</TopupType><LocLatitude>0.0</LocLatitude><LocLongitude>0.0</LocLongitude><LoginType>POS</LoginType><AppType>MS</AppType><AgentFee>0</AgentFee><ProductDesc></ProductDesc><TerminalId>D1V0980000452</TerminalId><DeviceUID>D1V0980000452357412506722847</DeviceUID><Token>6a7c7b41-3ce9-4039-be66-b175a9d829df</Token></ConfirmReq> ");
            //var aa = a.TxnListReq("<TxnListReq><Version>3.0</Version><TimeStamp>201901171158306000</TimeStamp><MessageID>"+Guid.NewGuid()+"</MessageID><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><TxnPerPage>5</TxnPerPage><PageNo>1</PageNo><DeviceUID>353790086780243</DeviceUID><Token>f978ae99-da10-402b-8dc1-d9e22d7864f1</Token></TxnListReq>");
            //var ab = a.AgentCustomerBindingInfoReq("<AgentCustomerBindingInfoReq><Version>3.0</Version><TimeStamp>201901161135030080</TimeStamp><MessageID>ea88eb97-b509-41d3-9eb8-870f986143bb</MessageID><Email>09968115542</Email><Password>3mvuhLILmGeKs09z1IAcqnXDCRBwYWkxMTE=</Password><RequestedBy>09692922675</RequestedBy><TaxID>0000000000056</TaxID><Ref1>A00001329</Ref1><Ref2></Ref2><Ref3></Ref3><Ref4></Ref4><Ref5></Ref5><Amount></Amount><TopupType>S</TopupType><IsQR></IsQR><DeviceUID>353790086780243</DeviceUID><Token>64e19812-fa8d-4b79-929b-eac24dfa436e</Token></AgentCustomerBindingInfoReq>");
            //var aa = a.ConfirmPGReq("<ConfirmPGReq><Version>3.0</Version><TimeStamp>201901171135030080</TimeStamp><MessageID>" + Guid.NewGuid() + "</MessageID><Email>09968115542</Email><Password>3mvuhLILmGeKs09z1IAcqnXDCRBwYWkxMTE=</Password><RequestedBy>09692922675</RequestedBy><TaxID>0000000000056</TaxID><AgentFee>200.00</AgentFee><ProductDesc /><LocLatitude>37.421998333333335</LocLatitude><LocLongitude>-122.08400000000002</LocLongitude><Ref1>A00001291</Ref1><Ref2></Ref2><Ref3></Ref3><Ref4></Ref4><Ref5></Ref5><CustomerID>01257987</CustomerID><Invoices><Invoice><Amount>150525.00</Amount><InvoiceNumber>I-1000031</InvoiceNumber></Invoice><Invoice><Amount>150525.00</Amount><InvoiceNumber>I-1000022</InvoiceNumber></Invoice></Invoices><PartnerCode>001</PartnerCode><Token>4aWzfxgr2ZyLr- m1lzBlj9MSgt46usMNUXVCyCGDOrSiCJcgfaoic5wO9LtcWx2VafWkmxVfpU5XpMFAN8LMgTKVGnn-9J3y_LQYQxiWOiG1LafCIu2vhmBb-vnLNOMsrOJl40QmlkKIpolCw78EhgShlDEx7qYTZf8Iw7uhC-Dwsfqs_OHONeMyqRmx5JdEo32bjw</Token></ConfirmPGReq>");

            APIModel _apiModel = new APIModel();

            byte[] buffer = new byte[base.Request.ContentLength];
            string resData = "";
            this.writeLog("******************* 1-Stop Start **********************");
            this.writeLog("SESSION ID : " + Session.SessionID);
            this.writeLog("CLIENT IP : " + Request.UserHostAddress);

            try
            {

                var headers = Request.Headers;
                var keys = headers.AllKeys;
                var appVersion = string.Empty;
                if (keys.Contains("APP-VERSION"))
                {
                    appVersion = headers["APP-VERSION"].ToString();
                    this.writeLog("APP-VERSION : " + appVersion);
                }
                else
                {
                    this.writeLog("APP-VERSION not in header");
                }

                base.Request.InputStream.Read(buffer, 0, base.Request.ContentLength);
                string reqData = Encoding.ASCII.GetString(buffer).Trim();
               reqData = ClientEncryptDecryptString(reqData, "D", out errMsg); //this.zDecompress(reqData);   
                reqData = reqData.Replace("&", "&amp;");
                //TEST
                //reqData = "<LoginReq><Version>3.0</Version><AppVersion>3.33.0</AppVersion><DeviceInfo>google,sdk_gphone_x86,4.14.150+,R</DeviceInfo><TimeStamp>202011051213123630</TimeStamp><LoginType>MS</LoginType><Email>09692922675</Email><Password>PFnco5X7hSb2lH9Qv1b9HFLzm10wOTY5MjkyMjY3NQ==</Password><MessageID>0e85ba95-1869-4c96-91ac-09a9e6692819</MessageID><DeviceUID>352635595644243</DeviceUID><DeviceToken>e3gBZDZMSRu5bQbH9YY1IT:APA91bHy3J9B5r6PTkCZLOGtdVP2kD1OU6CIxyMfwntis3Lf8tnRDaKrmO1b1v7saJ9vfu3M2CcLs2wI4qRbHvBU8fzLcLDdlw-C2QvseTUW2ARhVNsX9BAABgCQD7O3AsBmZSM3BYWf</DeviceToken><TerminalId></TerminalId></LoginReq> ";


                string result = maskSensitiveData(reqData);
                this.writeLog("REQ XML : " + result);

                if ((reqData == null) || (reqData == ""))
                {
                    this.writeLog("No Data");
                    resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "UNKNOWN ERROR");
                }
                else
                {
                    try
                    {
                        if (reqData == null)
                        {
                            this.writeLog("No Data");
                            resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "UNKNOWN ERROR");
                        }
                        else if (reqData == "X")
                        {
                            this.writeLog("Decrypted Error");
                            resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "UNKNOWN ERROR");
                        }
                        else
                        {
                            string methodName = this.getMethodName(reqData);
                            string code = string.Empty;
                            string descp = string.Empty;
                            bool isLock = false;

                            if (appVersion != null)
                            {
                                appVersion = appVersion.Replace(".", string.Empty);
                            }
                            isLock = IsUserLock(reqData, methodName);

                            writeLog("User Lock Status : " + isLock);
                            if (!isLock)
                            {
                                var methodsToControlMMBustTickt = new string[4] { "FavouriteEserviceReq", "MoreEserviceReq", "FavouriteEserviceReqV2", "MoreEserviceReqV2" };
                                var methodsToControlMPUandJCB = new string[2] { "FavouriteEpaymentReq", "MoreEpaymentReq" };
                                if (methodsToControlMMBustTickt.Contains(methodName) || methodsToControlMPUandJCB.Contains(methodName))
                                {
                                    resData = this.executeMethod(methodName, reqData, appVersion);
                                }
                                else if (_apiService.IsMethodNeedUserTokenCheck(methodName))
                                {
                                    resData = this.executeMethod(methodName, reqData);

                                }
                                else
                                {
                                    bool res = false;
                                    if (methodName == "GetProfileReq")
                                    {
                                        writeLog("Method Name is not GetProfileReq");
                                        res = CheckUserToken_GetProfileReq(reqData, out code, out descp);
                                    }
                                    else
                                    {
                                        res = CheckUserToken(reqData, out code, out descp);
                                    }

                                    if (!res)
                                    {
                                        if (methodName.Equals("DeepLinkProfileReq") && code.Equals("401"))
                                        {
                                            resData = this.executeMethod(methodName, reqData);
                                            var deeplink = Utils.GetDataFromXMLString(resData, "DeepLink");
                                            writeLog("Deeplink : " + deeplink);
                                            resData = GetDeepLinkError(code, descp, deeplink);
                                        }
                                        else
                                            resData = this.getErrorXml("1.0", Utils.getTimeStamp(), code, descp);
                                    }
                                    else
                                    {
                                        writeLog("CheckUserToken is true");
                                        if(_apiService.IsMethodNeedToCheckAppVersion(methodName))
                                        {
                                            var tagName = (methodName.Equals("DenominationListReq")) ? "taxID" : "TaxID";
                                            var taxId = Utils.GetDataFromXMLString(reqData, tagName);
                                            if (!_apiService.IsValidAppVersionToUseBiller(appVersion, taxId)) 
                                            {
                                                writeLog(string.Format("Current app version {0} can't use this biller taxId : {1}", appVersion, taxId));
                                                resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "01", ConfigurationManager.AppSettings["ApplicationUpdateMessage"].ToString());
                                            }
                                            else 
                                            {
                                                resData = this.executeMethod(methodName, reqData);
                                            }
                                        }
                                        else
                                        {
                                            resData = this.executeMethod(methodName, reqData);
                                        }

                                    }
                                }
                            }
                            else
                            {
                                code = ((int)ResponseCode.Locked).ToString();
                                descp = ConfigurationManager.AppSettings["LockMessage"].ToString();

                                resData = this.getErrorXml("1.0", Utils.getTimeStamp(), code, descp);
                            }
                            if (resData.Equals("noMethod"))
                            {
                                resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "INVALID METHOD");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        writeLog("Error in request page : " + ex.ToString());
                        resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "UNKNOWN ERROR");
                    }
                }
            }
            catch (Exception ex)
            {
                writeLog("Error in Index : " + ex.ToString());
                resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", "UNKNOWN ERROR");
            }
            base.Response.ClearContent();
            base.Response.ClearHeaders();
            string logResData = resData;
            if (resData.Contains("GetProfileRes"))
            {
                logResData = Utils.RemovePhotoByteString(resData);
            }
            this.writeLog("RES XML : " + logResData);

            if (!string.IsNullOrEmpty(resData))
            {
                if (Utils.CheckParentNodeNameXML(resData, "Error"))
                {
                    string resCode = Utils.GetDataFromXMLString(resData, "ResCode");
                    this.writeLog("errResData is not null!!!");
                    this.writeLog("ResCode : " + resCode);
                    string internalServerErrorCode = ((int)Utils.HTTPResponseCodes.InternalServerError).ToString();
                    string badRequestErrorCode = ((int)Utils.HTTPResponseCodes.BadRequest).ToString();
                    string systemErrorCode = ((int)Utils.HTTPResponseCodes.SystemError).ToString();
                    string resdesc = Utils.GetDataFromXMLString(resData, "ResDesc");
                    bool IsunexceptedErrorMsg = Regex.IsMatch(resdesc.ToLower(), @"\bunexpected\b");
                    bool IsInternalErrorMsg = Regex.IsMatch(resdesc.ToLower(), @"\binternal error\b");
                    if ((resCode.Equals(internalServerErrorCode)
                        || IsunexceptedErrorMsg
                        || IsInternalErrorMsg
                        || resCode.Equals(badRequestErrorCode)
                        || resCode.Equals(systemErrorCode))
                        && !resdesc.Equals("User does not exist.")
                        && !resdesc.Equals("Agents not found."))
                    {
                        var customErrorMessage = GetCustomErrorMessage();
                        resData = Utils.ReplaceCustomElementForXML(resData, "ResDesc", customErrorMessage);
                    }
                }
            }
            else
            {
                this.writeLog("resData is null");
                resData = this.getErrorXml("1.0", Utils.getTimeStamp(), "97", GetCustomErrorMessage());
            }

            resData = ClientEncryptDecryptString(resData, "E", out errMsg);//this.zCompress(resData);
            _apiModel.responseData = resData;

            this.writeLog("******************* 1-Stop End **********************");
            return resData;
        }

        public string zCompress(string str)
        {
            try
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(str);
                MemoryStream ms = new MemoryStream();
                Deflater dfl = new Deflater();

                Stream s = new DeflaterOutputStream(ms, dfl);
                s.Write(b, 0, b.Length);
                s.Flush();
                s.Close();
                byte[] c = (byte[])ms.ToArray();
                return Convert.ToBase64String(c);
            }
            catch
            {
                return null;
            }
        }

        private string executeMethod(string methodName, string inputXml)
        {
            try
            {
                Logger.writeLog("method Name : " + methodName, ref log);
                object[] args = new object[] { inputXml };
                Type type = typeof(A2A_process);
                object target = type.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, null, null);
                return (string)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, target, args);
            }
            catch (Exception exception)
            {
                this.writeLog("Error in executeMethod : " + exception.ToString());
                return "noMethod";
            }
        }

        private string executeMethod(string methodName, string inputXml, string appVersion)
        {
            try
            {
                Logger.writeLog("method Name : " + methodName, ref log);
                object[] args = new object[] { inputXml, appVersion };
                Type type = typeof(A2A_process);
                object target = type.InvokeMember(null, BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, null, null);
                return (string)type.InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, null, target, args);
            }
            catch (Exception exception)
            {
                this.writeLog("Error in executeMethod : " + exception.ToString());
                return "noMethod";
            }
        }


        public string getErrorXml(string version, string timeStamp, string errCode, string errDesc)
        {
            this.writeLog("getErrorXml errCode : " + errCode + "errDesc : " + errDesc);
            StringBuilder builder = new StringBuilder();
            builder.Append("<Error>");
            builder.Append("<version>" + version + "</version>");
            builder.Append("<timeStamp>" + timeStamp + "</timeStamp>");
            builder.Append("<ResCode>" + errCode + "</ResCode>");
            builder.Append("<ResDesc>" + GetErrorMessage(errDesc) + "</ResDesc>");
            builder.Append("</Error>");
            return builder.ToString();
        }

        private string GetErrorMessage(string errMessage)
        {
            var knownErrorMessage = ConfigurationManager.AppSettings["KnownErrorMessages"].ToString().Split(';').Where(x => x == errMessage).FirstOrDefault();

            if (string.IsNullOrEmpty(knownErrorMessage))
            {
                return GetCustomErrorMessage();
            }
            return knownErrorMessage;
        }

        private string GetCustomErrorMessage()
        {
            var customErrorMessage = ConfigurationManager.AppSettings["CustomErrorMessage"].ToString();
            return customErrorMessage;
        }

        private string getMethodName(string xml)
        {
            this.writeLog("getMethodName fun:APIController");
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            this.writeLog("document.LoadXml(xml):APIController");
            if (document.ChildNodes.Count > 1) return document.ChildNodes.Item(1).Name;

            return document.ChildNodes.Item(0).Name;
        }

        public string zDecompress(string str)
        {
            byte[] b = Convert.FromBase64String(str);
            string ret = "";
            int l = 0;
            byte[] w = new byte[1024];
            Stream s = new InflaterInputStream(new MemoryStream(b));

            try
            {
                while (true)
                {
                    int i = s.Read(w, 0, w.Length);
                    if (i > 0)
                    {
                        l += i;
                        ret += Encoding.ASCII.GetString(w, 0, i);
                    }
                    else
                    {
                        break;
                    }
                }
                s.Flush();
                s.Close();

                return ret;
            }
            catch
            {
                return null;
            }
        }

        public string ClientEncryptDecryptString(string inStr, string Option, out string errDetails)
        {
            string OutputStr = inStr;
            errDetails = "";
            writeLog("ClientEncryptDecryptString start...");
            try
            {
                SinaptIQPKCS7.PKCS7 PKCS7 = new SinaptIQPKCS7.PKCS7();
                string publicCert = System.Configuration.ConfigurationManager.AppSettings["ClientPublicCert"].ToString();
                string privateCert = System.Configuration.ConfigurationManager.AppSettings["ClientPrivateCert"].ToString();
                string privateCertPassword = System.Configuration.ConfigurationManager.AppSettings["ClientPrivatePass"].ToString();

                switch (Option)
                {
                    case "E":
                        writeLog("Do encrypt");
                        OutputStr = PKCS7.encryptMessage(inStr, PKCS7.getPublicCert(publicCert));
                        break;
                    case "D":
                        writeLog("Do decrypt");
                        writeLog("Here");
                        OutputStr = PKCS7.decryptMessage(inStr.Replace("\0", "").Replace(" ", "+"), PKCS7.getPrivateCert(privateCert, privateCertPassword));
                       break;
                    case "PUBLICK":
                        OutputStr = PKCS7.getPublicCert(publicCert).GetPublicKeyString();
                        if (OutputStr.Length > 1000)
                        {
                            //truncate the key
                            OutputStr = OutputStr.Substring(0, 999).ToString();
                        }
                        break;
                    case "PRIVATEK":
                        OutputStr = PKCS7.getPrivateCert(privateCert, privateCertPassword).GetPublicKeyString();
                        if (OutputStr.Length > 1000)
                        {
                            //truncate the key
                            OutputStr = OutputStr.Substring(0, 999).ToString();
                        }
                        break;
                    default:
                        OutputStr = inStr;
                        break;
                }
                string logData = string.Empty;

                if (OutputStr.Contains("UpdateProfileV2Req"))
                {
                    logData = Utils.RemovePhotoByteString(OutputStr);
                }
                writeLog("Decryption Result :" + logData);
            }
            catch (Exception ex)
            {
                writeLog("ClientEncryptDecryptString (err) : " + ex.ToString());
            }

            return OutputStr;

        }

        #region Extend Custom Token Check for get profile
        public bool CheckUserToken_GetProfileReq(string reqXml, out string code, out string descp)
        {
            string whitelistusername = ConfigurationManager.AppSettings["WhiteListUserName"].ToString();
            string version = string.Empty;

            string Usertoken = string.Empty;
            string DB_token = string.Empty;
            string AgentUserID = string.Empty;
            bool result = true;
            code = string.Empty;
            descp = string.Empty;

            string response = string.Empty;
            Hashtable ht = Utils.getHTableFromXML(reqXml);

            if (ht.ContainsKey("Version"))
            {
                version = ht["Version"].ToString();
            }
            if (ht.ContainsKey("AgentUserID"))
            {

                AgentUserID = ht["AgentUserID"].ToString();
            }
            if (ht.ContainsKey("Token"))
            {
                Usertoken = ht["Token"].ToString();
            }

            if (version == "3.0")
            {
                DataSet ds = null;
                bool flg = false;
                string errMsg = string.Empty;

                if (String.IsNullOrEmpty(AgentUserID))
                {
                    code = "00";
                    descp = "Success";
                    writeLog(descp + errMsg);
                    return true;
                }

                if (version == "3.0" && (AgentUserID != whitelistusername))
                {
                    flg = _agentWCF.GetToken(AgentUserID, out errMsg, out ds);

                    if (flg)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DB_token = ds.Tables[0].Rows[0]["Token"].ToString();

                            if (DB_token != Usertoken)
                            {
                                code = "401";
                                descp = "Login Expired.";
                                writeLog(descp + errMsg);
                                return false;
                            }
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        public bool IsUserLock(string reqXml, string methodName)
        {
            string userId = string.Empty;
            string userStatus = string.Empty;
            string errMsg = string.Empty;
            bool isLock = false;

            if (_apiService.IsMethodNeedToCheckUserLock(methodName))
            {
                userId = getUserIdFromXML(reqXml);

                writeLog("Method IsUserLock: UserId => " + userId);

                //Add this condition cuz old app user's request include null value instead of Userid value
                var methodNames = new string[2] { "ServiceFeeReq", "BillerServiceFeeReq" };
                if ((methodNames.Contains(methodName) && string.IsNullOrEmpty(userId)) || string.IsNullOrEmpty(userId))
                {
                    writeLog(string.Format("In Method IsUserLock, methodName => {0} and userId is NULL", methodName));
                    isLock = false;
                    return isLock;
                }

                if (_agentWCF.GetUserStatusByUserId(userId, out userStatus, out errMsg))
                {
                    if (userStatus != "Y")
                    {
                        isLock = true;
                    }
                }
                else
                {
                    writeLog("Error in IsUserLock : " + errMsg);
                }
            }
            return isLock;
        }

        public string getUserIdFromXML(string reqXml)
        {
            string userId = string.Empty;

            Hashtable ht = Utils.getHTableFromXML(reqXml);
            if (ht.ContainsKey("Email"))
            {
                userId = ht["Email"].ToString();
            }
            if (ht.ContainsKey("AgentUserID"))
            {
                userId = ht["AgentUserID"].ToString();
            }
            if (ht.ContainsKey("AgentUserId"))
            {
                userId = ht["AgentUserId"].ToString();
            }
            if (ht.ContainsKey("LoginID"))
            {
                userId = ht["LoginID"].ToString();
            }

            return userId;
        }

        private string GetDeepLinkError(string code, string desc, string deeplink)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<DeepLinkProfileRes>");
            sb.Append(string.Format("<DeepLink>{0}</DeepLink>", deeplink));
            sb.Append(string.Format("<ResCode>{0}</ResCode>", code));
            sb.Append(string.Format("<ResDesc>{0}</ResDesc>", desc));
            sb.Append("</DeepLinkProfileRes>");
            return sb.ToString();
        }

    }
}
