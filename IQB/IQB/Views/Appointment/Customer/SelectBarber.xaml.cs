using IQB.EntityLayer.AppointmentModels;
using IQB.IQBServices.AppointmentServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectBarber : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        private BarberService objBarberService = new BarberService();
        private List<Mappedbarber> barberList = null;
        int salonId = 0;
        int serviceId = 0;
        int barberId = 0;
        string babrberName = "";
        private bool _openAsPopup = false;
        private int _customerID;
        public SelectBarber(string serviceID = "0", string SalonID = "0", bool openAsPopup = false, int customerID = 0)
        {
            salonId = Convert.ToInt16(SalonID);
            serviceId = Convert.ToInt16(serviceID);
            InitializeComponent();
            _openAsPopup = openAsPopup;
            _customerID = customerID;
            BarberList.RefreshCommand = new Command<string>((s) => GetBarberListList(serviceID, SalonID));
            indicator.IsVisible = false;
            gdPopHead.IsVisible = _openAsPopup;
        }

        private async void GetBarberListList(string serviceID, string SalonID)
        {
            barberList = new List<Mappedbarber>();
            //var salonListResponse = await objSalonService.GetSalonList(userName);
            var appointmentListResponse = await objBarberService.GetBarberListBySalonServiceID(serviceID, SalonID);
            if (appointmentListResponse?.StatusCode == 200)
            {
                frmNoRecord.IsVisible = false;
                BarberList.IsVisible = true;

                if (appointmentListResponse?.Response?.Count() > 0)
                    barberList.AddRange(appointmentListResponse.Response);
                BarberList.ItemsSource = null;
                BarberList.ItemsSource = barberList;
                BarberList.EndRefresh();
            }
            else
            {
                frmNoRecord.IsVisible = true;
                BarberList.IsVisible = false;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                BarberList.ItemsSource = barberList;
            }

            else
            {
                BarberList.ItemsSource = barberList.Where(x => x.BarberName.StartsWith(e.NewTextValue));
            }
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            BarberList.BeginRefresh();
        }
        private async void ViewBarber_Tapped(object sender, EventArgs e)
        {

            //await Navigation.PopModalAsync();
            //await App.Current.MainPage.Navigation.PushAsync(new NavigationPage(new CreateAppointment()));
            var barberID = Convert.ToInt16((e as TappedEventArgs).Parameter?.ToString());
            if (_openAsPopup)
            {
                await Navigation.PopModalAsync();
                MessagingCenter.Send($"{serviceId},{barberID}", "BarberSelected");
            }
            else
            {

                indicator.IsVisible = true;
                await Task.Delay(TimeSpan.FromSeconds(2));
                await Navigation.PushAsync(new CreateAppointment(serviceId, barberID, _customerID));
                indicator.IsVisible = false;
            }

        }


        private async void CloseBarber_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(false);
        }


        //private void imgCloseHint_Tapped(object sender, EventArgs e)
        //{
        //    frmHintList.IsVisible = false;
        //    frmHintList.HeightRequest = 0;
        //    frmHintList.Margin = new Thickness(0);
        //}
    }
}
