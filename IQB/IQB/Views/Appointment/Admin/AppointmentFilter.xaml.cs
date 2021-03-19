using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppointmentFilter : ContentPage
	{
        private List<string> statuses = new List<string>();
        public AppointmentFilter ()
		{
			InitializeComponent ();
            BindStatus();

        }

        private void SelectStatus_Tapped(object sender, EventArgs e)
        {
            pickerStatus.Focus();
            pickerStatus.SelectedIndexChanged += (s, ev) =>
            {
                //selectedCountry = (Country)pickerStatus.SelectedItem;
                lblStatus.Text = ((string)pickerStatus.SelectedItem);
                lblStatus.TextColor = Color.FromHex("#FFFFFF");


               

            };
        }

        private async void CrossIconTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnApplyClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            MessagingCenter.Send($"{ lblStatus.Text ?? ""}", "AppointmentStatusSelected");
        }
        private void BindStatus()
        {
            statuses.Add("Booked");
            statuses.Add("Cancelled");
            statuses.Add("Completed");
            statuses.Add("All");
            pickerStatus.ItemsSource = statuses;
        }
    }
}