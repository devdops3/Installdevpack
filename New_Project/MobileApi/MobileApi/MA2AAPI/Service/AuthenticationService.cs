using A2AAPI.Models;
using log4net;
using MA2AAPI.Class;
using MA2AAPI.Constants;
using MA2AAPI.Models;
using MA2AAPI.Models.AggregateLoginModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MA2AAPI.Service
{
    public class AuthenticationService
    {
        private readonly MA2AAPIWCF.ServiceClient _agentWCF;
        private readonly ResponseService _responseService;
        public AuthenticationService()
        {
            _agentWCF = new MA2AAPIWCF.ServiceClient();
            _responseService = new ResponseService();
        }
        #region Log
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void writeLog(string msg)
        {
            Logger.writeLog(msg, ref log);
        }
        #endregion

        public string IsMobileAppNeedTobeUpdated(string appVersion)
        {
            string MobileAppVersion = appVersion.Replace(".", "");
            var newappversion = ConfigurationManager.AppSettings["newversion"].ToString();
            string BackendAppVersion = newappversion.Replace(".", "");
            if (int.Parse(MobileAppVersion) >= int.Parse(BackendAppVersion)) return "N";
            return "Y";
        }

        public bool IsWhitelistedUser(string email, string password)
        {
            string whitelistusername = ConfigurationManager.AppSettings["WhiteListUserName"].ToString();
            string whitelistpassword = ConfigurationManager.AppSettings["WhiteListPassword"].ToString();
            return whitelistusername == email && whitelistpassword == password;
        }

        public string IsDemoModeOn(DataRow userRow)
        {
            string profileId = userRow["PROFILEID"] == null ? "-" : userRow["PROFILEID"].ToString();
            var profileListString = ConfigurationManager.AppSettings["ProfileList"].ToString();
            if (string.IsNullOrEmpty(profileListString) || string.IsNullOrEmpty(profileId)) return "N";
            var profileList = profileListString.Split(',').ToList();
            var profile = profileList.FirstOrDefault(x => x == profileId);
            if (string.IsNullOrEmpty(profile)) return "N";
            return "Y";
        }

        public string InsertToken(string email, out DataSet dsForToken, out string errmsg)
        {
            if (!(_agentWCF.insertToken(email, out dsForToken, out errmsg))) return string.Empty;
            var tokenTable = dsForToken.Tables[0];
            var tokenRow = tokenTable.Rows[0];
            return tokenRow["Token"] == null ? "-" : tokenRow["Token"].ToString();
        }

        public string GetReward(DataTable rewardTable, DataTable rewardWinnerTable, DataRow userRow)
        {
            writeLog("In GetReward method : ");
            writeLog("Checking Reward have or not");
            if (rewardTable.Rows.Count <= 0) return string.Empty;

            var reward = new Reward();
            reward = PopulateRewardResponse(rewardTable);
            var isChain = userRow["IsChain"].ToString();
            var userRegisterType = (string.IsNullOrEmpty(isChain)) ? "N" : isChain;
            writeLog("Checking RegisterType valid or not for Reward");
            if (!reward.IsChain.Equals("A") && !reward.IsChain.Equals(userRegisterType)) return string.Empty;

            var userLoginDevice = userRow["LOGINDEVICE"].ToString();
            writeLog("Checking LoginDevice valid or not for Reward");
            if (!reward.LoginDevice.Equals("A") && !reward.LoginDevice.Equals(userLoginDevice)) return string.Empty;

            writeLog("Start checking Reward is Valid or not");
            if (IsValidReward(reward, rewardWinnerTable))
            {
                writeLog("RewardResponse for GetReward method : RewardId : " + reward.Id);
                return reward.Id.ToString();
            }
            else return string.Empty;

        }

        public RewardWinner PopulateRewardWinnerResponse(DataTable rewardWinnerTable)
        {
            writeLog("Start Populating RewardWinner Response");
            var rewardWinner = new RewardWinner();
            var rewardWinnerRow = rewardWinnerTable.Rows[0];
            rewardWinner.Id = rewardWinnerRow["Id"].ToString();
            rewardWinner.RewardId = rewardWinnerRow["RewardId"].ToString();
            rewardWinner.RewardDateTime = Convert.ToDateTime(rewardWinnerRow["RewardDateTime"].ToString());
            writeLog("RewardWinner Response is : " + JsonConvert.SerializeObject(rewardWinner));
            writeLog("End Populating RewardWinner Response");
            return rewardWinner;
        }

        public Reward PopulateRewardResponse(DataTable rewardTable)
        {
            writeLog("Start Populating Reward Response");
            var rewardRow = rewardTable.Rows[0];
            var reward = new Reward() 
            {
                Id = rewardRow["Id"].ToString(),
                Title = rewardRow["Title"].ToString(),
                StartDate = Convert.ToDateTime(rewardRow["StartDate"].ToString()),
                EndDate = Convert.ToDateTime(rewardRow["EndDate"].ToString()),
                LoginDevice = rewardRow["LoginDevice"].ToString(),
                IsChain = rewardRow["IsChain"].ToString(),
                ResetDay = Convert.ToInt32(rewardRow["ResetDay"].ToString()),
                AutoReset = Convert.ToBoolean(rewardRow["AutoReset"])
            };
            writeLog("Reward Response is : " + JsonConvert.SerializeObject(reward));
            writeLog("End Populating Reward Response");
            return reward;
        }

        public bool IsValidReward(Reward reward, DataTable rewardWinnerTable)
        {
            writeLog("Checking Date valid or not for Reward");
            if (!CheckAvailableRewardWithDates(reward.StartDate, reward.EndDate)) return false;

            //if user did not recieve Reward yet then this user will get Reward.
            writeLog("Checking User already got reward or not for Reward");
            if (rewardWinnerTable.Rows.Count <= 0) return true;

            var rewardWinner = new RewardWinner();
            rewardWinner = PopulateRewardWinnerResponse(rewardWinnerTable);

            writeLog("Checking AutoReset Date for Reward");
            if ((rewardWinner.RewardDateTime.Date == DateTime.Now.Date) || !reward.AutoReset) return false;

            return ResetRewardDate(reward, rewardWinner);
        }

        public bool ResetRewardDate(Reward reward, RewardWinner rewardWinner)
        {
            writeLog("In ResetRewardDate Method");
            var resetDate = rewardWinner.RewardDateTime.AddDays(reward.ResetDay);
            var currentDate = DateTime.Now.Date;
            if(resetDate.Date > reward.EndDate.Date) return false;
            if (resetDate.Date >= currentDate.Date)
            {
                var diffDate = resetDate.Date - currentDate.Date;
                if (diffDate.Days != 0) return false;
            }
            writeLog("Reseting RewardDate is successful.");
            writeLog("End checking Reward is Valid or not");
            return true;
        }

        public bool CheckAvailableRewardWithDates(DateTime startDate, DateTime endDate)
        {
            writeLog("In CheckAvailableRewardWithDates method");
            var rewardDateFlag = false;
            for (var availableRewardDay = startDate; availableRewardDay.Date <= endDate.Date; availableRewardDay = availableRewardDay.AddDays(1))
            {
                if (availableRewardDay.Date == DateTime.Now.Date)
                {
                    writeLog("Current date is valid for reward.");
                    rewardDateFlag = true;
                    break;
                }
            }
            return rewardDateFlag;
        }

        public string UpdatePairedDevice(string loginType, string email, string password, string LogIn_DeviceID, string devicetoken, string appVersion, string deviceInfo, string terminalId,
            out string errmsg, DataRow userRow, string messageID, string TodayTxnCount, string TodayTxnAmount, string token, string mobileNeedsUpdate, string isDemoModeOn, DataSet loginDataSet)
        {
            var appVersionWithoutDot = appVersion.Replace(".", string.Empty);
            if (loginType == ConfigurationManager.AppSettings["LoginType_POS"].ToString())
            {
                if (_agentWCF.UpdateDeviceWithTerminalId(email, password, LogIn_DeviceID, devicetoken, appVersion, deviceInfo, terminalId, out errmsg))
                    return GetLoginResponse(email, userRow, messageID, TodayTxnCount, TodayTxnAmount, token, mobileNeedsUpdate, isDemoModeOn, loginDataSet, loginType, appVersionWithoutDot, LogIn_DeviceID);
            }

            else
            {
                if (_agentWCF.UpdateDevice(email, password, LogIn_DeviceID, devicetoken, appVersion, deviceInfo, out errmsg))
                    return GetLoginResponse(email, userRow, messageID, TodayTxnCount, TodayTxnAmount, token, mobileNeedsUpdate, isDemoModeOn, loginDataSet, loginType, appVersionWithoutDot, LogIn_DeviceID);
            }

            return string.Empty;
        }

        public string UpdateUnPairedDevice(string loginType, string email, string password, string LogIn_DeviceID, string devicetoken, string appVersion, string deviceInfo, string terminalId,
           out string errmsg, DataRow userRow, string messageID, string TodayTxnCount, string TodayTxnAmount, string token, string isupdate, string isDemoModeOn, DataSet loginDataSet)
        {
            var appVersionWithoutDot = appVersion.Replace(".", string.Empty);
            if (loginType == ConfigurationManager.AppSettings["LoginType_POS"].ToString())
            {
                if (_agentWCF.UpdateDeviceWithTerminalId(email, password, LogIn_DeviceID, devicetoken, appVersion, deviceInfo, terminalId, out errmsg))
                    return GetLoginResponse(email, userRow, messageID, TodayTxnCount, TodayTxnAmount, token, isupdate, isDemoModeOn, loginDataSet, loginType, appVersionWithoutDot, LogIn_DeviceID);

            }
            else
            {
                if (_agentWCF.UpdateDevice(email, password, LogIn_DeviceID, devicetoken, appVersion, deviceInfo, out errmsg))
                    return GetLoginResponse(email, userRow, messageID, TodayTxnCount, TodayTxnAmount, token, isupdate, isDemoModeOn, loginDataSet, loginType, appVersionWithoutDot, LogIn_DeviceID);
            }

            return _responseService.GetLoginUpdateFailResponse();

        }
        public string GetLoginResponse(string email, DataRow userRow, string messageID, string TodayTxnCount, string TodayTxnAmount,
            string token, string mobileNeedsUpdate, string isDemoModeOn, DataSet loginDataSet, string loginType, string appVersion
            , string deviceId)
        {
            var loginResponse = GetLoginResponse(userRow, messageID, TodayTxnCount, TodayTxnAmount, mobileNeedsUpdate, token, isDemoModeOn);
            loginResponse.RewardId = GetReward(loginDataSet.Tables[3], loginDataSet.Tables[4], userRow);

            return (new XMLSerializationService<LoginResponse>()).SerializeData(loginResponse);
        }

        public string GetHomeResponse(DataRow userRow, string messageID, string TodayTxnCount, string TodayTxnAmount,
            string token, string mobileNeedsUpdate, string isDemoModeOn, DataSet loginDataSet, string loginType,
            string appVersion)
        {
            var homeResponse = GetHomeResponse(userRow, messageID, TodayTxnCount, TodayTxnAmount, mobileNeedsUpdate, token, isDemoModeOn);

            var mobileVersionTable = loginDataSet.Tables[3];
            homeResponse.UpgradeResponse = GetUpgradeResponse(mobileVersionTable, loginType);

            var notiTable = loginDataSet.Tables[4];
            homeResponse.PushNotificationResponse = GetNotificationResponse(notiTable);

            var configTable = loginDataSet.Tables[5];
            homeResponse.SystemConfigurationResponse = GetSystemConfigurationResponse(configTable);

            var favouriteEPaymentTable = loginDataSet.Tables[6];
            homeResponse.FavouriteEpaymentListResponse = GetFavouriteEPaymentResponse(favouriteEPaymentTable, appVersion, loginType);

            var favouriteEServiceTable = loginDataSet.Tables[7];
            homeResponse.FavouriteEserviceListResponse = GetFavouriteEServiceListResponse(favouriteEServiceTable, appVersion, loginType);

            var promotionTable = loginDataSet.Tables[8];
            homeResponse.PromotionListResponse = GetPromotionListResponse(promotionTable);

            return (new XMLSerializationService<AggregateHomeResponse>()).SerializeData(homeResponse);
        }



        public LoginResponse GetLoginResponse(DataRow userRow, string messageID,
            string TodayTxnCount, string TodayTxnAmount, string mobileNeedsUpdate, string token, string isDemoModeOn)
        {
            var userRight = userRow["USERRIGHTSXML"].ToString();
            var userRightHt = Utils.GetHTableFromRightsXML(userRight);

            var rePrint = "Y";
            if (userRightHt.ContainsKey("reprint")) rePrint = userRightHt["reprint"].ToString();

            var printSaleReport = "Y";
            if (userRightHt.ContainsKey("printSalereport")) printSaleReport = userRightHt["printSalereport"].ToString();

            var printEPaymentSummaryReport = "Y";
            if (userRightHt.ContainsKey("printEPaymentSummaryReport")) printEPaymentSummaryReport = userRightHt["printEPaymentSummaryReport"].ToString();

            string profileImageUrl = null;
            if (!string.IsNullOrEmpty(userRow["SelfiePhotoUrl"].ToString())) profileImageUrl = GetImageUrl(userRow["SelfiePhotoUrl"].ToString());


            var agentCode = userRow["AGENTCODE"].ToString();
            string botStdBarCodeStr = Utils.getCBMStandard("0000000000005", "", agentCode, "000000000000", "10000");
            writeLog("BarCode String " + botStdBarCodeStr + " AgentCOde " + agentCode);
            string barcodeurl = Utils.generateQRCode(botStdBarCodeStr, agentCode);
            writeLog("After Scanning BarCode");
            string code = "00";
            string desp = "Success";

            var loginResponse = new LoginResponse
            {
                Version = "1.0",
                TimeStamp = DateTime.Now.ToString("yyyyMMddhhmmssffff"),
                MessageID = messageID,
                ResCode = code,
                ResDesc = desp,
                UserName = userRow["AGENTUSERNAME"].ToString(),
                AgentID = userRow["AGENTID"].ToString(),
                AgentName = userRow["AGENTNAME"].ToString(),
                AgentCode = agentCode,
                AgentLogo = string.Empty,
                AggrementType = userRow["AGREEMENTTYPE"].ToString(),
                Balance = userRow["AVAILABLEBALANCE"].ToString(),
                UserLevel = userRow["USERLEVEL"].ToString(),
                BranchCode = userRow["BRANCHCODE"] == null ? "-" : userRow["BRANCHCODE"].ToString(),
                BranchName = userRow["BRANCHNAME"] == null ? "-" : userRow["BRANCHNAME"].ToString(),
                BranchAddress = userRow["BRANCHADDRESS"] == null ? "-" : userRow["BRANCHADDRESS"].ToString(),
                SupportPhone = userRow["SUPPORTPHONE"] == null ? "-" : userRow["SUPPORTPHONE"].ToString(),
                TodayTxnCount = TodayTxnCount,
                TodayTxnAmount = TodayTxnAmount,
                CreditLimitAmount = (userRow["CREDITLIMIT"] == null ? "-" : userRow["CREDITLIMIT"].ToString()),
                CreditLimitDay = (userRow["CREDITTERM"] == null ? "-" : userRow["CREDITTERM"].ToString()),
                CreditLimitStartDay = (userRow["CREDITTERMSTART"] == null ? "-" : userRow["CREDITTERMSTART"].ToString()),
                ShowBalance = (userRow["SHOWBALANCE"] == DBNull.Value) ? "N" : userRow["SHOWBALANCE"].ToString(),
                IsUpdate = mobileNeedsUpdate,
                MyQR = barcodeurl,
                Token = token,
                AgentUserID = userRow["AGENTUSERID"].ToString(),
                Reprint = rePrint,
                PrintSaleReport = printSaleReport,
                PrintEPaymentSummaryReport = printEPaymentSummaryReport,
                TermsAndConditions = userRow["TERMSANDCONDITIONS"].ToString(),
                IsMerchant = userRow["IsMerchant"].ToString(),
                IsEpaymentAvailable = userRow["IsEPaymentAvailable"].ToString(),
                IsDemoModeOn = isDemoModeOn,
                VerifyStatus = userRow["VerifyStatus"].ToString(),
                VerifyDescription = ConfigurationManager.AppSettings["VerifyDescription"].ToString(),
                VerifyDialogueShow = ConfigurationManager.AppSettings["VerifyDialogueShow"].ToString(),
                VerifyDialogueTitle = ConfigurationManager.AppSettings["VerifyDialogueTitle"].ToString(),
                UserProfileImage = profileImageUrl,
                StarRate = userRow["StarRate"].ToString()

            };

            return loginResponse;
        }
        public AggregateHomeResponse GetHomeResponse(DataRow userRow, string messageID,
            string TodayTxnCount, string TodayTxnAmount, string mobileNeedsUpdate, string token, string isDemoModeOn)
        {
            var userRight = userRow["USERRIGHTSXML"].ToString();
            var userRightHt = Utils.GetHTableFromRightsXML(userRight);

            var rePrint = "Y";
            if (userRightHt.ContainsKey("reprint")) rePrint = userRightHt["reprint"].ToString();

            var printSaleReport = "Y";
            if (userRightHt.ContainsKey("printSalereport")) printSaleReport = userRightHt["printSalereport"].ToString();

            var printEPaymentSummaryReport = "Y";
            if (userRightHt.ContainsKey("printEPaymentSummaryReport")) printEPaymentSummaryReport = userRightHt["printEPaymentSummaryReport"].ToString();

            string profileImageUrl = null;
            if (!string.IsNullOrEmpty(userRow["SelfiePhotoUrl"].ToString())) profileImageUrl = GetImageUrl(userRow["SelfiePhotoUrl"].ToString());

            var agentCode = userRow["AGENTCODE"].ToString();
            string botStdBarCodeStr = Utils.getCBMStandard("0000000000005", "", agentCode, "000000000000", "10000");
            writeLog("BarCode String " + botStdBarCodeStr + " AgentCOde " + agentCode);
            string barcodeurl = Utils.generateQRCode(botStdBarCodeStr, agentCode);
            writeLog("After Scanning BarCode");
            string code = "00";
            string desp = "Success";

            var homeResponse = new AggregateHomeResponse
            {
                Version = "1.0",
                TimeStamp = DateTime.Now.ToString("yyyyMMddhhmmssffff"),
                MessageID = messageID,
                ResCode = code,
                ResDesc = desp,
                LoginResponse = new HomeLoginResponse
                {
                    UserName = userRow["AGENTUSERNAME"].ToString(),
                    AgentID = userRow["AGENTID"].ToString(),
                    AgentName = userRow["AGENTNAME"].ToString(),
                    AgentCode = agentCode,
                    AgentLogo = string.Empty,
                    AggrementType = userRow["AGREEMENTTYPE"].ToString(),
                    Balance = userRow["AVAILABLEBALANCE"].ToString(),
                    UserLevel = userRow["USERLEVEL"].ToString(),
                    BranchCode = userRow["BRANCHCODE"] == null ? "-" : userRow["BRANCHCODE"].ToString(),
                    BranchName = userRow["BRANCHNAME"] == null ? "-" : userRow["BRANCHNAME"].ToString(),
                    BranchAddress = userRow["BRANCHADDRESS"] == null ? "-" : userRow["BRANCHADDRESS"].ToString(),
                    SupportPhone = userRow["SUPPORTPHONE"] == null ? "-" : userRow["SUPPORTPHONE"].ToString(),
                    TodayTxnCount = TodayTxnCount,
                    TodayTxnAmount = TodayTxnAmount,
                    CreditLimitAmount = userRow["CREDITLIMIT"] == null ? "-" : userRow["CREDITLIMIT"].ToString(),
                    CreditLimitDay = userRow["CREDITTERM"] == null ? "-" : userRow["CREDITTERM"].ToString(),
                    CreditLimitStartDay = userRow["CREDITTERMSTART"] == null ? "-" : userRow["CREDITTERMSTART"].ToString(),
                    ShowBalance = (userRow["SHOWBALANCE"] == DBNull.Value) ? "N" : userRow["SHOWBALANCE"].ToString(),
                    IsUpdate = mobileNeedsUpdate,
                    MyQR = barcodeurl,
                    Token = token,
                    AgentUserID = userRow["AGENTUSERID"].ToString(),
                    Reprint = rePrint,
                    PrintSaleReport = printSaleReport,
                    PrintEPaymentSummaryReport = printEPaymentSummaryReport,
                    TermsAndConditions = userRow["TERMSANDCONDITIONS"].ToString(),
                    IsMerchant = userRow["IsMerchant"].ToString(),
                    IsEpaymentAvailable = userRow["IsEPaymentAvailable"].ToString(),
                    IsDemoModeOn = isDemoModeOn,
                    VerifyStatus = userRow["VerifyStatus"].ToString(),
                    VerifyDescription = ConfigurationManager.AppSettings["VerifyDescription"].ToString(),
                    VerifyDialogueShow = ConfigurationManager.AppSettings["VerifyDialogueShow"].ToString(),
                    VerifyDialogueTitle = ConfigurationManager.AppSettings["VerifyDialogueTitle"].ToString(),
                    UserProfileImage = profileImageUrl,
                    StarRate = userRow["StarRate"].ToString()
                }
            };

            return homeResponse;
        }

        private UpgradeResponse GetUpgradeResponse(DataTable mobileVersionTable, string loginType)
        {
            try
            {
                var mobileData = mobileVersionTable.Rows[0];
                var upgradeResponse = new UpgradeResponse
                {
                    AppUrl = ConfigurationManager.AppSettings["AppUrl"].ToString(),
                    AppVersion = ConfigurationManager.AppSettings["AppVersion"].ToString(),
                    LatestAppVersion = mobileData["Value"].ToString(),
                    UserManual = IsPOS(loginType) ? ConfigurationManager.AppSettings["PosUserManualUrl"].ToString() : ConfigurationManager.AppSettings["ManualUrl"].ToString()
                };
                return upgradeResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetUpgradeResponse : " + ex.Message);
                return new UpgradeResponse();
            }

        }

        private bool IsPOS(string loginType)
        {
            return !string.IsNullOrEmpty(loginType) && loginType == ConfigurationManager.AppSettings["LoginType_POS"].ToString();
        }

        private bool IsMobile(string loginType)
        {
            return loginType == LoginType.MS.ToString();
        }

        bool IsPOSMMBusTicket(string loginType, string taxId)
        {
            return (!string.IsNullOrEmpty(loginType) && loginType == "POS" && IsMMBusTicket(taxId));
        }

        bool IsMMBusTicket(string taxId)
        {
            return taxId == ConfigurationManager.AppSettings["MmBusticket"].ToString();
        }

        private PushNotificationResponse GetNotificationResponse(DataTable notificationTable)
        {
            try
            {
                var notificationResponse = new PushNotificationResponse
                {
                    PushNotifications = new PushNotifications { PushNotification = new List<PushNotification>() }
                };

                foreach (DataRow notification in notificationTable.Rows)
                {
                    var noti = new PushNotification();
                    // This is for '&' serialization case
                    if (notification["TITLE"] != null)
                        noti.Title = notification["TITLE"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    if (notification["MESSAGE"] != null)
                        noti.Message = notification["MESSAGE"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    noti.NotificationID = notification["AgentUserNotiListId"].ToString();
                    noti.CreatedDate = notification["CREATEDDATE"].ToString();
                    var isRead = Convert.ToBoolean(notification["IsRead"].ToString());
                    noti.IsRead = isRead ? "true" : "false";
                    noti.NotificationType = notification["NotificationType"].ToString();
                    notificationResponse.PushNotifications.PushNotification.Add(noti);
                }
                return notificationResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetNotificationResponse : " + ex.Message);
                return new PushNotificationResponse();
            }
        }

        private SystemConfigurationResponse GetSystemConfigurationResponse(DataTable configurationTable)
        {
            try
            {
                var systemConfigurationResponse = new SystemConfigurationResponse
                {
                    SystemConfigurationList = new SystemConfigurationList { SystemConfigurations = new List<SystemConfiguration>() }
                };
                foreach (DataRow systemConfiguration in configurationTable.Rows)
                {
                    var configuration = new SystemConfiguration
                    {
                        Id = systemConfiguration["Id"].ToString(),
                        Key = systemConfiguration["Key"].ToString(),
                        Value = systemConfiguration["Value"].ToString()
                    };
                    systemConfigurationResponse.SystemConfigurationList.SystemConfigurations.Add(configuration);
                }
                return systemConfigurationResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetSystemConfigurationResponse : " + ex.Message);
                return new SystemConfigurationResponse();
            }
        }

        private FavouriteEpaymentListResponse GetFavouriteEPaymentResponse(DataTable ePaymentTable, string appVersion, string loginType)
        {
            try
            {
                var favouriteEPaymentResponse = new FavouriteEpaymentListResponse
                {
                    EPaymentList = new FavouriteEpaymentList { ePayment = new List<EPayment>() }
                };
                var invalidEPaymentForMobileList = ConfigurationManager.AppSettings["InvalidEPaymentForMobile"].Split(',');
                foreach (DataRow favouriteEpayment in ePaymentTable.Rows)
                {
                    var paymentType = favouriteEpayment["PaymentType"].ToString();
                    writeLog("count : " + paymentType);
                    if (!IsValidVersionMPUandJCB(int.Parse(appVersion)) && invalidEPaymentForMobileList.Contains(paymentType)) continue;
                    if (IsMobile(loginType) && invalidEPaymentForMobileList.Contains(paymentType)) continue;
                    var ePayment = new EPayment
                    {
                        Id = favouriteEpayment["Id"].ToString(),
                        Description = favouriteEpayment["Description"].ToString(),
                        PaymentMode = favouriteEpayment["PaymentMode"].ToString(),
                        PaymentType = paymentType
                    };
                    if (paymentType == "A+ Wallet")
                    {
                        ePayment.LogoUrl = ConfigurationManager.AppSettings["EpaymentLogoUrl"].ToString() + "APlus.png";
                    }
                    else
                    {
                        ePayment.LogoUrl = ConfigurationManager.AppSettings["EpaymentLogoUrl"].ToString() + paymentType + ".png";
                    }
                    favouriteEPaymentResponse.EPaymentList.ePayment.Add(ePayment);
                }
                return favouriteEPaymentResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetFavouriteEPaymentResponse : " + ex.Message);
                return new FavouriteEpaymentListResponse();
            }
        }

        private FavouriteEserviceListResponse GetFavouriteEServiceListResponse(DataTable favouriteEServiceTable, string appVersion, string loginType)
        {
            try
            {
                var favouriteEServiceResponse = new FavouriteEserviceListResponse
                {
                    FavouriteEserviceList = new FavouriteEserviceList { Biller = new List<Biller>() }
                };

                foreach (DataRow favEservice in favouriteEServiceTable.Rows)
                {
                    var taxId = favEservice["TaxId"].ToString();
                    var billerId = favEservice["BillerId"].ToString();
                    if (IsPOSMMBusTicket(loginType, taxId)) continue;
                    if (IsMMBusTicket(taxId) && !IsValidVersionForMMBusTicket(int.Parse(appVersion))) continue;

                    var eService = new Biller
                    {
                        TaxId = taxId,
                        CategoryId = favEservice["CategoryId"].ToString(),
                        CategoryName = favEservice["CategoryName"].ToString(),
                        BillerId = billerId,
                        BillerName = favEservice["BillerName"].ToString(),
                        LogoUrl = ConfigurationManager.AppSettings["BillerLogoUrl"].ToString() + billerId + ".png",
                        Label = favEservice["Label"] != DBNull.Value ? favEservice["Label"].ToString() : string.Empty
                    };
                    favouriteEServiceResponse.FavouriteEserviceList.Biller.Add(eService);
                }
                return favouriteEServiceResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetFavouriteEServiceListResponse : " + ex.Message);
                return new FavouriteEserviceListResponse();
            }
        }
        bool IsValidVersionForMMBusTicket(int appVersion)
        {
            var validVersionForMMBusTicket = ConfigurationManager.AppSettings["ValidVersionForMMBusTicket"].ToString();
            var intValidVersion = int.Parse(validVersionForMMBusTicket);
            return appVersion >= intValidVersion;
        }
        private PromotionListResponse GetPromotionListResponse(DataTable promotionTable)
        {
            try
            {
                var promotionListResponse = new PromotionListResponse
                {
                    Promotions = new Promotions { Promotion = new List<Promotion>() }
                };
                foreach (DataRow promotionRow in promotionTable.Rows)
                {
                    var billerId = promotionRow["BillerId"].ToString();
                    var id = promotionRow["Id"].ToString();
                    var promotion = new Promotion
                    {
                        Id = promotionRow["Id"].ToString(),
                        Type = promotionRow["Type"].ToString(),
                        TaxId = promotionRow["TaxId"].ToString(),
                        CategoryId = promotionRow["CategoryId"].ToString(),
                        CategoryName = promotionRow["CategoryName"].ToString(),
                        BillerId = billerId,
                        BillerName = promotionRow["BillerName"].ToString(),
                        BillerLogoUrl = ConfigurationManager.AppSettings["BillerLogoUrl"].ToString() + billerId + ".png",
                        PromotionUrl = promotionRow["Url"].ToString(),
                        PromotionPhotoUrl = ConfigurationManager.AppSettings["PromotionPhotoUrl"].ToString() + id + ".png"
                    };
                    promotionListResponse.Promotions.Promotion.Add(promotion);
                }
                return promotionListResponse;
            }
            catch (Exception ex)
            {
                writeLog("Errror in GetPromotionListResponse : " + ex.Message);
                return new PromotionListResponse();
            }
        }
        bool IsValidVersionMPUandJCB(int appVersion)
        {
            var validVersionForMPUandJCB = ConfigurationManager.AppSettings["MinimumVersionForMPUandJCB"].ToString();
            var intValidVersion = int.Parse(validVersionForMPUandJCB);
            return appVersion >= intValidVersion;
        }

        public string formatDate(string dt)
        {
            DateTime dtTmp = Convert.ToDateTime(dt);
            return dtTmp.ToString("dd/M/yyyy HH:mm:ss");
        }
        public bool IsContinueFromAnotherDevice(UserStatusResp userStatusResponse, string deviceUID)
        {
            return userStatusResponse.DeviceUID != deviceUID;
        }

        public bool IsContinueFromAnotherDevice(string oldDeviceID, string newDeviceID)
        {
            return oldDeviceID != newDeviceID;
        }

        public bool IsOTPVerifiedExpired(DateTime OTPVerifiedTime)
        {
            writeLog(" IsOTPVerifiedExpired starts");
            var expiredMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["OTPVerifiedExpiredMinute"].ToString());
            return OTPVerifiedTime.AddMinutes(expiredMinutes) < DateTime.Now;
        }
        public void GetProfilePhoto(string SelfiePhotoUrl, string SelfiePhotoWithIdUrl, out string SelfiePhoto, out string SelfiePhotoWithIdCard)
        {
            SelfiePhoto = string.Empty;
            SelfiePhotoWithIdCard = string.Empty;
            //get s3 photo 64 byte
            S3FileService s3FileServe = new S3FileService();
            var selfiePhotoRes = s3FileServe.GetDataFromS3(SelfiePhotoUrl);
            var selfiePhotoWithIdRes = s3FileServe.GetDataFromS3(SelfiePhotoWithIdUrl);

            S3PhotoResponse selfiePhotoResponse = JsonConvert.DeserializeObject<S3PhotoResponse>(selfiePhotoRes);
            S3PhotoResponse selfiePhotoWithIdResponse = JsonConvert.DeserializeObject<S3PhotoResponse>(selfiePhotoWithIdRes);

            if (selfiePhotoResponse.ResCode == "000")
            {
                SelfiePhoto = selfiePhotoResponse.PhotoUrl;
            }

            if (selfiePhotoResponse.ResCode == "000")
            {
                SelfiePhotoWithIdCard = selfiePhotoWithIdResponse.PhotoUrl;
            }
        }

        public string GetImageUrl(string s3KeyName)
        {
            string imageUrl = string.Empty;
            var s3Service = new S3FileService();
            var responseJson = s3Service.GetImageUrlFromS3(s3KeyName);
            writeLog("ImageUrl | Response Json : "+responseJson);
            var imageRespnose = JsonConvert.DeserializeObject<S3PhotoPresignedUrlResponse>(responseJson);
            writeLog("ImageUrl | image Url after deserialization : " + imageRespnose.PresignedUrl);
            if (imageRespnose.ResCode == "000")
            {
                imageUrl = imageRespnose.PresignedUrl;
            }

            return imageUrl;
        }
    }
}