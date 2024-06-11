using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{

	[XmlRoot(ElementName = "PushNotification")]
	public class PushNotification
	{
		[XmlElement(ElementName = "NotificationID")]
		public string NotificationID { get; set; }
		[XmlElement(ElementName = "Title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "Message")]
		public string Message { get; set; }
		[XmlElement(ElementName = "CreatedDate")]
		public string CreatedDate { get; set; }
		[XmlElement(ElementName = "IsRead")]
		public string IsRead { get; set; }
		[XmlElement(ElementName = "NotificationType")]
		public string NotificationType { get; set; }
	}


	[XmlRoot(ElementName = "PushNotificationRes")]
	public class PushNotificationResponse
	{
		[XmlElement(ElementName = "PushNotifications")]
		public PushNotifications PushNotifications { get; set; }
	}

	public class PushNotifications
	{
		[XmlElement(ElementName = "PushNotification")]
		public List<PushNotification> PushNotification { get; set; }
	}
}
