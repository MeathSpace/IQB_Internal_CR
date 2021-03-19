using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace IQB.IQBServices.AuthServices
{
    internal class MyProfileService : IDisposable
    {
        public async Task<ApiResult> UpdateProfile(MyProfileModel model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueupdatecustomerdetails.php");
            string QueryString = "firstname=" + model.FirstName + "&lastname=" + model.LastName + "&email=" + model.Email + "&password=" + model.Password + "&mobile=" + model.MobileNo + "&dob=" + model.DOB + "&salonid=" + model.SalonId+ "&maketingemails=" + model.IsMailEnabled;
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
            }
            return resultObject;
        }

        //public async Task<ApiResult> UploadProfileImage(MyProfileModel model)
        //{
        //    string CallAddressString = CommonEL.GetCallUrl("upload_profile_image.php");
        //    string QueryString = "userid=" + model.id + "&image=" + model.ProfileImage + "&imagename=" + model.FileName;
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