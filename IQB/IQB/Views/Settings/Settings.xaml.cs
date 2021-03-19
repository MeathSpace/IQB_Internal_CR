using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.ApplicationManagement;
using System;

using Xamarin.Forms;

namespace IQB.Views.Settings
{
    public partial class Settings : ContentView
    {

        bool IsTapped = false;

        public Settings()
        {
            InitializeComponent();
        }

        private async void OnAppInfoTapped(object sender, EventArgs args)
        {
            if (!IsTapped)
            {
                IsTapped = true;
                await Navigation.PushAsync(new AppInfo());
                IsTapped = false;
            }
        }

        private async void OnResetApplicationTapped(object sender, EventArgs args)
        {
            if (!IsTapped)
            {
                IsTapped = true;
                var answer = await App.Current.MainPage.DisplayAlert("Alert!", "Are you sure you want to change the location?", "Yes", "No");
                if (answer)
                {
                    LoginViewModel.DestroyLoginCredData();
                    SalonViewModel.DestroySalonCodeData();

                    App.GoToMainRoot();
                }
                IsTapped = false;
            }
        }

        private async void OnDeleteApplicationTapped(object sender, EventArgs args)
        {
            string userName = string.Empty;
            string imageName = string.Empty;
            int salonID = 0;
            if (!IsTapped)
            {
                IsTapped = true;

                var answer = await App.Current.MainPage.DisplayAlert("Alert!", "Are you sure want to delete your account?", "Yes", "No");
                if (answer)
                {
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        SalonViewModel salonobj = new SalonViewModel();
                        salonID = salonobj.GetSalonId();
                        LoginViewModel loginobj = new LoginViewModel();
                        userName = Convert.ToString(loginobj.GetUserId());
                        string imagePath = loginobj.GetUserProfileImageName();
                        string[] partPaths = imagePath.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if (partPaths != null && partPaths.Length > 0)
                        {
                            imageName = partPaths[partPaths.Length - 1];
                        }

                        SettingsModel viewModel = new SettingsModel()
                        {
                            salonid = salonID,
                            username = userName,
                            imagename = imageName
                        };
                        //Call delete account
                        SettingsService serviceobj = new SettingsService();
                        ApiResult res = new ApiResult();
                        res = await serviceobj.DeleteAccount(viewModel);
                        if (res != null && res.StatusCode == 200)
                        {
                            await App.Current.MainPage.DisplayAlert("Success!", Convert.ToString(res.Response), "Ok");
                            //Destroy login store data
                            LoginViewModel.DestroyLoginCredData();
                            //Go to login screen
                            App.GoToLoginRootSecondary();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error!", Convert.ToString(res.Response), "Ok");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    }
                }
            }
            IsTapped = false;
        }

        private async void OnSalonInfoTapped(object sender, EventArgs args)
        {
            if (!IsTapped)
            {
                IsTapped = true;
                using (SalonViewModel obj = new SalonViewModel())
                {
                    obj.IsConnectShowed = false;
                    await Navigation.PushAsync(new SalonInfo(obj), true);
                }
                IsTapped = false;
            }
        }

    }
}