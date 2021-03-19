using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace IQB.Views.Customer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerFilter : ContentPage
	{
       
        public CustomerFilter ()
		{
			InitializeComponent ();
            

        }

        private async void btnApplyClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send($"{ entryFirstName.Text??""},{ entryLastName.Text??""}", "CustomernameSelected");
            await Navigation.PopModalAsync();
        }

        private async void CrossIconTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}