using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models
{
    [XmlRoot(ElementName = "ePaymentSummaryReportRes")]
    public class EPaymentSummaryReportResV2
    {
        [XmlAttribute("ResCode")]
        public string ResponseCode { get; set; }
        [XmlAttribute("ResDesc")]
        public string ResponseDescription { get; set; }
        [XmlAttribute]
        public bool IsPrintable { get; set; }
        [XmlArray("TransactionStatuses")]
        public List<TransactionStatus> TransactionStatuses { get; set; }
    }

    [XmlRoot(ElementName = "TransactionStatus")]
    public class TransactionStatus
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Count")]
        public string Count { get; set; }
        [XmlAttribute("Amount")]
        public string Amount { get; set; }
        public List<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        [XmlIgnore]
        public string TransactionStatus { get; set; }
        [XmlAttribute]
        public string PaymentType { get; set; }
        [XmlAttribute]
        public string Count { get; set; }
        [XmlAttribute]
        public string Amount { get; set; }
    }
}