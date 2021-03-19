using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.HomeManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupJoin : ContentPage
    {
        public GroupJoinViewModel viewModel = null;
        private HomeViewModel homeViewModel = null;

        public int noofUsers = 0;


        //public Task PageClosedTask { get { return tcs.Task; } }
        //private TaskCompletionSource<bool> tcs { get; set; }

        public GroupJoin(HomeViewModel homeViewModel)
        {
            InitializeComponent();

            //BindServiceList();
            this.viewModel = new GroupJoinViewModel();
            BindingContext = viewModel;
            this.homeViewModel = homeViewModel;

            if (viewModel.SelectedServicePrice == "0.00")
            {
                stPrice.IsVisible = false;
                stPriceBlank.IsVisible = true;
            }

            lstGroupJoin.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };
            noofUsers = 0;

            OnBackButtonPressed();
        }

        private async void BindServiceList(int barberId)
        {
            viewModel.IsRefreshing = true;
            ObservableCollection<ServiceListModel> objList = new ObservableCollection<ServiceListModel>();
            int salonId = 0;
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            ApiResult res = new ApiResult();
            using (BarberService obj = new BarberService())
            {
                res = await obj.GetServicesByBarberIdSalonId(barberId, salonId);
            }

            if (res != null && res.StatusCode == 200)
            {
                List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                foreach (var item in result)
                {
                    ServiceListModel Service = new ServiceListModel();
                    Service.ServiceName = item.ServiceName;
                    Service.ServicePrice = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;

                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.Estimated_wait_time));
                    Service.ServiceEstimatedTime = (int)timeSpan.TotalHours + " hrs : " + (int)timeSpan.TotalMinutes + " mins ";
                    //Service.ServiceEstimatedTime = timeSpan.ToString(@"hh\:mm") + " mins";


                    if (item.least_EST_wait_time != null)
                    {
                        TimeSpan _timespanforleast_EST_wait_time = TimeSpan.FromMinutes(Convert.ToInt32(item.least_EST_wait_time));
                        //Service.least_EST_wait_time = _timespanforleast_EST_wait_time.ToString(@"hh\:mm") + " mins";
                        Service.least_EST_wait_time = (int)_timespanforleast_EST_wait_time.Hours + " hrs : " + (int)_timespanforleast_EST_wait_time.Minutes + " mins ";
                    }
                    else
                    {
                        Service.least_EST_wait_time = "00:00 mins";
                    }
                    //Service.ServiceEstimatedTime = item.Estimated_wait_time + "mins";
                    if (Service.least_EST_wait_time == "00:00 mins")
                    {
                        Application.Current.Properties["EstimateWaitTime"] = "Next";
                    }
                    else
                    {
                        Application.Current.Properties["EstimateWaitTime"] = Service.least_EST_wait_time;
                    }
                    viewModel.EstimatedWaitTime = "Estimated wait time:" + " " + Application.Current.Properties["EstimateWaitTime"] as string;
                    Application.Current.Properties["SelectBarberName"] = item.BarberNName;
                    viewModel.SelectBarberName = Application.Current.Properties["SelectBarberName"] as string;
                    Service.SelectBarberID = item.BarberId;
                    //Application.Current.Properties["SelectBarberID"] = item.BarberId;
                    Service.Status = "unselected";
                    Service.ServiceID = item.ServiceId;
                    objList.Add(Service);
                }

                ServiceListPopup.ItemsSource = objList;
                //     lblServiceCount.Text = "(" + Convert.ToString(objList.Count()) + " service(s) available)";
            }
            else if (res.StatusMessage == "No records found")
            {
                spServiceList.IsVisible = false;
                spNoService.IsVisible = true;
            }
            viewModel.IsRefreshing = false;
        }

        private async void RefreshServiceList(int barberId)
        {
            try
            {
                viewModel.IsRefreshing = true;
                ObservableCollection<ServiceListModel> objList = new ObservableCollection<ServiceListModel>();

                ServiceListPopup.ItemsSource = null;
                objList.Clear();

                int salonId = 0;
                using (SalonViewModel objSalon = new SalonViewModel())
                {
                    salonId = objSalon.GetSalonId();
                }

                ApiResult res = new ApiResult();
                using (BarberService obj = new BarberService())
                {
                    res = await obj.GetServicesByBarberIdSalonId(barberId, salonId);
                }

                if (res != null && res.StatusCode == 200)
                {
                    List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                    foreach (var item in result)
                    {
                        ServiceListModel Service = new ServiceListModel();
                        Service.ServiceName = item.ServiceName;
                        Service.ServicePrice = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;

                        TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.Estimated_wait_time));
                        //Service.ServiceEstimatedTime = timeSpan.ToString(@"hh\:mm") + " mins";
                        Service.ServiceEstimatedTime = (int)timeSpan.TotalHours + " hrs : " + (int)timeSpan.TotalMinutes + " mins ";


                        if (item.least_EST_wait_time != null)
                        {
                            TimeSpan _timespanforleast_EST_wait_time = TimeSpan.FromMinutes(Convert.ToInt32(item.least_EST_wait_time));
                            //Service.least_EST_wait_time = _timespanforleast_EST_wait_time.ToString(@"hh\:mm") + " mins";
                            Service.least_EST_wait_time = (int)_timespanforleast_EST_wait_time.TotalHours + " hrs : " + (int)_timespanforleast_EST_wait_time.TotalMinutes + " mins ";
                        }
                        else
                        {
                            Service.least_EST_wait_time = "00:00 mins";
                        }
                        //Service.ServiceEstimatedTime = item.Estimated_wait_time + "mins";
                        if (Service.least_EST_wait_time == "00:00 mins")
                        {
                            Application.Current.Properties["EstimateWaitTime"] = "Next";
                        }
                        else
                        {
                            Application.Current.Properties["EstimateWaitTime"] = Service.least_EST_wait_time;
                        }
                        viewModel.EstimatedWaitTime = "Estimated wait time:" + " " + Application.Current.Properties["EstimateWaitTime"] as string;
                        Application.Current.Properties["SelectBarberName"] = item.BarberNName;
                        viewModel.SelectBarberName = Application.Current.Properties["SelectBarberName"] as string;
                        Service.SelectBarberID = item.BarberId;
                        //Application.Current.Properties["SelectBarberID"] = item.BarberId;
                        Service.Status = "unselected";
                        Service.ServiceID = item.ServiceId;
                        objList.Add(Service);
                    }

                    ServiceListPopup.ItemsSource = objList;
                    //        lblServiceCount.Text = "(" + Convert.ToString(objList.Count()) + " service(s) available)";
                }
                else if (res.StatusMessage == "No records found")
                {
                    spServiceList.IsVisible = false;
                    spNoService.IsVisible = true;
                }
                viewModel.IsRefreshing = false;
            }
            catch (Exception ex)
            {

            }
        }

        //private void BarberName_Clicked(object sender, EventArgs e)
        //{
        //    var item = (Button)sender;
        //    var list = BarberListPopup.ItemsSource as ObservableCollection<ViewModel.HomeManagement.BarbersListModel>;
        //    var itemSelected = item.CommandParameter as ViewModel.HomeManagement.BarbersListModel;
        //    viewModel.barberBtnText = itemSelected.BarberName;

        //    foreach (var item1 in list)
        //    {
        //        if (item1 == itemSelected)
        //            item1.Status = "selected";
        //        else
        //            item1.Status = "unselected";
        //    }
        //    BarberListPopup.ItemsSource = list;
        //    BindServiceList(itemSelected.BarberId);
        //    btnServicePopup.IsEnabled = true;
        //}

        //private void ServiceName_Clicked(object sender, EventArgs e)
        //{
        //    var item = (Button)sender;
        //    var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
        //    var itemSelected = item.CommandParameter as ServiceListModel;

        //    viewModel.btnText = itemSelected.ServiceName;
        //    viewModel.SelectedServicePrice = "Service Price : £" + itemSelected.ServicePrice;


        //    foreach (var item1 in list)
        //    {
        //        if (item1 == itemSelected)
        //            item1.Status = "selected";
        //        else
        //            item1.Status = "unselected";
        //    }
        //    ServiceListPopup.ItemsSource = list;
        //}

        private async void OnGroupJoinClosed(object sender, EventArgs args)
        {
            homeViewModel.IsModalOpen = false;
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void OnJoinGroup(object sender, EventArgs args)
        {
            if (viewModel.SelectedServiceId != 0)
            {
                //Button btn = (Button)sender;
                //int id = (int)btn.CommandParameter;
                int id = Convert.ToInt32((((sender as Frame).Content as StackLayout).Children[0] as Label).Text.ToString());
                bool validateUser = viewModel.ValidateUserNameandBarber(id);
                if (validateUser)
                {
                    if (id != 1)
                    {
                        noofUsers = noofUsers + 1;
                    }
                    viewModel.ChangeGroupJoinList(id, "join");
                }
                else
                {
                    await DisplayAlert("Alert!", "Please provide a name and select a barber", "OK");
                }
                viewModel.GroupJoinIsEnabled = viewModel.CheckIfJoinQueueEnabled();
            }
            else
            {
                await DisplayAlert("Alert!", "Please provide a name and select a barber", "OK");
            }
        }

        private void OnRemoveGroup(object sender, EventArgs args)
        {

            //Button btn = (Button)sender;

            //int id = (int)btn.CommandParameter;
            int id = Convert.ToInt32((((sender as Frame).Content as StackLayout).Children[0] as Label).Text.ToString());
            if (id != 1)
            {
                noofUsers = noofUsers - 1;
            }
            viewModel.ChangeGroupJoinList(id, "remove");
            viewModel.GroupJoinIsEnabled = viewModel.CheckIfJoinQueueEnabled();
        }

        private async void OnGroupJoinQueue(object sender, EventArgs args)
        {
            if (NetworkConnectionMgmt.IsConnectedToNetwork())
            {
                if (viewModel.SelectedServiceId == 0)
                {
                    await DisplayAlert("Alert!", "You have not selected any services", "Ok");
                }
                else if (viewModel.SelectedServiceId == -1)
                {
                    gropjoin_withoutServiceid();
                }
                else
                {
                    if (viewModel.GroupJoinIsEnabled)
                    {
                        viewModel.GroupJoinIsEnabled = false;
                        int salonId = 0;
                        bool MessageShow = false;
                        string userName = string.Empty;
                        string FirstLastName = string.Empty;
                        string qgCode = UtilityEL.GetRandomString(6);
                        bool retResult = false;
                        int selectedServiceID = 0;
                        int selectedBarberID = 0;
                        // var itemSelected = ServiceListPopup.SelectedItem as ServiceListModel;
                        ApiResult res = new ApiResult();


                        var answer = await DisplayAlert("Alert!", "You have selected " + noofUsers + " users to join the queue with you, would you like to proceed?", "Yes", "No");
                        if (answer)
                        {
                            gdGroupJoin.Opacity = 0.1;
                            viewModel.IsLoaderEnabled = true;
                            NavigationPage.SetHasNavigationBar(this, false);
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
                                await DisplayAlert("Alert!", "You cannot re-join the queue today, due to a place cancellation.", "OK");
                            }
                            else
                            {
                                bool chckIsSel = false;
                                //Insert functionality for JoinQueue
                                int position = 0;
                                using (BarberService obj = new BarberService())
                                {
                                    ObservableCollection<GroupListModel> JoinList = viewModel.GroupJoinList;
                                    foreach (var item in JoinList)
                                    {
                                        UserQueueJoinInsertModel JoinModel = new UserQueueJoinInsertModel();
                                        //Check if user has confirmed join
                                        if (item.IsSelected == true)
                                        {
                                            chckIsSel = true;
                                            res = await obj.CheckQueuePosition(salonId);
                                            if (res != null && res.StatusCode == 200)
                                            {
                                                position = Convert.ToInt32(res.Response);
                                                position += 1;
                                                //check if it is main user
                                                if (item.Id == 1)
                                                {
                                                    JoinModel.username = userName;
                                                    JoinModel.firstlastname = item.UserName + " *";
                                                    JoinModel.barbername = item.BarberNameForPanel;
                                                }
                                                else
                                                {
                                                    JoinModel.username = userName + "-" + item.Id;
                                                    JoinModel.firstlastname = item.UserName + " *";
                                                    JoinModel.barbername = item.BarberNameForPanel;
                                                }

                                                JoinModel.position = position;
                                                JoinModel.rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy");
                                                JoinModel.timejoinedq = DateTime.Now.ToString("H:mm:ss");
                                                JoinModel.salonid = salonId;
                                                JoinModel.qgcode = qgCode;
                                                JoinModel.ServiceId = item.ServiceIdForPanel;
                                                selectedServiceID = item.ServiceIdForPanel;
                                                selectedBarberID = item.BarberIDForPanel;
                                                JoinModel.BarberId = item.BarberIDForPanel;
                                                res = await obj.InsertInJoinQueue(JoinModel);
                                                if (res != null && res.StatusCode == 200)
                                                {
                                                    MessageShow = true;
                                                }
                                                else
                                                {
                                                    MessageShow = false;
                                                }
                                            }
                                        }
                                    }
                                    if (MessageShow)
                                    {
                                        homeViewModel.HasResultChangeInModal = true;
                                        homeViewModel.IsWaitingToJoinQueue = true;
                                        homeViewModel.ShowQueueNotJoinedSections = false;
                                        await DisplayAlert("Success!", "You have succesdfully joined the queue.", "OK");
                                        // homeViewModel.SetHomeData(true, selectedServiceID, selectedBarberID);
                                    }
                                    if (!chckIsSel)
                                    {
                                        // await DisplayAlert("Alert!", "No user is selected to join queue", "OK");
                                    }

                                    //tcs.SetResult(true);
                                    homeViewModel.IsModalOpen = false;
                                    await Navigation.PopAsync();
                                    //await Navigation.PopModalAsync();
                                }
                            }
                            viewModel.GroupJoinIsEnabled = true;
                            viewModel.SelectedServiceId = 0;
                            NavigationPage.SetHasNavigationBar(this, true);
                            viewModel.IsLoaderEnabled = false;
                            gdGroupJoin.Opacity = 1;
                        }
                        else
                        {
                            viewModel.GroupJoinIsEnabled = true;
                        }
                        //else
                        //{
                        //    //await DisplayAlert("Error!", CommonEL.NoUserSelectedGroupJoin, "Ok");
                        //    //viewModel.SetDefaultMenuList();
                        //    //lstGroupJoin.ItemsSource = viewModel.GroupJoinList;

                        //    foreach (var item in viewModel.GroupJoinList)
                        //    {
                        //        viewModel.ChangeGroupJoinList(item.Id, "remove");
                        //    }
                        //    viewModel.GroupJoinList.Clear();
                        //    viewModel.SetDefaultMenuList();
                        //    lstGroupJoin.ItemsSource = viewModel.GroupJoinList;
                        //}
                    }

                    else
                    {
                        await DisplayAlert("Alert!", "You cannot join group with selected options", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
            }
        } 

        private async void gropjoin_withoutServiceid()
        {
            if (viewModel.GroupJoinIsEnabled)
            {
                viewModel.GroupJoinIsEnabled = false;
                int salonId = 0;
                bool MessageShow = false;
                string userName = string.Empty;
                string FirstLastName = string.Empty;
                string qgCode = UtilityEL.GetRandomString(6);
                bool retResult = false;
                int selectedServiceID = 0;
                int selectedBarberID = 0;
                // var itemSelected = ServiceListPopup.SelectedItem as ServiceListModel;
                ApiResult res = new ApiResult();


                var answer = await DisplayAlert("Alert!", "You have selected " + noofUsers + " users to join the queue with you, would you like to proceed?", "Yes", "No");
                if (answer)
                {
                    gdGroupJoin.Opacity = 0.1;
                    viewModel.IsLoaderEnabled = true;
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
                        await DisplayAlert("Alert!", "You cannot re-join the queue today, due to a place cancellation.", "OK");
                    }
                    else
                    {
                        bool chckIsSel = false;
                        //Insert functionality for JoinQueue
                        int position = 0;
                        using (BarberService obj = new BarberService())
                        {
                            ObservableCollection<GroupListModel> JoinList = viewModel.GroupJoinList;
                            foreach (var item in JoinList)
                            {
                                UserQueueJoinInsertModel JoinModel = new UserQueueJoinInsertModel();
                                //Check if user has confirmed join
                                if (item.IsSelected == true)
                                {
                                    chckIsSel = true;
                                    res = await obj.CheckQueuePosition(salonId);
                                    if (res != null && res.StatusCode == 200)
                                    {
                                        position = Convert.ToInt32(res.Response);
                                        position += 1;
                                        //check if it is main user
                                        if (item.Id == 1)
                                        {
                                            JoinModel.username = userName;
                                            JoinModel.firstlastname = item.UserName + " *";
                                            JoinModel.barbername = item.BarberNameForPanel;
                                        }
                                        else
                                        {
                                            JoinModel.username = userName + "-" + item.Id;
                                            JoinModel.firstlastname = item.UserName + " *";
                                            JoinModel.barbername = item.BarberNameForPanel;
                                        }


                                        JoinModel.position = position;
                                        JoinModel.rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy");
                                        JoinModel.timejoinedq = DateTime.Now.ToString("H:mm:ss");
                                        JoinModel.salonid = salonId;
                                        JoinModel.qgcode = qgCode;
                                        JoinModel.ServiceId = 0;
                                        selectedServiceID = 0;
                                        selectedBarberID = item.BarberIDForPanel;
                                        JoinModel.BarberId = item.BarberIDForPanel;
                                        res = await obj.InsertInJoinQueue(JoinModel);
                                        if (res != null && res.StatusCode == 200)
                                        {
                                            MessageShow = true;
                                        }
                                        else
                                        {
                                            MessageShow = false;
                                        }
                                    }
                                }
                            }
                            if (MessageShow)
                            {
                                homeViewModel.HasResultChangeInModal = true;
                                homeViewModel.IsWaitingToJoinQueue = true;
                                homeViewModel.ShowQueueNotJoinedSections = false;

                                homeViewModel.SetHomeData(true, selectedServiceID, selectedBarberID);
                            }
                            if (!chckIsSel)
                            {
                                await DisplayAlert("Alert!", "No user is selected to join queue", "OK");
                            }

                            //tcs.SetResult(true);
                            homeViewModel.IsModalOpen = false;
                            await Navigation.PopModalAsync();
                        }
                    }
                    viewModel.GroupJoinIsEnabled = true;
                    viewModel.SelectedServiceId = 0;
                    viewModel.IsLoaderEnabled = false;
                    gdGroupJoin.Opacity = 1;
                }
                else
                {
                    await DisplayAlert("Error!", CommonEL.NoUserSelectedGroupJoin, "Ok");
                }
            }
        }

        private void Service_Clicked(object sender, EventArgs e)
        {
            stGroupJoin.Opacity = 0.1;
            gdPopup.IsVisible = true;
        }

        private void Cancel_Tapped(object sender, EventArgs e)
        {
            viewModel.barberBtnText = "Select Barber";
            viewModel.btnText = "Select Service";
            viewModel.SelectedServicePrice = "";
            refreshBarberlist();
            viewModel.SelectedServiceId = 0;
            btnServicePopup.IsEnabled = false;
            gdGroupJoinPopup.IsVisible = false;
            stGroupJoin.Opacity = 1;
        }

        private void Ok_Tapped()
        {
            int index = 0;
            var selectedBarber = Application.Current.Properties["groupJoinListItem"] as GroupListModel;

            var list = lstGroupJoin.ItemsSource as ObservableCollection<GroupListModel>;


            if (viewModel.barberBtnText != "Select Barber" && viewModel.btnText != "Select Service")
            {
                foreach (var item1 in list)
                {
                    if (item1 == selectedBarber)
                    {
                        //Application.Current.Properties["SelectBarberName"] as string
                        item1.IsPanelVisible = true;
                        if (viewModel.barberBtnText == "Any Barber")
                        {
                            item1.BarberNameForPanel = Application.Current.Properties["SelectBarberName"] as string;
                        }
                        else
                        {
                            item1.BarberNameForPanel = viewModel.barberBtnText;
                        }
                        item1.ServiceNameForPanel = viewModel.btnText;
                        item1.ServicePriceForPanel = viewModel.SelectedServicePrice;
                        item1.ServiceIdForPanel = viewModel.SelectedServiceId;
                        item1.BarberIDForPanel = viewModel.BarberIdForPanel;

                        //if(Device.RuntimePlatform == Device.iOS)
                        //{
                        //    lstGroupJoin
                        //}
                        index = list.IndexOf(item1);


                    }
                }
                
                
                if(index == 4 && Device.RuntimePlatform == Device.iOS)
                {
                    lstGroupJoin.ItemsSource = null;
                    lstGroupJoin.ItemsSource = list;
                }
                else
                {
                    lstGroupJoin.ItemsSource = list;
                }
            }

            


            gdGroupJoinPopup.IsVisible = false;
            NavigationPage.SetHasNavigationBar(this, true);
            stGroupJoin.Opacity = 1;
            viewModel.barberBtnText = "Select Barber";
            viewModel.btnText = "Select Service";
            viewModel.SelectedServicePrice = "";
            viewModel.BarberIdForPanel = 0;
            refreshBarberlist();
            btnServicePopup.IsEnabled = false;
        }

        private async void gdPopupCancel_Tapped(object sender, EventArgs e)
        {
            gdPopup.IsVisible = false;
            gdBarberPopup.IsVisible = true;
        }

        private async void selectServiceOk_Tapped(object sender, EventArgs e)
        {
            if (viewModel.SelectedServiceId == 0)
            {
                await DisplayAlert("Alert!", "Please select a service", "Ok");
            }
            else
            {
                gdPopup.IsVisible = false;
                gdGroupJoinPopup.IsVisible = true;
            }
        }

        private void SelectBarber_Clicked(object sender, EventArgs e)
        {
            stGroupJoin.Opacity = 0.1;
            //      lblBarberCount.Text = "(" + Convert.ToString(viewModel.BarberListForPopup.Count()) + " selection(s) available)";
            gdBarberPopup.IsVisible = true;
            gdGroupJoinPopup.IsVisible = false;
        }

        private void gdBarberPopupCancel_Tapped(object sender, EventArgs e)
        {
            gdBarberPopup.IsVisible = false;
            stGroupJoin.Opacity = 1;
            stGroupJoin.IsVisible = true;
            NavigationPage.SetHasNavigationBar(this, true);
            //   gdGroupJoinPopup.IsVisible = true;
        }

        private void BtnBarberOk_Tapped(object sender, EventArgs e)
        {
            gdBarberPopup.IsVisible = false;
            gdGroupJoinPopup.IsVisible = true;
        }

        private void SelectOptions_Clicked(object sender, EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            stGroupJoin.Opacity = 0.1;
            //   gdGroupJoinPopup.IsVisible = true;

            if (viewModel.SelectedServicePrice == "")
            {
                viewModel.PriceBlankOrNot = false;
            }
            var item = (Button)sender;
            var itemSelected = item.CommandParameter as GroupListModel;
            Application.Current.Properties["groupJoinListItem"] = itemSelected;

            gdBarberPopup.IsVisible = true;
        }

        private void ServiceListPopup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ServiceListPopup.SelectedItem != null)
            {



                var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;

                var itemSelected = ServiceListPopup.SelectedItem as ServiceListModel;

                viewModel.btnText = itemSelected.ServiceName;
                viewModel.SelectedServicePrice = itemSelected.ServicePrice;

                //
                stPrice.IsVisible = true;
                stPriceBlank.IsVisible = false;

                viewModel.SelectedServiceId = itemSelected.ServiceID;

                foreach (var item1 in list)
                {
                    if (item1 == itemSelected)
                    {
                        item1.Status = "selected";
                        viewModel.BarberIdForPanel = itemSelected.SelectBarberID;

                    }
                    else
                    {
                        item1.Status = "unselected";
                    }
                }
                ServiceListPopup.ItemsSource = list;
                ServiceListPopup.SelectedItem = null;
                viewModel.PriceBlankOrNot = true;

                gdPopup.IsVisible = false;
                Ok_Tapped();
            }
        }

        private async void BarberListPopup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (BarberListPopup.SelectedItem != null)
            {
                var q = BarberListPopup.SelectedItem as ViewModel.HomeManagement.BarbersListModel;
                if (q != null && !q.IsBarberOffline)
                {
                    var list = BarberListPopup.ItemsSource as ObservableCollection<ViewModel.HomeManagement.BarbersListModel>;
                    if (BarberListPopup.SelectedItem != null)
                    {
                        var itemSelected = BarberListPopup.SelectedItem as ViewModel.HomeManagement.BarbersListModel;
                        viewModel.barberBtnText = itemSelected.BarberName;

                        foreach (var item1 in list)
                        {
                            if (item1 == itemSelected)
                            {
                                item1.Status = "selected";
                                // Application.Current.Properties["EstimateWaitTime"] = item1.EstimatedTime;
                                //Application.Current.Properties["SelectBarberName"] = item1.BarberName;
                            }
                            else
                            {
                                item1.Status = "unselected";
                            }
                        }

                        //viewModel.SelectBarberName = Application.Current.Properties["SelectBarberName"] as string;
                        BarberListPopup.ItemsSource = list;
                        BindServiceList(itemSelected.BarberId);
                        //RefreshServiceList(itemSelected.BarberId);
                        btnServicePopup.IsEnabled = true;
                    }
                    gdBarberPopup.IsVisible = false;
                    //gdGroupJoinPopup.IsVisible = true;
                    stGroupJoin.Opacity = 0.1;
                    gdPopup.IsVisible = true;
                }

                else
                {
                    await DisplayAlert("Alert!", "Barber is currently offline", "OK");
                }
                BarberListPopup.SelectedItem = null;
            }
        }

        public void refreshBarberlist()
        {
            try
            {
                var list = BarberListPopup.ItemsSource as ObservableCollection<ViewModel.HomeManagement.BarbersListModel>;
                foreach (var item1 in list)
                {
                    item1.Status = "unselected";
                }
                BarberListPopup.ItemsSource = list;
            }
            catch (Exception es)
            {

            }

        }

        public void refreshServicelist()
        {
            try
            {
                //ServiceListPopup.ItemSelected += async (sender, e) =>
                //{
                //    if (e.SelectedItem != null)
                //    {
                //        string selectedItem = e.SelectedItem.ToString();

                //        // clear selected item
                //        ServiceListPopup.SelectedItem = null;
                //    }
                //};   
            }
            catch (Exception es)
            {

            }

        }

        private void ServiceButton_Clicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
            var itemSelected = item.CommandParameter;
            stPrice.IsVisible = true;
            stPriceBlank.IsVisible = false;
            foreach (var item1 in list)
            {
                if (item1 == itemSelected)
                {
                    item1.Status = "selected";
                    viewModel.btnText = item1.ServiceName;
                    viewModel.SelectedServicePrice = item1.ServicePrice;
                    viewModel.SelectedServiceId = item1.ServiceID;
                    viewModel.BarberIdForPanel = item1.SelectBarberID;

                }
                else
                {
                    item1.Status = "unselected";
                }
            }
            ServiceListPopup.ItemsSource = list;
            viewModel.PriceBlankOrNot = true;
        }

        private void ServiceButton_Clicked_1(object sender, EventArgs e)
        {
            var item = (Button)sender;
            var list = BarberListPopup.ItemsSource as ObservableCollection<ViewModel.HomeManagement.BarbersListModel>;
            var itemSelected = item.CommandParameter;
            var _selectedBarberId = 0;

            foreach (var item1 in list)
            {
                if (item1 == itemSelected)
                {
                    item1.Status = "selected";
                    viewModel.barberBtnText = item1.BarberName;
                    _selectedBarberId = item1.BarberId;
                }
                else
                {
                    item1.Status = "unselected";
                }
            }
            BarberListPopup.ItemsSource = list;
            BindServiceList(_selectedBarberId);

            btnServicePopup.IsEnabled = true;
            BarberListPopup.SelectedItem = null;

        }


    }
}