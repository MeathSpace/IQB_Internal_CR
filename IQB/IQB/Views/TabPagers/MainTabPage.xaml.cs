using IQB.EntityLayer.AppointmentModels;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.MenuManagement;
using IQB.Views.AdminManagement;
using IQB.Views.ApplicationManagement;
using IQB.Views.Appointment.Admin;
using IQB.Views.LoginManagement;
using IQB.Views.QueueList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQB.EntityLayer.Common;
using IQB.Views.MenuItems;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IQB.Utility;

namespace IQB.Views.TabPagers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabPage : ContentPage
    {
        public MasterDetailsViewModel viewModel = null;
        public static bool QueueListTimerStat { get; set; }
        public static bool HomeTimerStat { get; set; }

        public bool IsRedirectedToProfile = false;

        public MainTabPage(bool tabIndexPassed)
        {
            InitializeComponent();

            //if(Device.RuntimePlatform == Device.iOS)
            //{
            //    this.Padding = new Thickness(0, 0, 0, 20);
            //}

            //     if (Device.RuntimePlatform == Device.iOS)
            //     {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                MessagingCenter.Subscribe<string>(this, "MessagingProfileSender", (abc) =>
                   {

                       ProfileImg.Source = null;

                       using (LoginViewModel obj = new LoginViewModel())
                       {
                           ProfileImg.Source = new UriImageSource
                           {
                               Uri = new Uri(obj.GetUserProfileImageName()),
                               CachingEnabled = false
                           };
                       }
                   });


                if (tabIndexPassed)
                {
                    SfTabContainer.SelectedIndex = 3;

                    if (Application.Current.Properties["UserLevel"].ToString() == "2")  //Admin\
                    {
                        AppointmentsTabContentView.Content = new IQB.Views.Appointment.Admin.ManageAppointment();
                    }

                    else
                    {
                        AppointmentsTabContentView.Content = new IQB.Views.Appointment.Customer.ManageAppointment();
                    }
                }

                else
                {
                    if ((Application.Current.Properties["ModuleAvailability"].ToString() != "1"))
                    {
                        if (Application.Current.Properties["UserLevel"].ToString() == "2")  //Admin\
                        {
                            AppointmentsTabContentView.Content = new IQB.Views.Appointment.Admin.ManageAppointment();
                        }

                        else
                        {
                            //     AppointmentsTabContentView.Content = new CustAppointmentView();
                            AppointmentsTabContentView.Content = new IQB.Views.Appointment.Customer.ManageAppointment();
                        }
                    }
                }


                this.viewModel = new MasterDetailsViewModel(IsRedirectedToProfile);

                using (LoginViewModel obj = new LoginViewModel())
                {
                    ProfileImg.Source = new UriImageSource
                    {
                        Uri = new Uri(obj.GetUserProfileImageName()),//todo
                        CachingEnabled = false
                    };
                }

                using (LoginViewModel obj = new LoginViewModel())
                {
                    UserNameLabel.Text = obj.GetName();
                }

                using (SalonViewModel obj = new SalonViewModel())
                {
                    UserDetailLabel.Text = obj.GetSalonName();
                }

                HomeTabContentView.Content = new Home.Home();

                SettingsTabContentView.Content = new SettingsView();
                if ((Application.Current.Properties["ModuleAvailability"].ToString() != "2"))
                {
                    QListTabContentView.Content = new QueueingList();
                }



            }
            catch(Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "MainTabPage.xaml", "MainPage");
            }

        }

        private async void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 3 && (Application.Current.Properties["UserLevel"].ToString() == "2"))
            {
                FilterIcon.IsVisible = true;
            }
            else
            {
                FilterIcon.IsVisible = false;
            }

            if (Application.Current.Properties["ModuleAvailability"].ToString() != "0")
            {
                if (e.Index == 2 && (Application.Current.Properties["ModuleAvailability"].ToString() == "2"))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
                }

                if (e.Index == 3 && (Application.Current.Properties["ModuleAvailability"].ToString() == "1"))
                {
                    await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
                }
            }
        }

        private async void NavigateToProfile(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyProfile(viewModel));
        }

        private async void HelpIconTapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new About());

            using (SalonViewModel obj = new SalonViewModel())
            {
                obj.IsConnectShowed = false;
                await Navigation.PushAsync(new SalonInfo(obj), true);
            }

        }

        private async void LogoutIconTapped(object sender, EventArgs e)
        {
            var ans = await App.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to Logout?", "Yes", "No");
            if (ans)
            {
                MenuMaster.HomeTimerStat = false;
                MenuMaster.QueueListTimerStat = false;
                //Device.StartTimer(TimeSpan.FromSeconds(6), () =>
                //{
                //    DelCredStore();
                //    return string.IsNullOrEmpty(GetCredStore());
                //});
                DelCredStore();
                await Navigation.PushAsync(new Logout());
            }
        }

        private async void FilterIconTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AppointmentFilter());
        }

        //private string GetCredStore()
        //{
        //    string result = string.Empty;

        //    Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName).FirstOrDefault();

        //    if (grpJoinedAccount != null)
        //    {
        //        result = grpJoinedAccount.Username;
        //    }

        //    return result;
        //}

        private void DelCredStore()
        {
            //if (!string.IsNullOrEmpty(GetCredStore()))
            {
                Account grpJoinedAccount = AccountStore.Create().FindAccountsForService(CommonEL.GroupJoinServiceName)
                    .FirstOrDefault();

                if (grpJoinedAccount != null)
                {
                    AccountStore.Create().Delete(grpJoinedAccount, CommonEL.GroupJoinServiceName);
                }
            }
            //else
            //{
            //    DelCredStore();
            //}
        }
    }
}

