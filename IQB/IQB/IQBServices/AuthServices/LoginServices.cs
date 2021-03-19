namespace IQB.IQBServices.AuthServices
{
    using IQB.Utility;
    using EntityLayer.Common;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class LoginServices : IDisposable
    {
        public async Task<ApiResult> LoginUser(string email, string password, int salonid, string deviceToken, string deviceType)
        {
            WriteErrorLog errLog = new WriteErrorLog();

            if (NetworkConnectionMgmt.IsConnectedToNetwork())
            {
                string CallAddressString = CommonEL.GetCallUrl("iqueuelogin.php");
                string QueryString = "email=" + email + "&password=" + password + "&salonid=" + salonid + "&deviceToken=" + deviceToken + "&deviceType=" + deviceType;
                // string QueryString = "email=" + email + "&password=" + password + "&salonid=" + salonid;
                string resultJson = await Webservices.PostService(CallAddressString, QueryString);
                ApiResult resultObject = new ApiResult();
                try
                {
                    if (!string.IsNullOrWhiteSpace(resultJson))
                    {
                        resultObject = UtilityEL.ToApiModel(resultJson);
                    }
                }
                catch (Exception ex)
                {
                    resultObject = new ApiResult()
                    {
                        StatusCode = 201,
                        StatusMessage = ex.Message
                    };
                    errLog.WinrtErrLogTest(ex);
                }

                return resultObject;
            }
            else
            {
                return new ApiResult()
                {
                    StatusCode = 201,
                    StatusMessage = CommonEL.NoNetworkErrorMsg
                };
            }
        }

        public async Task<ApiResult> UserAuthentication(string email, int salonId)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            if (NetworkConnectionMgmt.IsConnectedToNetwork())
            {
                //string CallAddressString = CommonEL.GetCallUrl("iqueuelogin.php");
                //string QueryString = "email=" + email + "&password=" + password + "&salonid=" + salonid;
                //string resultJson = await Webservices.PostService(CallAddressString, QueryString);


                string CallAddressString = CommonEL.GetCallUrl("GetUserLevelByEmail.php");
                var jsonRequest = new { Email = email, SalonId = salonId };
                var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
                string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);


                ApiResult resultObject = new ApiResult();
                try
                {
                    if (!string.IsNullOrWhiteSpace(resultJson))
                    {
                        resultObject = UtilityEL.ToApiModel(resultJson);
                    }
                }
                catch (Exception ex)
                {
                    resultObject = new ApiResult()
                    {
                        StatusCode = 201,
                        StatusMessage = ex.Message
                    };
                    errLog.WinrtErrLogTest(ex);
                }

                return resultObject;
            }
            else
            {
                return new ApiResult()
                {
                    StatusCode = 201,
                    StatusMessage = CommonEL.NoNetworkErrorMsg
                };
            }
        }

        public async Task<string> ActivateAccount(string query)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuedirectquery.php");
            string QueryString = "query=" + query;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            return resultJson;
        }
        public async Task<ApiResult> ActivateAccountRaw(string query)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            string CallAddressString = CommonEL.GetCallUrl("iqueuedirectquery2.php");
            var jsonRequest = new
            {
                query=query
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);
            ApiResult resultObject = new ApiResult();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = UtilityEL.ToApiModel(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new ApiResult()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
                errLog.WinrtErrLogTest(ex);
            }

            return resultObject;
        }

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}