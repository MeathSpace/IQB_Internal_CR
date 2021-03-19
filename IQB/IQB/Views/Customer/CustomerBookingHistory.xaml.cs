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
    public partial class CustomerBookingHistory : ContentPage
    {
        public CustomerBookingHistoryViewModel viewModel = null;
        public CustomerBookingHistory(int customerId)
        {
            this.viewModel = new CustomerBookingHistoryViewModel(customerId);
            BindingContext = viewModel;
            InitializeComponent();
            lblToDate.Text = "Date (To)";
            lblFromDate.Text = "Date (From)";

            string pickerDate = string.Empty;

            toDatePicker.Unfocused += (s, e) =>
            {
                DateTime datevalue = ((DatePicker)s).Date;

                lblToDate.Text = datevalue.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Application.Current.Properties["toDate"] = lblToDate.Text;
                ((DatePicker)s).IsVisible = false;
            };

            fromDatePicker.Unfocused += (s, e) =>
            {
                DateTime datevalue = ((DatePicker)s).Date;

                lblFromDate.Text = datevalue.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
                Application.Current.Properties["fromDate"] = lblFromDate.Text;
                ((DatePicker)s).IsVisible = false;
            };
            Application.Current.Properties["IsProfileViewed"] = "";
        }
        private async void OnCustBookingHistoryClosed(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                await Navigation.PopAsync();
            }
        }

        private void lblToDate_Tapped(object sender, EventArgs e)
        {
            toDatePicker.Focus();
        }

        private void lblFromDate_Tapped(object sender, EventArgs e)
        {
            fromDatePicker.Focus();
        }

    }
}