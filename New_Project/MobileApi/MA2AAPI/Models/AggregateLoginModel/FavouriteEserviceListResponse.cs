using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
	[XmlRoot(ElementName = "Biller")]
	public class Biller
	{
		[XmlAttribute(AttributeName = "CategoryId")]
		public string CategoryId { get; set; }
		[XmlAttribute(AttributeName = "CategoryName")]
		public string CategoryName { get; set; }
		[XmlAttribute(AttributeName = "BillerId")]
		public string BillerId { get; set; }
		[XmlAttribute(AttributeName = "TaxId")]
		public string TaxId { get; set; }
		[XmlAttribute(AttributeName = "BillerName")]
		public string BillerName { get; set; }
		[XmlAttribute(AttributeName = "LogoUrl")]
		public string LogoUrl { get; set; }
		[XmlAttribute(AttributeName = "Label")]
		public string Label { get; set; }
	}

	[XmlRoot(ElementName = "FavouriteEserviceListRes")]
	public class FavouriteEserviceListResponse
	{
		[XmlElement(ElementName = "FavouriteEserviceList")]
		public FavouriteEserviceList FavouriteEserviceList { get; set; }
	}

	public class FavouriteEserviceList
	{
		[XmlElement(ElementName = "Biller")]
		public List<Biller> Biller { get; set; }
	}

}