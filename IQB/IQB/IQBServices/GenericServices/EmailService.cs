namespace IQB.IQBServices.GenericServices
{
    using EntityLayer.Common;
    using EntityLayer.GenericModels;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class EmailService : IDisposable
    {
        public async Task<string> RegistrationEmail(RegistrationEmailEL model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuesendemail.php");
            string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&username=" + model.username + "&password=" + model.password + "&activationcode=" + model.activationcode + "&email=" + model.email + "&vSalonname=" + model.vSalonname + "&vSalonTel=" + model.vSalonTel + "&vSalonWeb=" + model.vSalonWeb;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            return resultJson;
        }

        public async Task<string> ForgotPasswordEmail(ForgotPasswordEmail model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuesendpassword.php");
            string QueryString = "firstname=" + model.firstname + "&lastname=" + model.lastname + "&password=" + model.password + "&email=" + model.email + "&vSalonname=" + model.vSalonname + "&vSalonTel=" + model.vSalonTel + "&vSalonWeb=" + model.vSalonWeb;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            return resultJson;
        }





        public async Task<ApiResult> SendEmailToAdmin(ServiceEmail _serviceEmail)
        {
            string CallAddressString = CommonEL.GetCallUrl("SendEmailToAdmin.php");
            //string QueryString = "Body=" + Body;
            //string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            var jsonRequest = _serviceEmail;
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);

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
        //public async Task<ApiResult> SendEmailToAdmin(string Body)
        //{
        //    string CallAddressString = CommonEL.GetCallUrl("SendEmailToAdmin.php");
        //    string QueryString = "Body=" + Body;
        //    string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        //    ApiResult resultObject = new ApiResult();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = UtilityEL.ToApiModel(resultJson);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new ApiResult()
        //        {
        //            StatusCode = 201,
        //            StatusMessage = ex.Message
        //        };
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