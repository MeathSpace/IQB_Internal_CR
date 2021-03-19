namespace IQB.IQBServices.AuthServices
{
    using EntityLayer.AuthModel;
    using EntityLayer.Common;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class RegistrationService : IDisposable
    {
        public async Task<ApiResult> CheckEmailExistence(string email, int salonId)
        {
            string QueryString = CommonEL.GetCallUrl("/iqueuecheckemail.php?checkEmail=" + email + "&salonid=" + salonId);
            //string QueryString = CommonEL.GetCallUrl("/iqueuecheckemail__BKP2019-10-28.php?checkEmail=" + email + "&salonid=" + salonId);
            string resultJson = await Webservices.GetService(QueryString);
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
            }
            return resultObject;
        }
        public async Task<RegisterAdminResponseModel> RegisterUser(RegistrationModel model, int canSendEmail)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueregister.php");
            string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&password=" + model.password + "&email=" + model.email + "&activationcode=" + model.activationcode + "&activated=" + model.activated + "&loggedin=" + model.loggedin + "&registerdate=" + model.registerdate + "&salonid=" + model.salonid + "&maketingemails=" + canSendEmail + "&UserLevel=" + model.UserLevel + "&IsBarber=" + model.IsBarber;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            RegisterAdminResponseModel resultObject = new RegisterAdminResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<RegisterAdminResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new RegisterAdminResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<RegisterAdminResponseModel> RegisterAdminUser(RegistrationModel model, int canSendEmail)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueregister.php");
            //string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&password=" + model.password + "&email=" + model.email + "&activationcode=" + model.activationcode + "&activated=" + model.activated + "&loggedin=" + model.loggedin + "&registerdate=" + model.registerdate + "&salonid=" + model.salonid + "&maketingemails=" + canSendEmail + "&UserLevel=" + model.UserLevel;
            string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&password=" + model.password + "&email=" + model.email + "&activationcode=" + model.activationcode + "&activated=" + model.activated + "&loggedin=" + model.loggedin + "&registerdate=" + model.registerdate + "&salonid=" + model.salonid + "&maketingemails=" + canSendEmail + "&UserLevel=" + model.UserLevel + "&IsBarber=" + model.IsBarber;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            RegisterAdminResponseModel resultObject = new RegisterAdminResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<RegisterAdminResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new RegisterAdminResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }



        public async Task<DeleteRegisteredUserResponseModel> DeleteRegisteredUser(string UserId)
        {
            string CallAddressString = CommonEL.GetCallUrl("DeleteRegisteredUser.php");

            var jsonRequest = new { UserId = UserId};
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            DeleteRegisteredUserResponseModel resultObject = new DeleteRegisteredUserResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<DeleteRegisteredUserResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new DeleteRegisteredUserResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;


           


        }
        //public async Task<RegistrationResultModelAPI> RegisterUser(RegistrationModel model)
        //{
        //    string CallAddressString = CommonEL.GetCallUrl("iqueueregister.php");
        //    string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&password=" + model.password + "&email=" + model.email + "&activationcode=" + model.activationcode + "&activated=" + model.activated + "&loggedin=" + model.loggedin + "&registerdate=" + model.registerdate + "&salonid=" + model.salonid;
        //    string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        //    RegistrationResultModelAPI resultObject = new RegistrationResultModelAPI();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = JsonConvert.DeserializeObject<RegistrationResultModelAPI>(resultJson);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new RegistrationResultModelAPI();
        //    }
        //    return resultObject;
        //}

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}