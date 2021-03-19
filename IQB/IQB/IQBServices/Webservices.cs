namespace IQB.IQBServices
{
    using IQB.EntityLayer.ServiceModels;
    using EntityLayer.Common;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class Webservices
    {
        public static async Task<string> GetService(string queryString)
        {
            string resultJson = string.Empty;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(CommonEL.baseurl);
                var response = await client.GetAsync(queryString);
                resultJson = response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                resultJson = string.Empty;
            }
            return resultJson;
        }

        public static async Task<string> PostService(string callAddressString, string queryString)
        {
            string resultJson = string.Empty;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(CommonEL.baseurl);
                StringContent str = new StringContent(queryString, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await client.PostAsync(new Uri(CommonEL.baseurl + callAddressString), str);
                resultJson = response.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                resultJson = string.Empty;
            }
            return resultJson;
        }

        public static async Task<string> PostService_RAW(string callAddressString,string Parameter)
        {
            string resultJson = string.Empty;
            
            try
            {
                using (var c = new HttpClient())
                {
                    var client = new System.Net.Http.HttpClient();
                   // var jsonRequest = new { ServiceName = serviceModel.ServiceName, SalonId = serviceModel.SalonId, ServicePrice=serviceModel.ServicePrice };
                   // var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
                    HttpContent content = new StringContent(Parameter, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(new Uri(CommonEL.baseurl + callAddressString), content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                        resultJson = result.ToString();
                    }
                }
            }
            catch 
            {

                resultJson = string.Empty;
            }
            return resultJson;
        }

      
    }
}