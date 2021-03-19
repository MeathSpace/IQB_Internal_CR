using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Customer;
using IQB.ViewModel.MenuManagement;
using IQB.Views.AdminManagement;
using IQB.Views.ApplicationManagement;
using IQB.Views.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace IQB.Views.MenuItems
{
    public partial class MenuMaster : MasterDetailPage
    {
        public MasterDetailsViewModel viewModel = null;
        public static bool QueueListTimerStat { get; set; }
        public static bool HomeTimerStat { get; set; }

        public bool IsRedirectedToProfile = false;

        public MenuMaster(bool IsRedirectedToProfile)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                InitializeComponent();
                Application.Current.Properties["IsGridEnabled"] = "false";
                Application.Current.Properties["ChangeColor"] = "1";
                this.IsRedirectedToProfile = IsRedirectedToProfile;

                this.viewModel = new MasterDetailsViewModel(IsRedirectedToProfile);
                BindingContext = viewModel;
                string SalonName = string.Empty;
                using (SalonViewModel obj = new SalonViewModel())
                {
                    SalonName = obj.GetSalonName();
                }

                lblSalonName.Text = SalonName;

                QueueListTimerStat = false;
                HomeTimerStat = false;
                // Initial navigation, this can be used for our home page
                if (!IsRedirectedToProfile)
                {
                    App.IsOnHomeScreen = true;
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Home.Home)));
                }
                else
                {
                    App.IsOnHomeScreen = false;
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MyProfile), viewModel));
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                var item = (MenuListModel)e.SelectedItem;
                Type page = item.TargetType;
                QueueListTimerStat = false;
                HomeTimerStat = false;
                int check = 0;
                Application.Current.Properties["ChangeColor"] = "1";
                int r = Convert.ToInt32(Application.Current.Properties["UserLevel"]);
                if (item.Id == 1)
                {
                    App.IsOnHomeScreen = true;
                }
                else
                {
                    App.IsOnHomeScreen = false;
                }
                if (item.Id == 3)
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(page, viewModel));
                }
                else if (item.Id != 5)
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                }
                if (Convert.ToInt32(Application.Current.Properties["UserLevel"]) == 1 && item.Id == 5)
                {
                    Application.Current.Properties["IsGridEnabled"] = "true";

                    var q = Convert.ToInt16(Application.Current.Properties["_SalonId"]);
                    var q1 = Convert.ToString(Application.Current.Properties["_UserEmail"]).ToLower();

                    ApiResult res = new ApiResult();

                    using (BarberSalonService obj = new BarberSalonService())
                    {
                        res = await obj.GetStaffDetailsByEmailId(q1, q);
                    }

                    if (res != null && res.StatusCode == 200)
                    {
                        List<Rootobject> result = UtilityEL.ToModelListNew<Rootobject>(res.Response);
                        Detail = new NavigationPage((Page)Activator.CreateInstance(page, result[0].BarberId));
                    }
                    else
                    {
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MyProfileBlank)));
                    }
                }
                viewModel.ChangeMenuList(item.Id);
                IsPresented = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        private void OnTapCloseMenuButton(object sender, EventArgs args)
        {
            IsPresented = false;
        }
    }
}