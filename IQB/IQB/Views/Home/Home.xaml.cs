using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.Interfaces;
using IQB.IQBServices;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.HomeServices;
using IQB.Utility;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.HomeManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.Views.Home
{
    public partial class Home : ContentView
    {
        HomeViewModel homeViewModel = null;

        bool _value = false;
        bool isbusy = false;

        public Home()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                InitializeComponent();

                if (Device.RuntimePlatform == Device.iOS)
                {
                    SelectBarberBtn.CornerRadius = 20;
                    AutoJoinBtn.CornerRadius = 20;
                    GroupJoinBtn.CornerRadius = 20;
                }


               if(Application.Current.Properties["ExpiringMsg"].ToString() != "")
                {
                    ExpiringStack.IsVisible = true;
                    ExpiringLabel.Text = Application.Current.Properties["ExpiringMsg"].ToString();
                }


                homeViewModel = new HomeViewModel();
                //BindServiceList();

                MessagingCenter.Subscribe<string>(this, "JoinedQSender", async (abc) =>
                {
                    try
                    {
                        homeViewModel = new HomeViewModel();
                        homeViewModel.HasResultChangeInModal = true;
                        homeViewModel.IsWaitingToJoinQueue = true;
                        homeViewModel.ShowQueueNotJoinedSections = false;
                        BindingContext = homeViewModel;
                    }
                    catch (Exception ex)
                    {

                    }
                });

                BindingContext = homeViewModel;


                //barberListPage = new BarberList(homeViewModel);

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    Device.OpenUri(new Uri("http://" + homeViewModel.AdImageLinkURL));
                };

                imgAdvertisement.GestureRecognizers.Add(tapGestureRecognizer);

                lstQueueStatus.ItemTapped += (object sender, ItemTappedEventArgs e) =>
                {
                    // don't do anything if we just de-selected the row
                    if (e.Item == null) return;
                    // do something with e.SelectedItem
                    ((ListView)sender).SelectedItem = null; // de-select the row
                };
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void OnGroupJoinTapped(object sender, EventArgs args)
        {
            homeViewModel.IsModalOpen = false;
            if (GroupJoinStack.Opacity != 0)
            {
                HidePopUpOptions();
                WriteErrorLog errLog = new WriteErrorLog();
                try
                {
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        if (!homeViewModel.IsModalOpen && !homeViewModel.IsQueueApiRunning)
                        {
                            homeViewModel.HasResultChangeInModal = false;

                            homeViewModel.IsModalOpen = true;

                            GroupJoin page = new GroupJoin(homeViewModel);
                            await Navigation.PushAsync(page);

                            //await page.PageClosedTask; //will wait here for response

                            //homeViewModel.IsModalOpen = false;

                            //if (homeViewModel.HasResultChangeInModal)
                            //{
                            //    homeViewModel.SetHomeData();
                            //}
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    }
                }
                catch (Exception ex)
                {
                    errLog.WinrtErrLogTest(ex);
                }
            }
        }

        private async void OnSelectBarberTapped(object sender, EventArgs args)
        {
            homeViewModel.IsModalOpen = false;
            if (SelectBarberStack.Opacity != 0)
            {
                HidePopUpOptions();
                WriteErrorLog errLog = new WriteErrorLog();
                try
                {
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        if (!homeViewModel.IsModalOpen && !homeViewModel.IsQueueApiRunning)
                        {
                            homeViewModel.HasResultChangeInModal = false;

                            homeViewModel.IsModalOpen = true;


                            BarberList page = new BarberList(homeViewModel);
                            await Navigation.PushAsync(page);

                            //await page.PageClosedTask; //will wait here for response

                            //homeViewModel.IsModalOpen = false;

                            //if (homeViewModel.HasResultChangeInModal)
                            //{
                            //    homeViewModel.SetHomeData();
                            //}
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    }
                }
                catch (Exception ex)
                {
                    errLog.WinrtErrLogTest(ex);
                }
            }
        }

        private async void OnAutoJoinQueue(object sender, EventArgs args)
        {

            if (AutoJoinStack.Opacity != 0)
            {
                HidePopUpOptions();
                //spHome.Opacity = 0.1;
                //gdPopup.IsVisible = true;
                //BindServiceList();
                await Navigation.PushModalAsync(new BarberServiceList(0, ""));
            }
        }

        private async void gdPopupOk_Tapped(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (homeViewModel.ServiceId != 0)
                {
                    gdPopup.IsVisible = false;
                    spHome.Opacity = 1;
                    if (homeViewModel.ServiceId != -1 && homeViewModel.ServiceId != 0)
                    {
                        if (NetworkConnectionMgmt.IsConnectedToNetwork())
                        {
                            if (!homeViewModel.IsModalOpen && !homeViewModel.IsQueueApiRunning)
                            {
                                homeViewModel.IsShowApiRunningLoader = true;

                                var ans = await App.Current.MainPage.DisplayAlert("Alert!", "Are you sure that you want to join the queue with " + homeViewModel.SelectBarberName + "?", "Yes", "No");
                                if (ans)
                                {
                                    homeViewModel.IsModalOpen = true;
                                    int salonId = 0;
                                    string userName = string.Empty;
                                    string FirstLastName = string.Empty;
                                    bool retResult = false;
                                    ApiResult res = new ApiResult();
                                    //Checking If User is already joined in the queue
                                    using (SalonViewModel objSalon = new SalonViewModel())
                                    {
                                        salonId = objSalon.GetSalonId();
                                    }
                                    using (LoginViewModel objLogin = new LoginViewModel())
                                    {
                                        userName = Convert.ToString(objLogin.GetUserId());
                                        FirstLastName = objLogin.GetFirstName() + " " + objLogin.GetLastName();
                                    }
                                    using (BarberService obj = new BarberService())
                                    {
                                        res = await obj.CheckUserJoinedQueue(userName, salonId);
                                    }
                                    if (res != null && res.StatusCode == 200)
                                    {
                                        List<CheckUserReturnResult> result = UtilityEL.ToModelList<CheckUserReturnResult>(res.Response);
                                        if (result.Count > 0)
                                        {
                                            retResult = true;
                                        }
                                    }
                                    if (retResult)
                                    {
                                        await App.Current.MainPage.DisplayAlert("Alert!", "You cannot re-join the queue today, due to a place cancellation.", "OK");
                                    }
                                    else
                                    {
                                        //Insert functionality for JoinQueue
                                        int position = 0;
                                        using (BarberService obj = new BarberService())
                                        {
                                            res = await obj.CheckQueuePosition(salonId);
                                            if (res != null && res.StatusCode == 200)
                                            {
                                                position = Convert.ToInt32(res.Response);
                                                position += 1;
                                                UserQueueJoinInsertModel autoJoinModel = new UserQueueJoinInsertModel()
                                                {
                                                    position = position,
                                                    username = userName,
                                                    firstlastname = FirstLastName,
                                                    rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy"),
                                                    timejoinedq = DateTime.Now.ToString("H:mm:ss"),
                                                    //barbername = "Any Barber",
                                                    barbername = homeViewModel.SelectBarberName,
                                                    salonid = salonId,
                                                    qgcode = "N/A",
                                                    ServiceId = homeViewModel.ServiceId,
                                                    BarberId = homeViewModel.SelectBarberID
                                                };
                                                res = await obj.InsertInJoinQueue(autoJoinModel);
                                                if (res != null && res.StatusCode == 200)
                                                {
                                                    int success = Convert.ToInt32(res.Response);
                                                    if (success == 1)
                                                    {
                                                        //await DisplayAlert("Success!", res.StatusMessage, "OK");
                                                        //homeViewModel.IsShowApiRunningLoader = true;
                                                        homeViewModel.IsWaitingToJoinQueue = true;
                                                        homeViewModel.ShowQueueNotJoinedSections = false;
                                                        await homeViewModel.SetHomeData(true, 0, 0);
                                                    }
                                                    else if (success == 0)
                                                    {
                                                        await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                                    }
                                                }
                                                else
                                                {
                                                    await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                                }
                                            }
                                            else
                                            {
                                                await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                            }
                                        }
                                    }
                                }

                                homeViewModel.IsShowApiRunningLoader = false;
                                homeViewModel.IsModalOpen = false;
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                        }
                    }
                    else if (homeViewModel.ServiceId == -1)
                    {
                        AutoJoinForNoService();
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Alert !", "Please select a service", "OK");
                }
                refreshServicelist();
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void AutoJoinForNoService()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                gdPopup.IsVisible = false;
                spHome.Opacity = 1;
                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    if (!homeViewModel.IsModalOpen && !homeViewModel.IsQueueApiRunning)
                    {
                        homeViewModel.IsShowApiRunningLoader = true;

                        var ans = await App.Current.MainPage.DisplayAlert("Alert!", "Are you sure that you want to join the queue?", "Yes", "No");
                        if (ans)
                        {
                            homeViewModel.IsModalOpen = true;
                            int salonId = 0;
                            string userName = string.Empty;
                            string FirstLastName = string.Empty;
                            bool retResult = false;
                            ApiResult res = new ApiResult();
                            //Checking If User is already joined in the queue
                            using (SalonViewModel objSalon = new SalonViewModel())
                            {
                                salonId = objSalon.GetSalonId();
                            }
                            using (LoginViewModel objLogin = new LoginViewModel())
                            {
                                userName = Convert.ToString(objLogin.GetUserId());
                                FirstLastName = objLogin.GetFirstName() + " " + objLogin.GetLastName();
                            }
                            using (BarberService obj = new BarberService())
                            {
                                res = await obj.CheckUserJoinedQueue(userName, salonId);
                            }
                            if (res != null && res.StatusCode == 200)
                            {
                                List<CheckUserReturnResult> result = UtilityEL.ToModelList<CheckUserReturnResult>(res.Response);
                                if (result.Count > 0)
                                {
                                    retResult = true;
                                }
                            }
                            if (retResult)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert!", "You cannot re-join the queue today, due to a place cancellation.", "OK");
                            }
                            else
                            {
                                //Insert functionality for JoinQueue
                                int position = 0;
                                using (BarberService obj = new BarberService())
                                {
                                    res = await obj.CheckQueuePosition(salonId);
                                    if (res != null && res.StatusCode == 200)
                                    {
                                        position = Convert.ToInt32(res.Response);
                                        position += 1;
                                        UserQueueJoinInsertModel autoJoinModel = new UserQueueJoinInsertModel()
                                        {
                                            position = position,
                                            username = userName,
                                            firstlastname = FirstLastName,
                                            rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy"),
                                            timejoinedq = DateTime.Now.ToString("H:mm:ss"),
                                            barbername = "Any Barber",
                                            salonid = salonId,
                                            qgcode = "N/A",
                                            ServiceId = 0,
                                        };
                                        res = await obj.InsertInJoinQueue(autoJoinModel);
                                        if (res != null && res.StatusCode == 200)
                                        {
                                            int success = Convert.ToInt32(res.Response);
                                            if (success == 1)
                                            {
                                                //await DisplayAlert("Success!", res.StatusMessage, "OK");
                                                //homeViewModel.IsShowApiRunningLoader = true;
                                                homeViewModel.IsWaitingToJoinQueue = true;
                                                homeViewModel.ShowQueueNotJoinedSections = false;
                                                await homeViewModel.SetHomeData(true, 0, 0);
                                            }
                                            else if (success == 0)
                                            {
                                                await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                            }
                                        }
                                        else
                                        {
                                            await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                        }
                                    }
                                    else
                                    {
                                        await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                    }
                                }
                            }
                        }

                        homeViewModel.IsShowApiRunningLoader = false;
                        homeViewModel.IsModalOpen = false;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        public void refreshServicelist()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
                foreach (var item1 in list)
                {
                    item1.Status = "unselected";
                    homeViewModel.ServiceId = 0;

                }
                ServiceListPopup.ItemsSource = list;
            }
            catch (Exception es)
            {
                errLog.WinrtErrLogTest(es);
            }

        }

        private void ServiceName_Clicked(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var item = (Button)sender;
                var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
                var itemSelected = item.CommandParameter as ServiceListModel;

                foreach (var item1 in list)
                {
                    if (item1 == itemSelected)
                    {
                        item1.Status = "selected";
                        homeViewModel.ServiceId = itemSelected.ServiceID;
                    }
                    else
                        item1.Status = "unselected";
                }
                ServiceListPopup.ItemsSource = list;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void BindServiceList()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                homeViewModel.IsRefreshing = true;
                ObservableCollection<ServiceListModel> objList = new ObservableCollection<ServiceListModel>();
                int salonId = 0;
                using (SalonViewModel objSalon = new SalonViewModel())
                {
                    salonId = objSalon.GetSalonId();
                }

                ApiResult res = new ApiResult();

                //using (BarberSalonService obj = new BarberSalonService())
                //{
                //   // res = await obj.GetServicesBySalonId(salonId);
                //}
                using (BarberService obj = new BarberService())
                {
                    res = await obj.GetServicesByBarberIdSalonId(777777, salonId);

                }

                if (res != null && res.StatusCode == 200)
                {
                    List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                    if (result.Count > 0)
                    {
                        foreach (var item in result)
                        {
                            ServiceListModel Service = new ServiceListModel();
                            Service.ServiceID = item.ServiceId;
                            Service.ServiceName = item.ServiceName;
                            Service.ServicePrice = item.ServicePrice;
                            TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.Estimated_wait_time));
                            //Service.ServiceEstimatedTime = timeSpan.ToString(@"hh\:mm") + " mins";
                            Service.ServiceEstimatedTime = (int)timeSpan.Hours + " hrs : " + (int)timeSpan.Minutes + " mins ";
                            Service.Status = "unselected";
                            homeViewModel.SelectBarberName = item.BarberNName;
                            homeViewModel.SelectBarberID = item.BarberId;
                            if (item.least_EST_wait_time == null)
                            {
                                Application.Current.Properties["AutojoinEstimatedWaitTime"] = "Next";
                            }
                            else
                            {
                                TimeSpan timeSpan1 = TimeSpan.FromMinutes(Convert.ToInt32(item.least_EST_wait_time));
                                //Service.least_EST_wait_time = timeSpan1.ToString(@"hh\:mm") + " mins";
                                Service.least_EST_wait_time = (int)timeSpan1.Hours + " hrs : " + (int)timeSpan1.Minutes + " mins ";

                                if (Service.least_EST_wait_time == "00:00 mins")
                                {
                                    Application.Current.Properties["AutojoinEstimatedWaitTime"] = "Next";
                                }
                                else
                                {
                                    Application.Current.Properties["AutojoinEstimatedWaitTime"] = Service.least_EST_wait_time;
                                }
                            }
                            homeViewModel.AutojoinEstimatedWaitTime = "Estimated wait time:" + " " + Application.Current.Properties["AutojoinEstimatedWaitTime"];
                            objList.Add(Service);

                        }
                        ServiceListPopup.ItemsSource = objList;
                    }
                    else
                    {

                    }
                }
                else
                {
                    //gdPopup.IsVisible = false;
                    AutoJoinForNoService();
                }
                homeViewModel.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        void OnCancelTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            TapGestureRecognizer btn = (TapGestureRecognizer)sender;
            string username = (string)btn.CommandParameter;
            App.Current.MainPage.DisplayAlert("Message", username, "Ok");
        }

        void individualCancelClicked(object sender, EventArgs args)
        {
            App.Current.MainPage.DisplayAlert("Message", "Opened", "Ok");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            bool result = resultvalue();
            if (result)
            {
                gdPopup.IsVisible = false;
                ShowDisplayAlert();
            }

        }

        private bool resultvalue()
        {
            Application.Current.Properties["Status"] = true;
            _value = (Application.Current.Properties["Status"] as bool?) ?? false;
            return _value;
        }

        private async void ShowDisplayAlert()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var ans = await App.Current.MainPage.DisplayAlert("Alert!", "Are you sure that you want to join the queue with Any Barber?", "Yes", "No");
                if (ans)
                {
                    homeViewModel.IsModalOpen = true;
                    int salonId = 0;
                    string userName = string.Empty;
                    string FirstLastName = string.Empty;
                    bool retResult = false;
                    ApiResult res = new ApiResult();
                    //Checking If User is already joined in the queue
                    using (SalonViewModel objSalon = new SalonViewModel())
                    {
                        salonId = objSalon.GetSalonId();
                    }
                    using (LoginViewModel objLogin = new LoginViewModel())
                    {
                        userName = Convert.ToString(objLogin.GetUserId());
                        FirstLastName = objLogin.GetFirstName() + " " + objLogin.GetLastName();
                    }
                    using (BarberService obj = new BarberService())
                    {
                        res = await obj.CheckUserJoinedQueue(userName, salonId);
                    }
                    if (res != null && res.StatusCode == 200)
                    {
                        List<CheckUserReturnResult> result = UtilityEL.ToModelList<CheckUserReturnResult>(res.Response);
                        if (result.Count > 0)
                        {
                            retResult = true;
                        }
                    }
                    if (retResult)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert!", "You cannot re-join the queue today, due to a place cancellation.", "OK");
                    }
                    else
                    {
                        //Insert functionality for JoinQueue
                        int position = 0;
                        using (BarberService obj = new BarberService())
                        {
                            res = await obj.CheckQueuePosition(salonId);
                            if (res != null && res.StatusCode == 200)
                            {
                                position = Convert.ToInt32(res.Response);
                                position += 1;
                                UserQueueJoinInsertModel autoJoinModel = new UserQueueJoinInsertModel()
                                {
                                    position = position,
                                    username = userName,
                                    firstlastname = FirstLastName,
                                    rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy"),
                                    timejoinedq = DateTime.Now.ToString("H:mm:ss"),
                                    barbername = "Any Barber",
                                    salonid = salonId,
                                    qgcode = "N/A"
                                };
                                res = await obj.InsertInJoinQueue(autoJoinModel);
                                if (res != null && res.StatusCode == 200)
                                {
                                    int success = Convert.ToInt32(res.Response);
                                    if (success == 1)
                                    {
                                        //await DisplayAlert("Success!", res.StatusMessage, "OK");
                                        //homeViewModel.IsShowApiRunningLoader = true;
                                        homeViewModel.IsWaitingToJoinQueue = true;
                                        homeViewModel.ShowQueueNotJoinedSections = false;
                                        await homeViewModel.SetHomeData(true, 0, 0);
                                    }
                                    else if (success == 0)
                                    {
                                        await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                    }
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                                }
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Error!", res.StatusMessage, "OK");
                            }
                        }
                    }
                }
                homeViewModel.IsShowApiRunningLoader = false;
                homeViewModel.IsModalOpen = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private void ServiceListPopup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
                var itemSelected = ServiceListPopup.SelectedItem as ServiceListModel;

                foreach (var item1 in list)
                {
                    if (item1 == itemSelected)
                    {
                        item1.Status = "selected";
                        homeViewModel.ServiceId = itemSelected.ServiceID;
                    }
                    else
                        item1.Status = "unselected";
                }
                ServiceListPopup.ItemsSource = list;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private void gdPopupCancel_Tapped(object sender, EventArgs e)
        {
            gdPopup.IsVisible = false;
            spHome.Opacity = 1;
            refreshServicelist();
        }

        private async void OpenServicesOptionTapped(object sender, EventArgs e)
        {
            if (Application.Current.Properties["ModuleAvailability"].ToString() != "2")
            {
                if (!isbusy)
                {
                    isbusy = true;
                    if (GroupJoinStack.Opacity == 0)
                    {
                        SelectServicesStack.IsVisible = true;
                        MainContentGrid.Opacity = 0.1;
                        SectServiceFrame.Source = "resource://IQB.Resources.Image.cross.svg";
                        await GroupJoinStack.FadeTo(1, 300, Easing.CubicIn);
                        await AutoJoinStack.FadeTo(1, 300, Easing.CubicIn);
                        await SelectBarberStack.FadeTo(1, 300, Easing.CubicIn);
                    }

                    else
                    {
                        HidePopUpOptions();
                    }
                    isbusy = false;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
            }
        }

        private async void HidePopUpOptions()
        {
            SectServiceFrame.Source = "resource://IQB.Resources.Image.add.svg";
            await SelectBarberStack.FadeTo(0, 300, Easing.CubicOut);
            await AutoJoinStack.FadeTo(0, 300, Easing.CubicOut);
            await GroupJoinStack.FadeTo(0, 300, Easing.CubicOut);
            SelectServicesStack.IsVisible = false;
            MainContentGrid.Opacity = 1;
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (MainScroller.ScrollY == 0)
            {
                ShowQListButton.IsVisible = QListContainer.IsVisible;
            }

            if (MainScroller.ScrollY >= 800)
            {
                ShowQListButton.IsVisible = false;
            }
        }

        private void ShowQListStatusTapped(object sender, EventArgs e)
        {
            //   var y = QueueListContainer.Y; var parent = QueueListContainer.ParentView; while (parent != null) { y += parent.Y; parent = parent.ParentView; }
            //   ShowQListButton.IsVisible = false;
            MainScroller.ScrollToAsync(QueueListContainer, ScrollToPosition.Start, true);
        }

        private void OnMoreTapped(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {

                switch (homeViewModel?.MoreText)
                {
                    case " + ":
                        {
                            homeViewModel.MoreText = " - ";
                            homeViewModel.PInfo = homeViewModel?.Info;
                            homeViewModel.IsMoreTapped = true;
                        }
                        break;
                    case " - ":
                        {
                            if (homeViewModel?.Info.Length > 100)
                            {
                                homeViewModel.PInfo = $"{homeViewModel?.Info?.Substring(0, 100)}...";
                                homeViewModel.MoreText = " + ";
                            }
                            else
                            {
                                homeViewModel.PInfo = homeViewModel?.Info;
                                homeViewModel.MoreText = string.Empty;
                            }
                            homeViewModel.IsMoreTapped = false;
                        }
                        break;
                    default:
                        {
                            if (homeViewModel?.Info.Length > 100)
                            {
                                homeViewModel.PInfo = $"{homeViewModel?.Info?.Substring(0, 100)}...";
                                homeViewModel.MoreText = " + ";
                            }
                            else
                            {
                                homeViewModel.PInfo = homeViewModel?.Info;
                                homeViewModel.MoreText = string.Empty;
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, nameof(Home), "OnMoreTapped");
            }

        }

    }
}