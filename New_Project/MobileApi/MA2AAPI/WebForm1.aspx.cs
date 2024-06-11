using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;

namespace MA2AAPI
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string taxid = "0000000000017";
            string merchantlogourl = string.Empty;
            string merchantlistpath = string.Empty;
            string _merchent = string.Empty;
            StringBuilder sb = new StringBuilder();
            string test = ConfigurationManager.AppSettings["V"].ToString();
            if (!String.IsNullOrEmpty(taxid) && taxid == ConfigurationManager.AppSettings["EasyPoints"].ToString())
            {
                merchantlogourl = ConfigurationManager.AppSettings["EasyPointsMerchantLogoUrl"].ToString();
                merchantlistpath = ConfigurationManager.AppSettings["EasyPointsMerchantListPath"].ToString();
            }
            else
            {
                merchantlogourl = ConfigurationManager.AppSettings["MerchantLogoUrl"].ToString();
                merchantlistpath = ConfigurationManager.AppSettings["MerchantListPath"].ToString();
            }
            string[] _merchantlist = System.IO.File.ReadAllLines(merchantlistpath);
            int _totalmerchant = _merchantlist.Count();
            sb.Append("<MerListRes ResCode=" + "\"00\"  ResDesc=" + "\"success\" version=" + "\"1.0\" total=" + "\"" + _merchantlist.Count() + "\" timeStamp=" + "\"" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "\" messageID=" + "\"" + "AJDKJVCK" + "\"" + ">");

            for (int i = 0; i < _totalmerchant; i++)
            {

                _merchent = _merchantlist[i];

                string[] strArray = _merchent.Split(";".ToCharArray());
                sb.Append("<Merchant merID=" + "\"" + strArray[0] + "\" merName=" + "\"" + strArray[1]
                    + "\" merlogo=" + "\"" + merchantlogourl + strArray[2] + "\" merUrl= " + "\"" + strArray[3]
                     + "\"/>");

            }
            sb.Append("</MerListRes>");
          //  A2A_process a2a = new A2A_process();
         //   string request = @"<ConfirmReq><Version>1.0</Version><TimeStamp>201702211056159720</TimeStamp><MessageID>42d0a6fa-a6c-440f-a58b-3b0487669c</MessageID><Email>lulunb1u1</Email><Password>NiS2foLhEzocRWp/g7e2Zhna0P9sdWx1bmIxdTE=</Password><TaxID>5555555555555</TaxID><Ref1></Ref1><Ref2>1000</Ref2><Ref3>09794937567</Ref3><Ref4></Ref4><Ref5></Ref5><Ref6></Ref6><Amount>1000</Amount><TopupType>A</TopupType><LocLatitude>16.8022379</LocLatitude><LocLongitude>96.1614398</LocLongitude><AppType>MS</AppType><AgentFee></AgentFee><ProductDesc></ProductDesc></ConfirmReq>";
//      string request=@" <MLMProductListReq>
//<Version>1.0</Version>
//<TimeStamp>yyyyMMddhhmmssffff</TimeStamp>
//<MessageID>768866yyhhhhhh</MessageID>
//<Email>lulunb1u1</Email>
//<Password>NiS2foLhEzocRWp/g7e2Zhna0P9sdWx1bmIxdTE=</Password>
//</MLMProductListReq>";
       //  string response=   a2a.ConfirmReq(request);
    //  a2a.MLMProductListReq(request);
            //MA2AAPIWCF.ServiceClient _agentWCF = new MA2AAPIWCF.ServiceClient();
            //DataSet meterDivisionDs = new DataSet();
            //string errmsg = string.Empty;
            //if (_agentWCF.getMeterDivisionList(out meterDivisionDs, out errmsg))
            //    {
            //        StringBuilder _meterDivisionListRes = new StringBuilder();
            //        string totalcount = string.Empty;
            //        try
            //        {



            //            _meterDivisionListRes.Append("<DivisionListRes>");
            //            _meterDivisionListRes.Append("<ResCode>00</ResCode>");
            //            _meterDivisionListRes.Append("<ResDesc>success</ResDesc>");
            //            _meterDivisionListRes.Append("<Version>1.0</Version>");
            //            _meterDivisionListRes.Append("<TimeStamp>" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "</TimeStamp>");
            //            _meterDivisionListRes.Append("<MessageID>DJFDDLFJIOEHFDKNV</MessageID>");


            //            if (meterDivisionDs.Tables[1].Rows.Count > 0)
            //            {
            //                for (int i = 0; i < meterDivisionDs.Tables[0].Rows.Count; i++)
            //                {
            //                    string divisionId = meterDivisionDs.Tables[0].Rows[i]["DIVISIONID"].ToString();
            //                    string divisionName = meterDivisionDs.Tables[0].Rows[i]["DIVISIONNAME"].ToString();
            //                    string divisionNameEng = meterDivisionDs.Tables[0].Rows[i]["DIVISIONNAMEENG"].ToString();
            //                    _meterDivisionListRes.Append("<Division>");
            //                    _meterDivisionListRes.Append("<DivisionID>" + divisionId + "</DivisionID>");
            //                    _meterDivisionListRes.Append("<DivisionName>" + divisionName + "</DivisionName>");
            //                    _meterDivisionListRes.Append("<DivisionNameEng>" + divisionNameEng + "</DivisionNameEng>");
            //                    _meterDivisionListRes.Append("<Townships>");
            //                    for (int t = 0; t < meterDivisionDs.Tables[1].Rows.Count; t++)
            //                    {
            //                        if (meterDivisionDs.Tables[1].Rows[t]["DIVISIONID"].ToString() == divisionId)
            //                        {
            //                            _meterDivisionListRes.Append("<Township>");
            //                            _meterDivisionListRes.Append("<TownshipCode>" + meterDivisionDs.Tables[1].Rows[t]["TOWNSHIPCODE"].ToString() + "</TownshipCode>");
            //                            _meterDivisionListRes.Append("<TownshipName>" + meterDivisionDs.Tables[1].Rows[t]["TOWNSHIPNAME"].ToString() + "</TownshipName>");
            //                            _meterDivisionListRes.Append("<TownshipNameEng>" + meterDivisionDs.Tables[1].Rows[t]["TOWNSHIPNAMEENG"].ToString() + "</TownshipNameEng>");
            //                            _meterDivisionListRes.Append("<Biller>" + meterDivisionDs.Tables[1].Rows[t]["BILLER"].ToString() + "</Biller>");
            //                            _meterDivisionListRes.Append("</Township>");
            //                        }
            //                    }
            //                    _meterDivisionListRes.Append("</Townships>");
            //                    _meterDivisionListRes.Append("</Division>");
            //                    //string expiry = string.Empty;
            //                    ////storeCardID="1" agentID="A0004" name="name" panmask="1234 5678 xxxx 0987" expiryDate="12/12" isDefault="Y"
            //                    //expiry = meterDivisionDs.Tables[0].Rows[i]["EXPIRY"].ToString();
            //                    ////expiry=DateTime.ParseExact(expiry, "ddMMyyyy", CultureInfo.InvariantCulture).ToShortDateString();
            //                    //_meterDivisionListRes.Append("<StoreCard storeCardID=" + "\"" + meterDivisionDs.Tables[0].Rows[i]["STORECARDID"].ToString() + "\" agentID=" + "\"" +
            //                    //        meterDivisionDs.Tables[0].Rows[i]["AGENTID"].ToString() + "\" name=" + "\"" +
            //                    //        meterDivisionDs.Tables[0].Rows[i]["NAME"].ToString() + "\" panmask=" + "\"" +
            //                    //        meterDivisionDs.Tables[0].Rows[i]["PANMASK"].ToString() + "\" expiryDate=" + "\"" +
            //                    //        expiry + "\" isDefault=" + "\"" + meterDivisionDs.Tables[0].Rows[i]["ISDEFAULT"].ToString() + "\" />");

            //                }
            //            }
            //            _meterDivisionListRes.Append("</DivisionListRes>");
            //        }
            //        catch (Exception ex)
            //        {
            //           // writeLog("Exception Error occur in get StoreCard List:" + ex.Message.ToString());
            //        }
            //         _meterDivisionListRes.ToString();
            //}
            //if (_agentWCF.get_Denomination("3333333333333", out ds, out errmsg))//(_agentWCF.getBillerList(categoryID,out ds, out errmsg)) //txnperpage, pageno,
            //{

            //    StringBuilder sb = new StringBuilder();


            //    if (ds.Tables[0].Rows.Count > 0)
            //    {

            //        DataTable dt = getDenomination(ds.Tables[0].Rows[0]["DENOMINATIONXML"].ToString());
            //        sb.Append("<DenominationListRes version=" + "\"1.0\" total=" + "\"" + dt.Rows.Count.ToString() + "\" timeStamp=" + "\"" + System.DateTime.Now.ToString("yyyyMMddhhmmssffff") + "\"" + ">");
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {

            //            sb.Append("<Denomination  value=" + "\"" + dt.Rows[i][1].ToString() + "\" text=" + "\"" + dt.Rows[i][0].ToString() + "\"/>");
            //        }
            //    }
            //    sb.Append("</DenominationListRes>");
            //    string returns = string.Empty;
            //    returns=sb.ToString();

            //}
//            string req = @"<StoreCardListReq>
//<Version>1.0</Version>
//<TimeStamp>201512281522170112</TimeStamp>
//<MessageID>768866yyfdghhhhhh</MessageID>
//<Email>myo111</Email>
//<Password>HZerwA1MH4aYxnBYYLNUjArpLYZteW8xMTE=</Password>
//<CardPerPage>10</CardPerPage>
//<PageNo>1</PageNo>
//</StoreCardListReq>";         

//            string req = @"<RemoveCardRes>
//<Version>1.0</Version>
//<TimeStamp>201512281522170112</TimeStamp>
//<MessageID>768866yyfdghhhh</MessageID>
//<Email>myo111</Email>
//<Password>HZerwA1MH4aYxnBYYLNUjArpLYZteW8xMTE=</Password>
//<StoreCardID>3</StoreCardID>
//</RemoveCardRes>";

//            A2A_process a = new A2A_process();
//          // a.RegisterReq(req);
           
//           a.RemoveCardReq(req);
        }

        public DataTable getDenomination(string sourceXml)
        {
            StringBuilder sbJson = new StringBuilder();

            DataSet dsTemp = null;
            dsTemp = Utils.ConvertXMLtoDataset(sourceXml);

            return dsTemp.Tables[0];


        }

    }
}