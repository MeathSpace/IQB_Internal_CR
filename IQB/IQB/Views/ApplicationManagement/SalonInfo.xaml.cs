using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.HomeModels;
using IQB.EntityLayer.SalonModels;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.IQBServices.SalonServices;
using IQB.Utility;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IQB.IQBServices.AdminServices;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberModel;
using IQB.Views.PhotoGallery;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalonInfo : ContentPage
    {
        public int rate = 0;
        public string salonname = string.Empty;
        public string username = string.Empty;
        public string personname = string.Empty;
        public int salonid = 0;
        private SalonServices objSalonService = new SalonServices();
        SalonDetailsResponse salonDetailsResponse = new SalonDetailsResponse();

        private SalonViewModel salonViewModel = null;
        public SalonInfo(SalonViewModel _salonViewModel)
        {
            InitializeComponent();
            //btnConnect.BackgroundColor = Color.FromHex("#758094");            
            this.salonViewModel = _salonViewModel;
            BindingContext = salonViewModel;
            if(!String.IsNullOrEmpty(salonViewModel.Salon_ImageName))
            SalonMainImage.Source = new UriImageSource
            {
                Uri = new Uri(salonViewModel.Salon_ImageName),
                CachingEnabled = false,
            };
            GetBarbersList();
            GetSocialNetworkInfo();
            RateContainer.IsVisible = false;
            if (!salonViewModel.IsConnectShowed)
            {
                RateContainer.IsVisible = true;
                if (CheckUserAlreadyRated())
                {
                    using (LoginViewModel objLogin = new LoginViewModel())
                    {
                        RateSlider.Value = double.Parse(objLogin.GetRateScore());
                        RateSlider.IsEnabled = false;
                        RateSalon.IsEnabled = false;
                        RateSalon.BackgroundColor = Color.LightGray;
                    }
                }
            }
        }

        private async void GetSocialNetworkInfo()
        {
            salonDetailsResponse = await objSalonService.GetSalonDetailsByID(salonViewModel.SalonId.ToString());
        }

        private void btnConnect_OnClick(object sender, EventArgs args)
        {
            //ConnectImage.IsEnabled = false;
            //ConnectLabel.IsEnabled = false;
            btnConnect.IsEnabled = false;
            salonViewModel.StoreSalonCode(salonViewModel.SCode, salonViewModel.SalonName, salonViewModel.SalonId, salonViewModel.Salon_ImageName, salonViewModel.TelephoneNo, salonViewModel.Link, salonViewModel.address, salonViewModel.state, salonViewModel.city, salonViewModel.PostalCode, salonViewModel.FtpFolderPath);
            App.GoToLoginRootSecondary();
            btnConnect.IsEnabled = true;
            //ConnectImage.IsEnabled = true;
            //ConnectLabel.IsEnabled = true;
        }

        private void OnTapGestureRecognizerTel(object sender, EventArgs args)
        {
            Device.OpenUri(new Uri(String.Format("tel:{0}", salonViewModel.TelephoneNo)));
        }

        private void OnTapGestureRecognizerWeb(object sender, EventArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(salonViewModel.Link))
                    Device.OpenUri(new Uri("http://" + salonViewModel.Link));
            }
            catch { }
        }

        private async void RateSalon_Clicked(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            string validationMessage = string.Empty;
            try
            {
                indicator.IsVisible = true;
                var IsLogin = Application.Current.Properties["UserName"] != null ? Application.Current.Properties["UserName"].ToString() : null;
                if (!string.IsNullOrEmpty(IsLogin))
                {
                    if (RateSlider.Value != 0)
                    {
                        if (NetworkConnectionMgmt.IsConnectedToNetwork())
                        {
                            bool ValidateRating = CheckUserAlreadyRated();
                            if (ValidateRating)
                            {
                                //RateSalon.IsEnabled = false;
                                //RateSlider.IsEnabled = false;
                                //RateText.Text = "You have already rated us. \n Thank you for your rate!";
                                validationMessage = "You have already rated us. \n Thank you for your rate!";
                            }
                            else
                            {
                                RatingEL ratingModel = new RatingEL()
                                {
                                    salonid = salonViewModel.SalonId,
                                    personname = personname,
                                    username = username,
                                    salonname = salonname,
                                    ratesystem = "Y",
                                    ratingscore = Convert.ToString(RateSlider.Value),
                                    //ratingscore = Convert.ToString(rate),
                                    daterated = DateTime.Now.ToString("dd/MM/yyyy")
                                };
                                ApiResult res = new ApiResult();
                                using (HomeService objHS = new HomeService())
                                {
                                    res = await objHS.UpdateCustomerRateSalon(ratingModel);
                                    if (res != null && res.StatusCode == 200)
                                    {
                                        res = await objHS.InsertCustomerRateSalon(ratingModel);
                                        if (res != null && res.StatusCode == 200)
                                        {
                                            await DisplayAlert("Alert!", "Thank you for rating us!", "OK");
                                            LoginViewModel obj = new LoginViewModel();
                                            obj.StoreLoginCredData(obj.GetUserName(), obj.GetUserName(), obj.GetUserProfileImageName(), obj.GetIsRemembered(), obj.GetUserId(), obj.GetFirstName(), obj.GetLastName(), obj.GetPassword(), obj.GetDOB(), obj.GetMobileNo(), ratingModel.ratesystem, ratingModel.ratingscore, obj.GetMailInfo());
                                            //RateSalon.IsEnabled = false;
                                            //RateSlider.IsEnabled = false;
                                            //RateText.Text = "You have already rated us. \n Thank you for your rate!";
                                            //validationMessage = "You have already rated us. \n Thank you for your rate!";
                                        }
                                        else
                                        {
                                            await DisplayAlert("Alert!", "Oops! Something went wrong. Please try after sometime.", "OK");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Alert!", "Oops! Something went wrong. Please try after sometime.", "OK");
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(validationMessage))
                            {
                                App.toastConfig.Message = validationMessage;
                                UserDialogs.Instance.Toast(App.toastConfig);
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Alert!", "Please select a rating", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Alert!", "Please login!", "OK");
                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
                if (ex.Message.Contains("UserName"))
                {
                    await DisplayAlert("Alert!", "Please login!", "OK");
                }
                else
                {
                    await DisplayAlert("Alert!", "Oops! Something went wrong. Please try after sometime.", "OK");
                }

            }
        }

        public bool CheckUserAlreadyRated()
        {
            bool ValidateRate = false;
            string ratingsystem = string.Empty;
            using (LoginViewModel objLogin = new LoginViewModel())
            {
                ratingsystem = objLogin.GetRateSystem();
                username = Convert.ToString(objLogin.GetUserId());
                personname = objLogin.GetFirstName() + " " + objLogin.GetLastName();
            }
            if (ratingsystem == "Y")
            {
                ValidateRate = true;
            }
            else
            {
                ValidateRate = false;
            }
            return ValidateRate;
        }

        public async void GetBarbersList()
        {

            int salonId = salonViewModel.SalonId;
            //using (SalonViewModel objSalon = new SalonViewModel())
            //{
            //    salonId = objSalon.GetSalonId();
            //}
            //List<BarberReturnResult> result = new List<BarberReturnResult>();
            ApiResult res = new ApiResult();
            BarberListContainer.IsVisible = false;
            using (BarberSalonService obj = new BarberSalonService())
            {
                res = await obj.GetBarberBySalonId(salonId);
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberListSalonPage> BarberlistItems = new List<BarberListSalonPage>();
                List<BarberReturnResult> result = UtilityEL.ToModelList<BarberReturnResult>(res.Response);

                foreach (var x in result)
                {
                    BarberListSalonPage obj = new BarberListSalonPage();
                    obj.BarberName = x.BarberName;
                    obj.BarberId = x.BarberId;
                    //if (string.IsNullOrEmpty(x.BarberPic))
                    //{
                    //    obj.BarberImage = "noimage.png";
                    //}
                    //else
                    //{
                    //    obj.BarberImage = CommonEL.BarberImageLocation + "barbers_profile_pics" + "/" + x.BarberPic;
                    //}
                    obj.BarberImage = x.BarberPic;
                    BarberlistItems.Add(obj);

                }

                if (BarberlistItems?.Count > 0)
                {
                    BarberListContainer.IsVisible = true;
                    BarbersListView.ItemsSource = BarberlistItems;
                }
            }

        }

        private async void GetBarberServiceList(int barberId)
        {
            try
            {
                ObservableCollection<ServiceListModel> objList = new ObservableCollection<ServiceListModel>();
                BarberServicesListView.IsRefreshing = true;
                BarberServicesListView.ItemsSource = null;

                // int salonId = 0;
                int salonId = salonViewModel.SalonId;
                //using (SalonViewModel objSalon = new SalonViewModel())
                //{
                //    salonId = objSalon.GetSalonId();
                //}

                ApiResult res = new ApiResult();
                using (BarberService obj = new BarberService())
                {
                    //res = await obj.GetAllServicesByBarberIdSalonId(barberId, salonId);
                    res = await obj.GetSalonBarberMapping(barberId, salonId);
                    
                }

                if (res != null && res.StatusCode == 200)
                {
                    List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);


                    if (result.Count != 0)
                    {
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
                        BarberServicesListView.ItemsSource = objList;
                        BarberServicesListView.IsRefreshing = false;
                    }
                    else
                    {
                        BarberServicesListView.IsRefreshing = false;
                        BarberServicesListView.IsVisible = false;
                        spNoService.IsVisible = true;
                    }

                }
                else if (res.StatusMessage == "No records found")
                {
                    BarberServicesListView.IsRefreshing = false;
                    BarberServicesListView.IsVisible = false;
                    spNoService.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                BarberServicesListView.IsRefreshing = false;
                BarberServicesListView.IsVisible = false;
                spNoService.IsVisible = true;
            }
        }

        private void BarbersListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            BarberListSalonPage x = (e.ItemData as BarberListSalonPage);
            BarberSerVicesPopUp.IsVisible = true;
            BarberServicesListView.IsRefreshing = true;
            BarberServicesListView.IsVisible = true;
            spNoService.IsVisible = false;
            GetBarberServiceList(x.BarberId);
            gdMain.Opacity = 0.1;
        }

        private void BarberServicesCancelTapped(object sender, EventArgs e)
        {
            gdMain.Opacity = 1;
            BarberSerVicesPopUp.IsVisible = false;
        }

        private void ServiceListPopup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BarberServicesListView.SelectedItem = null;
        }

        private async void UploadTwitterLinkTapped(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(salonDetailsResponse.Response.SalonTwitter))
                Device.OpenUri(new Uri("http://" + salonDetailsResponse.Response.SalonTwitter));
            // await DisplayAlert("Twitter", salonDetailsResponse.Response.SalonTwitter, "OK");
        }

        private async void UploadFacebookLinkTapped(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(salonDetailsResponse.Response.SalonFacebook))
                Device.OpenUri(new Uri("http://" + salonDetailsResponse.Response.SalonFacebook));
            //await DisplayAlert("Twitter", salonDetailsResponse.Response.SalonFacebook, "OK");
        }

        private async void UploadInstagramLinkTapped(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(salonDetailsResponse.Response.SalonInstagram))
                Device.OpenUri(new Uri("http://" + salonDetailsResponse.Response.SalonInstagram));
            //await DisplayAlert("Twitter", salonDetailsResponse.Response.SalonInstagram, "OK");
        }

        private async void NavigateToHelp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Help());
        }

        private async void BackButtonTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnTapGestureRecognizerImage(object sender, EventArgs e)
        {
            int salonId = salonViewModel.SalonId;
            await Navigation.PushAsync(new PhotoGalleryPage(salonId, salonViewModel.IsConnectShowed));
        }
    }

    public class BarberListSalonPage
    {
        public int BarberId { get; set; }
        public string BarberName { get; set; }
        public string BarberImage { get; set; }
    }
}