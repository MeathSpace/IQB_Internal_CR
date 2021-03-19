using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.AppointmentServices
{
    public class BarberService : IDisposable
    {

        public async Task<MappedBarberModel> GetBarberListBySalonServiceID(string serviceID, string SalonID)
        {
            //serviceID = "1"; SalonID = "7";
            string CallAddressString = CommonEL.GetCallUrl("GetBarberListBySalonServiceID.php"); ;
            var jsonRequest = new
            {
                SalonId = SalonID,
                ServiceId = serviceID
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);




            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"AppointmentId\":\"1\",\"AppointmentStatus\":\"Booked\",\"AppointmentDate\":\"06.05.2016\", \"AppointmentStartTime\":\"18:30\",\"AppointmentEndTime\":\"18:15\"},{\"AppointmentId\":\"2\",\"AppointmentStatus\":\"Queue\",\"AppointmentDate\":\"03.06.2016\",\"AppointmentStartTime\":\"17:15\",\"AppointmentEndTime\":\"17:30\"}],\"StatusMessage\":\"Success\"}";
            // string resultJson = "{\"StatusCode\":200,\"Response\":[ {\"BarberId\":\"1\",\"BarberName\":\"Sudip Bhattacharya\",},{\"BarberId\":\"2\",\"BarberName\":\"Abhijeet Chowdhury\",}],\"StatusMessage\":\"Success\"}";
            MappedBarberModel resultObject = new MappedBarberModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<MappedBarberModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new MappedBarberModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
