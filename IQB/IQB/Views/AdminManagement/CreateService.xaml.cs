using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.HomeManagement;
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
    public partial class CreateService : ContentPage
    {
        public CreateServiceViewModel viewModel = null;
        private HomeViewModel homeViewModel = null;
        public CreateService()
        {
            InitializeComponent();
            this.viewModel = new CreateServiceViewModel();
            BindingContext = viewModel;
            OnBackButtonPressed();
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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void entrySerPrice_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            this.viewModel.ServicePrice = Convert.ToString(e.Value);
        }
    }
}