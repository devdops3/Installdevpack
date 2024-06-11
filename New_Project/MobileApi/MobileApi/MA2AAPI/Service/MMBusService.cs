using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using log4net;
using System.Reflection;

namespace MA2AAPI.Service
{
    public class MMBusService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }

        public void PopulateMMBusTransaction(StringBuilder transactionStringBuilder, DataSet ds, int i, string REFID2, string NRC, string expirye,
        string imgURL, string total, string agentName, string smsMessage, string taxId, string amount, SMSHelper smsHelper, string mapTaxId, string biller,
        string ref1Name, string ref1Value, string ref2Value, string txnId, string serviceFee, string ref4Value, string REFID1, string REFID3,
        string REFID1Name, string REFID3Name, decimal agentFee, string branchCode)
        {
            writeLog("This is MmBusticket.");
            var actualTransactionAmount = double.Parse(amount) + double.Parse(serviceFee);
            var shurl = GetShortUrl(ref4Value);
            writeLog("ShortUrl : " + shurl);
            ref4Value = shurl;
            actualTransactionAmount = AddAgentAmountToTotalAmount(actualTransactionAmount);
            var smsMsg = smsHelper.GetMessageBiller(txnId, agentName, mapTaxId, biller, "", "", "", ds.Tables[0].Rows[i]["REF4NAME"].ToString(), "", "", "", shurl, actualTransactionAmount.ToString("#,##0.00"), serviceFee, total, branchCode);

            transactionStringBuilder.Append("<Txn txnID=" + "\"" + ds.Tables[0].Rows[i]["TRANSACTIONID"].ToString() +
                                         "\" txnDate=" + "\"" +
                                         ds.Tables[0].Rows[i]["TRANSACTIONDATETIME"].ToString() + "\" txnDesc=" + "\"" +
                                         ds.Tables[0].Rows[i]["PRODUCTDESC"].ToString() + "\" txnAmount=" + "\"" +
                                         ds.Tables[0].Rows[i]["TRANSACTIONAMOUNT"].ToString() + "\" ref1=" + "\"" +
                                         REFID1 + "\" ref2=" + "\"" + REFID2 + "\" ref3=" + "\"" +
                                         REFID3 + "\" ref4=" + "\"" + ref4Value + "\" ref5=" + "\"" +
                                         ds.Tables[0].Rows[i]["REFID5"].ToString() + "\" ref1Name=" + "\"" +
                                         REFID1Name + "\" ref2Name=" + "\"" +
                                         ds.Tables[0].Rows[i]["REF2NAME"].ToString() + "\" ref3Name=" + "\"" + REFID3Name +
                                         "\" ref4Name=" + "\"" +
                                         ds.Tables[0].Rows[i]["REF4NAME"].ToString() + "\" ref5Name=" + "\"" +
                                         ds.Tables[0].Rows[i]["REF5NAME"].ToString() + "\" BillerName=" + "\"" +
                                         ds.Tables[0].Rows[i]["COMPANYNAME"].ToString() + "\" txnStatus=" + "\"" +
                                         ds.Tables[0].Rows[i]["TRANSACTIONSTATUS"].ToString() + "\" billerLogo=" + "\"" +
                                         imgURL + "\" locLatitude=" + "\"" +
                                         ds.Tables[0].Rows[i]["LATITUDE"].ToString() + "\" locLongitude=" + "\"" +
                                         ds.Tables[0].Rows[i]["LONGITUDE"].ToString() + "\" agentFee=" + "\"" + agentFee +
                                         "\" total=" + "\"" +
                                         total + "\" agentName=" + "\"" + agentName + "\" sms=" + "\"" + smsMsg +
                                         "\"  TaxID=" + "\"" + taxId + "\" />");
        }

        public string GetShortUrl(string longUrl)
        {
            var shortUrl = string.Empty;
            var shortUrlUser = ConfigurationManager.AppSettings["ShortUrlUser"];
            var shortUrlToken = ConfigurationManager.AppSettings["ShortUrlToken"];
            var shortUrlApi = ConfigurationManager.AppSettings["ShortUrlApi"];
            try
            {
                var jsonData = "{\"LongUrl\":\"" + longUrl + "\"}";
                var body = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var client = new HttpClient
                {
                    DefaultRequestHeaders = { { "ShortUrlUser", shortUrlUser }, { "ShortUrlToken", shortUrlToken } }
                };

                using (var response = client.PostAsync(new Uri(shortUrlApi), body).Result)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var urlResponse = JsonConvert.DeserializeObject<dynamic>(result);

                    if (urlResponse != null)
                    {
                        shortUrl = urlResponse.ShortUrl;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return shortUrl;
        }

        private double AddAgentAmountToTotalAmount(double netAmount)
        {
            var agentFeePercent = Convert.ToDouble(ConfigurationManager.AppSettings["MMBusAgentPercent"].ToString());
            writeLog("Net Amount : "+netAmount);
            var netTransactionPercent = 100 - agentFeePercent;
            writeLog("Net Transaction Percent : " + netTransactionPercent);
            var totalAmount = (netAmount *100) / netTransactionPercent;
            writeLog("Total Amount : " + totalAmount);
            return totalAmount;
        }
    }
}