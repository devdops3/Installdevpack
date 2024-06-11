using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
	[XmlRoot(ElementName = "ePayment")]
	public class EPayment
	{
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "PaymentType")]
		public string PaymentType { get; set; }
		[XmlAttribute(AttributeName = "Description")]
		public string Description { get; set; }
		[XmlAttribute(AttributeName = "PaymentMode")]
		public string PaymentMode { get; set; }
		[XmlAttribute(AttributeName = "LogoUrl")]
		public string LogoUrl { get; set; }
	}

	[XmlRoot(ElementName = "FavouriteEpaymentListRes")]
	public class FavouriteEpaymentListResponse
	{
		[XmlElement(ElementName = "FavouriteEpaymentList")]
		public FavouriteEpaymentList EPaymentList { get; set; }
	}

	public class FavouriteEpaymentList
	{
		[XmlElement(ElementName = "ePayment")]
		public List<EPayment> ePayment { get; set; }
	}
}