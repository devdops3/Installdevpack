using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MA2AAPI.Models
{
    public class Reward
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LoginDevice { get; set; }
        public string IsChain { get; set; }
        public int ResetDay { get; set; }
        public bool AutoReset { get; set; }
    }

    public class RewardWinnerDetail 
    {
        public string RewardId { get; set; }
        public string PartnerCode { get; set; }
        public string UserId { get; set; }
        public DateTime RewardDateTime { get; set; }
        public double RewardAmount { get; set; }
        public int AgentId { get; set; }
    }

    public class RewardWinner
    {
        public string Id { get; set; }
        public string RewardId { get; set; }
        public DateTime RewardDateTime { get; set; }
    }

    [XmlRoot(ElementName = "RewardWinnerRes")]
    public class RewardWinnerResponse
    {
        [XmlElement(ElementName = "ResCode")]
        public string ResCode { get; set; }
        [XmlElement(ElementName = "ResDesc")]
        public string ResDesc { get; set; }
        [XmlElement(ElementName = "RewardAmount")]
        public string RewardAmount { get; set; }
        [XmlElement(ElementName = "Balance")]
        public string AvailableBalance { get; set; }
    }
}