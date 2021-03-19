using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.ApplicationManagement;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IQB.Views.Settings;
using System.Collections.ObjectModel;
using IQB.Views.AdminManagement;
using IQB.Views.Customer;
using IQB.Views.SalonManagement;
using IQB.Views.Appointment.Admin;
using IQB.Views.Report;

namespace IQB.Views.TabPagers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentView
    {
        bool IsTapped = false;
        ObservableCollection<SettingsViewModel> GeneralSettingsListCustomer { get; set; }
        ObservableCollection<SettingsViewModel> GeneralSettingsList { get; set; }
        ObservableCollection<SettingsViewModel> AdminSettingsList { get; set; }

        public SettingsView()
        {
            InitializeComponent();

            GeneralSettingsListCustomer = new ObservableCollection<SettingsViewModel>(new[]
            {
                new SettingsViewModel { Name = "App Info", ImageSrc = "resource://IQB.Resources.Image.app-info.svg" },
                new SettingsViewModel { Name = "Delete Account", ImageSrc = "resource://IQB.Resources.Image.Delete-account.svg" },
                new SettingsViewModel { Name = "Change Location", ImageSrc = "resource://IQB.Resources.Image.Change-location.svg" },
                new SettingsViewModel { Name = "Salon Info", ImageSrc = "resource://IQB.Resources.Image.Salon-info.svg" },
            });

            GeneralSettingsList = new ObservableCollection<SettingsViewModel>(new[]
            {
                new SettingsViewModel { Name = "App Info", ImageSrc = "resource://IQB.Resources.Image.app-info.svg" },
                new SettingsViewModel { Name = "Delete Account", ImageSrc = "resource://IQB.Resources.Image.Delete-account.svg" },
                new SettingsViewModel { Name = "Change Location", ImageSrc = "resource://IQB.Resources.Image.Change-location.svg" },
                new SettingsViewModel { Name = "Salon Info", ImageSrc = "resource://IQB.Resources.Image.Salon-info.svg" },
                new SettingsViewModel { Name = "Barber Day Off", ImageSrc = "resource://IQB.Resources.Image.Salon-info.svg" },
            });

            if (Application.Current.Properties["UserLevel"].ToString() == "0")
            {
                SettingsListView.ItemsSource = GeneralSettingsListCustomer;
            }
            else
            {
                SettingsListView.ItemsSource = GeneralSettingsList;
            }

            if (Application.Current.Properties["UserLevel"].ToString() == "2")  //Admin\
            {
                sfTabsStack.IsVisible = true;

                AdminSettingsList = new ObservableCollection<SettingsViewModel>(new[]
                {
                new SettingsViewModel { Name = "Manage Services", ImageSrc = "resource://IQB.Resources.Image.Manage-Services.svg" },
                new SettingsViewModel { Name = "Manage Staff", ImageSrc = "resource://IQB.Resources.Image.Manage-Staff.svg" },
                new SettingsViewModel { Name = "My Customers", ImageSrc = "resource://IQB.Resources.Image.My-Customers.svg" },
                new SettingsViewModel { Name = "Manage Salon Text", ImageSrc = "resource://IQB.Resources.Image.Manage-Salon-Text.svg" },
                new SettingsViewModel { Name = "Manage Salon", ImageSrc = "resource://IQB.Resources.Image.SalonSettings.svg" },
                new SettingsViewModel { Name = "Manage Appointment Settings", ImageSrc = "resource://IQB.Resources.Image.AppointmentSettings.svg" },
                new SettingsViewModel { Name = "Manage Reports", ImageSrc = "resource://IQB.Resources.Image.ReportSettings.svg" },
                });

            }

            else
            {
                SettingsListView.Margin = new Thickness(0, -50, 0, 0);
            }
        }

        private void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (sfTabsStack.SelectedIndex == 1)
            {
                SettingsListView.ItemsSource = AdminSettingsList;
            }
            else
            {
                SettingsListView.ItemsSource = GeneralSettingsList;
            }
        }

        private async void SettingsListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            SettingsListView.SelectedItem = null;
            switch ((e.Item as SettingsViewModel).Name)
            {

                case "Barber Day Off":

                    if (!IsTapped)
                    {
                        IsTapped = true;
                        await Navigation.PushAsync(new BarberDayOffPage());
                        IsTapped = false;
                    }
                    break;



                case "App Info":

                    if (!IsTapped)
                    {
                        IsTapped = true;
                        await Navigation.PushAsync(new AppInfo());
                        IsTapped = false;
                    }
                    break;

                case "Delete Account":

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

                    break;

                case "Change Location":

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
                    break;

                case "Salon Info":

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

                    break;

                case "Manage Services":

                    await Navigation.PushAsync(new ServiceList());
                    break;

                case "Manage Staff":

                    await Navigation.PushAsync(new ManageStaff());
                    break;

                case "My Customers":

                    await Navigation.PushAsync(new CustomerList());
                    break;

                case "Manage Salon Text":

                    await Navigation.PushAsync(new SalonText());
                    break;

                case "Manage Salon":

                    await Navigation.PushAsync(new ManageSalon());
                    break;

                case "Manage Appointment Settings":

                    if (Application.Current.Properties["ModuleAvailability"].ToString() != "1")
                    {
                        await Navigation.PushAsync(new AppointmentSettings());
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
                    }
                    break;

                case "Manage Reports":
                    await Navigation.PushAsync(new ReportGraph());
                    break;

            }
        }
    }

    class SettingsViewModel
    {
        public string Name { get; set; }

        public string ImageSrc { get; set; }
    }
}