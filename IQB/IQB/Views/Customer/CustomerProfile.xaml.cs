using Acr.UserDialogs;
using IQB.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerProfile : ContentPage
    {
        public CustomerProfileViewModel viewModel = null;
        List<ListCustomerID> _listCustomerID = new List<ListCustomerID>();
        public string CustomerDataId = string.Empty;
        public CustomerProfile(List<ListCustomerID> CustomerID)
        {
            try
            {
                _listCustomerID = new List<ListCustomerID>();
                ListCustomerID _CustomerID = new ListCustomerID();
                //Application.Current.Properties["CustomerID"] = CustomerID;
                //int Customerid= Convert.ToInt32(Application.Current.Properties["CustomerID"]);
                _CustomerID.CustomerID = CustomerID[0].CustomerID;
                CustomerDataId = Convert.ToString(CustomerID[0].CustomerID);
                _listCustomerID.Add(_CustomerID);
                this.viewModel = new CustomerProfileViewModel(_listCustomerID);
                BindingContext = viewModel;
                InitializeComponent();
                
                Application.Current.Properties["IsProfileViewed"] = "true";
            }
            catch (Exception es)
            {

            }
        }

        private void PickPictureTapped(object sender, EventArgs e)
        {

        }
        private void ClearDate(object sender, EventArgs e)
        {

        }


        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Application.Current.Properties["CustomerCountToSendMail"] = 1;
            await Navigation.PushAsync(new CustomerMail(_listCustomerID));
        }

        private async void OnCloseTapped(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                //await Application.Current.MainPage.Navigation.PushModalAsync(new AdminSettings());
                await Navigation.PopAsync();
                // await Navigation.PushAsync(new AdminSettings());
            }
        }

        private async void SendEmail_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["CustomerCountToSendMail"] = 1;
            await Navigation.PushAsync(new CustomerMail(_listCustomerID));
        }

        private async void SelectStartDate_Tapped(object sender, EventArgs e)
        {
            var StartDate = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig
            {
                IsCancellable = false,
                OkText = "Confirm",
                CancelText = "",

            });
            if (StartDate.Ok)
            {
              
                lblStartDate.Text = StartDate.SelectedDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Application.Current.Properties["toDate"] = lblStartDate.Text;
            }

        }

        private async  void SelectEndDate_Tapped(object sender, EventArgs e)
        {
            var EndDate = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig
            {
                IsCancellable = false,
                OkText = "Confirm",
                CancelText = "",

                
            });
            if (EndDate.Ok)
            {

                lblEndDate.Text = EndDate.SelectedDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Application.Current.Properties["fromDate"] = lblEndDate.Text;
            }
        }

        private void SearchButton_Tapped(object sender, EventArgs e)
        {
            viewModel.BindCustomerBookingHistoryList(CustomerDataId, Convert.ToString(Application.Current.Properties["fromDate"]), Convert.ToString(Application.Current.Properties["toDate"]));
            
        }
    }
}