using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.EntityLayer.StaffModels;
using IQB.Views.ApplicationManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.AdminServices
{
    public class StaffServices : IDisposable
    {
        
        public async Task<ApiResult> GetStaffBySalonId(int salonid)
        {
            //string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php"); ;
            //string QueryString = "salonid=" + salonid;
            //string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            string resultJson = "{\"StatusCode\": 200,\"Response\": [{\"StaffId\": \"28481\",\"StaffName\": \"Abc\"}, {\"StaffId\": \"28481\",\"StaffName\": \"Xyz\" }],\"StatusMessage\": \"Success\"}";
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
        /// To delete the Barber according to the saloon
        /// </summary>
        /// <param name="SalonId"></param>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        public async Task<ApiResult> DeleteStaff(int SalonId, int BarberId)
        {
            string CallAddressString = CommonEL.GetCallUrl("DeleteBarber.php"); ;
            var jsonRequest = new { SalonId = SalonId, BarberId = BarberId };
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
        /// To Create Barber
        /// </summary>
        /// <param name="staffmodel"></param>
        /// <returns></returns>
        public async Task<ApiResult> PostCreateStaff(StaffModel staffmodel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostCreateBarber.php"); ;
            var jsonRequest = new { FirstName = staffmodel.FirstName, LastName = staffmodel.LastName, PreferredName = staffmodel.PreferredName, EmployeePasscode=staffmodel.EmployeePasscode, EmailAddress=staffmodel.EmailAddress, Estimated_wait_time=staffmodel.Estimatedwaittime, AvailableForAppointments = staffmodel.AvailableForAppointments,  Salonid =staffmodel.SalonId, Services=staffmodel.LstService, };
            //is_avl_for_appt = 0 // //is_avl_for_appt = 1
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
        /// To Update Barber
        /// </summary>
        /// <param name="staffmodel"></param>
        /// <returns></returns>
        public async Task<ApiResult> PostUpdateStaff(StaffModel staffmodel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostUpdateBarber.php"); ;
            var jsonRequest = new { BarberId=staffmodel.BarberId, FirstName = staffmodel.FirstName, LastName = staffmodel.LastName, PreferredName = staffmodel.PreferredName, EmployeePasscode = staffmodel.EmployeePasscode, EmailAddress = staffmodel.EmailAddress, Estimated_wait_time = staffmodel.Estimatedwaittime, AvailableForAppointments = staffmodel.AvailableForAppointments, Salonid = staffmodel.SalonId, Services = staffmodel.LstService };
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



        public async Task<IsAdminBarberResponse> AdminLinkBarber(string email, int salonId, bool isBarberFlag)
        {
            IsAdminBarberResponse resultObject = new IsAdminBarberResponse();
            string CallAddressString = CommonEL.GetCallUrl("CheckAdminCumBarber.php"); ;
            var jsonRequest = new { Email = email, SalonId = salonId, barberFlag = isBarberFlag.ToString().ToLower() };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<IsAdminBarberResponse>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new IsAdminBarberResponse()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }



        /// <summary>
        /// Check Service Status JoinQueue By ServiceID
        /// </summary>

        public async Task<ApiResult> ServiceStatusJoinQueueByServiceID(int SalonId,int BarberId,int ServiceId)
        {
            string CallAddressString = CommonEL.GetCallUrl("CheckSrvcStusJoinQueueByServiceID.php"); ;
            var jsonRequest = new { SalonId = SalonId, BarberId = BarberId, ServiceId= ServiceId };
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
