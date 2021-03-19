using IQB.EntityLayer.Common;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQB.Views.ApplicationManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Settings
{
    public partial class Help : ContentPage
    {
        HelpViewModel viewModel = null;
        public Help()
        {
            InitializeComponent();
            viewModel = new HelpViewModel();
            BindingContext = viewModel;
        }
        private async void OnHelpBackClicked(object sender, EventArgs args)
        {
            //homeViewModel.IsModalOpen = false;
            await Navigation.PopModalAsync();
        }

        private async void OnEmailClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Email("Help Query", "support@iqueuebarbers.com"), true);
        }

        private void OnTapGestureRecognizerTel(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri($"tel:{viewModel.ContactTel}"));
        }
    }
}