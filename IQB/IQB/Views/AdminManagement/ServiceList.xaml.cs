using Acr.UserDialogs;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
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
	public partial class ServiceList : ContentPage
	{
        public ServiceListViewModel viewModel = null;
        private HomeViewModel homeViewModel = null;
      
        public ServiceList ()
		{
			InitializeComponent();

            //this.viewModel = new ServiceListViewModel();
            //BindingContext = viewModel;
            //this.homeViewModel = homeViewModel;
            //ServiceList1.ItemAppearing += (s, e) => {
            //    ServiceList1.HeightRequest = (viewModel.ServiceList.Count * ServiceList1.RowHeight) + 500;
            //};


        }

        //private async void OnCloseTapped(object sender, EventArgs e)
        //{
        //    if (viewModel.IsBackIconEnabled)
        //    {
        //        viewModel.IsBackIconEnabled = false;
        //        // homeViewModel.IsModalOpen = false;
        //        await Navigation.PopAsync();
        //    }
        //}

        //private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new CreateService());
        //}


        //private async void btnServiceEdit_Clicked(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    int serviceID = Convert.ToInt16(btn.CommandParameter);

        //    await Navigation.PushAsync(new UpdateService(serviceID));
        //}

        //private void btnServiceDelete_Clicked(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    int serviceID = Convert.ToInt16(btn.CommandParameter);

        //    using (SalonViewModel obj = new SalonViewModel())
        //    {
        //       int SalonID = obj.GetSalonId();
        //    }
        //}

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel = new ServiceListViewModel();
            BindingContext = viewModel;
            this.homeViewModel = homeViewModel;
        }
        private void imgCloseHint_Tapped(object sender, EventArgs e)
        {
            frmHintList.IsVisible = false;
            frmHintList.HeightRequest = 0;
            frmHintList.Margin = new Thickness(0);
        }

        private async void EditService_Tapped(object sender, EventArgs e)
        {
            int serviceID = Convert.ToInt16((e as TappedEventArgs).Parameter?.ToString());

            await Navigation.PushAsync(new UpdateService(serviceID));

           
        }

        private async void DeleteService_Tapped(object sender, EventArgs e)
        {
            bool x = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Confirm",
                Message = "Do you want to delete this Salon?",
                OkText = "Yes",
                CancelText = "No"
            });
            if (x)
            {
                int serviceID = Convert.ToInt16((e as TappedEventArgs).Parameter?.ToString());

                using (SalonViewModel obj = new SalonViewModel())
                {
                    int SalonID = obj.GetSalonId();
                }
            }
        }

        private async void AddService_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateService());
        }
    }
}