using IQB.EntityLayer.Common;
using IQB.EntityLayer.ReportModels;
using IQB.EntityLayer.SalonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.ReportServices
{
    public class ReportService : IDisposable
    {
        string CallAddressString;
        public async Task<ReportResponse> GetReport(string QueryString, int isAdmin=0)
        {
            if (isAdmin ==1)
            {
                CallAddressString = CommonEL.GetCallUrl($"GetSalonPerformanceReportGraph.php?{QueryString}");
            }

            if (isAdmin == 2)
            {
                CallAddressString = CommonEL.GetCallUrl($"GetBarberPerformanceReportGraph.php?{QueryString}");
            }

            if (isAdmin == 3)
            {
                CallAddressString = CommonEL.GetCallUrl($"AppointmentReportGraph.php?{QueryString}");
            }

            if (isAdmin == 4)
            {
                CallAddressString = CommonEL.GetCallUrl($"AppointmentReportGraph.php?{QueryString}");
            }




            string resultJson = await Webservices.GetService(CallAddressString);

            ReportResponse resultObject = new ReportResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<ReportResponse>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new ReportResponse()
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
