using IQB.ViewModel.AdminManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.AdminManagement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateService : ContentPage
	{
        public UpdateServiceViewModel viewModel = null;
        public UpdateService (int ServiceID)
		{
			InitializeComponent ();
            this.viewModel = new UpdateServiceViewModel(ServiceID);
            BindingContext = viewModel;
            //viewModel.SelectedServiceId = ServiceID;
        }

        private async void OnUpdateServiceTapped(object sender, EventArgs args)
        {
        }
   
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                // homeViewModel.IsModalOpen = false;
                await Navigation.PopAsync();
            }
        }

        private void entrySerPrice_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            this.viewModel.ServicePrice = Convert.ToString(e.Value);
        }
    }
}