using System.Diagnostics;

namespace IQB.ViewModel.HomeManagement
{
    using AuthViewModel;
    using IQB.Utility;
    using Base;
    using EntityLayer.Common;
    using EntityLayer.HomeModels;
    using IQBServices;
    using IQBServices.HomeServices;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Views.MenuItems;
    using Xamarin.Auth;
    using Xamarin.Forms;

    public class HomeViewModel : BaseViewModel
    {
        public bool HasResultChangeInModal = false;
        public bool IsJoinedQueuePreviousState = false;
        public bool IsTimerRunning = false;
        public bool IsModalOpen = false;
        private int CurrentBannerId = 0;

        public HomeViewModel()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                ShowQueueNotJoinedSections = false;
                ShowQueueStatusList = false;
                ShowAllCancelButton = false;
                ShowInfo = false;
                ShowSystemStatus = false;
                IsCancelEnabled = true;
                IsQueueApiRunning = true;
                OnlineOfflineStat = "online.png";
                MainCancelImage = Color.FromHex("#F44336");
                OnlineOfflineStatText = "ON";
                PopUpServicesFrameVisibility = false;
                StatusBackGroundColor = "#11C02D";

                using (LoginViewModel obj = new LoginViewModel())
                {
                    UserId = obj.GetUserId();
                    errLog.WinrtErrLogTest(Convert.ToString(UserId));
                }

                using (SalonViewModel obj = new SalonViewModel())
                {
                    SalonId = obj.GetSalonId();
                }
                errLog.WinrtErrLogTest(Convert.ToString("SalonId = obj.GetSalonId()"));
                ClearGroupJoinData();

                this.SetQListTest(new List<QueueStatusListModel>());
                errLog.WinrtErrLogTest(Convert.ToString("SetQListTest(new List<QueueStatusListModel>()"));
                IsShowApiRunningLoader = true;
                errLog.WinrtErrLogTest(Convert.ToString("IsShowApiRunningLoader = true"));
                this.CallHomeData();
                errLog.WinrtErrLogTest(Convert.ToString("this.CallHomeData()"));
                //this.SetBanners();
                errLog.WinrtErrLogTest(Convert.ToString("this.SetBanners()"));
                MenuMaster.HomeTimerStat = true;
                errLog.WinrtErrLogTest(Convert.ToString("MenuMaster.HomeTimerStat = true"));
                Device.StartTimer(new TimeSpan(0, 0, 10), CallSetBanner);  //changed for build
                                                                           // errLog.WinrtErrLogTest(Convert.ToString("Device.StartTimer(new TimeSpan(0, 0, 10), CallSetBanner)"));
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
            //errLog.WinrtErrLogTest(Convert.ToString("HomeViewModel -- catch (Exception ex)"));
        }

        #region Methods

        public void SetQListTest(List<QueueStatusListModel> list)
        {
            try
            {
                if (QList != null && QList.Count() > 0)
                {
                    if (list != null && list.Count() > 0)
                    {
                        //Check if sort order has changed or not

                        bool IsSortOrderChanged = false;

                        for (int i = 0; i < QList.Count(); i++)
                        {
                            if (QList[i].QSId != "#" && QList[i].QSId != "")
                            {
                                if (list.Where(t => t.UserName == QList[i].QSUserName).Count() > 0)
                                {
                                    QueueStatusListModel temp = list.Where(t => t.UserName == QList[i].QSUserName).FirstOrDefault();

                                    if (temp.SlNo != Convert.ToInt32(QList[i].QSId))
                                    {
                                        IsSortOrderChanged = true;
                                    }
                                }
                            }

                            if (IsSortOrderChanged)
                            {
                                break;
                            }
                        }

                        if (!IsSortOrderChanged)
                        {
                            ObservableCollection<HomeQueueStatusListModel> removeList = new ObservableCollection<HomeQueueStatusListModel>();
                            List<string> modifiedUserNames = new List<string>();
                            for (int i = 0; i < QList.Count(); i++)
                            {
                                if (QList[i].QSId != "#")
                                {
                                    if (list.Where(t => t.UserName == QList[i].QSUserName).Count() > 0)
                                    {
                                        QueueStatusListModel temp = list.Where(t => t.UserName == QList[i].QSUserName).FirstOrDefault();
                                        modifiedUserNames.Add(temp.UserName);

                                        QList[i].SQIsCancelVisible = true;
                                        QList[i].QSBarber = temp.Barber;
                                        QList[i].QSName = temp.Name;
                                        //QList[i].QSTime = temp.TimeData;
                                        QList[i].QSTime = ConvertToTimeFromNumber(temp.TimeData);
                                        QList[i].QSId = Convert.ToString(temp.SlNo);
                                        QList[i].QSUserName = temp.UserName;
                                        QList[i].CancelImage = Color.FromHex("#F44336");
                                        QList[i].RedArrowImage = "red_arrow.png";
                                    }
                                    else
                                    {
                                        removeList.Add(QList[i]);
                                    }
                                }
                            }

                            if (removeList != null && removeList.Count() > 0)
                            {
                                foreach (HomeQueueStatusListModel item in removeList)
                                {
                                    QList.Remove(item);
                                }
                            }

                            if (modifiedUserNames != null && modifiedUserNames.Count() > 0)
                            {
                                if (list.Where(t => !modifiedUserNames.Contains(t.UserName)).Count() > 0)
                                {
                                    foreach (QueueStatusListModel item in list.Where(t => !modifiedUserNames.Contains(t.UserName)).ToList())
                                    {
                                        QList.Add(new HomeQueueStatusListModel()
                                        {
                                            SQIsCancelVisible = true,
                                            QSBarber = item.Barber,
                                            QSName = item.Name,
                                            //QSTime = item.TimeData,
                                            QSTime = ConvertToTimeFromNumber(item.TimeData),
                                            QSId = Convert.ToString(item.SlNo),
                                            QSUserName = item.UserName,
                                            CancelImage = Color.FromHex("#F44336"),
                                            RedArrowImage = "red_arrow.png"
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (QueueStatusListModel item in list.OrderBy(t => t.SlNo).ToList())
                                {
                                    QList.Add(new HomeQueueStatusListModel()
                                    {
                                        SQIsCancelVisible = true,
                                        QSBarber = item.Barber,
                                        QSName = item.Name,
                                        // QSTime = item.TimeData,
                                        QSTime = ConvertToTimeFromNumber(item.TimeData),
                                        QSId = Convert.ToString(item.SlNo),
                                        QSUserName = item.UserName,
                                        CancelImage = Color.FromHex("#F44336"),
                                        RedArrowImage = "red_arrow.png"
                                    });
                                }
                            }

                            //Set the order

                            //List<QueueStatusListModel> tempList = new List<QueueStatusListModel>();
                            //for (int i = 0; i < QList.Count; i++)
                            //{
                            //    if (QList[i].QSId != "#")
                            //    {
                            //        tempList.Add(new QueueStatusListModel()
                            //        {
                            //            Barber = QList[i].QSBarber,
                            //            Name = QList[i].QSName,
                            //            SlNo = Convert.ToInt32(QList[i].QSId),
                            //            TimeData = QList[i].QSTime,
                            //            UserName = QList[i].QSUserName
                            //        });
                            //    }
                            //}

                            //tempList = tempList.OrderBy(t => t.SlNo).ToList();

                            //for (int i = 0; i < tempList.Count; i++)
                            //{
                            //    QList[i + 1].QSBarber = tempList[i].Barber;
                            //    QList[i + 1].QSId = Convert.ToString(tempList[i].SlNo);
                            //    QList[i + 1].QSName = tempList[i].Name;
                            //    QList[i + 1].QSTime = tempList[i].TimeData;
                            //    QList[i + 1].QSUserName = tempList[i].UserName;
                            //}
                        }
                        else
                        {
                            ObservableCollection<HomeQueueStatusListModel> removeList = new ObservableCollection<HomeQueueStatusListModel>();
                            for (int i = 0; i < QList.Count(); i++)
                            {
                                if (QList[i].QSId != "#")
                                {
                                    removeList.Add(QList[i]);
                                }
                            }

                            if (removeList != null && removeList.Count() > 0)
                            {
                                foreach (HomeQueueStatusListModel item in removeList)
                                {
                                    QList.Remove(item);
                                }
                            }

                            foreach (QueueStatusListModel item in list.OrderBy(t => t.SlNo).ToList())
                            {
                                QList.Add(new HomeQueueStatusListModel()
                                {
                                    SQIsCancelVisible = true,
                                    QSBarber = item.Barber,
                                    QSName = item.Name,
                                    //QSTime = item.TimeData,
                                    QSTime = ConvertToTimeFromNumber(item.TimeData),
                                    QSId = Convert.ToString(item.SlNo),
                                    QSUserName = item.UserName,
                                    CancelImage = Color.FromHex("#F44336"),
                                    RedArrowImage = "red_arrow.png"
                                });
                            }
                        }
                    }
                    else
                    {
                        ObservableCollection<HomeQueueStatusListModel> removeList = new ObservableCollection<HomeQueueStatusListModel>();
                        for (int i = 0; i < QList.Count(); i++)
                        {
                            if (QList[i].QSId != "#")
                            {
                                removeList.Add(QList[i]);
                            }
                        }

                        if (removeList != null && removeList.Count() > 0)
                        {
                            foreach (HomeQueueStatusListModel item in removeList)
                            {
                                QList.Remove(item);
                            }
                        }

                        //QList = new ObservableCollection<HomeQueueStatusListModel>();
                        //QList.Add(new HomeQueueStatusListModel() { SQIsCancelVisible = false, QSBarber = "Barber", QSName = "Name", QSTime = "Time(hh:mm)", QSId = "#", CancelImage = "cancel.png" });
                    }
                }
                else
                {
                    QList = new ObservableCollection<HomeQueueStatusListModel>();
                    //           QList.Add(new HomeQueueStatusListModel() { SQIsCancelVisible = false, QSBarber = "Barber", QSName = "Name", QSTime = "Time(hh:mm)", QSId = "#", CancelImage = "cancel.png", MakeHeaderBold = "Bold" });

                    if (list != null && list.Count > 0)
                    {
                        foreach (QueueStatusListModel item in list.OrderBy(t => t.SlNo).ToList())
                        {
                            QList.Add(new HomeQueueStatusListModel()
                            {
                                SQIsCancelVisible = true,
                                QSBarber = item.Barber,
                                QSName = item.Name,
                                // QSTime = item.TimeData,
                                QSTime = ConvertToTimeFromNumber(item.TimeData),
                                QSId = Convert.ToString(item.SlNo),
                                QSUserName = item.UserName,
                                CancelImage = Color.FromHex("#F44336"),
                                RedArrowImage = "red_arrow.png"
                            });
                        }
                    }
                }







                //if (list != null && list.Count > 0)
                //{
                //    QList = new ObservableCollection<HomeQueueStatusListModel>();
                //    QList.Add(new HomeQueueStatusListModel() { SQIsCancelVisible = false, QSBarber = "Barber", QSName = "Name", QSTime = "Time(hh:mm)", QSId = "#", CancelImage = "cancel.png" });

                //    //if (QList != null && QList.Count() > 0)
                //    //{
                //    //    for (int i = 0; i < QList.Count(); i++)
                //    //    {
                //    //        if (QList[i].QSId != "#")
                //    //        {
                //    //            if (list.Where(t => t.UserName == QList[i].QSUserName).Count() > 0)
                //    //            {
                //    //                QueueStatusListModel temp = list.Where(t => t.UserName == QList[i].QSUserName).FirstOrDefault();
                //    //                if(QList[i].QSId != Convert.ToString(temp.SlNo))
                //    //                {
                //    //                    App.Current.MainPage.DisplayAlert("Queue postion changed", QList[i].QSName + "'(s) Queue position has been changed to " + Convert.ToString(temp.SlNo), "OK");
                //    //                }
                //    //            }
                //    //        }
                //    //    }
                //    //}

                //    foreach (QueueStatusListModel item in list.OrderBy(t => t.SlNo).ToList())
                //    {
                //        QList.Add(new HomeQueueStatusListModel()
                //        {
                //            SQIsCancelVisible = true,
                //            QSBarber = item.Barber,
                //            QSName = item.Name,
                //            QSTime = item.TimeData,
                //            QSId = Convert.ToString(item.SlNo),
                //            QSUserName = item.UserName,
                //            CancelImage = "cancel.png"
                //        });
                //    }
                //}
                //else
                //{
                //    QList = new ObservableCollection<HomeQueueStatusListModel>();
                //    QList.Add(new HomeQueueStatusListModel() { SQIsCancelVisible = false, QSBarber = "Barber", QSName = "Name", QSTime = "Time(hh:mm)", QSId = "#", CancelImage = "cancel.png" });
                //}

                //Message to show queue position change
                if (list != null && list.Where(t => t.IsChangedPosition == true).Count() > 0)
                {
                    foreach (QueueStatusListModel item in list.Where(t => t.IsChangedPosition == true).OrderBy(t => t.SlNo).ToList())
                    {
                        if (MenuMaster.HomeTimerStat)
                        {
                            //Device.BeginInvokeOnMainThread(() =>
                            //{
                            //    App.Current.MainPage.DisplayAlert("Q-Position Changed!", item.Name + "('s) queue position has been changed to " + item.SlNo, "Ok");
                            //});
                            if (item?.SlNo != null && !string.IsNullOrEmpty(item?.Name))
                                App.Current.MainPage.DisplayAlert("Q-Position Changed!", item.Name + "('s) queue position has been changed to " + item.SlNo, "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SetInfo(string info)
        {
            this.Info = info;
            PInfo = info;
            MoreText = string.Empty;
            if (info?.Length > 100 && !IsMoreTapped)
            {
                PInfo = $"{info?.Substring(0, 100)}...";
                MoreText = " + ";
            }
            if(info?.Length > 100 && IsMoreTapped)
            {
                MoreText = " - ";
            }
        }

        public async Task SetHomeData(bool IsHideViews, int serviceID, int barberID)
        {
            if (IsHideViews)
            {
                //ShowQueueNotJoinedSections = false;
                //ShowQueueStatusList = false;
                //ShowAllCancelButton = false;
                //ShowInfo = false;
                //ShowSystemStatus = false;
            }
            //if (NetworkConnectionMgmt.IsConnectedToNetwork())
            //{
            //    IsShowApiRunningLoader = true;
            //}


            IsQueueApiRunning = true;
            IsCancelEnabled = false;

            if (QList != null && QList.Count() > 0)
            {
                for (int i = 0; i < QList.Count(); i++)
                {
                    QList[i].CancelImage = Color.FromHex("#878787");
                }
            }

            MainCancelImage = Color.FromHex("#878787");


            HomeEL result = new HomeEL();

            using (HomeService obj = new HomeService())
            {
                string devicetype = string.Empty;
                if (Device.OS == TargetPlatform.Android)
                {
                    devicetype = "android";
                }
                else
                {
                    devicetype = "iOS";
                }

                string DeviceToken = string.Empty;
                if (!string.IsNullOrWhiteSpace(App.DeviceToken))
                {
                    DeviceToken = App.DeviceToken;
                }

                //result = await obj.GetHomeData(UserId, SalonId, GetStoredGroupCode());


                var gcCodeObtained = GetStoredGroupCode();
                //ApiResult apiRes = Task.Run(async () => await obj.GetHomeDataNew(UserId, SalonId, gcCode,DeviceToken, devicetype, barberID, serviceID)).Result;
                ApiResult apiRes = await obj.GetHomeDataNew(UserId, SalonId, gcCodeObtained, DeviceToken, devicetype, barberID, serviceID);
                if (apiRes != null && apiRes.StatusCode == 200)
                {
                    result = UtilityEL.ToModelHome(apiRes.Response);

                    if (result == null)
                    {
                        result = new HomeEL();
                    }
                }
                else
                {
                    result = new HomeEL();
                }
            }
            //if(result.SalonText!="")
            if (!string.IsNullOrEmpty(result.SalonText))
            {
                Application.Current.Properties["SalonText"] = result.SalonText;
            }
            else
            {
                Application.Current.Properties["SalonText"] = "";
            }


            //Waiting for join process to complete
            if (result.QueueJoinStat == 1)
            {
                result.QStatusList = new List<QueueStatusListModel>();
                IsWaitingToJoinQueue = true;
            }
            else
            {
                IsWaitingToJoinQueue = false;
            }

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    SetQListTest(result.QStatusList);
            //});
            SetQListTest(result.QStatusList);
            SetInfo(result.SalonText);

            Queuing = result.Queuing;
            EstimatedWaitTime = result.EstimatedWaitTime;
            BarbersOnDuty = result.BarbersOnDuty;
            NextPositionAvailable = result.NextPositionAvailable;
            gcCode = result.gcCode;
            IsJoinedQueue = result.IsJoinedQueue;

            if (!string.IsNullOrWhiteSpace(gcCode))
            {
                SaveGroupJoinedData(gcCode);
            }
            else
            {
                DeleteGroupJoinedData();
            }

            if (result.SystemStatus)
            {
                OnlineOfflineStat = "online.png";
                OnlineOfflineStatText = "ON";
                StatusBackGroundColor = "#11C02D";
            }
            else
            {
                OnlineOfflineStat = "offline.png";
                OnlineOfflineStatText = "OFF";
                StatusBackGroundColor = "#F44336";
            }


            if (!result.SystemStatus || result.IsJoinedQueue || (result.QueueJoinStat == 1))
            {
                PopUpServicesFrameVisibility = false;
            }

            if (result.SystemStatus && !result.IsJoinedQueue)
            {
                PopUpServicesFrameVisibility = true;
            }

            if (result.IsJoinedQueue)
            {
                ShowQueueNotJoinedSections = false;

                if (IsWaitingToJoinQueue)
                {
                    ShowQueueStatusList = false;
                }
                else
                {
                    ShowQueueStatusList = true;
                }
            }
            else
            {
                if (result.SystemStatus)
                {
                    ShowQueueNotJoinedSections = true;
                }
                else
                {
                    ShowQueueNotJoinedSections = false;
                }
                ShowQueueStatusList = false;
            }



            if (result.IsGroupJoined)
            {
                ShowAllCancelButton = true;
            }
            else
            {
                ShowAllCancelButton = false;
            }

            if (!string.IsNullOrWhiteSpace(result.SalonText))
            {
                ShowInfo = true;
            }
            else
            {
                ShowInfo = false;
            }

            ShowSystemStatus = true;
            IsCancelEnabled = true;
            IsQueueApiRunning = false;
            IsShowApiRunningLoader = false;

            MainCancelImage = Color.FromHex("#F44336");
            //if (result.IsJoinedQueue && !string.IsNullOrWhiteSpace(App.DeviceToken))
            //{
            //    using (HomeService obj = new HomeService())
            //    {
            //        string devicetype = string.Empty;
            //        if (Device.OS == TargetPlatform.Android)
            //        {
            //            devicetype = "android";
            //        }
            //        else
            //        {
            //            devicetype = "iOS";
            //        }
            //        await obj.InsertPushNotificationDevices(Convert.ToString(UserId), App.DeviceToken, SalonId, devicetype, gcCode);
            //    }
            //}

            //Show joinqueue message if applicable
            if (result.ShowStatMessag && App.IsApplicationAwake && MenuMaster.HomeTimerStat)
            {
                if (result.QueueJoinStat == 2)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    App.Current.MainPage.DisplayAlert("Success!", "You have joined the queue successfully!", "Ok");
                    //});

             //       App.Current.MainPage.DisplayAlert("Success!", "You have joined the queue successfully!", "Ok");
                }
                else if (result.QueueJoinStat == 3)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    App.Current.MainPage.DisplayAlert("Failed!", "Sorry! Couldn't join the queue at the moment, please try again later.", "Ok");
                    //});

                    App.Current.MainPage.DisplayAlert("Failed!", "Sorry! Couldn't join the queue at the moment, please try again later.", "Ok");
                }
            }

            if (!IsTimerRunning)
            {
                SetHomeDataInterval();
            }
        }

        public bool CallSetBanner()
        {
            if (MenuMaster.HomeTimerStat && App.IsApplicationAwake)
            {
                SetBanners();
            }
            return MenuMaster.HomeTimerStat;
        }

        public async void CallHomeData()
        {
            await SetHomeData(false, 0, 0);
        }

        public async void SetBanners()
        {
            ApiResult apiResult = new ApiResult();

            using (HomeService obj = new HomeService())
            {
                apiResult = await obj.GetAdList();
            }

            if (apiResult != null && apiResult.StatusCode == 200)
            {
                List<AdModelAPI> result = UtilityEL.ToModelList<AdModelAPI>(apiResult.Response);

                if (result != null && result.Count() > 0)
                {
                    AdModelAPI temp = new AdModelAPI();
                    if (result.Where(t => t.id > CurrentBannerId).Count() > 0)
                    {
                        temp = result.Where(t => t.id > CurrentBannerId).FirstOrDefault();
                    }
                    else
                    {
                        temp = result.FirstOrDefault();
                    }

                    if (!string.IsNullOrWhiteSpace(temp.PicName))
                    {
                        CurrentBannerId = temp.id;
                        AdImage = ImageSource.FromUri(new Uri(CommonEL.AdLocation + temp.PicName));
                        ShowAd = true;
                        AdImageLinkURL = temp.Url;
                    }
                    else
                    {
                        ShowAd = false;
                    }
                }
                else
                {
                    CurrentBannerId = 0;
                    ShowAd = false;
                }
            }
            else
            {
                ShowAd = false;
            }
        }

        //public bool SetHomeDataInterval()
        //{
        //    if (MenuMaster.HomeTimerStat)
        //    {
        //        //if(IsJoinedQueuePreviousState == IsJoinedQueue)
        //        //{
        //        //    if (!IsTimerRunning)
        //        //    {
        //        //        if (IsJoinedQueue)
        //        //        {
        //        //            Device.StartTimer(TimeSpan.FromSeconds(5), SetHomeDataInterval);
        //        //        }
        //        //        else
        //        //        {
        //        //            Device.StartTimer(TimeSpan.FromSeconds(10), SetHomeDataInterval);
        //        //        }
        //        //        IsTimerRunning = true;
        //        //    }
        //        //    else
        //        //    {
        //        //        if (!IsQueueApiRunning && !IsModalOpen && App.IsApplicationAwake)
        //        //        {
        //        //            SetHomeData();
        //        //        }
        //        //    }
        //        //    return false;
        //        //}
        //        //else
        //        //{
        //        //    IsJoinedQueuePreviousState = IsJoinedQueue;
        //        //    IsTimerRunning = true;
        //        //    if (IsJoinedQueue)
        //        //    {
        //        //        Device.StartTimer(TimeSpan.FromSeconds(5), SetHomeDataInterval);
        //        //    }
        //        //    else
        //        //    {
        //        //        Device.StartTimer(TimeSpan.FromSeconds(10), SetHomeDataInterval);
        //        //    }
        //        //    return false;
        //        //}

        //        IsTimerRunning = true;

        //        if (IsJoinedQueue)
        //        {
        //            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        //            {
        //                Task.Factory.StartNew(async () =>
        //                {
        //                    // Do the actual request and wait for it to finish.

        //                    if (!IsQueueApiRunning && !IsModalOpen && App.IsApplicationAwake)
        //                    {
        //                        await SetHomeData(false);
        //                    }
        //                    // Switch back to the UI thread to update the UI
        //                    Device.BeginInvokeOnMainThread(() =>
        //                    {
        //                        // Now repeat by scheduling a new request
        //                        SetHomeDataInterval();
        //                    });
        //                });

        //                // Don't repeat the timer (we will start a new timer when the request is finished)
        //                return false;
        //            });
        //        }
        //        else
        //        {
        //            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
        //            {
        //                Task.Factory.StartNew(async () =>
        //                {
        //                    // Do the actual request and wait for it to finish.
        //                    if (!IsQueueApiRunning && !IsModalOpen && App.IsApplicationAwake)
        //                    {
        //                        await SetHomeData(false);
        //                    }
        //                    // Switch back to the UI thread to update the UI
        //                    Device.BeginInvokeOnMainThread(() =>
        //                    {
        //                        // Now repeat by scheduling a new request
        //                        SetHomeDataInterval();
        //                    });
        //                });

        //                // Don't repeat the timer (we will start a new timer when the request is finished)
        //                return false;
        //            });
        //        }
        //    }
        //    return false;
        //}

        public bool SetHomeDataInterval()
        {
            if (MenuMaster.HomeTimerStat)
            {
                IsTimerRunning = true;

                if (IsJoinedQueue)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            // Do the actual request and wait for it to finish.

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (!IsQueueApiRunning && !IsModalOpen && App.IsApplicationAwake)
                                {
                                    await SetHomeData(false, 0, 0);
                                }
                                SetHomeDataInterval();
                            });
                        });

                        // Don't repeat the timer (we will start a new timer when the request is finished)
                        return false;
                    });
                }
                else
                {
                    Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            // Do the actual request and wait for it to finish.
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (!IsQueueApiRunning && !IsModalOpen && App.IsApplicationAwake)
                                {
                                    await SetHomeData(false, 0, 0);
                                }
                                SetHomeDataInterval();
                            });
                        });

                        // Don't repeat the timer (we will start a new timer when the request is finished)
                        return false;
                    });
                }
            }
            return false;
        }

        private void SaveGroupJoinedData(string gjCode)
        {
            Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName).FirstOrDefault();

            if (grpJoinedAccount != null)
            {
                AccountStore.Create().Delete(grpJoinedAccount, CommonEL.GroupJoinServiceName);
            }

            Account account = new Account(gjCode);
            account.Properties.Add("UserId", Convert.ToString(UserId));
            AccountStore.Create().Save(account, CommonEL.GroupJoinServiceName);
        }

        private void DeleteGroupJoinedData()
        {
            Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName).FirstOrDefault();

            if (grpJoinedAccount != null)
            {
                AccountStore.Create().Delete(grpJoinedAccount, CommonEL.GroupJoinServiceName);
            }
        }

        private void ClearGroupJoinData()
        {
            Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName).FirstOrDefault();
            if (grpJoinedAccount != null)
            {
                int StoredUserId = Convert.ToInt32(grpJoinedAccount.Properties["UserId"]);

                if (StoredUserId != UserId)
                {
                    AccountStore.Create().Delete(grpJoinedAccount, CommonEL.GroupJoinServiceName);
                }
            }
        }

        private string GetStoredGroupCode()
        {
            string result = string.Empty;

            Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName).FirstOrDefault();

            if (grpJoinedAccount != null)
            {
                result = grpJoinedAccount.Username;
            }

            return result;
        }

        public string ConvertToTimeFromNumber(string number)
        {
            string hh, mm, ss, result = string.Empty;
            var timeSpan = TimeSpan.Zero;
            if (number != "0" && number != "00:00" & number != "0:0")
            {
                // var timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(number));
                TimeSpan.TryParse(number, out timeSpan);
                //hh = Convert.ToString(timeSpan.Hours);
                //mm = Convert.ToString(timeSpan.Minutes);
                //ss = Convert.ToString(timeSpan.Seconds);
                //result = hh + ":" + mm;
                result = timeSpan.ToString(@"hh\:mm");
            }
            else
            {
                result = "Next";
            }
            return result;
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        #endregion

        #region Controls

        private string _SelectBarberName;

        public string SelectBarberName
        {
            get { return _SelectBarberName; }
            set { _SelectBarberName = value; OnPropertyChanged("SelectBarberName"); }
        }



        private string m_SelectBarberImage;

        public string SelectBarberImage
        {
            get { return m_SelectBarberImage; }
            set { m_SelectBarberImage = value; OnPropertyChanged("SelectBarberImage"); }
        }


        private int _SelectBarberID;

        public int SelectBarberID
        {
            get { return _SelectBarberID; }
            set { _SelectBarberID = value; OnPropertyChanged("SelectBarberID"); }
        }

        private string _AutojoinEstimatedWaitTime;

        public string AutojoinEstimatedWaitTime
        {
            get { return _AutojoinEstimatedWaitTime; }
            set { _AutojoinEstimatedWaitTime = value; OnPropertyChanged("AutojoinEstimatedWaitTime"); }
        }


        private bool m_IsJoinedQueue;

        public bool IsJoinedQueue
        {
            get { return m_IsJoinedQueue; }
            set { m_IsJoinedQueue = value; OnPropertyChanged("IsJoinedQueue"); }
        }

        private bool m_ShowQueueNotJoinedSections;

        public bool ShowQueueNotJoinedSections
        {
            get { return m_ShowQueueNotJoinedSections; }
            set { m_ShowQueueNotJoinedSections = value; OnPropertyChanged("ShowQueueNotJoinedSections"); }
        }

        private bool m_ShowQueueStatusList;

        public bool ShowQueueStatusList
        {
            get { return m_ShowQueueStatusList; }
            set { m_ShowQueueStatusList = value; OnPropertyChanged("ShowQueueStatusList"); }
        }

        private bool m_ShowAllCancelButton;

        public bool ShowAllCancelButton
        {
            get { return m_ShowAllCancelButton; }
            set { m_ShowAllCancelButton = value; OnPropertyChanged("ShowAllCancelButton"); }
        }

        private bool m_ShowInfo;

        public bool ShowInfo
        {
            get { return m_ShowInfo; }
            set { m_ShowInfo = value; OnPropertyChanged("ShowInfo"); }
        }

        private bool m_IsQueueApiRunning;

        public bool IsQueueApiRunning
        {
            get { return m_IsQueueApiRunning; }
            set { m_IsQueueApiRunning = value; OnPropertyChanged("IsQueueApiRunning"); }
        }


        private bool m_IsShowApiRunningLoader;

        public bool IsShowApiRunningLoader
        {
            get { return m_IsShowApiRunningLoader; }
            set { m_IsShowApiRunningLoader = value; OnPropertyChanged("IsShowApiRunningLoader"); }
        }

        private bool m_IsWaitingToJoinQueue;

        public bool IsWaitingToJoinQueue
        {
            get { return m_IsWaitingToJoinQueue; }
            set { m_IsWaitingToJoinQueue = value; OnPropertyChanged("IsWaitingToJoinQueue"); }
        }

        private bool m_ShowSystemStatus;

        public bool ShowSystemStatus
        {
            get { return m_ShowSystemStatus; }
            set { m_ShowSystemStatus = value; OnPropertyChanged("ShowSystemStatus"); }
        }

        private bool m_IsStatusListRefreshing;

        public bool IsStatusListRefreshing
        {
            get { return m_IsStatusListRefreshing; }
            set { m_IsStatusListRefreshing = value; OnPropertyChanged("IsStatusListRefreshing"); }
        }

        private bool m_IsCancelEnabled;

        public bool IsCancelEnabled
        {
            get { return m_IsCancelEnabled; }
            set { m_IsCancelEnabled = value; OnPropertyChanged("IsCancelEnabled"); }
        }


        private bool m_PopUpServicesFrameVisibility;
        public bool PopUpServicesFrameVisibility
        {
            get { return m_PopUpServicesFrameVisibility; }
            set { m_PopUpServicesFrameVisibility = value; OnPropertyChanged("PopUpServicesFrameVisibility"); }
        }



        private bool m_ShowAd;

        public bool ShowAd
        {
            get { return m_ShowAd; }
            set { m_ShowAd = value; OnPropertyChanged("ShowAd"); }
        }

        private ImageSource m_OnlineOfflineStat;

        public ImageSource OnlineOfflineStat
        {
            get { return m_OnlineOfflineStat; }
            set { m_OnlineOfflineStat = value; OnPropertyChanged("OnlineOfflineStat"); }
        }



        private string m_OnlineOfflineStatText;

        public string OnlineOfflineStatText
        {
            get { return m_OnlineOfflineStatText; }
            set { m_OnlineOfflineStatText = value; OnPropertyChanged("OnlineOfflineStatText"); }
        }


        private string m_StatusBackGroundColor;

        public string StatusBackGroundColor
        {
            get { return m_StatusBackGroundColor; }
            set { m_StatusBackGroundColor = value; OnPropertyChanged("StatusBackGroundColor"); }
        }

        private ImageSource m_AdImage;

        public ImageSource AdImage
        {
            get { return m_AdImage; }
            set { m_AdImage = value; OnPropertyChanged("AdImage"); }
        }

        private string m_AdImageLinkURL;

        public string AdImageLinkURL
        {
            get { return m_AdImageLinkURL; }
            set { m_AdImageLinkURL = value; OnPropertyChanged("AdImageLinkURL"); }
        }

        private string m_Queuing;

        public string Queuing
        {
            get { return m_Queuing; }
            set { m_Queuing = value; OnPropertyChanged("Queuing"); }
        }

        private string m_EstimatedWaitTime;

        public string EstimatedWaitTime
        {
            get { return m_EstimatedWaitTime; }
            set { m_EstimatedWaitTime = value; OnPropertyChanged("EstimatedWaitTime"); }
        }

        private string m_BarbersOnDuty;

        public string BarbersOnDuty
        {
            get { return m_BarbersOnDuty; }
            set { m_BarbersOnDuty = value; OnPropertyChanged("BarbersOnDuty"); }
        }

        private string m_NextPositionAvailable;

        public string NextPositionAvailable
        {
            get { return m_NextPositionAvailable; }
            set { m_NextPositionAvailable = value; OnPropertyChanged("NextPositionAvailable"); }
        }

        private string m_Info;

        public string Info
        {
            get { return m_Info; }
            set { m_Info = value; OnPropertyChanged("Info"); }
        }
        
        private bool _IsMoreTapped;

        public bool IsMoreTapped
        {
            get { return _IsMoreTapped; }
            set { _IsMoreTapped = value; OnPropertyChanged("IsMoreTapped"); }
        }

        private string m_PInfo;

        public string PInfo
        {
            get { return m_PInfo; }
            set { m_PInfo = value; OnPropertyChanged("PInfo"); }
        }
        private string _moreText;

        public string MoreText
        {
            get { return _moreText; }
            set { _moreText = value; OnPropertyChanged("MoreText"); }
        }
        private ObservableCollection<HomeQueueStatusListModel> m_QList;

        public ObservableCollection<HomeQueueStatusListModel> QList
        {
            get { return m_QList; }
            set { m_QList = value; OnPropertyChanged("QList"); }
        }

        private int m_UserId;

        public int UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; OnPropertyChanged("UserId"); }
        }

        private int m_SalonId;

        public int SalonId
        {
            get { return m_SalonId; }
            set { m_SalonId = value; OnPropertyChanged("SalonId"); }
        }

        private int m_ServiceId;

        public int ServiceId
        {
            get { return m_ServiceId; }
            set { m_ServiceId = value; OnPropertyChanged("ServiceId"); }
        }



        private string m_gcCode;

        public string gcCode
        {
            get { return m_gcCode; }
            set { m_gcCode = value; OnPropertyChanged("gcCode"); }
        }

        private Color m_MainCancelImage;
        public Color MainCancelImage
        {
            get { return m_MainCancelImage; }
            set { m_MainCancelImage = value; OnPropertyChanged("MainCancelImage"); }
        }

        #endregion

        #region Commands

        private Command m_CancelCommand;
        public ICommand CancelCommand
        {
            get { return m_CancelCommand ?? (m_CancelCommand = new Command(OnCancelTapped)); }
        }

        private Command m_GroupCancelCommand;
        public ICommand GroupCancelCommand
        {
            get { return m_GroupCancelCommand ?? (m_GroupCancelCommand = new Command(OnGroupCancelTapped)); }
        }

        public async void OnCancelTapped(object s)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (IsCancelEnabled && !IsModalOpen)
                {
                    //IsStatusListRefreshing = true;
                    IsShowApiRunningLoader = true;
                    IsCancelEnabled = false;
                    IsModalOpen = true;

                    string username = Convert.ToString(s);

                    string ClientName = QList.Where(t => t.QSUserName == username).FirstOrDefault().QSName;

                    var accept = await App.Current.MainPage.DisplayAlert("Delete!", "Are you sure you want to cancel booking of " + ClientName + "?", "Yes", "No");

                    if (accept)
                    {
                        bool stat = false;

                        using (HomeService obj = new HomeService())
                        {
                            stat = await obj.DeleteQueue(SalonId, username, "", false);
                        }

                        if (stat)
                        {
                            await App.Current.MainPage.DisplayAlert("Success", "Booking cancelled successfully.", "Ok");
                            await SetHomeData(true, 0, 0);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error!", "Failed cancel booking", "Ok");
                        }
                    }

                    IsShowApiRunningLoader = false;
                    IsCancelEnabled = true;
                    IsStatusListRefreshing = false;
                    IsModalOpen = false;
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        public async void OnGroupCancelTapped()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (IsCancelEnabled && !IsModalOpen)
                {
                    //IsStatusListRefreshing = true;
                    IsShowApiRunningLoader = true;
                    IsCancelEnabled = false;
                    IsModalOpen = true;

                    var accept = await App.Current.MainPage.DisplayAlert("Delete!", "Are you sure you want cancel all booking?", "Yes", "No");

                    if (accept)
                    {
                        bool stat = false;

                        using (HomeService obj = new HomeService())
                        {
                            stat = await obj.DeleteQueue(SalonId, "", gcCode, true);
                        }

                        if (stat)
                        {
                            await App.Current.MainPage.DisplayAlert("Success", "All booking cancelled successfully.", "Ok");
                            await SetHomeData(true, 0, 0);
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error!", "Failed to cancel all booking", "Ok");
                        }
                    }

                    IsShowApiRunningLoader = false;
                    IsCancelEnabled = true;
                    IsStatusListRefreshing = false;
                    IsModalOpen = false;
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        #endregion
    }

    public class HomeQueueStatusListModel : BaseViewModel
    {
        private string m_QSId;
        public string QSId
        {
            get { return m_QSId; }
            set { m_QSId = value; OnPropertyChanged("QSId"); }
        }

        private string m_QSUserName;
        public string QSUserName
        {
            get { return m_QSUserName; }
            set { m_QSUserName = value; OnPropertyChanged("QSUserName"); }
        }

        private string m_QSName;
        public string QSName
        {
            get { return m_QSName; }
            set { m_QSName = value; OnPropertyChanged("QSName"); }
        }

        private string m_QSBarber;
        public string QSBarber
        {
            get { return m_QSBarber; }
            set { m_QSBarber = value; OnPropertyChanged("QSBarber"); }
        }

        private string m_QSTime;
        public string QSTime
        {
            get { return m_QSTime; }
            set { m_QSTime = value; OnPropertyChanged("QSTime"); }
        }

        private bool m_SQIsCancelVisible;
        public bool SQIsCancelVisible
        {
            get { return m_SQIsCancelVisible; }
            set { m_SQIsCancelVisible = value; OnPropertyChanged("SQIsCancelVisible"); }
        }

        private Color m_CancelImage;
        public Color CancelImage
        {
            get { return m_CancelImage; }
            set { m_CancelImage = value; OnPropertyChanged("CancelImage"); }
        }

        private ImageSource m_RedArrowImage;
        public ImageSource RedArrowImage
        {
            get { return m_RedArrowImage; }
            set { m_RedArrowImage = value; OnPropertyChanged("RedArrowImage"); }
        }

        private string m_MakeHeaderBold;
        public string MakeHeaderBold
        {
            get { return m_MakeHeaderBold; }
            set { m_MakeHeaderBold = value; OnPropertyChanged("MakeHeaderBold"); }
        }
    }
}
