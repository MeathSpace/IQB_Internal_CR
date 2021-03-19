using Acr.UserDialogs;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.GenericModels;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.IQBServices.GenericServices;
using IQB.ViewModel.AuthViewModel;
using System;

using Xamarin.Forms;

namespace IQB.Views.LoginManagement
{
    public partial class ForgotPassword : ContentPage
    {
        public EnableViewModel ViewModel = null;
        public ForgotPassword()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            //Region commented due to design Change
            #region LoginFrame

            //var semiTransparentColor = new Color(1, 1, 1, 0.1);
            //ForgotPwdFrame.BackgroundColor = semiTransparentColor;
            //ForgotPwdFrame.BackgroundColor = Color.White;
            //LoginFrame.Opacity = 0.1;
            //ForgotPwdFrame.OutlineColor = Color.White;
            //ForgotPwdFrame.CornerRadius = (float)5.0;

            #endregion LoginFrame

            ViewModel = new EnableViewModel();

            BindingContext = ViewModel;
        }

        private async void CancelClick(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void btnForgotPasswordClicked(object sender, EventArgs args)
        {
            if (bhvIsValidUserEmail.IsValid)
            {
                ViewModel.IsEnabled = false;

                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    int SalonId = 0;
                    string SalonName = string.Empty;
                    string SalonTelNo = string.Empty;
                    string SalonWeb = string.Empty;

                    using (SalonViewModel obj = new SalonViewModel())
                    {
                        SalonId = obj.GetSalonId();
                        SalonName = obj.GetSalonName();
                        SalonWeb = obj.GetSalonLink();
                        SalonTelNo = obj.GetSalonTel();
                    }

                    string email = entryEmail.Text;

                    ApiResult apiCheckExistence = new ApiResult();

                    using (RegistrationService objRegService = new RegistrationService())
                    {
                        apiCheckExistence = await objRegService.CheckEmailExistence(email, SalonId);
                    }

                    if (apiCheckExistence != null && apiCheckExistence.StatusCode == 200)
                    {
                        EmailExistCheckModel checkExistence = UtilityEL.ToModel<EmailExistCheckModel>(apiCheckExistence.Response);

                        if (checkExistence != null && checkExistence.id > 0)
                        {
                            using (EmailService obj = new EmailService())
                            {
                                ForgotPasswordEmail emailModel = new ForgotPasswordEmail()
                                {
                                    email = email,
                                    firstname = checkExistence.FirstName,
                                    lastname = checkExistence.LastName,
                                    password = checkExistence.Password,
                                    vSalonname = SalonName,
                                    vSalonTel = SalonTelNo,
                                    vSalonWeb = SalonWeb
                                };

                                string result = await obj.ForgotPasswordEmail(emailModel);

                                await DisplayAlert("Message", result, "Ok");

                                await Navigation.PopModalAsync(true);
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Email id doesn't exist!", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Email id doesn't exist!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                }

                ViewModel.IsEnabled = true;
            }
            else
            {
                //bhvIsValidUserEmail.IsShow = true;

                string validationMessage = string.Empty;
                if (!bhvIsValidUserEmail.IsValid)
                {
                    validationMessage = $"{bhvIsValidUserEmail.Message}";
                }
               
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    App.toastConfig.Message = validationMessage;
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
            }
        }
    }
}