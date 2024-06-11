using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MA2AAPI.Class
{
    public class TransactionManager
    {
        public TransactionManager()
        { }

        #region
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }

        #endregion

        public void PopulateSayaTransaction(StringBuilder transactionBuilder, DataSet ds,
        int i, string amount, decimal total,
        string agentName, string taxID, string biller,
        string txnID, string serviceFee, string branchCode,
        string REFID1, string REFID2, string REFID3, string ref4Value,
        string REFID1Name, string REFID3Name, string imgURL, SMSHelper smsH, string smsMsg, decimal agentFee, string agentLogo, string REFID5)
        {
            writeLog("Populate Saya Txns");
            string am = double.Parse(amount).ToString("#,##0.00");
            string totalAmt = total.ToString("#,##0.00");
            smsMsg = smsH.getMessageBiller(agentName, taxID, biller, "Reg Mobile No.", "Package",
                    string.Empty, "Ref", REFID1, REFID3, string.Empty, txnID.ToString(), am, serviceFee, totalAmt, branchCode);



            transactionBuilder.Append("<Txn txnID=" + "\"" + ds.Tables[0].Rows[i]["TRANSACTIONID"].ToString() +
                      "\" txnDate=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONDATETIME"].ToString() + "\" txnDesc=" + "\"" +
                      ds.Tables[0].Rows[i]["PRODUCTDESC"].ToString() + "\" txnAmount=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONAMOUNT"].ToString() + "\" ref1=" + "\"" +
                      REFID1 + "\" ref2=" + "\"" + string.Empty + "\" ref3=" + "\"" +
                      REFID3 + "\" ref4=" + "\"" + ref4Value + "\" ref5=" + "\"" +
                      REFID5 + "\" ref1Name=" + "\"" +
                      REFID1Name + "\" ref2Name=" + "\"" +
                      string.Empty + "\" ref3Name=" + "\"" + REFID3Name +
                      "\" ref4Name=" + "\"" +
                      ds.Tables[0].Rows[i]["REF4NAME"].ToString() + "\" ref5Name=" + "\"" +
                      ds.Tables[0].Rows[i]["REF5NAME"].ToString() + "\" BillerName=" + "\"" +
                      ds.Tables[0].Rows[i]["COMPANYNAME"].ToString() + "\" txnStatus=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONSTATUS"].ToString() + "\" billerLogo=" + "\"" +
                      imgURL + "\" locLatitude=" + "\"" +
                      ds.Tables[0].Rows[i]["LATITUDE"].ToString() + "\" locLongitude=" + "\"" +
                      ds.Tables[0].Rows[i]["LONGITUDE"].ToString() + "\" agentFee=" + "\"" + agentFee +
                      "\" total=" + "\"" +
                      total + "\" agentName=" + "\"" + agentName
                      + "\" sms=" + "\"" + smsMsg +
                      "\" agentLogo=" + "\"" + agentLogo +
                      "\"  TaxID=" + "\"" + taxID + "\" />");

        }

        public void PopulateAyaPayCashInTransaction(StringBuilder transactionBuilder, DataSet ds,
        int i, string amount, decimal total,
        string agentName, string taxID, string biller,
        string txnID, string serviceFee, string branchCode,
        string REFID1, string REFID2, string REFID3, string ref4Value,
        string REFID1Name, string REFID3Name, string imgURL, SMSHelper smsH, string smsMsg, decimal agentFee, string agentLogo, string REFID5)
        {
            writeLog("Populate AyaPayCash Txns");
            string am = double.Parse(amount).ToString("#,##0.00");
            string totalAmt = total.ToString("#,##0.00");
            smsMsg = string.Empty;



            transactionBuilder.Append("<Txn txnID=" + "\"" + ds.Tables[0].Rows[i]["TRANSACTIONID"].ToString() +
                      "\" txnDate=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONDATETIME"].ToString() + "\" txnDesc=" + "\"" +
                      ds.Tables[0].Rows[i]["PRODUCTDESC"].ToString() + "\" txnAmount=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONAMOUNT"].ToString() + "\" ref1=" + "\"" +
                      REFID1 + "\" ref2=" + "\"" + string.Empty + "\" ref3=" + "\"" +
                      string.Empty + "\" ref4=" + "\"" + string.Empty + "\" ref5=" + "\"" +
                      string.Empty + "\" ref1Name=" + "\"" +
                      REFID1Name + "\" ref2Name=" + "\"" +
                      string.Empty + "\" ref3Name=" + "\"" + string.Empty +
                      "\" ref4Name=" + "\"" +
                      string.Empty + "\" ref5Name=" + "\"" +
                      string.Empty + "\" BillerName=" + "\"" +
                      ds.Tables[0].Rows[i]["COMPANYNAME"].ToString() + "\" txnStatus=" + "\"" +
                      ds.Tables[0].Rows[i]["TRANSACTIONSTATUS"].ToString() + "\" billerLogo=" + "\"" +
                      imgURL + "\" locLatitude=" + "\"" +
                      ds.Tables[0].Rows[i]["LATITUDE"].ToString() + "\" locLongitude=" + "\"" +
                      ds.Tables[0].Rows[i]["LONGITUDE"].ToString() + "\" agentFee=" + "\"" + agentFee +
                      "\" total=" + "\"" +
                      total + "\" agentName=" + "\"" + agentName
                      + "\" sms=" + "\"" + smsMsg +
                      "\" agentLogo=" + "\"" + agentLogo +
                      "\"  TaxID=" + "\"" + taxID + "\" />");
        }
    }
}