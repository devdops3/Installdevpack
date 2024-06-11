using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models
{
    [XmlRoot(ElementName = "Txn")]
    public class EServiceTransaction
    {

        [XmlAttribute(AttributeName = "COMPANYNAME")]
        public string CompanyName { get; set; }

        [XmlAttribute(AttributeName = "COUNT")]
        public string Count { get; set; }

        [XmlAttribute(AttributeName = "txnAmount")]
        public string TransactionAmount { get; set; }
    }

    [XmlRoot(ElementName = "TxnSearchReportRes")]
    public class EServiceSummaryReportResponseV2
    {

        [XmlElement(ElementName = "Txn")]
        public List<EServiceTransaction> Transactions { get; set; }

        [XmlAttribute(AttributeName = "ResCode")]
        public string ResponseCode { get; set; }

        [XmlAttribute(AttributeName = "ResDesc")]
        public string ResponseDescription { get; set; }

        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }

        [XmlAttribute(AttributeName = "totalamount")]
        public string Totalamount { get; set; }

        [XmlAttribute]
        public bool IsPrintable { get; set; }
    }

}
