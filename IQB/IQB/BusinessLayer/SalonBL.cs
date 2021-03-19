using IQB.EntityLayer;
using IQB.IQBServices;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IQB.BusinessLayer
{
    public class SalonBL
    {
        public async Task<FetchSalonByIdReturnResultModel> GetSalonById(string SalonCode)
        {
            string QueryString = "iqbsalons.php?SalonCode=" + SalonCode;
            string resultJson = await Webservices.GetService(QueryString);
            FetchSalonByIdReturnResultModel resultObject = new FetchSalonByIdReturnResultModel();
            if (!string.IsNullOrWhiteSpace(resultJson))
            {
                resultObject = JsonConvert.DeserializeObject<FetchSalonByIdReturnResultModel>(resultJson);
            }
            return resultObject;
        }
    }
}