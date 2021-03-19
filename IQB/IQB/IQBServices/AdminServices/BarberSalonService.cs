using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.AdminServices
{
   public class BarberSalonService : IDisposable
    {
        /// <summary>
        /// to get the list for service list
        /// </summary>
        /// <param name="SalonId"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetServicesBySalonId(int SalonId)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetServicesBySalonId.php"); ;
            var jsonRequest = new { SalonId = SalonId };
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

        public async Task<ApiResult> GetBarberServicesBySalonId(int SalonId)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberServicesBySalonId.php"); ;
            var jsonRequest = new { SalonId = SalonId };
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

        /// <summary>
        /// To get the list for manage staff.
        /// </summary>
        /// <param name="SalonId"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetBarberBySalonId(int SalonId)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberBySalonId.php"); ;
            var jsonRequest = new { SalonId = SalonId };
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

        public async Task<ApiResult> GetAppointmentAvlBarberBySalonId(int SalonId)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberListBySalonIdForAppointment.php"); ;
            var jsonRequest = new { SalonId = SalonId };
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

        public async Task<ApiResult> GetServiceDetailsBySalonIdServiceId(int ServiceId)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetServiceDetailsByServiceId.php"); ;
            var jsonRequest = new {ServiceId = ServiceId };
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

        /// <summary>
        /// This is used to get the particular barber details.
        /// </summary>
        /// <param name="SalonId"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public async Task<ApiResult> GetStaffDetailsBySalonIdStaffId(int BarberID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberDetailsByBerberId.php"); ;
            var jsonRequest = new { BarberId = BarberID };
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

        public async Task<ApiResult> GetStaffDetailsByEmailId(string EmailId, int SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberDetailsByMailId.php"); 
            var jsonRequest = new { Email = EmailId, SalonId = SalonID };
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

        public async Task<ApiResult> DeleteService(int SalonId, int ServiceId)
        {
            string CallAddressString = CommonEL.GetCallUrl("DeleteService.php"); ;
            var jsonRequest = new { SalonId = SalonId, ServiceId = ServiceId };
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

        public async Task<ApiResult> PostCreateService(ServiceModel serviceModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostCreateService.php"); ;
            //string QueryString = "ServiceName="+servicemodel.ServiceName+"&"+"SalonId=" + servicemodel.SalonId+"&"+ "ServicePrice="+servicemodel.ServicePrice;
            var jsonRequest = new { ServiceName = serviceModel.ServiceName, SalonId = serviceModel.SalonId, ServicePrice = serviceModel.ServicePrice };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);
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

        public async Task<ApiResult> PostUpdateService(ServiceModel serviceModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostUpdateService.php"); ;
           // string QueryString = "ServiceName=" + servicemodel.ServiceName + "&" + "SalonId=" + servicemodel.SalonId + "&" + "ServiceId="+servicemodel.ServiceId+ "ServicePrice=" + servicemodel.ServicePrice;
            var jsonRequest = new { ServiceName = serviceModel.ServiceName, SalonId = serviceModel.SalonId, ServiceId=serviceModel.ServiceId, ServicePrice = serviceModel.ServicePrice };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);
            //  string resultJson = await Webservices.PostService_RAW(CallAddressString, servicemodel);

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

        public async Task<ApiResult> PostUpdateSalonText(int SalonId, string SalonText)
        {
            //string CallAddressString = CommonEL.GetCallUrl("PostUpdateSalonText.php"); ;
            //var jsonRequest = new { SalonId = SalonId, SalonText = SalonText };
            //var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            //ApiResult resultObject = new ApiResult();
            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(resultJson))
            //    {
            //        resultObject = UtilityEL.ToApiModel(resultJson);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    resultObject = new ApiResult()
            //    {
            //        StatusCode = 201,
            //        StatusMessage = ex.Message
            //    };
            //}
            //return resultObject;

            string CallAddressString = CommonEL.GetCallUrl("PostUpdateSalonText.php");//iqueueHomeScreenRet.php
            string QueryString = "SalonId=" + SalonId + "&SalonText=" + SalonText;
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


        public async Task<ApiResult> GetSetBarberDayOff(int SalonId, int UserId, int[] array)
        {
            string CallAddressString = CommonEL.GetCallUrl("InsertDayOff.php"); ;
            var jsonRequest = new { SalonId = SalonId, BarberId = UserId, DaysOff = array };
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
