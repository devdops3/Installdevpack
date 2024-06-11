namespace MA2AAPI.Class
{
    public class EmailApiModel
    {
       
            public string toEmail { get; set; }
            public string ccEmail { get; set; }
            public string bccEmail { get; set; }
            public string fromEmailAddress { get; set; }
            public string fromEmailDisplayName { get; set; }
            public string subject { get; set; }
            public string msgBody { get; set; }
            public string category { get; set; }
            public string identifierKey { get; set; }
            public string identifierValue { get; set; }
            public bool isAttached { get; set; }
            public string attFilePath { get; set; }
            public string attFileName { get; set; }
            public bool isHTML { get; set; }
        

    }

    public class StoreCardListRequestData
    {
        public string version { get; set; }
        public string timeStamp { get; set;}
        public string messageid { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public int cardParPage { get; set; }
        public int pageNo { get; set; }
        public bool validateResult { get; set; }
        public string rescode { get; set; }
        public string resdesc { get; set; }
    }

    public class StoreCardListResponseData
    {
        public string version { get; set; }
        public string timestamp { get; set; }
        public string messageid { get; set; }
        public string total { get; set; }
        public string cardPerPage { get; set; }
        public string responseCode { get; set; }
        public string responseDesc { get; set; }
    }
    public class storeCardDetail
    {
        public string storeCardID { get; set; }
        public string agentID { get; set; }
        public string name { get; set; }
        public string panMask { get; set; }
        public string expiryDate { get; set; }
        public string isDefault { get; set; }
       
    }

    public class SetDefaultCardReqData
    {
        public string version { get; set; }
        public string timeStamp { get; set; }
        public string messageid { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public string storecardid { get; set; }
        public bool validateResult { get; set; }
        public string rescode { get; set; }
        public string resdesc { get; set; }
    }
    //<StoreCardListRes 
    //version="1.0" total="100" cardPerPage="10" timeStamp="yyyyMMddhhmmssffff"  messageID="768866yyhhhhhh" ResCode="00"  ResDesc="success">

    //<StoreCard 
    //storeCardID="1" agentID="A0004" name="name" panmask="1234 5678 xxxx 0987" expiryDate="12/12" isDefault="Y"/>
    //</StoreCardListRes>
//    <StoreCardListReq>
//<Version>1.0</Version>
//<TimeStamp>yyyyMMddhhmmssffff</TimeStamp>
//<MessageID>768866yyhhhhhh</MessageID>
//<Email>userID</Email>
//<Password>xxxxxxxxxx</Password>
//<CardPerPage>10</CardPerPage>
//<PageNo>1</PageNo>
//</StoreCardListReq>


}
