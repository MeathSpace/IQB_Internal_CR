using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.HomeServices
{
    public class SettingsService : IDisposable
    {
        //public async Task<UserJoinQueueResultModelAPI> InsertInJoinQueue(UserQueueJoinInsertModel model)
        //{
        //    string CallAddressString = CommonEL.GetCallUrl("iqueueinsertinjoinq.php");
        //    string QueryString = "position=" + model.position + "&username=" + model.username + "&firstlastname=" + model.firstlastname + "&rdatejoinedq=" + model.rdatejoinedq + "&timejoinedq=" + model.timejoinedq + "&barbername=" + model.barbername + "&salonid=" + model.salonid + "&qgcode=" + model.qgcode;
        //    string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        //    UserJoinQueueResultModelAPI resultObject = new UserJoinQueueResultModelAPI();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = JsonConvert.DeserializeObject<UserJoinQueueResultModelAPI>(resultJson);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new UserJoinQueueResultModelAPI();
        //    }
        //    return resultObject;
        //}
        public async Task<ApiResult> DeleteAccount(SettingsModel model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuedeleteaccount.php");
            string QueryString = "username=" + model.username + "&salonid=" + model.salonid + "&imagename=" + model.imagename;
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
