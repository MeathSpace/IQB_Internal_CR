namespace IQB.IQBServices.HomeServices
{
    using IQB.Utility;
    using EntityLayer.Common;
    using EntityLayer.HomeModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeService : IDisposable
    {
        public async Task<ApiResult> UpdateCustomerRateSalon(RatingEL model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueuserrate.php");
            string QueryString = "username=" + model.username + "&ratesystem=" + model.ratesystem + "&ratingscore=" + model.ratingscore + "&salonid=" + model.salonid;
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
        public async Task<ApiResult> InsertCustomerRateSalon(RatingEL model)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqbuserrate.php");
            string QueryString = "personname=" + model.personname + "&barbername=" + model.salonname + "&daterated=" + model.daterated + "&rate=" + model.ratingscore + "&salonid=" + model.salonid;
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
        public async Task<ApiResult> InsertPushNotificationDevices(string username, string devicetoken, int salonid, string devicetype, string gcCode)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuedevicetoken.php");
            string QueryString = "UserName=" + username + "&token=" + devicetoken + "&salonid=" + salonid + "&type=" + devicetype + "&gcCode=" + gcCode;
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
        #region Home Services

        public async Task<HomeEL> GetHomeData(int userid, int salonid, string gjCode)
        {
            HomeEL result = new HomeEL();

            //Check for force update
            await CheckForForceupdateData(userid, salonid, gjCode);

            //Get other datas
            ApiResult objGetOtherData = await GetHomeQueueMiscDataApiModel(salonid);
            if (objGetOtherData != null && objGetOtherData.StatusCode == 200)
            {
                HomeQueueMiscDataApiModel otherData = UtilityEL.ToModel<HomeQueueMiscDataApiModel>(objGetOtherData.Response);

                if (otherData != null && otherData.id > 0)
                {
                    result.NextPositionAvailable = Convert.ToString(otherData.NAP);
                    result.Queuing = Convert.ToString(otherData.TotalQueue);
                    result.BarbersOnDuty = Convert.ToString(otherData.BarbersOnDuty);
                    result.EstimatedWaitTime = otherData.ExpTime;
                    result.SalonText = otherData.SalonText;
                    result.SystemStatus = (!string.IsNullOrWhiteSpace(otherData.SystemStatus) && otherData.SystemStatus.ToLower() == "online") ? true : false;
                }
                else
                {
                    result.SystemStatus = false;
                }
            }
            else
            {
                result.SystemStatus = false;
            }

            //Checking if user is in queue or not
            ApiResult objCheckIfUserJoinedQueueOrNot = await CheckIfUserJoinedQueueOrNot(userid, salonid);

            if (objCheckIfUserJoinedQueueOrNot != null && objCheckIfUserJoinedQueueOrNot.StatusCode == 200)
            {
                CheckJoinQueueApiResult checkData = UtilityEL.ToModel<CheckJoinQueueApiResult>(objCheckIfUserJoinedQueueOrNot.Response);

                if (checkData != null && checkData.id > 0)
                {
                    result.IsJoinedQueue = true;
                    result.gcCode = checkData.QGCode;

                    if (!string.IsNullOrWhiteSpace(checkData.QGCode) && checkData.QGCode.ToLower() != "n/a")
                    {
                        result.IsGroupJoined = true;

                        //Call api to get the list
                        ApiResult statListApi = await GetAllStatusList(userid, salonid, checkData.QGCode);

                        if (statListApi != null && statListApi.StatusCode == 200)
                        {
                            List<CheckJoinQueueApiResult> statData = UtilityEL.ToModelList<CheckJoinQueueApiResult>(statListApi.Response);

                            if (statData != null && statData.Count > 0)
                            {
                                foreach (CheckJoinQueueApiResult item in statData)
                                {
                                    QueueStatusListModel tempStatList = new QueueStatusListModel()
                                    {
                                        Barber = item.BarberName,
                                        Name = item.FirstLastName,
                                        SlNo = item.QPosition,
                                        UserName = item.Username
                                    };

                                    tempStatList.TimeData = await GetSelectBarberApiModel(salonid, item.QPosition, item.BarberName);

                                    result.QStatusList.Add(tempStatList);
                                }
                            }
                            else
                            {
                                result.IsGroupJoined = false;
                            }
                        }
                        else
                        {
                            result.IsGroupJoined = false;
                        }
                    }
                    else
                    {
                        result.IsGroupJoined = false;
                    }

                    if (!result.IsGroupJoined)
                    {
                        //Generate the list from current data
                        QueueStatusListModel tempStatList = new QueueStatusListModel()
                        {
                            Barber = checkData.BarberName,
                            Name = checkData.FirstLastName,
                            SlNo = checkData.QPosition,
                            UserName = checkData.Username
                        };

                        tempStatList.TimeData = await GetSelectBarberApiModel(salonid, checkData.QPosition, checkData.BarberName);

                        result.QStatusList.Add(tempStatList);
                    }
                }
                else
                {
                    result.IsJoinedQueue = false;
                    result.IsGroupJoined = false;
                }
            }
            else
            {
                result.IsJoinedQueue = false;
                result.IsGroupJoined = false;

                if (!string.IsNullOrWhiteSpace(gjCode) && gjCode.ToLower() != "n/a")
                {
                    //Call api to get the list
                    ApiResult statListApi = await GetAllStatusList(userid, salonid, gjCode);

                    if (statListApi != null && statListApi.StatusCode == 200)
                    {
                        List<CheckJoinQueueApiResult> statData = UtilityEL.ToModelList<CheckJoinQueueApiResult>(statListApi.Response);

                        if (statData != null && statData.Count > 0)
                        {
                            foreach (CheckJoinQueueApiResult item in statData)
                            {
                                QueueStatusListModel tempStatList = new QueueStatusListModel()
                                {
                                    Barber = item.BarberName,
                                    Name = item.FirstLastName,
                                    SlNo = item.QPosition,
                                    UserName = item.Username
                                };

                                tempStatList.TimeData = await GetSelectBarberApiModel(salonid, item.QPosition, item.BarberName);

                                result.QStatusList.Add(tempStatList);
                            }

                            result.IsGroupJoined = true;
                            result.gcCode = gjCode;
                            result.IsJoinedQueue = true;
                        }
                    }
                }
            }
            if (result.QStatusList != null && result.QStatusList.Count() > 0)
            {
                result.QStatusList = result.QStatusList.OrderBy(t => t.SlNo).ToList();
            }

            return result;
        }

        public async Task<ApiResult> GetHomeDataNew(int userid, int salonid, string gjCode, string token, string type, int barberId, int serviceId)
        {
            //    await errLog.WinrtErrLogTest("Task<ApiResult> GetHomeDataNew(");

            //    string CallAddressString = CommonEL.GetCallUrl("adminMergedRet2.php");//iqueueHomeScreenRet.php

            //    string QueryString = "username=" + Convert.ToString(userid) + "&salonid=" + Convert.ToString(salonid) + "&gcCode=" + gjCode.Replace("\\", "").Replace("\"", "") + "&token=" + token + "&type=" + type + "&barberId=" + barberId + "&serviceId=" + serviceId;
            //     await errLog.WinrtErrLogTest(QueryString);
            //     string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            //    //string resultJson = "{StatusCode:200,StatusMessage:Success,Response:{SalonText:,IsJoinedQueue:true,IsGroupJoined:false,gcCode:,SystemStatus:true,Queuing:5,EstimatedWaitTime:1:25 Mins,BarbersOnDuty:2,NextPositionAvailable:4,QueueJoinStat:2,ShowStatMessag:false,QStatusList:[{SlNo:3,UserName:14435,Name:Ady Jacinto,Barber:Brad,TimeData:00:55,IsChangedPosition:false}]}}";
            //    ApiResult resultObject = new ApiResult();
            //    try
            //    {
            //      //  var newResult = string.Empty;
            //        if (!string.IsNullOrWhiteSpace(resultJson))
            //        {
            //            //newResult = resultJson.Replace("\\", "").Replace("\"", "");
            //          //  await errLog.WinrtErrLogTest("if (!string.IsNullOrWhiteSpace(resultJson))");
            //            resultObject = UtilityEL.ToApiModel(resultJson);
            //            //resultObject = UtilityEL.ToApiModel(newResult);
            //            //resultObject.BarberID = 0;
            //            //resultObject.Response = null;
            //            //resultObject.StatusMessage = "";
            //            //resultObject.BarberID = 0;
            //            //resultObject.Cnt = 0;
            //        }
            //     //   await errLog.WinrtErrLogTest(newResult);
            //    }
            //    catch (Exception ex)
            //    {
            //        await errLog.WinrtErrLogTest("GetHomeData - Api -  catch (Exception ex)");
            //        resultObject = new ApiResult()
            //        {
            //            StatusCode = 201,
            //            StatusMessage = ex.Message
            //        };
            //    }

            //    return resultObject;
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}



            //string CallAddressString = CommonEL.GetCallUrl("adminMergedRet2.php");
            //var jsonRequest = new
            //{
            //    username = Convert.ToString(userid),
            //    salonid = Convert.ToString(salonid),
            //    gcCode = gjCode,
            //    token = token,
            //    type = type,
            //    barberId = barberId,
            //    serviceId = serviceId
            //};

            //var serializedJsonRequest = JsonConvert.SerializeObject(jsonRequest);
            //string resultJson = await Webservices.PostService(CallAddressString, serializedJsonRequest);

            string CallAddressString = CommonEL.GetCallUrl("adminMergedRet2.php");//iqueueHomeScreenRet.php
            string QueryString = "?username=" + Convert.ToString(userid) + "&salonid=" + Convert.ToString(salonid) + "&gcCode=" + gjCode + "&token=" + token + "&type=" + type + "&barberId=" + barberId + "&serviceId=" + serviceId;

            //string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            string resultJson = await Webservices.GetService(CallAddressString + QueryString);
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

        private async Task<ApiResult> CheckIfUserJoinedQueueOrNot(int userid, int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuecheckjoinedq.php");
            string QueryString = "username=" + userid + "&salonid=" + salonid;
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

        private async Task<ApiResult> GetAllStatusList(int userid, int salonid, string gcCode)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuecheckjoinedq.php");
            string QueryString = "username=" + userid + "&salonid=" + salonid + "&gcCode=" + gcCode;
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

        private async Task<ApiResult> GetHomeQueueMiscDataApiModel(int salonid)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueadminsettingsRet2.php");
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

        private async Task<string> GetSelectBarberApiModel(int salonid, int QPosition, string barbername)
        {
            string result = string.Empty;
            string CallAddressString = CommonEL.GetCallUrl("iqueuebarberselect.php");
            string QueryString = "salonid=" + salonid + "&barbername=" + barbername;
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            List<SelectBarberApiModel> resultObject = new List<SelectBarberApiModel>();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = (List<SelectBarberApiModel>)JsonConvert.DeserializeObject(resultJson, typeof(List<SelectBarberApiModel>));

                    if (resultObject != null && resultObject.Count > 0)
                    {
                        SelectBarberApiModel data = resultObject.FirstOrDefault();

                        decimal hours = Convert.ToDecimal((data.EWT * QPosition) / 60);
                        decimal Minutes = Convert.ToDecimal((data.EWT * QPosition));
                        Minutes = Minutes % 60;


                        int HourInt = Convert.ToInt32(hours);
                        int MinuteInt = Convert.ToInt32(Minutes);

                        result = ((HourInt < 10) ? "0" : "") + HourInt.ToString() + ":" + ((MinuteInt < 10) ? "0" : "") + MinuteInt.ToString();
                    }
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        public async Task<bool> DeleteQueue(int salonid, string checkUsername, string gcCode, bool IsGroupDelete)
        {
            bool result = false;
            string CallAddressString = CommonEL.GetCallUrl("iqueuedeletejoinedq.php");
            string QueryString = "salonid=" + salonid + "&checkUsername=" + checkUsername + "&Cancelling=Y";
            if (IsGroupDelete)
            {
                QueryString = "salonid=" + salonid + "&gcCode=" + gcCode + "&Cancelling=Y";
            }
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
            ApiResult resultObject = new ApiResult();
            try
            {
                if (!string.IsNullOrWhiteSpace(resultJson))
                {
                    resultObject = UtilityEL.ToApiModel(resultJson);

                    if (resultObject != null && resultObject.StatusCode == 200)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public async Task CheckForForceupdateData(int userid, int salonid, string gcCode)
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueuecheckforceupdate.php");
            string QueryString = "username=" + userid + "&salonid=" + salonid + "&gcCode=" + gcCode + "&Cancelling=Y";
            string resultJson = await Webservices.PostService(CallAddressString, QueryString);
        }


        public async Task<ApiResult> GetAdList()
        {
            string CallAddressString = CommonEL.GetCallUrl("iqueueselectad.php");
            string QueryString = "";
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

        #endregion

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}
