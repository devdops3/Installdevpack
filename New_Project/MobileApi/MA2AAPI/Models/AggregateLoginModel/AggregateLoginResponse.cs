using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
	[XmlRoot(ElementName = "AggregateLoginRes")]
	public class AggregateLoginResponse
	{
		[XmlElement(ElementName = "Version")]
		public string Version { get; set; }
		[XmlElement(ElementName = "TimeStamp")]
		public string TimeStamp { get; set; }
		[XmlElement(ElementName = "MessageID")]
		public string MessageID { get; set; }
		[XmlElement(ElementName = "ResCode")]
		public string ResCode { get; set; }
		[XmlElement(ElementName = "ResDesc")]
		public string ResDesc { get; set; }
		[XmlElement(ElementName = "LoginRes")]
		public HomeLoginResponse LoginResponse { get; set; }
		[XmlElement(ElementName = "UpgradeRes")]
		public UpgradeResponse UpgradeResponse { get; set; }
		[XmlElement(ElementName = "PushNotificationRes")]
		public PushNotificationResponse PushNotificationResponse { get; set; }
		[XmlElement(ElementName = "SystemConfigurationListRes")]
		public SystemConfigurationResponse SystemConfigurationResponse { get; set; }
		[XmlElement(ElementName = "FavouriteEpaymentListRes")]
		public FavouriteEpaymentListResponse FavouriteEpaymentListResponse { get; set; }
		[XmlElement(ElementName = "FavouriteEserviceListRes")]
		public FavouriteEserviceListResponse FavouriteEserviceListResponse { get; set; }
		[XmlElement(ElementName = "PromotionListRes")]
		public PromotionListResponse PromotionListResponse { get; set; }
		[XmlElement(ElementName = "StatusInquiryRes")]
		public StatusInquiryResponse StatusInquiryResponse { get; set; }

	}

	[XmlRoot(ElementName = "AggregateHomeRes")]
	public class AggregateHomeResponse
	{
		[XmlElement(ElementName = "Version")]
		public string Version { get; set; }
		[XmlElement(ElementName = "TimeStamp")]
		public string TimeStamp { get; set; }
		[XmlElement(ElementName = "MessageID")]
		public string MessageID { get; set; }
		[XmlElement(ElementName = "ResCode")]
		public string ResCode { get; set; }
		[XmlElement(ElementName = "ResDesc")]
		public string ResDesc { get; set; }
		[XmlElement(ElementName = "LoginRes")]
		public HomeLoginResponse LoginResponse { get; set; }
		[XmlElement(ElementName = "UpgradeRes")]
		public UpgradeResponse UpgradeResponse { get; set; }
		[XmlElement(ElementName = "PushNotificationRes")]
		public PushNotificationResponse PushNotificationResponse { get; set; }
		[XmlElement(ElementName = "SystemConfigurationListRes")]
		public SystemConfigurationResponse SystemConfigurationResponse { get; set; }
		[XmlElement(ElementName = "FavouriteEpaymentListRes")]
		public FavouriteEpaymentListResponse FavouriteEpaymentListResponse { get; set; }
		[XmlElement(ElementName = "FavouriteEserviceListRes")]
		public FavouriteEserviceListResponse FavouriteEserviceListResponse { get; set; }
		[XmlElement(ElementName = "PromotionListRes")]
		public PromotionListResponse PromotionListResponse { get; set; }	
	}
}