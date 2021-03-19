using IQB.ViewModel.AdminManagement;
using IQB.Views.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.GenericModels;
using IQB.IQBServices;
using IQB.IQBServices.GenericServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Email : ContentPage
	{
        public EmailViewModel viewModel = null;

        public Email(string subject="",string to="",bool SendToAdmin = true)
		{
			InitializeComponent();
            if(Device.RuntimePlatform == Device.iOS)
            {
                this.Padding = new Thickness(0, 20, 0, 0);
            }

            entryEmail.IsVisible = false;
            bdrFrom.IsVisible = false;
            lblMailTo.Text = SendToAdmin ? "Salon Admin(s)" : to;
            this.viewModel = new EmailViewModel();
            BindingContext = viewModel;
            viewModel.Subject = subject;
            viewModel.SendToAdmin = SendToAdmin;
            viewModel.To = to;
            var prop = Application.Current.Properties["UserName"];
            var IsLogin = Application.Current.Properties["UserName"] != null ? Application.Current.Properties["UserName"].ToString() : null;
            if (string.IsNullOrEmpty(IsLogin))
            {
                entryEmail.IsVisible = true;
                bdrFrom.IsVisible = true;
            }
            else
                viewModel.From = $"{Application.Current.Properties["_UserEmail"]}";
        }
        
        private async void CrossIconTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void EntryEmail_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(entryEmail.Text))
            {
                entryEmail.Text = string.Empty;
            }
            else
            {
                char[] letters = entryEmail.Text.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                entryEmail.Text = new string(letters);
            }
            //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
            if (bhvIsValidEmail.IsShow)
            {
                App.toastConfig.Message = bhvIsValidEmail.Message;
                UserDialogs.Instance.Toast(App.toastConfig);
            }
        }

        private async void BtnSubmit_OnClicked(object sender, EventArgs e)
        {
            if (entryEmail.IsVisible)
            {
                if(bhvIsValidEmail.IsValid)
                    SendEmail();
                else
                {
                    App.toastConfig.Message = bhvIsValidEmail.Message;
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
            }
            else
                SendEmail();
        }

        private async void SendEmail()
        {

            if (!string.IsNullOrEmpty(viewModel.EmailBody))
            {
                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    indicator.IsVisible = true;
                    ServiceEmail _serviceEmail = new ServiceEmail();
                    _serviceEmail.Subject = viewModel.Subject;
                    _serviceEmail.Body = viewModel.EmailBody;
                 //   _serviceEmail.SendToAdmin = $"{viewModel.SendToAdmin}";
                 // Chnaged as client said the mail to go to just support @iqbbarbers.com
                    _serviceEmail.SendToAdmin = String.Empty;
                    _serviceEmail.To = viewModel.To;
                    _serviceEmail.From = viewModel.From;
                    _serviceEmail.SalonId = Convert.ToString(Application.Current.Properties["_SalonId"]);
                    ApiResult res = new ApiResult();
                    using (EmailService obj = new EmailService())
                    {
                        res = await obj.SendEmailToAdmin(_serviceEmail);
                        if (res != null && res.StatusCode == 200)
                            await App.Current.MainPage.DisplayAlert("Email", res.StatusMessage, "OK");
                        else
                            await App.Current.MainPage.DisplayAlert("Email", "Email not sent. Please try again after sometime.", "OK");
                    }
                    indicator.IsVisible = false;
                    await Navigation.PopModalAsync();
                }
                else
                    await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
            }
            else
            {
                App.toastConfig.Message = "Please enter a message.";
                UserDialogs.Instance.Toast(App.toastConfig);
            }

            viewModel.IsBackIconEnabled = true;
            viewModel.IsRefreshing = false;
           
        }
    }
}