using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Globalization;
using MA2AAPI.Class;
using System.Security.Cryptography;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using log4net;
using MA2AAPI;
using MA2AAPI.Service;

/// <summary>
/// Summary description for Utils
/// </summary>
public class Utils
{
    private static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static void writeLog(string logstring)
    {
        Logger.writeLog(logstring, ref log);
    }

    public Utils()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool CheckParentNodeNameXML(string xmlData, string parentNodeName)
    {
        writeLog("CheckParentNodeNameXML");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);
        XmlElement root = xmlDoc.DocumentElement;
        string parentNode = root.Name.ToString();
        writeLog("CheckParentNodeNameXML parentNode : " + parentNode);
        if (parentNode.Equals(parentNodeName))
            return true;
        return false;

    }

    public static string GetDataFromXMLString(string xmlData, string xmlNodeName)
    {
        writeLog("GetDataFromXMLString");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);

        XmlNodeList elemlist = xmlDoc.GetElementsByTagName(xmlNodeName);

        string result = elemlist[0].InnerXml;
        writeLog("GetDataFromXMLString result : " + result);
        return result;
    }

    public static string ReplaceCustomElementForXML(string xmlData, string xmlNodeName, string customElement)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);
        writeLog("ReplaceCustomElementForXML");
        XmlNodeList elemlist = xmlDoc.GetElementsByTagName(xmlNodeName);
        writeLog("Before ReplaceCustomElementForXML GetElementsByTagName : " + elemlist[0].InnerXml);
        elemlist[0].InnerXml = customElement;
        writeLog("After ReplaceCustomElementForXML : " + elemlist[0].InnerXml);
        return xmlDoc.InnerXml.ToString();
    }

    public static string RemovePhotoByteString(string requestXML)
    {
        var requestXMLElemnt = XElement.Parse(requestXML);
        string SelfiePhotoByteStr = requestXMLElemnt.Element("SelfiePhoto").Value.ToString();
        string PhotoWithIdByteStr = requestXMLElemnt.Element("SelfiePhotoWithIdCard").Value.ToString();
        if (!string.IsNullOrEmpty(SelfiePhotoByteStr) && !string.IsNullOrEmpty(PhotoWithIdByteStr))
        {
            requestXML = requestXML.Replace(SelfiePhotoByteStr, string.Empty);
            requestXML = requestXML.Replace(PhotoWithIdByteStr, string.Empty);
        }
        return requestXML;

    }

    public static string GenerateRandomPassword(int passwordLength)
    {
        return System.Web.Security.Membership.GeneratePassword(passwordLength, 1);
    }

    public static string stringToBase64(string st)
    {
        byte[] b = new byte[st.Length];
        for (int i = 0; i < st.Length; i++)
        {
            b[i] = Convert.ToByte(st[i]);
        }
        return Convert.ToBase64String(b);
    }

    public static string ComputeHash(string plainText, string saltKey)
    {
        byte[] saltBytes = null, plainTextBytes = null, plainTextWithSaltBytes = null, hashBytes = null, hashWithSaltBytes = null;
        string hashValue = "";
        HashAlgorithm algorithm = null;

        try
        {
            saltBytes = Convert.FromBase64String(saltKey);

            // Convert plain text into a byte array.
            plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.        
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Compute hash value of our plain text with appended salt.        
            algorithm = new SHA1Managed();
            hashBytes = algorithm.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.        
            hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            // Copy hash bytes into resulting array.        
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.        
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.        
            hashValue = Convert.ToBase64String(hashWithSaltBytes);
        }
        catch
        {
            throw;
        }
        finally
        {
            saltBytes = null;
            plainTextBytes = null;
            plainTextWithSaltBytes = null;
            hashBytes = null;
            hashWithSaltBytes = null;
        }

        return hashValue;
    }

    public static Hashtable getHTableFromXML(string requestXML)
    {
        Hashtable ht = new Hashtable();
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(requestXML);
        XmlNodeList xnl = xdoc.ChildNodes;
        if (xnl.Count > 0)
        {

            XmlNode rootNode;
            if (xnl.Count > 1)
                rootNode = xnl.Item(1);
            else

                rootNode = xnl.Item(0);

            XmlNodeList subNodes = rootNode.ChildNodes;
            if (subNodes.Count >= 1)
            {
                foreach (XmlNode xn in subNodes)
                {
                    ht.Add(xn.Name, xn.InnerText);
                }
            }
            else
            {
                ht = null;
            }
        }
        else
        {
            ht = null;
        }

        return ht;
    }

    public static Hashtable GetHTableFromRightsXML(string requestXML)
    {
        Hashtable ht = new Hashtable();
        XmlDocument xdoc = new XmlDocument();
        xdoc.LoadXml(requestXML);
        XmlNodeList xnl = xdoc.ChildNodes;
        if (xnl.Count > 0)
        {

            XmlNode rootNode;
            if (xnl.Count > 1)
                rootNode = xnl.Item(1);
            else

                rootNode = xnl.Item(0);

            XmlNodeList subNodes = rootNode.ChildNodes;

            if (subNodes.Count >= 1)
            {
                foreach (XmlNode xn in subNodes)
                {
                    if (xn.InnerText.Length == 1)
                    {
                        ht.Add(xn.Name, xn.InnerText);
                    }
                    else
                    {
                        for (int i = 0; i < xn.ChildNodes.Count; i++)
                        {
                            ht.Add(xn.ChildNodes[i].Name, xn.ChildNodes[i].InnerText);
                        }
                    }

                }
            }
            else
            {
                ht = null;
            }
        }
        else
        {
            ht = null;
        }

        return ht;
    }

    public static Hashtable getBillerIDHashTable(string biller663)
    {
        Hashtable hashtable = new Hashtable();
        string[] strArray = biller663.Split(";".ToCharArray());
        for (int i = 0; i < strArray.Length; i++)
        {
            string[] strArray2 = strArray[i].Split(":".ToCharArray());
            if (strArray2.Length > 1)
            {
                hashtable.Add(strArray2[0], strArray2[1]);
            }
        }
        return hashtable;
    }
    public static string getTimeStamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
    }

    public static DateTime getStrToDate(string dtStr, string format)
    {
        return DateTime.ParseExact(dtStr, format, CultureInfo.InvariantCulture);
    }
    public static double getAmountDBL(string amt)
    {
        if (amt.Length > 2)
        {
            amt = amt.Substring(0, amt.Length - 2) + "." + amt.Substring(amt.Length - 2, 2);
            return double.Parse(amt);
        }
        return double.Parse(amt);
    }
    public static string getFrom12DigitToOrginal(string amt)
    {
        if (amt.Length > 2)
        {
            amt = amt.Substring(0, amt.Length - 2);
            return double.Parse(amt).ToString();
        }
        return "0";
    }
    public static double getAgentFee(double amount, float feePercent,
                         double feeFlat
                         )
    {
        double agentFee = 0;
        if (feePercent > 0)
        {
            agentFee = (feePercent / 100) * amount;
        }


        if (feeFlat > 0)
        {
            agentFee += feeFlat;
        }

        agentFee = roundAmount(agentFee);

        return agentFee;

    }
    public static double roundAmount(double amt)
    {
        string amtStr = amt.ToString("##0.00");
        try
        {
            return double.Parse(amtStr);
        }
        catch
        {
            return 0.0;
        }
    }
    public static string getFromOrginalTo12Digit(string amt)
    {
        amt += "00";
        return amt.PadLeft(12, '0');
    }
    public static string getAmountFromDBL(double amt)
    {
        string[] strAmt = amt.ToString().Split('.');
        string resultAmt = "";
        if (strAmt.Length > 1)
        {
            resultAmt = strAmt[0] + strAmt[1].PadRight(2, '0');
        }
        else
        {
            resultAmt = strAmt[0] + "00";
        }
        return resultAmt.PadLeft(12, '0');
    }

    public static string changeStatusToName(string statusChk)
    {
        string dataOutput = null;
        switch (statusChk)
        {
            case "PE": dataOutput = "Pending"; break;
            case "EX": dataOutput = "Expired"; break;
            case "PA": dataOutput = "Paid"; break;
            case "PM": dataOutput = "Paid(More Mismatched)"; break;
            case "PL": dataOutput = "Paid(Less Mismatched)"; break;
            case "CA": dataOutput = "Canceled"; break;
            case "RE": dataOutput = "Rejected"; break;
            case "MM": dataOutput = "MismatchedAmount"; break;
            case "RF": dataOutput = "RefNotFound"; break;
            case "BC": dataOutput = "Browser Closed"; break;
            case "FA": dataOutput = "Failed"; break;
            case "VO": dataOutput = "Voided"; break;
            case "IN": dataOutput = "Browser Closed"; break;
            case "NA": dataOutput = "No-Action"; break;
        }
        return dataOutput;
    }
    public static double getFee(double amount, float feePercent,
                        double feeFlat
                        )
    {
        double agentFee = 0;
        if (feePercent > 0)
        {
            agentFee = (feePercent / 100) * amount;
        }


        if (feeFlat > 0)
        {
            agentFee += feeFlat;
        }

        agentFee = Math.Round(agentFee, 0);

        return agentFee;

    }
    public static Hashtable getMerchantCodeHashtable(string merchantCode)
    {
        Hashtable hashtable = new Hashtable();
        string[] strArray = merchantCode.Split(";".ToCharArray());
        for (int i = 0; i < strArray.Length; i++)
        {
            string[] strArray2 = strArray[i].Split(":".ToCharArray());
            if (strArray2.Length > 1)
            {
                hashtable.Add(strArray2[0], strArray2[1]);
            }
        }
        return hashtable;
    }
    public static bool sendMail(EmailApiModel model, string email, string username, string password)
    {
        bool result = false;
        string errMsg = "";
        string msgBody = "";

        msgBody = "Dear " + username + ",";
        model.toEmail = email;
        model.subject = "Password Assistance";
        model.msgBody = msgBody + "\n \n           Your Password is " + password + "";
        model.fromEmailAddress = "alert@2c2p.com";
        model.fromEmailDisplayName = "2C2P";
        model.isHTML = true;

        string toEmail = model.toEmail == null ? "" : model.toEmail;
        string ccEmail = model.ccEmail == null ? "" : model.ccEmail;
        string bccEmail = model.bccEmail == null ? "" : model.bccEmail;
        string fromEmailAddress = model.fromEmailAddress == null ? "" : model.fromEmailAddress;
        string fromEmailDisplayName = model.fromEmailDisplayName == null ? "" : model.fromEmailDisplayName;
        string subject = model.subject == null ? "" : model.subject;
        msgBody = model.msgBody == null ? "" : model.msgBody;
        string category = model.category == null ? "" : model.category;
        string identifierKey = model.identifierKey == null ? "" : model.identifierKey;
        string identifierValue = model.identifierValue == null ? "" : model.identifierValue;
        bool isAttached = model.isAttached;
        string attFilePath = model.attFilePath == null ? "" : model.attFilePath;
        string attFileName = model.attFileName == null ? "" : model.attFileName;
        bool isHTML = model.isHTML;

        SendGridEmailService sendGridEmailService = new SendGridEmailService();
        SendGridEmailRequest sendGridEmailRequest = new SendGridEmailRequest
        {
            FromEmailDisplayName = fromEmailDisplayName,
            Subject = subject,
            MessageBody = msgBody,
            ToAddress = toEmail
        };

        if (sendGridEmailService.SendEmail(sendGridEmailRequest))
        {
            result = true;
        }

        return result;
    }

    public static string getCBMStandard(string taxID, string suffix, string refNo1, string refNo2, string amount)
    {
        string strResult = "";

        if (refNo1.Length > 16)
        {
            refNo1 = refNo1.Substring(refNo1.Length - 16, 16).PadLeft(16, '0');
        }
       
        if (refNo2.Length > 16)
        {
            refNo2 = refNo2.Substring(refNo2.Length - 16, 16).PadLeft(16, '0');
        }
       
        strResult = "*" + taxID + suffix + "\r" + refNo1 + "\r" + refNo2 + "\r" + amount;
        return strResult;

    }

    public static string generateQRCode(string barcodeData, string paymentCode)
    {
        string str = ConfigurationManager.AppSettings["barcodePath"].ToString(); // IIS
        string name = "AGENT_" + paymentCode; 
        string filename = str + name + ".gif";
        if (!File.Exists(filename))
        {
            string logoPath = ConfigurationManager.AppSettings["templatePath"].ToString();
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H; // 30%

            encoder.QRCodeScale = 4;
            Bitmap imgValue = encoder.Encode(barcodeData, Encoding.ASCII);

            Image logo = Image.FromFile(logoPath);

            int left = (imgValue.Width / 2) - (logo.Width / 2);

            int top = (imgValue.Height / 2) - (logo.Height / 2);
            Graphics g = Graphics.FromImage(imgValue);
            g.DrawImage(logo, new Point(left - 9, top - 8));

            imgValue.Save(filename, ImageFormat.Jpeg);
        }
        return (ConfigurationManager.AppSettings["barcodeUrl"].ToString() + name + ".gif");
    }

    public static string GetErrorString(string version, string resCode, string resDesc)
    {
        var sb = new StringBuilder();

        sb.Append("<Error>");
        sb.Append("<Version>" + version + "</Version>");
        sb.Append("<TimeStamp>" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
        sb.Append("<ResCode>" + resCode + "</ResCode>");
        sb.Append("<ResDesc>" + resDesc + "</ResDesc>");
        sb.Append("</Error>");

        return sb.ToString();
    }

    public static string GetErrorResponse(string rescode, string resdesp)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<Error>");
        sb.Append("<Version>1.0</Version>");
        sb.Append("<TimeStamp>" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
        sb.Append("<ResCode>" + rescode + "</ResCode>");
        sb.Append("<ResDesc>" + resdesp + "</ResDesc>");
        sb.Append("</Error>");
        return sb.ToString();
    }

    public static string GetErrorResponse(string code, string description, string timeStamp, string version)
    {
        var sb = new StringBuilder();
        sb.Append("<Error>");
        sb.Append("<Version>" + version + "</Version>");
        sb.Append("<TimeStamp>" + timeStamp + "</TimeStamp>");
        sb.Append("<ResCode>" + code + "</ResCode>");
        sb.Append("<ResDesc>" + description + "</ResDesc>");
        sb.Append("</Error>");
        return sb.ToString();
    }

    public static DataSet ConvertXMLtoDataset(string strxml)
    {
        DataSet ds = new DataSet();
        try
        {
            if (strxml == "")
            {
                ds = null;
            }
            else
            {
                StringReader sr = new StringReader(strxml);
                ds.ReadXml(sr);
            }
        }
        catch
        {
            ds = null;
        }

        return ds;

    }
    public static string maskString(string sourceString)
    {
        string result = "XX" + sourceString.Substring(2);
        return result;
    }

    public static string getEntryMode(string code)
    {
        if (code == "051")
        {
            return "Chip";
        }
        else
        {
            return "Chip";
        }
    }

    public enum HTTPResponseCodes
    {
        None,
        [Display(Name = "Success")]
        Success = 200,
        [Display(Name = "Bad Request")]
        BadRequest = 400,
        [Display(Name = "Unauthorized")]
        Unauthorized = 401,
        [Display(Name = "Internal Server Error")]
        InternalServerError = 500,
        [Display(Name = "System Error")]
        SystemError = 99

    };

    public static void GetPinExpiry(string str, out string pin, out string expiry)
    {
        pin = expiry = "-";
        if (!string.IsNullOrEmpty(str))
        {
            string[] strList = str.Split(' ').ToArray();
            pin = strList.FirstOrDefault();
            if (strList.Count() > 1)
            {
                expiry = strList.LastOrDefault();
            }
        }

    }

    public static bool IsSameDevice(string deviceId1, string deviceId2)
    {
        deviceId1 = deviceId1.Remove(6, 2);
        deviceId2 = deviceId2.Remove(6, 2);
        return deviceId1 == deviceId2;
    }

}

public class ProductEnquiryResponse
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string BillerCategory { get; set; }
    public string BillerCode { get; set; }
    public string BillerName { get; set; }
    public string BillerCurrency { get; set; }
    public string PartnerCurrency { get; set; }
    public string TransactionCurrency { get; set; }
    public List<BillerProduct> billerProduct { get; set; }

}
public class BillerProduct
{
    public string code { get; set; }
    public string description { get; set; }
    public string billingAmount { get; set; }
    public string Transactionamount { get; set; }
    public string PartnerAmount { get; set; }
}

public class Detail
{
    public string Type { get; set; }
    public string Deno { get; set; }
}

public class DescriptionDetail
{
    public string Type { get; set; }
    public string DenoDescription { get; set; }
}

public class ServiceList
{
    public string ServiceId { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}

public class MyanPwelDetail
{
    public string EventList { get; set; }
}

public class MyanPwelEvent
{
    public string id { get; set; }
    public string title { get; set; }
    public string eventStartDate { get; set; }
    public string eventEndDate { get; set; }
    public string nrcRequired { get; set; }
}


public class DescriptionList
{
    public string Description { get; set; }
    public double price { get; set; }
}

public static class EnumHelper
{
    public static string ToDisplayName(this Enum value)
    {
        var attribute = value.GetAttribute<DisplayAttribute>();
        return attribute == null ? value.ToString() : attribute.Name;
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return (T)attributes.FirstOrDefault();
    }
}