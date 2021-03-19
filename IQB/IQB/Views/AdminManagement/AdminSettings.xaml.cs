using IQB.Views.Customer;
using IQB.Views.SalonManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQB.Views.Appointment.Admin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IQB.Views.TabPagers;
using IQB.Views.Report;

namespace IQB.Views.AdminManagement
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminSettings : ContentView
    {
        public AdminSettings()
        {
            InitializeComponent();
        }

        private async void OnManageServicesTapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ServiceList());
        }

        private async void OnManageStaffTapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ManageStaff());
        }

        private async void OnMyCustomersTapped(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CustomerList());
        }

        private async void OnSalonTextTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SalonText());
        }
        private async void OnManageSalonTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageSalon());
        }

        private async void OnManageAppointmentTapped(object sender, EventArgs e)
        {
           App.Current.MainPage = new NavigationPage(new MainTabPage(true));
        //    await Navigation.PushAsync(new ManageAppointment());
        }

        private async void OnManageReservationTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageReservation());
           // await Navigation.PushAsync(new AppointmentSettings());
        }

        private async void OnManageAppointmentSettingsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AppointmentSettings());
        }

        private async void OnManageReportsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportGraph());
        }
    }
}
