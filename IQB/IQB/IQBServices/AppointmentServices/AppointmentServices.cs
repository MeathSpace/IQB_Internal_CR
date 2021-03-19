using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.AppointmentServices
{
    public class AppointmentServices : IDisposable
    {
        public async Task<BarberIDResponseModel> GetBarberIdByEmailId(string EmailId, int SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetBarberIdByEmailId.php");
            var jsonRequest = new { Email = EmailId, SalonId = SalonID };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            BarberIDResponseModel resultObject = new BarberIDResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<BarberIDResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new BarberIDResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<MappedAppointmentListResponseModel> GetUserAppointmentMapList(string UserID,string UserLevel)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetAllAppointment.php"); ;
            var jsonRequest = new
            {
                UserId = UserID,
                UserLevel = UserLevel
            };


            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //  string resultJson = "{\"StatusCode\": 200,\"Response\": {\"Appointment\": [{\"AppointmentID\": \"1\", \"SalonID\": \"2\", \"Date\": \"06/05/2016\",\"UserName\": \"1\",\"Status\": \"Booked\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\", \"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",  \"BarberId\": \"2\"}]}, {\"AppointmentID\": \"1\",\"SalonID\": \"2\",\"Date\": \"06/05/2016\",\"UserName\": \"1\",\"Status\": \"Cancelled\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\",\"To\": \"9:15\",\"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",\"BarberId\": \"2\"}]} ]},\"StatusMessage\": \"Success\"}";
            //string resultJson = "{\"StatusCode\":201,\"Response\":\"0\",\"StatusMessage\":\"No records found\"}";
            MappedAppointmentListResponseModel resultObject = new MappedAppointmentListResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<MappedAppointmentListResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new MappedAppointmentListResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }


        public async Task<GetAppointmentByIdResponseModel> GetAppointmentById(string AppoinmentID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetAppoinmentByID.php"); ;
            var jsonRequest = new
            {
                AppoinmentID = AppoinmentID
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //string resultJson = "{\"StatusCode\": 200,\"Response\": {\"Appointment\": [{\"AppointmentID\": \"1\", \"SalonID\": \"2\", \"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Booked\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\", \"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",  \"BarberId\": \"2\"}]}, {\"AppointmentID\": \"1\",\"SalonID\": \"2\",\"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Cancelled\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\",\"To\": \"9:15\",\"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",\"BarberId\": \"2\"}]} ]},\"StatusMessage\": \"Success\"}";

            //string resultJson = "{\"StatusCode\": 200,\"Response\":{\"AppointmentID\": \"1\",\"SalonID\": \"2\",\"Date\": \"10/07/2019\",\"UserName\":\"23644\",\"Status\": \"Booked\",\"IsReserved\": \"false\",\"Services\": [{\"ServiceId\": \"71\",\"From\": \"01:15 PM\",\"To\": \"02:15 PM\",\"BarberId\": \"1\",\"Price\": \"250.00\"},{\"ServiceId\": \"69\",\"From\": \"12:15 PM\",\"To\": \"12:45 PM\",\"BarberId\": \"1\",\"Price\": \"100.00\"}, {\"ServiceId\": \"59\",\"From\": \"11:15 AM\",\"To\": \"11:45 AM\",\"BarberId\": \"2\",\"Price\": \"50.00\"}]},\"StatusMessage\": \"Success\"}";
            GetAppointmentByIdResponseModel resultObject = new GetAppointmentByIdResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<GetAppointmentByIdResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new GetAppointmentByIdResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }



        public async Task<GetEngageBarberTimeSlotResponseModel> GetEngageBarberTimeSlot(string SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetEngageBarberTimeSlot.php");
            var jsonRequest = new
            {
               // SalonId = ,
                SalonId = SalonID,
                IsReserved = "N"
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //string resultJson = "{\"StatusCode\": 200,\"Response\": [{\"AppointmentId\": \"1\",\"BarberId\": \"1\",\"BarberName\":\"Sudip Bhattacharya\",\"Date\": \"10/07/2019\",\"From\": \"1:00 PM\",\"To\": \"2:15 PM\"},{\"AppointmentId\": \"2\",\"BarberId\": \"2\",\"BarberName\": \"Abhijeet Chowdhury\",\"Date\": \"10/07/2019\",\"From\": \"3:00 PM\",\"To\": \"4:15 PM\"}],\"StatusMessage\": \"Success\"}";
            GetEngageBarberTimeSlotResponseModel resultObject = new GetEngageBarberTimeSlotResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<GetEngageBarberTimeSlotResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new GetEngageBarberTimeSlotResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }



        public async Task<GetEngageReserveTimeBySalonidModel> GetEngageReserveTimeBySalonid(string SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetEngageReserveTimeBySalonid.php"); ;
            var jsonRequest = new
            {
                // SalonId = ,
                SalonId = SalonID,
                IsReserved = "Y"
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //string resultJson = "{\"StatusCode\": 200,\"Response\": [{\"Date\": \"12/07/2019\",\"From\": \"12:00 PM\",\"To\": \"1:15 PM\"},{\"Date\": \"12/07/2019\",\"From\": \"2:00 PM\",\"To\": \"3:15 PM\"}],\"StatusMessage\": \"Success\"}";
            GetEngageReserveTimeBySalonidModel resultObject = new GetEngageReserveTimeBySalonidModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<GetEngageReserveTimeBySalonidModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new GetEngageReserveTimeBySalonidModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<SetAppointmentResponseModel> SetAppointment(AppointmentRequestModel _appointmentRequestModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("SetAppoinment.php"); ;
            var jsonRequest = _appointmentRequestModel;
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);

           //string resultJson = "{\"StatusCode\": 200,	\"Response\": {\"AppoinmentID\": \"1\"},\"StatusMessage\": \"Success\"}";
            SetAppointmentResponseModel resultObject = new SetAppointmentResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SetAppointmentResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SetAppointmentResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }



        public async Task<SetAppoinmentStatusResponseModel> SetAppoinmentStatus(string AppointmentID,string Status)
        {
            //string CallAddressString = CommonEL.GetCallUrl("SetAppoinment.php"); ;
            //var jsonRequest = new
            //{
            //    AppointmentID = AppointmentID,
            //    Status= Status
            //};
            //var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            string resultJson = "{\"StatusCode\": 200,\"Response\": {\"Appointment\": [{\"AppointmentID\": \"1\", \"SalonID\": \"2\", \"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Booked\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\", \"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",  \"BarberId\": \"2\"}]}, {\"AppointmentID\": \"1\",\"SalonID\": \"2\",\"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Cancelled\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\",\"To\": \"9:15\",\"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",\"BarberId\": \"2\"}]} ]},\"StatusMessage\": \"Success\"}";
            SetAppoinmentStatusResponseModel resultObject = new SetAppoinmentStatusResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SetAppoinmentStatusResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SetAppoinmentStatusResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<DeleteAppointmentResponseModel> DeleteAppoinmentStatus(int AppointmentID,string StatusChangeReason)
        {
            string CallAddressString = CommonEL.GetCallUrl("DeleteAppointment.php"); ;
            var jsonRequest = new
            {
                AppointmentID = AppointmentID,
                StatusChangeReason= StatusChangeReason
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //string resultJson = "{\"StatusCode\": 200,\"Response\": {\"Appointment\": [{\"AppointmentID\": \"1\", \"SalonID\": \"2\", \"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Booked\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\", \"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",  \"BarberId\": \"2\"}]}, {\"AppointmentID\": \"1\",\"SalonID\": \"2\",\"Date\": \"06.05.2016\",\"UserName\": \"1\",\"Status\": \"Cancelled\",\"Services\": [{\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\",\"To\": \"9:15\",\"BarberId\": \"2\"}, {\"ServiceId\": \"1\",\"Price\": \"2.00\",\"From\": \"6:15\", \"To\": \"9:15\",\"BarberId\": \"2\"}]} ]},\"StatusMessage\": \"Success\"}";
            DeleteAppointmentResponseModel resultObject = new DeleteAppointmentResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<DeleteAppointmentResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new DeleteAppointmentResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<SetAppoinmentSettingsResponseModel> SetAppointmentSettings(AppoinmentSettingsRequestModel _appoinmentSettingsRequestModel)
        {
            string CallAddressString = CommonEL.GetCallUrl("SetAppointmentSettings.php");
            var jsonRequest = _appoinmentSettingsRequestModel;
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);

           // string resultJson = "{\"StatusCode\": 200,	\"Response\": {\"AppoinmentSettingsID\": \"1\"},\"StatusMessage\": \"Success\"}";
            SetAppoinmentSettingsResponseModel resultObject = new SetAppoinmentSettingsResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SetAppoinmentSettingsResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SetAppoinmentSettingsResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<AppointmentSettingsResponseModel> GetAppoinmentSettingsBysalonId(string SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetAppoinmentSettingsBysalonId.php"); ;
            var jsonRequest = new
            {
                SalonId = SalonID
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            //string resultJson = "{\"StatusCode\": 200,\"Response\": {\"AppoinmentSettingsID\": 12,\"NumberOfDaysAfterAppdate\": \"15\"},\"StatusMessage\":\"Success\"}";
            AppointmentSettingsResponseModel resultObject = new AppointmentSettingsResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<AppointmentSettingsResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new AppointmentSettingsResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
