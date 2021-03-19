using IQB.EntityLayer.Common;
using IQB.EntityLayer.SalonModels;
using IQB.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IQB.IQBServices.SalonServices
{
    public class SalonServices : IDisposable
    {
        WriteErrorLog errLog = new WriteErrorLog();
        public async Task<SalonResponseModel> CreateSalon(SalonModel model)
        {
            string CallAddressString = CommonEL.GetCallUrl("CreateSalon.php"); ;
            var jsonRequest = new
            {
                SalonAppIcon = model.SalonAppIcon,
                SalonName = model.SalonName,
                CustomerName = model.CustomerName,
                UserName = model.UserName,
                EmailAddress = model.EmailAddress,
                city = model.city,
                county = model.county,
                //PostCode = model.PostCode,
                PostCode = model.PostCode.ToUpper(),
                Address = model.Address,
                ContactTel = model.ContactTel,
                SalonWebsite = model.SalonWebsite,
                SalonTwitter = model.SalonTwitter ?? "",
                SalonFacebook = model.SalonFacebook ?? "",
                SalonInstagram = model.SalonInstagram ?? "",
                SalonOnStop = model.SalonOnStop,
                FtpFolderName = model.FtpFolderName,
                AccountExpiryDate = model.AccountExpiryDate,
                CountryCode = model.CountryCode,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                Currency = model.Currency,
                PaymentGatewaySettings = new List<PaymentGatewayData>(new[]
                {
                     new PaymentGatewayData
                     {
                         merchant_email = model.PaymentGatewaySettings[0].merchant_email
                     }
                }),
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            // errLog.WinrtErrLogTest(serializedJsonRequest, "RegisterAdmin", "SalonCreateServie");
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            SalonResponseModel resultObject = new SalonResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SalonResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SalonResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<SalonResponseModel> UpdateSalon(SalonModel model)
        {
            string CallAddressString = CommonEL.GetCallUrl("UpdateSalon.php"); ;
            var jsonRequest = new
            {
                SalonId = model.SalonId,
                SalonAppIcon = model.SalonAppIcon,
                SalonName = model.SalonName,
                CustomerName = model.CustomerName,
                UserName = model.UserName,
                EmailAddress = model.EmailAddress,
                city = model.city,
                county = model.county,
                PostCode = model.PostCode,
                Address = model.Address,
                ContactTel = model.ContactTel,
                SalonWebsite = model.SalonWebsite,
                SalonTwitter = model.SalonTwitter,
                SalonFacebook = model.SalonFacebook,
                SalonInstagram = model.SalonInstagram,
                SalonOnStop = model.SalonOnStop,
                FtpFolderName = model.FtpFolderName,
                AccountExpiryDate = model.AccountExpiryDate.Replace("0000-00-00", ""),
                CountryCode = model.CountryCode,
                UpdatedBy = model.UpdatedBy,
                UpdatedOn = model.UpdatedOn,
                Currency = model.Currency,
                PaymentGatewaySettings = new List<PaymentGatewayData>(new[]
                {
                     new PaymentGatewayData
                     {
                         merchant_email = model.PaymentGatewaySettings[0].merchant_email
                     }
                }),
                //PaymentGatewaySettings = new List<PaymentGatewayData>(new[]
                //{
                //     new PaymentGatewayData
                //     {
                //         merchant_id = model.PaymentGatewaySettings[0].merchant_id,
                //         private_key = model.PaymentGatewaySettings[0].private_key,
                //         public_key = model.PaymentGatewaySettings[0].public_key,
                //          tokenization_key = model.PaymentGatewaySettings[0].tokenization_key
                //     }
                //}),
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //errLog.WinrtErrLogTest(serializedJsonRequest, "RegisterAdmin", "UpdateSalonServie");
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            SalonResponseModel resultObject = new SalonResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SalonResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SalonResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<ApiResult> DeleteSalon(string SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("DeleteSalon.php"); ;
            var jsonRequest = new
            {
                SalonId = SalonID
            };
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

        public async Task<SalonDetailsResponse> GetSalonDetailsByID(string SalonID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetSalonDetailsById.php"); ;
            var jsonRequest = new
            {
                SalonId = SalonID
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            SalonDetailsResponse resultObject = new SalonDetailsResponse();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SalonDetailsResponse>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SalonDetailsResponse()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<SalonListResponseModel> GetSalonList(string UserName)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetSalonList.php"); ;
            var jsonRequest = new
            {
                UserName = UserName
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            SalonListResponseModel resultObject = new SalonListResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<SalonListResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new SalonListResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<MappedSalonListResponseModel> GetUserSalonMapList(string UserID)
        {
            string CallAddressString = CommonEL.GetCallUrl("GetUserSalonMapList.php"); ;
            var jsonRequest = new
            {
                UserId = UserID
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);

            MappedSalonListResponseModel resultObject = new MappedSalonListResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<MappedSalonListResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new MappedSalonListResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<MapUserSalonResponseModel> MapSalonUser(MapUserSalon model)
        {
            string CallAddressString = CommonEL.GetCallUrl("MapUserSalon.php"); ;
            var jsonRequest = new
            {
                UserId = model.UserId,
                SalonId = model.SalonId,
                CreatedBy = model.CreatedBy
            };
            var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //errLog.WinrtErrLogTest(serializedJsonRequest, "RegisterAdmin", "MapSalonUserServie");
            string resultJson = await Webservices.PostService_RAW(CallAddressString, serializedJsonRequest);

            MapUserSalonResponseModel resultObject = new MapUserSalonResponseModel();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<MapUserSalonResponseModel>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new MapUserSalonResponseModel()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }
        public async Task<ApiResult> RemoveMapUserSalon(MapUserSalon model)
        {
            string CallAddressString = CommonEL.GetCallUrl("RemoveMapUserSalon.php"); ;
            var jsonRequest = new
            {
                UserId = model.UserId,
                SalonId = model.SalonId,
                UpdatedBy = model.UpdatedBy
            };
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
        public async Task<Countries> GetCountryList()
        {
            string CallAddressString = CommonEL.GetCallUrl("GetCountryList.php"); ;

            string resultJson = await Webservices.GetService(CallAddressString);

            Countries resultObject = new Countries();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<Countries>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new Countries()
                {
                    StatusCode = 201,
                    StatusMessage = ex.Message
                };
            }
            return resultObject;
        }

        public async Task<Currencies> GetCurrencyList()
        {
            string CallAddressString = CommonEL.GetCallUrl("GetCurrencyList.php");

            string resultJson = await Webservices.GetService(CallAddressString);

            Currencies resultObject = new Currencies();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = JsonConvert.DeserializeObject<Currencies>(resultJson);
                }
            }
            catch (Exception ex)
            {
                resultObject = new Currencies()
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
