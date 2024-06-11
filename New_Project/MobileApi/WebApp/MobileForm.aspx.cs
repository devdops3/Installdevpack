using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class MobileForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtRequest.Text = string.Empty;
                txtCompress.Text = string.Empty;

                txtResponse.Text = string.Empty;
                //txtURL.Text = "http://192.168.0.9/123_MM/123MobileAgentAPI";
               // txtURL.Text = "http://localhost:57241/";
                //txtURL.Text = "http://localhost:57241/A2A/MobileA2AAPI";

                txtURL.Text = "http://localhost/MA2AAPI";
            }
        }
      
        protected void butSubmit_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            string responsedata = string.Empty;

            SSLPost sslPost = new SSLPost();
            string compresstext = txtCompress.Text;
            sslPost.postFraud(txtURL.Text, txtCompress.Text, out responsedata, out errMsg);
            responsedata = ClientEncryptDecryptString(responsedata, "D", out errMsg); //this.zDecompress(responsedata);
            txtResponse.Text = System.Xml.Linq.XDocument.Parse(responsedata).ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            string str = ClientEncryptDecryptString(txtCompress.Text, "D", out errMsg); //this.zDecompress(txtCompress.Text);
            txtResponse.Text = str;
        }

        #region Request
        protected void butLogin_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<LoginReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("</LoginReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butBiller_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();
            hb.Append("<BillerListReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("</BillerListReq>");

            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
        }

        protected void butInquiry_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<InquiryReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>desk11</Email>");
            hb.Append("<Password>WpB0YyjHdkM0f/hae/Rqkbg6b1xkZXNrMTE=</Password>");
            hb.Append("<TaxID>0000000000020</TaxID>");
            hb.Append("<InquiryType>M</InquiryType>");              // M for Manual ; Q for QRCode
            hb.Append("<Ref1>042014090276000170</Ref1>");                      // Payment Code
            hb.Append("<Ref2></Ref2>");  // InvoiceNo. If InquiryType is Q, it must has value
            hb.Append("<Ref3></Ref3>");
            hb.Append("<Amount></Amount>");             // If InquiryType is Q, it must has value
            hb.Append("<MessageID>"+Guid.NewGuid()+"</MessageID>");
            hb.Append("<IsQR></IsQR>");
            hb.Append("<RequestedBy>desk11</RequestedBy>");
            hb.Append("</InquiryReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();


            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butConfirm_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();
          
            hb.Append("<ConfirmReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:ffff") + "</TimeStamp>");
            hb.Append("<MessageID>92ea9ed0-b612-40d5-95f0-444042a4e463</MessageID>");
            hb.Append("<Email>lulunb1u1</Email>");
            hb.Append("<Password>ggncqpZmjIres0TlGVhE7zKWtZ5sdWx1bmIxdTE=</Password>");
            hb.Append("<TaxID>0000000000006</TaxID>");
            hb.Append("<Ref1>3M</Ref1>");
            hb.Append("<Ref2>1485</Ref2>");
            hb.Append("<Ref3>09789323948</Ref3>");
            hb.Append("<Ref4></Ref4>");
            hb.Append("<Ref5></Ref5>");
            hb.Append("<Amount>1485</Amount>");
            hb.Append("<TopupType>S</TopupType>");
            hb.Append("<LocLatitude>90</LocLatitude>");
            hb.Append("<LocLongitude>80</LocLongitude>");
            hb.Append("<AppType></AppType>");
            hb.Append("<AgentFee></AgentFee>");
            hb.Append("<ProductDesc></ProductDesc>");
            hb.Append("</ConfirmReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butTxnList_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();
            string ts = System.DateTime.Now.ToString("yyyyMMddhhmmssffff");
            hb.Append("<TxnListReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("<TxnPerPage>10</TxnPerPage>");
            hb.Append("<PageNo>1</PageNo>");
            hb.Append("</TxnListReq>");
            


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void buttxnDetail_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<TxnDetailReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("<TxnID>2</TxnID>");
            hb.Append("</TxnDetailReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butBatchList_Click(object sender, EventArgs e)
        {
             txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();
            string ts = System.DateTime.Now.ToString("yyyyMMddhhmmssffff");
            hb.Append("<BatchListReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("<BatchPerPage>10</BatchPerPage>");
            hb.Append("<PageNo>1</PageNo>");
            hb.Append("</BatchListReq>");

            //hb.Append("<BatchListRes version="+"\"1.0\" total="+"\"100\" BatchPerPage="+"\"10\" timeStamp="+"\""+ ts +">");
            //hb.Append("<Batch batchID=" + "\"11\" txnCount=" + "\"12\" totalAmount=" + "\"00001\" dateTime=" + "\"2014/09/04\" agentLogo="+"\"123333\" txnStatus="+"\"PA\" merchantLogo=" + "\"imageUrl\" batchStatus=" + "\"0\" />");
            //hb.Append("</BatchListRes>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());

        }

        protected void butBatchDetail_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<BatchDetailReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("<BatchID>1</BatchID>");
            hb.Append("</BatchDetailReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butCloseShift_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<CloseShiftReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>ffYmhw3R7C6WCX+5EuyUqTVuMjpsdWx1QDJjMnAuY29t</Password>");
            hb.Append("<BatchID>1</BatchID>");
            hb.Append("</CloseShiftReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
            //txtCompress.Text = this.zCompress(txtRequest.Text.Trim());
        }

        protected void butResetPassword_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<ResetPasswordReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("</ResetPasswordReq>");

            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
        }

        protected void butChangePassword_Click(object sender, EventArgs e)
        {
            txtRequest.Text = string.Empty;
            txtCompress.Text = string.Empty;

            StringBuilder hb = new StringBuilder();

            hb.Append("<ChangePasswordReq>");
            hb.Append("<Version>1.0</Version>");
            hb.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            hb.Append("<Email>lulu@2c2p.com</Email>");
            hb.Append("<Password>Password9!</Password>");
            hb.Append("<NewPassword>Password1!</NewPassword>");
            hb.Append("</ChangePasswordReq>");


            txtRequest.Text = System.Xml.Linq.XDocument.Parse(hb.ToString()).ToString();
        }
        #endregion


        #region CustomMethods
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

        //public string zDecompress(string str)
        //{
        //    byte[] b = Convert.FromBase64String(str);
        //    string ret = "";
        //    int l = 0;
        //    byte[] w = new byte[1024];
        //    Stream s = new InflaterInputStream(new MemoryStream(b));

        //    try
        //    {
        //        while (true)
        //        {
        //            int i = s.Read(w, 0, w.Length);
        //            if (i > 0)
        //            {
        //                l += i;
        //                ret += Encoding.ASCII.GetString(w, 0, i);
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        s.Flush();
        //        s.Close();

        //        return ret;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public string ClientEncryptDecryptString(string inStr, string Option, out string errDetails)
        {
            string OutputStr = inStr;
            errDetails = "";

            try
            {
                SinaptIQPKCS7.PKCS7 PKCS7 = new SinaptIQPKCS7.PKCS7();
                //get key from web config
                string publicCert = System.Configuration.ConfigurationManager.AppSettings["ClientPublicCert"].ToString();
                string privateCert = System.Configuration.ConfigurationManager.AppSettings["ClientPrivateCert"].ToString();
                string privateCertPassword = System.Configuration.ConfigurationManager.AppSettings["ClientPrivatePass"].ToString();

                switch (Option)
                {
                    case "E":
                        OutputStr = PKCS7.encryptMessage(inStr, PKCS7.getPublicCert(publicCert));
                        break;
                    case "D":
                        string startTime, endTime;
                        startTime = DateTime.Now.ToString("HH:mm:ss.fff");
                        OutputStr = PKCS7.decryptMessage(inStr.Replace("\0", "").Replace(" ", "+"), PKCS7.getPrivateCert(privateCert, privateCertPassword));
                        endTime = DateTime.Now.ToString("HH:mm:ss.fff");
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
            }
            catch (Exception ex)
            {
               // writeLog("ClientEncryptDecryptString (err) : " + ex.ToString());
            }

            return OutputStr;

        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            if (!String.IsNullOrEmpty(txtRequest.Text))
                // txtCompress.Text  = this.zCompress(txtRequest.Text.Trim());
                txtCompress.Text = ClientEncryptDecryptString(txtRequest.Text.Trim(), "E", out errMsg);
            
        }

       
    }
}