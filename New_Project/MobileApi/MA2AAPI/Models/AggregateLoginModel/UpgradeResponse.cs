using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
    [XmlRoot(ElementName = "UpgradeRes")]
	public class UpgradeResponse
	{
		[XmlElement(ElementName = "AppUrl")]
		public string AppUrl { get; set; }
		[XmlElement(ElementName = "AppVersion")]
		public string AppVersion { get; set; }
		[XmlElement(ElementName = "LatestAppVersion")]
		public string LatestAppVersion { get; set; }
		[XmlElement(ElementName = "UserManual")]
		public string UserManual { get; set; }
	}
}