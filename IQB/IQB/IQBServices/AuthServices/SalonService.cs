namespace IQB.IQBServices.AuthServices
{
    using EntityLayer.Common;
    using System;
    using System.Threading.Tasks;

    public class SalonService : IDisposable
    {
        //public async Task<List<SalonInfoAPIModel>> GetSalonInfo(string SalonCode)
        //{
        //    string QueryString = CommonEL.GetCallUrl("iqbsalons.php?SalonCode=" + SalonCode);
        //    string resultJson = await Webservices.GetService(QueryString);
        //    List<SalonInfoAPIModel> resultObject = new List<SalonInfoAPIModel>();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = (List<SalonInfoAPIModel>)JsonConvert.DeserializeObject(resultJson, typeof(List<SalonInfoAPIModel>));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new List<SalonInfoAPIModel>();
        //    }
        //    return resultObject;
        //}

        public async Task<ApiResult> GetSalonInfoNew(string SalonCode)
        {
            string QueryString = CommonEL.GetCallUrl("iqbsalons.php?SalonCode=" + SalonCode);
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

        //Get List of Salons by city
        public async Task<ApiResult> GetSalonListbyCity(string city)
        {
            string QueryString = CommonEL.GetCallUrl("retrieveSalonList.php?city=" + city);
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

        //Send Mail for Registering New Salon Request
        public async Task<ApiResult> MailSalonInterest(string Name,string Email,string MessageBody)
        {
            string CallAddressString = CommonEL.GetCallUrl("sendMailSalonRequest.php");
            string QueryString = "name=" + Name + "&email=" + Email + "&msgbody=" + MessageBody;
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

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}