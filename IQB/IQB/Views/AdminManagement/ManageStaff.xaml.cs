using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.HomeManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.Views.AdminManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageStaff : ContentPage
    {
        public ManageStaffViewModel viewModel = null;
        public ManageStaff()
        {

            try
            {
                InitializeComponent();

                //this.viewModel = new ManageStaffViewModel();
                //BindingContext = viewModel;
                //MessagingCenter.Subscribe<AddStaffMember>(this, "Popped",(a)=> { viewModel.BindStaffList(); });
                //var page = new AddStaffMember(0);
                //if (Application.Current.MainPage.Navigation.NavigationStack.Contains(page))
                //    Application.Current.MainPage.Navigation.RemovePage(page);
            }
            catch (Exception es)
            {

            }

        }

        private async void OnCloseTapped(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
               // await Application.Current.MainPage.Navigation.PushModalAsync(new AdminSettings());
               await Navigation.PopAsync();
                // await Navigation.PushAsync(new AdminSettings());
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //int BarberID = 0;
            //await Navigation.PushAsync(new AddStaffMember(BarberID));

        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int BarberID = Convert.ToInt16(btn.CommandParameter);

            var page = new AddStaffMember(BarberID);
            await Navigation.PushAsync(page);
        }

        private async void EditStaff_Tapped(object sender, EventArgs e)
        {
            int BarberID = Convert.ToInt16((e as TappedEventArgs).Parameter?.ToString());

            var page = new AddStaffMember(BarberID);
            await Navigation.PushAsync(page);
        }

        private async void DeleteStaff_Tapped(object sender, EventArgs e)
        {

        }

        private async void CreateStaff_Tapped(object sender, EventArgs e)
        {
            int BarberID = 0;
            await Navigation.PushAsync(new AddStaffMember(BarberID));
        }

        private void imgCloseHint_Tapped(object sender, EventArgs e)
        {
            frmHintList.IsVisible = false;
            frmHintList.HeightRequest = 0;
            frmHintList.Margin = new Thickness(0);
        }

        private void Content_PageAppering(object sender, EventArgs e)
        {
            this.viewModel = new ManageStaffViewModel();
            BindingContext = viewModel;
            MessagingCenter.Subscribe<AddStaffMember>(this, "Popped", (a) => { viewModel.BindStaffList(); });
        }
    }
}