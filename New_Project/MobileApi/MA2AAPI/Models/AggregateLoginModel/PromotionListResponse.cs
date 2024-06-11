using System.Collections.Generic;
using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
    [XmlRoot(ElementName = "Promotion")]
	public class Promotion
	{
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "Type")]
		public string Type { get; set; }
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
		[XmlAttribute(AttributeName = "BillerLogoUrl")]
		public string BillerLogoUrl { get; set; }
		[XmlAttribute(AttributeName = "PromotionUrl")]
		public string PromotionUrl { get; set; }
		[XmlAttribute(AttributeName = "PromotionPhotoUrl")]
		public string PromotionPhotoUrl { get; set; }
	}

	
	[XmlRoot(ElementName = "PromotionListRes")]
	public class PromotionListResponse
	{
		[XmlElement(ElementName = "Promotions")]
		public Promotions Promotions { get; set; }
    }

    public class Promotions
    {
		[XmlElement(ElementName = "Promotion")]
		public List<Promotion> Promotion { get; set; }
	}

}