using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddressAndLocation : ContentPage
	{
		public AddressAndLocation ()
		{
			InitializeComponent ();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.Padding = new Thickness(0, 20, 0, 0);
            }

            map.MoveToRegion(MapSpan.FromCenterAndRadius(
               new Position(52.057702, 1.159083), Distance.FromMiles(5))); // Santa Cruz golf course

            var position = new Position(52.057702, 1.159083); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "iQueue Barbers",
                Address = "8 Old Foundry Road, England, IP4as"
            };
            map.Pins.Add(pin);
        }

        private async void CrossIconTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}