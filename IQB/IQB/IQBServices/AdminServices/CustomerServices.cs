using IQB.EntityLayer.Common;
using IQB.EntityLayer.CustomerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.AdminServices
{
    public class CustomerServices : IDisposable
    {

        /// <summary>
        ///Customer List 
        /// </summary>
        /// <param name="salonid"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<ApiResult> GetCustomerBySalonId(int salonid, string firstName, string lastName, int CustSkip,int CustTake)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetCustomerBySalonId.php"); ;
            var jsonRequest = new { SalonId = salonid, FName = firstName, LName = lastName, CustSkip = CustSkip, CustTake = CustTake };
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

        public async Task<ApiResult> PostCustomerMailByCustomerId(CustomerMailModel _customerMailModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostCustomerMailByCustomerId.php"); ;
            var jsonRequest = new { CustomerId = _customerMailModel.CustomerId, Message = _customerMailModel.MailBody, Subject = _customerMailModel.MailSubject };
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

        public async Task<ApiResult> PostCustomerMailForAllCustomer(CustomerMailModel _customerMailModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("PostCustomerMailForAllCustomer.php"); ;
            var jsonRequest = new { CustomerId = _customerMailModel.CustomerId, Message = _customerMailModel.MailBody, Subject = _customerMailModel.MailSubject,salonId=_customerMailModel.SalonId };
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

        public async Task<ApiResult> GetCustomerDetailsByCustomerId(int SalonId, int CustomerId)
        {
            ////string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect2.php"); ;
            ////string QueryString = "salonid=" + salonid;
            ////string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            //string resultJson = "{\"StatusCode\": 200,\"Response\": [{\"CustomerId\": \"2\",\"CustomerName\": \"Abc\",\"CustomerEmail\": \"a@b\",\"CustomerPhone\": \"123456\",\"CustomerDOB\": \"22/02/1977\"}],\"StatusMessage\": \"Success\"}";

            string CallAddressString = CommonEL.GetCallUrl("GetCustomerDetailsByCustomerId.php"); ;
            var jsonRequest = new { SalonId = SalonId, CustomerId = CustomerId };
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

        public async Task<ApiResult> GetCustomerBookingHistory(string customerId, string startDate, string endDate)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetCustomerBookingHistory.php"); ;
            var jsonRequest = new { CustomerId = customerId, StartDt = startDate, EndDt = endDate };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);
            //string resultJson = "{\"StatusCode\": 200,\"Response\":{\"CustomerId\": \"19837\",\"CustomerFName\": \"Sudip\",\"CustomerLName\":\"Bhattacharya\",\"History\":[{\"BarberName\": \"Abhijeet Chowdhury\",\"ServiceName\": \"Hair Cutting\",\"DateOfBooking\": \"12/06/2017\"}, {\"BarberName\": \"Abhijeet Chowdhury\",\"ServiceName\": \"Hair Cutting\",\"DateOfBooking\": \"12/06/2017\"}]},\"StatusMessage\": \"Success\"}";

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
