using System.Xml.Serialization;

namespace MA2AAPI.Models.AggregateLoginModel
{
    [XmlRoot(ElementName = "LoginRes")]
	public class HomeLoginResponse
	{
		[XmlElement(ElementName = "UserName")]
		public string UserName { get; set; }
		[XmlElement(ElementName = "AgentID")]
		public string AgentID { get; set; }
		[XmlElement(ElementName = "AgentName")]
		public string AgentName { get; set; }
		[XmlElement(ElementName = "AgentLogo")]
		public string AgentLogo { get; set; }
		[XmlElement(ElementName = "AgentCode")]
		public string AgentCode { get; set; }
		[XmlElement(ElementName = "AggrementType")]
		public string AggrementType { get; set; }
		[XmlElement(ElementName = "Balance")]
		public string Balance { get; set; }
		[XmlElement(ElementName = "UserLevel")]
		public string UserLevel { get; set; }
		[XmlElement(ElementName = "BranchCode")]
		public string BranchCode { get; set; }
		[XmlElement(ElementName = "BranchName")]
		public string BranchName { get; set; }
		[XmlElement(ElementName = "BranchAddress")]
		public string BranchAddress { get; set; }
		[XmlElement(ElementName = "SupportPhone")]
		public string SupportPhone { get; set; }
		[XmlElement(ElementName = "TodayTxnCount")]
		public string TodayTxnCount { get; set; }
		[XmlElement(ElementName = "TodayTxnAmount")]
		public string TodayTxnAmount { get; set; }
		[XmlElement(ElementName = "CreditLimitAmount")]
		public string CreditLimitAmount { get; set; }
		[XmlElement(ElementName = "CreditLimitDay")]
		public string CreditLimitDay { get; set; }
		[XmlElement(ElementName = "CreditLimitStartDay")]
		public string CreditLimitStartDay { get; set; }
		[XmlElement(ElementName = "ShowBalance")]
		public string ShowBalance { get; set; }
		[XmlElement(ElementName = "IsUpdate")]
		public string IsUpdate { get; set; }
		[XmlElement(ElementName = "MyQR")]
		public string MyQR { get; set; }
		[XmlElement(ElementName = "Token")]
		public string Token { get; set; }
		[XmlElement(ElementName = "AgentUserID")]
		public string AgentUserID { get; set; }
		[XmlElement(ElementName = "Reprint")]
		public string Reprint { get; set; }
		[XmlElement(ElementName = "PrintSaleReport")]
		public string PrintSaleReport { get; set; }
		[XmlElement(ElementName = "PrintEPaymentSummaryReport")]
		public string PrintEPaymentSummaryReport { get; set; }
		[XmlElement(ElementName = "TermsAndConditions")]
		public string TermsAndConditions { get; set; }
		[XmlElement(ElementName = "IsMerchant")]
		public string IsMerchant { get; set; }
		[XmlElement(ElementName = "IsEpaymentAvailable")]
		public string IsEpaymentAvailable { get; set; }
		[XmlElement(ElementName = "IsDemoModeOn")]
		public string IsDemoModeOn { get; set; }
		[XmlElement(ElementName = "VerifyStatus")]
		public string VerifyStatus { get; set; }
		[XmlElement(ElementName = "VerifyDescription")]
		public string VerifyDescription { get; set; }
		[XmlElement(ElementName = "VerifyDialogueShow")]
		public string VerifyDialogueShow { get; set; }
		[XmlElement(ElementName = "VerifyDialogueTitle")]
		public string VerifyDialogueTitle { get; set; }
		[XmlElement(ElementName = "UserProfileImage")]
		public string UserProfileImage { get; set; }
		[XmlElement(ElementName = "StarRate")]
		public string StarRate { get; set; }
	}

	[XmlRoot(ElementName = "LoginRes")]
    public class LoginResponse : HomeLoginResponse
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
        [XmlElement(ElementName = "RewardId")]
        public string RewardId { get; set; }
	}
}