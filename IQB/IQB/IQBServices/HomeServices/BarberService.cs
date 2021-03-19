using IQB.EntityLayer.Common;
using IQB.ViewModel.AuthViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.IQBServices.HomeServices
{
    public class BarberService : IDisposable
    {
        //public async Task<List<BarberReturnResult>> BarberSelect(int salonid)
        //{
        //    string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php"); ;
        //    string QueryString = "salonid=" + salonid;
        //    string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        //    List<BarberReturnResult> resultObject = new List<BarberReturnResult>();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = (List<BarberReturnResult>)JsonConvert.DeserializeObject(resultJson, typeof(List<BarberReturnResult>));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new List<BarberReturnResult>();
        //    }
        //    return resultObject;
        //}
        public async Task<ApiResult> BarberSelect(int salonid)
        {
            //string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php");
            string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php?salonid=" + salonid);
            string QueryString = "salonid=" + salonid;
            string resultJson = await Webservices.GetService(CallAddressString);
            //string resultJson = await Webservices.PostService(CallAddressString, QueryString);
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

        public async Task<ApiResult> BarberServiceSelect(int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php");
            string QueryString = "salonid=";
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

        //public async Task<bool> CheckUserJoinedQueue(string checkUsername, int salonid)
        //{
        //    bool returnResult = false;
        //    string CallAddressString = CommonEL.GetCallUrl("iqueuejoinedqselect.php"); ;
        //    string QueryString = "checkUsername=" + checkUsername + "&salonid=" + salonid;
        //    string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        //    List<CheckUserReturnResult> resultObject = new List<CheckUserReturnResult>();
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(resultJson))
        //        {
        //            resultObject = (List<CheckUserReturnResult>)JsonConvert.DeserializeObject(resultJson, typeof(List<CheckUserReturnResult>));
        //            if (resultObject.Count > 0)
        //            {
        //                returnResult = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultObject = new List<CheckUserReturnResult>();
        //    }
        //    return returnResult;
        //}

        public async Task<ApiResult> CheckUserJoinedQueue(string checkUsername, int salonid)
        {

            string CallAddressString = CommonEL.GetCallUrl("iqueuejoinedqselect.php"); ;
            string QueryString = "checkUsername=" + checkUsername + "&salonid=" + salonid;
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

        public async Task<ApiResult> CheckQueuePosition(int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuecheckposition.php"); ;
            string QueryString = "salonid=" + salonid;
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

        public async Task<ApiResult> InsertInJoinQueue(UserQueueJoinInsertModel model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueinsertinjoinq_v2.php");//iqueueinsertinjoinq
            string QueryString = "position=" + model.position + "&username=" + model.username + "&firstlastname=" + model.firstlastname + "&barbername=" + model.barbername+ "&BarberId=" + model.BarberId + "&salonid=" + model.salonid + "&qgcode=" + model.qgcode + "&is_single_join=" + model.is_single_join + "&ServiceId=" + model.ServiceId; //"&rdatejoinedq=" + model.rdatejoinedq + "&timejoinedq=" + model.timejoinedq +
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);


            //string CallAddressString = CommonEL.GetCallUrl("iqueueinsertinjoinq_v2.php");//iqueueinsertinjoinq
            //var jsonRequest = new { position = model.position, username = model.username, firstlastname =model.firstlastname, barbername = model.barbername, salonid =model.salonid, qgcode =model.qgcode, is_single_join  = model.is_single_join, ServiceId =model.ServiceId};
            //var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);


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

        public async Task<ApiResult> PostAddJoinQueue(UserQueueJoinInsertModel model)
        {
            //string CallAddressString = CommonEL.GetCallUrl("iqueueinsertinjoinq_v2.php");//iqueueinsertinjoinq
            //string QueryString = "position=" + model.position + "&username=" + model.username + "&firstlastname=" + model.firstlastname + "&barbername=" + model.barbername + "&salonid=" + model.salonid + "&qgcode=" + model.qgcode + "&is_single_join=" + model.is_single_join; //"&rdatejoinedq=" + model.rdatejoinedq + "&timejoinedq=" + model.timejoinedq +
            //string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            string resultJson = "{\"StatusCode\":200,\"Response\":1,\"StatusMessage\":\"Successfully joined to the queue\"}";
           
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

        public async Task<ApiResult> GetSupportInfo(string id)
        {
            string CallAddressString = CommonEL.GetCallUrl("infoDesc.php?id="+id); ;
            //string QueryString = "id=" + id;
            string resultJson = await Webservices.GetService(CallAddressString);
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

        public async Task<ApiResult> GetServicesByBarberIdSalonId(int barberid, int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetServicesByBarberIdSalonId.php"); ;
            var jsonRequest = new { BarberId = barberid, SalonId = salonid };
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
            }
            return resultObject;
        }

        public async Task<ApiResult> GetAllServicesByBarberIdSalonId(int barberid, int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetAllServicesByBarberIdSalonId.php"); ;
            var jsonRequest = new { BarberId = barberid, SalonId = salonid };
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
            }
            return resultObject;
        }


        public async Task<ApiResult> GetSalonBarberMapping(int barberid, int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetSalonBarberMapping.php"); ;
            var jsonRequest = new { BarberId = barberid, SalonId = salonid };
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