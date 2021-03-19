using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.MenuManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.AdminManagement
{
    public partial class StaffProfile : ContentPage
    {
        public AddStaffMemberViewModel viewModel = null;
        public MasterDetailsViewModel MenuViewModel = null;
        public StaffProfile()
        {
            

          //  this.viewModel = new AddStaffMemberViewModel(149);
            //this.viewModel = new StaffProfileViewModel();
            BindingContext = viewModel;
            InitializeComponent();

            //MenuViewModel = viewModelMasterDetail;
        }

        private async void TapGestureRecognizer_Tapped_CanceButton(object sender, EventArgs e)
        {
            //if (viewModel.IsBackIconEnabled)
            //{
            //    viewModel.IsBackIconEnabled = false;
            //    await Navigation.PopAsync();
            //    // await Navigation.PushAsync(new ManageStaff());
            //    //await Application.Current.MainPage.Navigation.PushModalAsync(new ManageStaff());
            //}
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Text == "Profile")
            {
                lblProfile.BackgroundColor = Color.Black;
                lblServices.BackgroundColor = Color.LightGray;
                lblProfile.TextColor = Color.White;
                lblServices.TextColor = Color.Black;
                gdProfile.IsVisible = true;
                gdServices.IsVisible = false;
            }
            else if (lbl.Text == "Services")
            {
                lblProfile.BackgroundColor = Color.LightGray;
                lblServices.BackgroundColor = Color.Black;
                lblProfile.TextColor = Color.Black;
                lblServices.TextColor = Color.White;
                gdServices.IsVisible = true;
                gdProfile.IsVisible = false;

            }
        }
    }
}