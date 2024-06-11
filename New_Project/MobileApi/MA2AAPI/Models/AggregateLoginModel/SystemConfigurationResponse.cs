using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
	[XmlRoot(ElementName = "SystemConfiguration")]
	public class SystemConfiguration
	{
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "Key")]
		public string Key { get; set; }
		[XmlAttribute(AttributeName = "Value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "SystemConfigurationListRes")]
	public class SystemConfigurationResponse
	{
		[XmlElement(ElementName = "SystemConfigurationList")]
		public SystemConfigurationList SystemConfigurationList { get; set; }
	}


	public class SystemConfigurationList
	{
		[XmlElement(ElementName = "SystemConfiguration")]
		public List<SystemConfiguration> SystemConfigurations { get; set; }
	}

}