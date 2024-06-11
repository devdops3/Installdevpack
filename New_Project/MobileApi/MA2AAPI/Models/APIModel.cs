using System;
using System.Collections.Generic;

namespace A2AAPI.Models
{
    public class APIModel
    {
        public string responseData { get; set; }
    }

    public class AstrologerDetails
    {
        public string MaximumQty { get; set; }
        public string BirthTime { get; set; }
        public List<Astrologer> Astrologers { get; set; }
        public List<Product> Products { get; set; }
        public List<UnitPrice> UnitPrices { get; set; }
    }

    public class Astrologer
    {
        public string AstroId { get; set; }
        public string AstroName { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UnitPrice
    {
        public string Qty { get; set; }
        public string Price { get; set; }
    }

    public class MeterDevisionListReqData
    {
        public string version { get; set; }
        public string timeStamp { get; set; }
        public string messageid { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public bool validateResult { get; set; }
        public string rescode { get; set; }
        public string resdesc { get; set; }
    }
    public class MarlarMyineProductListReqData
    {
        public string version { get; set; }
        public string timeStamp { get; set; }
        public string messageid { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public bool validateResult { get; set; }
        public string rescode { get; set; }
        public string resdesc { get; set; }
    }
    public class DevisionListReqData
    {
        public string version { get; set; }
        public string timeStamp { get; set; }
        public string messageid { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public bool validateResult { get; set; }
        public string rescode { get; set; }
        public string resdesc { get; set; }
    }

    public class ReqModel
    {
        public string Version { get; set; }
        public string Timestamp { get; set; }
        public string MessageID { get; set; }
    }

    public class SetPasswordReq : ReqModel
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string RespCode { get; set; }
        public string RespDesc { get; set; }
    }

    public class LoginReq : ReqModel
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string AggrementType { get; set; }
        public string TodayTxnCount { get; set; }
        public string TodayTxnAmount { get; set; }
        public string LogIn_DeviceID { get; set; }
        public string DB_DeviceID { get; set; }
        public string IsPair { get; set; }
        public string DeviceToken { get; set; }
        public string AgentCode { get; set; }
        public string AppVersion { get; set; }
        public string DeviceInfo { get; set; }
        public string ShowBal { get; set; }
        public string NewAppVersion { get; set; }
        public string RespCode { get; set; }
        public string RespDesc { get; set; }
        public string Isupdate { get; set; }

    }
    public class CreateUserReq : ReqModel
    {
        public string PhoneNo { get; set; }
        public string Name { get; set; }
        public string RespCode { get; set; }
        public string RespDesc { get; set; }
    }

    public class UserStatusReq : ReqModel
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string DeviceUID { get; set; }
    }

    public class ResetOTPReq : ReqModel
    {
        public string LoginID { get; set; }
    }

    public class UpdateProfileReq : ReqModel
    {
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string NRC { get; set; }
        public string SecretWord { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsShop { get; set; }
        public string ShopName { get; set; }
        public string ShopType { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class GetProfileReq : ReqModel
    {
        public string AgentUserID { get; set; }
        public string phNo { get; set; }
    }

    public class AttemptCountReq : ReqModel
    {
        public string LoginID { get; set; }
        public bool OTPVerified { get; set; }
        public string DeviceUID { get; set; }
    }

    public class ResendCountReq : ReqModel
    {
        public string LoginID { get; set; }
    }

    public class RespModel
    {
        public string Version { get; set; }
        public string Timestamp { get; set; }
        public string RespCode { get; set; }
        public string RespDesc { get; set; }
    }

    public class GetProfileResp : RespModel
    {
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string PhNo { get; set; }
        public string SecretWord { get; set; }
        public string Nrc { get; set; }
        public int Division { get; set; }
        public int Township { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string IsShop { get; set; }
        public string ShopName { get; set; }
        public string BranchCode { get; set; }
        public string ShopType { get; set; }
        public string LatitudeLoc { get; set; }
        public string Longitude { get; set; }
        public string SelfiePhoto { get; set; }
        public string SelfiePhotoWithIdCard { get; set; }
        public string VerifyStatus { get; set; }
        public string VerifyStatusDesc { get; set; }

    }

 

    public class UserStatusResp : RespModel
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string NRC { get; set; }
        public string OTP { get; set; }
        public string OTPGeneratedTime { get; set; }
        public string OTPExpiry { get; set; }
        public bool OTPVerified { get; set; }
        public string OTPResendCount { get; set; }
        public bool OTPResendLocked { get; set; }
        public string OTPResendLockedTimestamp { get; set; }
        public string OTPAttemptedCount { get; set; }
        public bool OTPAttemptedLocked { get; set; }
        public string OTPAttemptedLockedTimestamp { get; set; }
        public bool IsPasswodSet { get; set; }
        public bool IsRegistered { get; set; }
        public string termsAndConditionsAgreement { get; set; }
        public string DeviceUID { get; set; }
        public DateTime OTPVerifiedTime { get; set; }
    }

    public class UpdateLoginReq : ReqModel
    {
        public string AgentUserID { get; set; }
        public string PhNo { get; set; }
        public string NewPassword { get; set; }
    }

    public class CheckExistingAgentUserIDReq : ReqModel
    {
        public string AgentUserID { get; set; }
    }

    public class AgentCustomerBindingInfoReq : ReqModel
    {
        public string PartnerCode { get; set; }
        public string Token { get; set; }
        public string AgentUserID { get; set; }
        public string CustomerID { get; set; }
    }

    public class InquiryPGReq : ReqModel
    {
        public string PartnerCode { get; set; }
        public string Token { get; set; }
        public string AgentUserID { get; set; }
        public string CustomerID { get; set; }
    }

    public class AgentCustomerBindingInfoRes : RespModel
    {
        public string CustomerID { get; set; }
        public string IsBound { get; set; }
    }

    public class BindAgentCustomerReqModel : ReqModel
    {
        public string AgentUserID { get; set; }
        public string CustomerID { get; set; }
    }

    public class BindAgentCustomerResModel : RespModel
    {
        
    }

    public class UnBindAgentCustomerReqModel : ReqModel
    {
        public string AgentUserID { get; set; }
        public string CustomerID { get; set; }
    }

    public class UnBindAgentCustomerResModel : RespModel
    {

    }


}