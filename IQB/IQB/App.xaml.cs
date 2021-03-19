using Acr.UserDialogs;

using Com.OneSignal;
using Com.OneSignal.Abstractions;

using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.Views;
using IQB.Views.AdminManagement;
using IQB.Views.ApplicationManagement;
using IQB.Views.MenuItems;
using IQB.Views.TabPagers;

using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace IQB
{
    public partial class App : Application
    {
        private static Application app;

        public static Application CurrentApp
        {
            get { return app; }
        }

        public static bool IsApplicationAwake { get; set; }

        public static string DeviceToken { get; set; }

        public static bool IsUserLoggedIn { get; set; }

        public static bool IsOnHomeScreen { get; set; }
        public static ToastConfig toastConfig { get; set; }
        public App()
        {
            InitializeComponent();

            //Register Syncfusion license

            // Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTExNTU0QDMxMzcyZTMxMmUzMGtGek5DclkwT09XTTZMeFM2N3UwalBPc0t1bjNPSkd3cjJGc2l0T20wbVk9");
            try
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTI5Njc4QDMxMzcyZTMyMmUzMFJDMUlhbkoxWEJvZWFHYVZUNjRNTEtSMFZKRm5HbGhaVlJ6RTNXbTZaSWM9");

                WriteErrorLog errLog = new WriteErrorLog();
                app = this;
                toastConfig = new ToastConfig(string.Empty);
                toastConfig.SetDuration(3000);
                toastConfig.SetBackgroundColor((Color)CurrentApp.Resources["ValidationBackground"]);
                toastConfig.Position = ToastPosition.Top;
                bool IsSalonCodeAuthenticated = false;
                bool IsLoginRemembered = false;
                IsUserLoggedIn = false;

                OneSignal.Current.IdsAvailable(IdsAvailable);

                using (SalonViewModel obj = new SalonViewModel())
                {
                    IsSalonCodeAuthenticated = obj.IsSalonAuthenticated();
                }

                //using (LoginViewModel obj = new LoginViewModel())
                //{
                //    IsLoginRemembered = obj.IsLoginCredRemembered();
                //}
                errLog.WinrtErrLogTest(IsSalonCodeAuthenticated.ToString(), IsLoginRemembered.ToString());

                if (IsSalonCodeAuthenticated)
                {
                    if (IsLoginRemembered)
                    {
                        App.GoToRoot();
                    }
                    else
                    {
                        App.GoToLoginRoot();
                    }
                }
                else
                {
                    App.GoToMainRoot();
                }

                //            //Remove this method to stop OneSignal Debugging  
                //            OneSignal.Current.SetLogLevel(LOG_LEVEL.VERBOSE, LOG_LEVEL.NONE);

                //            OneSignal.Current.StartInit("62bf335c-1418-47be-9252-ceccce2774e3")
                //            .Settings(new Dictionary<string, bool>() {
                //{ IOSSettings.kOSSettingsKeyAutoPrompt, false },
                //{ IOSSettings.kOSSettingsKeyInAppLaunchURL, false } })
                //            .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                //            .EndInit();

                //            // The promptForPushNotificationsWithUserResponse function will show the iOS push notification prompt. We recommend removing the following code and instead using an In-App Message to prompt for notification permission (See step 7)
                //            OneSignal.Current.RegisterForPushNotifications();
            }
            catch (Exception ex)
            {
            }
        }

        private void IdsAvailable(string userID, string pushToken)
        {
            App.DeviceToken = userID;
        }

        public static void GoToMainRoot()
        {
            CurrentApp.MainPage = new NavigationPage(new WelcomeScreen())
            {
                //BarBackgroundColor = Color.FromHex("#758094"),
                //BarTextColor = Color.White,

                //BarBackgroundColor =(Color)Application.Current.Resources["AppBackground"],
                //BarTextColor = Color.White,
            };
        }

        public static void GoToRoot(bool IsRedirectedToProfile = false)
        {
            CurrentApp.MainPage = new NavigationPage(new MainTabPage(false));
            //      CurrentApp.MainPage = new MenuMaster(IsRedirectedToProfile);
        }

        public static void GoToLoginRoot()
        {
            CurrentApp.MainPage = new NavigationPage(new Login());
            //CurrentApp.MainPage = new NavigationPage(new Views.AdminManagement.AddStaffMember());
        }

        public static void GoToLoginRootSecondary()
        {
            CurrentApp.MainPage = new NavigationPage(new Login());
        }

        //public static async void showNotification(string Title, string Message)
        //{
        //    await CurrentApp.MainPage.DisplayAlert(Title, Message, "ok");

        //    if (IsUserLoggedIn)
        //    {
        //        App.GoToRoot();
        //    }
        //}

        public static void showNotificationAndroid(string Title, string Message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await CurrentApp.MainPage.DisplayAlert(Title, Message, "ok");

                //Following code is commented to close the redirection to home page after tap on push ok message as per client feedback.
                //if (IsUserLoggedIn)
                //{
                //    App.GoToRoot();
                //}

                if (IsUserLoggedIn)
                {
                    if (Title.Contains("Appointment"))
                    {
                        CurrentApp.MainPage = new NavigationPage(new MainTabPage(true));
                    }
                    else
                    {
                        CurrentApp.MainPage = new NavigationPage(new MainTabPage(false));
                    }
                }
            });
        }

        //public static async void redirectToHomeScreen(string Title, string Message)
        //{
        //    await CurrentApp.MainPage.DisplayAlert(Title, Message, "ok");

        //    if (IsUserLoggedIn)
        //    {
        //        App.GoToRoot();
        //    }
        //}

        protected override void OnStart()
        {
            // Handle when your app starts
            IsApplicationAwake = true;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            IsApplicationAwake = false;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            IsApplicationAwake = true;
        }
    }
}