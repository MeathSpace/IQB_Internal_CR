using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.HomeManagement;
using IQB.Views.AdminManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.Views.Home
{
    public partial class BarberList : ContentPage
    {
        public BarberViewModel viewModel = null;
        private HomeViewModel homeViewModel = null;
        int CountSwitchOn;
        string _Servicename = string.Empty;

        // bool IsSwitchOn = true;

        // private ObservableCollection<BarberReturnResult> objList = null;

        //public Task PageClosedTask { get { return tcs.Task; } }
        //private TaskCompletionSource<bool> tcs { get; set; }

        public BarberList(HomeViewModel homeViewModel)
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string>(this, "JoinedQSender", async(abc) =>
            {
                try
                {
                    await Navigation.PopAsync();
                   
                }
                catch(Exception ex)
                {

                }
               
                
            });

            //BindServiceList();

            //ServiceListPopup.ItemsSource = objList;

            this.viewModel = new BarberViewModel();
            BindingContext = viewModel;
            barberList.ItemsSource = viewModel.BarberList;
            this.homeViewModel = homeViewModel;
            //tcs = new TaskCompletionSource<bool>();

            barberList.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

            OnBackButtonPressed();
        }

        //private void SetServicePopup()
        //{
        //    ObservableCollection<BarberReturnResult> objList = new ObservableCollection<BarberReturnResult>();

        //    objList.Add(new BarberReturnResult() { id = 1, BarberName = "Half Fade", Position = "£ 10.00" });
        //    objList.Add(new BarberReturnResult() { id = 2, BarberName = "Clipper Cut", Position = "£ 15.00" });
        //    objList.Add(new BarberReturnResult() { id = 3, BarberName = "High Fade Quiff", Position = "£ 30.00" });
        //    objList.Add(new BarberReturnResult() { id = 4, BarberName = "Messy Undercut", Position = "£ 35.00" });

        //    ServiceListPopup.ItemsSource = objList;

        //}

        private async void BindServiceList(int barberID)
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
                res = await obj.GetServicesByBarberIdSalonId(barberID, salonId);
            }

            if (res != null && res.StatusCode == 200)
            {
                List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                foreach (var item in result)
                {
                    ServiceListModel Service = new ServiceListModel();
                    Service.ServiceName = item.ServiceName;
                    //Service.ServicePrice = "Price:£" +item.ServicePrice;
                    Service.ServicePrice = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;
                    //Service.ServicePrice = "£" + item.ServicePrice;

                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.Estimated_wait_time));
                    Service.ServiceEstimatedTime = (int)timeSpan.TotalHours + " hrs : " + (int)timeSpan.TotalMinutes + " mins ";
                    //Service.ServiceEstimatedTime = timeSpan.ToString(@"hh\:mm") + " mins";

                    //Service.ServiceEstimatedTime = item.Estimated_wait_time + "mins";
                    Service.Status = "unselected";
                    Service.ServiceID = item.ServiceId;
                    objList.Add(Service);
                }

                ServiceListPopup.ItemsSource = objList;
                lblServiceCount.Text = "(" + Convert.ToString(objList.Count) + " service(s) available)";
            }
            else
            {
                gdPopup.IsVisible = false;
                JoinQueue_Noservice();
            }

            viewModel.IsRefreshing = false;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void OnAlertJoinQueueClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            var chosen_barber = btn.CommandParameter as ViewModel.HomeManagement.BarbersListModel;
            int barberid = chosen_barber.BarberId;
            if (chosen_barber.BarberOnline == "yes")
            {
                //StBarberList.Opacity = 0.1;
                //gdPopup.IsVisible = true;

                //BindServiceList(barberid);

                //lblBarberName.Text = chosen_barber.BarberName;
                //lblBarberID.Text = Convert.ToString(barberid);

                await Navigation.PushModalAsync(new BarberServiceList(barberid, chosen_barber.BarberName,"yes"));
            }
            else
            {
                await DisplayAlert("Alert!", "Barber is currently offline", "OK");
            }
        }

        private async void OnBarberSelectClosed(object sender, EventArgs args)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                homeViewModel.IsModalOpen = false;
                //tcs.SetResult(true);
                await Navigation.PopModalAsync();
            }
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (CountSwitchOn > 0)
            {
                var answer = await App.Current.MainPage.DisplayAlert("", "You can select only one service", "Ok", "Cancel");
                if (answer == true)
                {
                    CountSwitchOn--;
                    //BindServiceList();
                }
            }
            else
            {
                CountSwitchOn++;
                //IsSwitchOn = false;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void ServiceListPopup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
            var itemSelected = ServiceListPopup.SelectedItem;

            foreach (var item1 in list)
            {
                if (item1 == itemSelected)
                {
                    item1.Status = "selected";
                    Application.Current.Properties["_ServiceName"] = item1.ServiceName;
                    _Servicename = Application.Current.Properties["_ServiceName"] as string;
                    ConnectLabel.CommandParameter = itemSelected;
                    Application.Current.Properties["SelectedServiceName"] = item1.ServiceName;
                }
                else
                {
                    item1.Status = "unselected";
                    ConnectLabel.CommandParameter = null;

                }
            }
            ServiceListPopup.ItemsSource = list;
        }

        private void OnSelectClosed(object sender, EventArgs e)
        {
            gdPopup.IsVisible = false;
            StBarberList.Opacity = 1;
        }

        private void ServiceName_Clicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
            var itemSelected = item.CommandParameter;

            foreach (var item1 in list)
            {
                if (item1 == itemSelected)
                {
                    item1.Status = "selected";
                    ConnectLabel.CommandParameter = itemSelected;
                    Application.Current.Properties["SelectedServiceName"] = item1.ServiceName;

                }
                else
                {
                    item1.Status = "unselected";
                    ConnectLabel.CommandParameter = null;

                }
            }
            ServiceListPopup.ItemsSource = list;
        }

        private async void gdPopupCancel_Tapped(object sender, EventArgs e)
        {
            gdPopup.IsVisible = false;
            StBarberList.Opacity = 1;



        }

        private async void ConnectLabel_Clicked(object sender, EventArgs e)
        {
            if (ServiceListPopup.SelectedItem != null || ConnectLabel.CommandParameter != null)
            {
                var GetServiceidfromServiceListPopUp = ServiceListPopup.SelectedItem as ServiceListModel;
                var GetServiceidfromConnectLabel = ConnectLabel.CommandParameter as ServiceListModel;
                var ServiceID = GetServiceidfromServiceListPopUp == null ? GetServiceidfromConnectLabel.ServiceID : GetServiceidfromServiceListPopUp.ServiceID;


                gdPopup.IsVisible = false;
                StBarberList.Opacity = 1;

                if (viewModel.IsEnabled)
                {
                    viewModel.IsEnabled = false;
                    viewModel.IsBackIconEnabled = false;
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        Button btn = (Button)sender;
                        var chosen_service = btn.CommandParameter as ServiceListModel;
                        chosen_service = chosen_service ?? new ServiceListModel();
                        _Servicename = chosen_service.ServiceName == null ? _Servicename : chosen_service.ServiceName;
                        string barbername = lblBarberName.Text;
                        int barberid = Convert.ToInt16(lblBarberID.Text);
                        int salonId = 0;
                        string userName = string.Empty;
                        string FirstLastName = string.Empty;
                        bool retResult = false;
                        string _SelectedServiceName = Application.Current.Properties["SelectedServiceName"] as string;
                        ApiResult res = new ApiResult();
                        var answer = await DisplayAlert("Alert!", "Are you sure you want to join the queue with " + barbername + " and get a " + _SelectedServiceName + " service?", "Yes", "No");
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
                                //Insert functionality for JoinQueue
                                int position = 0;
                                using (BarberService obj = new BarberService())
                                {
                                    res = await obj.CheckQueuePosition(salonId);
                                    if (res != null && res.StatusCode == 200)
                                    {
                                        position = Convert.ToInt32(res.Response);
                                        position += 1;
                                        UserQueueJoinInsertModel singleJoinModel = new UserQueueJoinInsertModel()
                                        {
                                            position = position,
                                            username = userName,
                                            firstlastname = FirstLastName,
                                            rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy"),
                                            timejoinedq = DateTime.Now.ToString("H:mm:ss"),
                                            barbername = barbername,
                                            salonid = salonId,
                                            qgcode = "N/A",
                                            is_single_join = "yes",
                                            ServiceId = ServiceID,
                                            //ServiceId = chosen_service.ServiceID
                                            BarberId = barberid
                                        };
                                        res = await obj.InsertInJoinQueue(singleJoinModel);
                                        if (res != null && res.StatusCode == 200)
                                        {

                                            int success = Convert.ToInt32(res.Response);
                                            if (success == 1)
                                            {
                                                homeViewModel.HasResultChangeInModal = true;

                                                //await DisplayAlert("Success!", res.StatusMessage, "OK");

                                                //homeViewModel.IsShowApiRunningLoader = true;
                                                homeViewModel.IsWaitingToJoinQueue = true;
                                                homeViewModel.ShowQueueNotJoinedSections = false;

                                                homeViewModel.SetHomeData(true, ServiceID, Convert.ToInt32(lblBarberID.Text));
                                            }
                                            else if (success == 0)
                                            {
                                                await DisplayAlert("Error!", res.StatusMessage, "OK");
                                            }

                                            //tcs.SetResult(true);
                                            homeViewModel.IsModalOpen = false;
                                            await Navigation.PopModalAsync();

                                        }
                                        else
                                        {
                                            await DisplayAlert("Error!", res.StatusMessage, "OK");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Error!", res.StatusMessage, "OK");
                                    }
                                }
                            }
                            viewModel.IsLoaderEnabled = false;
                            gdGroupJoin.Opacity = 1;
                        }
                        ServiceListPopup.SelectedItem = null;
                    }
                    else
                    {
                        await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    }
                    viewModel.IsEnabled = true;
                    viewModel.IsBackIconEnabled = true;
                }
            }
            else
            {
                await DisplayAlert("Alert", "Please select a service", "OK");
            }
        }

        private async void JoinQueue_Noservice()
        {
            StBarberList.Opacity = 1;

            if (viewModel.IsEnabled)
            {
                viewModel.IsEnabled = false;
                viewModel.IsBackIconEnabled = false;
                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    string barbername = lblBarberName.Text;
                    int barberid = Convert.ToInt16(lblBarberID.Text);
                    int salonId = 0;
                    string userName = string.Empty;
                    string FirstLastName = string.Empty;
                    bool retResult = false;
                    ApiResult res = new ApiResult();
                    var answer = await DisplayAlert("Alert!", "Are you sure you want to join the queue with " + barbername + "", "Yes", "No");
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
                            //Insert functionality for JoinQueue
                            int position = 0;
                            using (BarberService obj = new BarberService())
                            {
                                res = await obj.CheckQueuePosition(salonId);
                                if (res != null && res.StatusCode == 200)
                                {
                                    position = Convert.ToInt32(res.Response);
                                    position += 1;
                                    UserQueueJoinInsertModel singleJoinModel = new UserQueueJoinInsertModel()
                                    {
                                        position = position,
                                        username = userName,
                                        firstlastname = FirstLastName,
                                        rdatejoinedq = DateTime.Now.ToString("dd/MM/yyyy"),
                                        timejoinedq = DateTime.Now.ToString("H:mm:ss"),
                                        barbername = barbername,
                                        salonid = salonId,
                                        qgcode = "N/A",
                                        is_single_join = "yes",
                                        ServiceId = 0,
                                        BarberId = barberid
                                    };
                                    res = await obj.InsertInJoinQueue(singleJoinModel);
                                    if (res != null && res.StatusCode == 200)
                                    {

                                        int success = Convert.ToInt32(res.Response);
                                        if (success == 1)
                                        {
                                            homeViewModel.HasResultChangeInModal = true;
                                            homeViewModel.IsWaitingToJoinQueue = true;
                                            homeViewModel.ShowQueueNotJoinedSections = false;
                                            homeViewModel.SetHomeData(true, 0, Convert.ToInt32(lblBarberID.Text));
                                        }
                                        else if (success == 0)
                                        {
                                            await DisplayAlert("Error!", res.StatusMessage, "OK");
                                        }
                                        homeViewModel.IsModalOpen = false;
                                        await Navigation.PopModalAsync();

                                    }
                                    else
                                    {
                                        await DisplayAlert("Error!", res.StatusMessage, "OK");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error!", res.StatusMessage, "OK");
                                }
                            }
                        }
                        viewModel.IsLoaderEnabled = false;
                        gdGroupJoin.Opacity = 1;
                    }
                    ServiceListPopup.SelectedItem = null;
                }
                else
                {
                    await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                }
                viewModel.IsEnabled = true;
                viewModel.IsBackIconEnabled = true;
            }
        }

        private void ServiceButton_Clicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
            var itemSelected = item.CommandParameter;

            foreach (var item1 in list)
            {
                if (item1 == itemSelected)
                {
                    item1.Status = "selected";
                    ConnectLabel.CommandParameter = item.CommandParameter;
                    Application.Current.Properties["SelectedServiceName"] = item1.ServiceName;
                }
                else
                {
                    item1.Status = "unselected";

                    // ConnectLabel.CommandParameter = null;
                }
            }
            ServiceListPopup.ItemsSource = list;
        }
    }
}