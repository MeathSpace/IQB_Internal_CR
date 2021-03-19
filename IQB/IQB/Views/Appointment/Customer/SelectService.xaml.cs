using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
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
    public partial class SelectService : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        private BarberSalonService objBarberSalonService = new BarberSalonService();
        //private List<MappedAppointment> appointmentList = null;
        private List<ServiceModel> result = null;

        ApiResult res = new ApiResult();
        int salonId = 0;
        private bool _openAsPopup = false;
        private int _customerID;
        public SelectService(bool openAsPopup = false,int customerID=0)
        {
            InitializeComponent();
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            _openAsPopup = openAsPopup;
            _customerID = customerID;
            gdPopHead.IsVisible = _openAsPopup;
            ServiceList.RefreshCommand = new Command<string>((s) => GetServiceList(salonId));
        }


        private async void GetServiceList(int salonId)
        {
            // appointmentList = new List<MappedAppointment>();
            //var salonListResponse = await objSalonService.GetSalonList(userName);
            var res = await objBarberSalonService.GetBarberServicesBySalonId(salonId);
            if (res?.StatusCode == 200)
            {
                result = UtilityEL.ToModelList<ServiceModel>(res.Response);
                if (result.Count() > 0)
                {
                    frmNoRecord.IsVisible = false;
                    ServiceList.IsVisible = true;

                    //if (res?.Response?.Count() > 0)
                    //appointmentList.AddRange(appointmentListResponse.Response);
                    ServiceList.ItemsSource = null;
                    ServiceList.ItemsSource = result;
                    ServiceList.EndRefresh();
                }
            }
            else
            {
                frmNoRecord.IsVisible = true;
                ServiceList.IsVisible = false;
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
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            ServiceList.BeginRefresh();
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ServiceList.ItemsSource = result;
            }

            else
            {
                ServiceList.ItemsSource = result.Where(x => x.ServiceName.StartsWith(e.NewTextValue));
            }
        }

        private async void EditService_Tapped(object sender, EventArgs e)
        {
            //var salonID = (e as TappedEventArgs).Parameter?.ToString();
            //await Navigation.PushAsync(new CreateSalon(salonID, true));
        }
        private async void DeleteService_Tapped(object sender, EventArgs e)
        {


        }

        private async void ViewService_Tapped(object sender, EventArgs e)
        {
            var serviceID = (e as TappedEventArgs).Parameter?.ToString();

            if (_openAsPopup)
            {
                await Navigation.PopModalAsync();

                MessagingCenter.Send(serviceID, "SelectBarber");

              //  await Navigation.PushModalAsync(new SelectBarber(serviceID, Convert.ToString(salonId),true));

            }
            else
                await Navigation.PushAsync(new SelectBarber(serviceID, Convert.ToString(salonId),false,_customerID));
        }

        private async void CloseService_Tapped(object sender, EventArgs e)
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
