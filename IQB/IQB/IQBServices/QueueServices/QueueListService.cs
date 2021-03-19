namespace IQB.IQBServices.QueueServices
{
    using EntityLayer.Common;
    using EntityLayer.QueueELS;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class QueueListService : IDisposable
    {
        public async Task<List<QueueListApiModel>> GetQueueList(int salonid, int PageNo)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuechecklist.php");
            string QueryString = "salonid=" + salonid + "&page_no=" + PageNo;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            List<QueueListApiModel> resultObject = new List<QueueListApiModel>();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = (List<QueueListApiModel>)JsonConvert.DeserializeObject(resultJson, typeof(List<QueueListApiModel>));
                }
            }
            catch (Exception ex)
            {
                resultObject = new List<QueueListApiModel>();
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