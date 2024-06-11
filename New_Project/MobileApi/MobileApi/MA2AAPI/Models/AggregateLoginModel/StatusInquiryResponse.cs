using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
    [XmlRoot(ElementName = "StatusInquiryRes")]
	public class StatusInquiryResponse
	{
		[XmlElement(ElementName = "LoginID")]
		public string LoginID { get; set; }
		[XmlElement(ElementName = "UserName")]
		public string UserName { get; set; }
		[XmlElement(ElementName = "OTP")]
		public string OTP { get; set; }
		[XmlElement(ElementName = "OTPGeneratedTime")]
		public string OTPGeneratedTime { get; set; }
		[XmlElement(ElementName = "OTPExpiry")]
		public string OTPExpiry { get; set; }
		[XmlElement(ElementName = "OTPVerified")]
		public bool OTPVerified { get; set; }
		[XmlElement(ElementName = "OTPResendCount")]
		public string OTPResendCount { get; set; }
		[XmlElement(ElementName = "OTPResendLocked")]
		public bool OTPResendLocked { get; set; }
		[XmlElement(ElementName = "OTPResendLockedTimestamp")]
		public string OTPResendLockedTimestamp { get; set; }
		[XmlElement(ElementName = "OTPAttemptedCount")]
		public string OTPAttemptedCount { get; set; }
		[XmlElement(ElementName = "OTPAttemptedLocked")]
		public bool OTPAttemptedLocked { get; set; }
		[XmlElement(ElementName = "OTPAttemptedLockedTimestamp")]
		public string OTPAttemptedLockedTimestamp { get; set; }
		[XmlElement(ElementName = "IsPasswodSet")]
		public bool IsPasswodSet { get; set; }
		[XmlElement(ElementName = "IsRegistered")]
		public bool IsRegistered { get; set; }
		[XmlElement(ElementName = "TermsAndConditions")]
		public string TermsAndConditions { get; set; }
		
	}
}