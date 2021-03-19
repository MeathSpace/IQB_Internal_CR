using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.Utility;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.HomeManagement;
using IQB.Views.TabPagers;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarberServiceList : ContentPage
    {
        HomeViewModel homeViewModel = null;
        public string isSingleJoinData = string.Empty;
        public BarberServiceList(int barberID = 0, string barberName = "",string isSingleJoin=null)
        {
            InitializeComponent();
            homeViewModel = new HomeViewModel();

            BindingContext = homeViewModel;
            BindServiceList(barberID, barberName);
            isSingleJoinData = isSingleJoin;
            //PopulateBarberImage();
        }

        private async void PopulateBarberImage()
        {
            ApiResult res = new ApiResult();

            using (BarberService obj = new BarberService())
            {
                res = await obj.BarberSelect(Convert.ToInt32(Application.Current.Properties["_SalonId"]));
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberReturnResult> result = UtilityEL.ToModelList<BarberReturnResult>(res.Response);
                var barber = result.Where(x => x.BarberId == homeViewModel.SelectBarberID).FirstOrDefault();


                if(barber!=null)
                {
                    if (string.IsNullOrEmpty(barber.BarberPic))
                    {
                        homeViewModel.SelectBarberImage = "noimage.png";
                        // ProfileImageNew1.src
                    }
                    else
                    {
                        homeViewModel.SelectBarberImage = CommonEL.BarberImageLocation + "barbers_profile_pics" + "/" + barber.BarberPic;
                    }
                }
                else
                {
                    homeViewModel.SelectBarberImage = "noimage.png";
                }
                

            }
            else
            {
                homeViewModel.SelectBarberImage = "noimage.png";
            }

        }
        private async void JoinQueue_Clicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (homeViewModel.ServiceId != 0)
                {
                    //gdPopup.IsVisible = false;
                    //spHome.Opacity = 1;
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
                                                    is_single_join = isSingleJoinData,
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
                                                        await App.Current.MainPage.DisplayAlert("Success!", "You have successfully joined the queue" , "OK");

                                                   //     await homeViewModel.SetHomeData(true, 0, 0);
                                                    //    await Navigation.PushAsync(new MainTabPage(false));
                                                       await Navigation.PopModalAsync();
                                                        MessagingCenter.Send("JoinedQ", "JoinedQSender");
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
                //           await Navigation.PopAsync();
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
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
                    item1.LabelColor = Color.FromHex("#878787");
                    item1.SourceTickImage = "resource://IQB.Resources.Image.JoinUntick.svg";
                    item1.IsServiceChecked = false;
                    homeViewModel.ServiceId = 0;

                }
                ServiceListPopup.ItemsSource = list;
            }
            catch (Exception es)
            {
                errLog.WinrtErrLogTest(es);
            }

        }

        private async void CrossIconTapped(object sender, EventArgs e)
        {

            await Navigation.PopModalAsync();
        }

        private void PickPictureTapped(object sender, EventArgs e)
        {

        }

        private void ServiceListPopup_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            ServiceListPopup.SelectedItem = null;
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
                var itemSelected = (ServiceListModel)e.Item;
                //item1.Status = "selected";
                //itemSelected.IsServiceChecked = true;
                //itemSelected.LabelColor = Color.FromHex("#0DA5B7");

                //     var itemSelected = ServiceListPopup.SelectedItem as ServiceListModel;

                foreach (var item1 in list)
                {
                    if (item1 == itemSelected)
                    {
                        //item1.Status = "selected";
                        if(!item1.IsServiceChecked)
                        {
                            item1.IsServiceChecked = true;
                            item1.LabelColor = Color.FromHex("#0DA5B7");
                            item1.SourceTickImage= "resource://IQB.Resources.Image.JoinTick.svg";
                            homeViewModel.ServiceId = itemSelected.ServiceID;

                        }
                        else
                        {
                            item1.LabelColor = Color.FromHex("#878787"); item1.SourceTickImage = "resource://IQB.Resources.Image.JoinUntick.svg"; item1.IsServiceChecked = false; homeViewModel.ServiceId = 0;
                        }
                        
                    }
                    else
                    { item1.LabelColor = Color.FromHex("#878787"); item1.SourceTickImage = "resource://IQB.Resources.Image.JoinUntick.svg"; item1.IsServiceChecked = false; }
                        
                }
                ServiceListPopup.ItemsSource = list;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        //private void selectService_Changed(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        //{
        //    WriteErrorLog errLog = new WriteErrorLog();
        //    try
        //    {


        //        var list = ServiceListPopup.ItemsSource as ObservableCollection<ServiceListModel>;
        //        var Serviceid = ((ServiceListModel)((SfCheckBox)sender).Parent.BindingContext).ServiceID;
        //        homeViewModel.ServiceId = Serviceid;
               
        //    }
        //    catch (Exception ex)
        //    {
        //        errLog.WinrtErrLogTest(ex);
        //    }
        //}

        private async void BindServiceList(int barberID, string barberName)
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
                    if (barberID == 0)
                        res = await obj.GetServicesByBarberIdSalonId(777777, salonId);
                    else
                        res = await obj.GetServicesByBarberIdSalonId(barberID, salonId);

                }

                if (res != null && res.StatusCode == 200)
                {
                    List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                    if (result.Count > 0)
                    {
                        //LabelServiceCount.Text = result.Count.ToString() + ((result.Count > 1) ? " services are avilable" : " service is avilable");
                        LabelServiceCount.Text = result.Count.ToString() + " Service(s) Available";
                        foreach (var item in result)
                        {
                            ServiceListModel Service = new ServiceListModel();
                            Service.ServiceID = item.ServiceId;
                            Service.ServiceName = item.ServiceName;
                            Service.ServicePrice = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;
                            Service.LabelColor = Color.FromHex("#878787");
                            Service.SourceTickImage = "resource://IQB.Resources.Image.JoinUntick.svg";
                            TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.Estimated_wait_time));
                            //Service.ServiceEstimatedTime = timeSpan.ToString(@"hh\:mm") + " mins";
                            Service.ServiceEstimatedTime = (int)timeSpan.Hours + " hrs : " + (int)timeSpan.Minutes + " mins ";
                            Service.Status = "unselected";
                            Service.IsServiceChecked = false;
                            if (!string.IsNullOrEmpty(barberName))
                                homeViewModel.SelectBarberName = barberName;
                            else
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

                       
                        frmNoRecord.IsVisible = false;
                        ServiceListPopup.IsVisible = true;
                    }
                    else
                    {
                        frmNoRecord.IsVisible = true;
                        ServiceListPopup.IsVisible = false;
                    }
                    PopulateBarberImage();
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


        private async void AutoJoinForNoService()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                // gdPopup.IsVisible = false;
                // spHome.Opacity = 1;
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

       
    }
}