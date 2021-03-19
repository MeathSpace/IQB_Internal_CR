using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.PhotoGallery
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZoomImagePage : ContentPage
	{
		public ZoomImagePage ( string src)
		{
			InitializeComponent ();
            Images.Source = src;

        }

        private async void CrossIcontapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}