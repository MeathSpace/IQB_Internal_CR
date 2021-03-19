using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using System;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalonRequest : ContentPage
    {
        public SalonRequest()
        {
            InitializeComponent();
        }

        private async void SubmitRequest_Clicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            string Name = txtName.Text;
            string Email = (!string.IsNullOrEmpty(txtEmail.Text))?txtEmail.Text.Trim():"";
            string MessageBody = txtMessage.Text; 
            if (bhvName.IsValid && bhvEmail.IsValid && !string.IsNullOrEmpty(MessageBody))
            {
                indicator.IsVisible = true;
                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    ApiResult res = new ApiResult();
                    using (SalonService obj = new SalonService())
                    {
                        res = await obj.MailSalonInterest(Name, Email, MessageBody);
                    }
                    if (res != null && res.StatusCode == 200)
                    {
                        await DisplayAlert("Success!", Convert.ToString(res.Response), "Ok");
                        await Navigation.PopAsync();                        
                    }
                    else if(res!=null && res.StatusCode==201 && Convert.ToString(res.Response).ToLower()=="error")
                    {
                        await DisplayAlert("Error!", Convert.ToString(res.StatusMessage), "Ok");
                    }
                    else if (res != null && res.StatusCode == 201 && Convert.ToString(res.Response).ToLower() == "invalid email")
                    {
                        await DisplayAlert("Error!", Convert.ToString(res.StatusMessage), "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                }
                indicator.IsVisible = false;
            }
            else
            {
                string validationMessage = string.Empty;
                if (!bhvName.IsValid)
                {
                    validationMessage = $"{bhvName.Message}";
                }
                else if (!bhvEmail.IsValid)
                {
                    validationMessage = $"{bhvEmail.Message}";
                }
                else if(string.IsNullOrEmpty(MessageBody))
                {
                    validationMessage = "Message can not be empty";
                }
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    App.toastConfig.Message = validationMessage;
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
            }
            indicator.IsVisible = false;
        }

    }
}