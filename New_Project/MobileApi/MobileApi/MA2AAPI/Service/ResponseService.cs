using MA2AAPI.Constants;
using System.Configuration;

namespace MA2AAPI.Service
{
    public class ResponseService
    {
       
        public string GetIncorrectPasswordResponse()
        {
            var code = "05";
            var description = "Incorrect Password";
            return Utils.GetErrorResponse(code, description);
        }

        public string GetLockedUserResponse()
        {
            var code = ((int)ResponseCode.Locked).ToString();
            var description = ConfigurationManager.AppSettings["LockMessage"].ToString();
            return Utils.GetErrorResponse(code, description);
        }

        public string GetIncorrectLoginIDResponse()
        {
            var code = "05";
            var description = "Incorrect LoginID";
            return Utils.GetErrorResponse(code, description);
        }

        public string GetAuthenticationFailResponse()
        {
            var code = "05";
            var description = "Authentication Failed";
            return Utils.GetErrorResponse(code, description);
        }

        public string GetLoginUpdateFailResponse()
        {
            var code = "05";
            var description = "Login Update Failed";
            return Utils.GetErrorResponse(code, description);
        }

        public string GetDeviceIdNotMatchResponse()
        {
            var code = "11";
            var description = "Device ID NOT Matched";
            return Utils.GetErrorResponse(code, description);
        }

    }
}