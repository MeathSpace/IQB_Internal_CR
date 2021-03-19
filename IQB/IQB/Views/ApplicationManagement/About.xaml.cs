using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.HomeModels;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.HomeManagement;
using IQB.Views.Settings;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public AboutViewModel viewModel = null;
        public int rate = 0;
        public string salonname = string.Empty;
        public string username = string.Empty;
        public string personname = string.Empty;
        public int salonid = 0;
        private SalonViewModel salonViewModel = null;
        public About()
        {
            InitializeComponent();
            this.salonViewModel = new SalonViewModel();
            BindingContext = salonViewModel;
        }
        private void About_OnAppearing(object sender, EventArgs e)
        {
            //if (CheckUserAlreadyRated())
            //{
            //    RateSlider.Value =
            //}
        }
        private void OnTapGestureRecognizerTel(object sender, EventArgs args)
        {
            Device.OpenUri(new Uri($"tel:{salonViewModel.TelephoneNo}"));
        }

        private async void OnEmailClicked(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new Email("About Salon Query","",true), true);
        }

        private async void OnHelpClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Help(), true);
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

        private async void RateSalon_Clicked(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            string validationMessage = string.Empty;
            try
            {
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
                                    salonid = salonid,
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
                                            validationMessage = "You have already rated us. \n Thank you for your rate!";
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
            }
            catch (Exception ex)
            {
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

        private async void OnAddressLocationClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddressAndLocation(), true);
        }

        private void OnTapGestureRecognizerWeb(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(salonViewModel.Link))
                Device.OpenUri(new Uri("http://" + salonViewModel.Link));
        }
    }
}
