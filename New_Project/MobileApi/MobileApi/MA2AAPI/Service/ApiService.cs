using log4net;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace MA2AAPI.Service
{
    public class ApiService
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void writeLog(string logstring)
        {
            Logger.writeLog(logstring, ref log);
        }
        public bool IsMethodNeedUserTokenCheck(string methodName)
        {
            var methodsDontNeedUserTokenCheck = new string[21] { "HomeReqV2","LoginReq","LoginReqV2", "SystemSettingReq", "OTPVerifyReq", 
                "StatusInquiryReq","StatusInquiryReqV2" ,"OTPReq","OTPReqV2", "SetPasswordReq","CreateUserReq",
           "ResetOTPAttemptedReq","SetAttemptCountReq", "SetAttemptCountReqV2","ResetOTPResendReq","RegisterReq","UserAmountReq" ,"InquiryReqPG",
            "UpgradeReq","TermsAndConditionsReq", "UpdateTermsAndConditionsAgreementReq"};
            return methodsDontNeedUserTokenCheck.Contains(methodName);
        }

        public bool IsMethodNeedToCheckUserLock(string methodName)
        {
            var methodsDontNeedUserLockCheck = new string[10] { "HomeReqV2","LoginReq","LoginReqV2", "RegisterReq", "CreateUserReq",
                "StatusInquiryReq","StatusInquiryReqV2" ,"TermsAndConditionsReq","CheckExistingAgentUserIDReq", "RewardWinnerReq"};
            return !methodsDontNeedUserLockCheck.Contains(methodName);
        }

        public bool IsMethodNeedToCheckAppVersion(string methodName)
        {
            var methodsNeedToCheck = new string[2] { "GetCanalPlusPackagesReq","DenominationListReq" };
            return methodsNeedToCheck.Contains(methodName);
        }

        public bool IsValidAppVersionToUseBiller(string currentAppVersion, string taxId)
        {
            if (taxId.Equals("0000000000052"))
            {
                var validAppVersion = ConfigurationManager.AppSettings["ValidAppVersionForCanalPlus"].ToString();
                return IsValidAppVersion(currentAppVersion, validAppVersion);
            }
            else if (taxId.Equals(ConfigurationManager.AppSettings["PubgUC"].ToString()))
            {
                var validAppVersion = ConfigurationManager.AppSettings["ValidAppVersionForPUBG"].ToString();
                return IsValidAppVersion(currentAppVersion, validAppVersion);
            }
            else return true;
        }

        public bool IsValidAppVersion(string currentAppVersion, string validAppVersion)
        {
            var result = false;
            if (int.Parse(currentAppVersion) >= int.Parse(validAppVersion))
                result = true;
            return result;
        }
    }
}