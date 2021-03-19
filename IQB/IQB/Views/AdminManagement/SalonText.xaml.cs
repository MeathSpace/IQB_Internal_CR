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
    public partial class SalonText : ContentPage
    {
        public SalonTextViewModel viewModel = null;
        public SalonText()
        {
            InitializeComponent();
            this.viewModel = new SalonTextViewModel();
            BindingContext = viewModel;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                await Navigation.PopAsync();
            }
        }
    }
}