using Acr.UserDialogs;
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
    public partial class AccountActivation : ContentPage
    {
        private string email, UserName, UserImage, FirstName, LastName, password, DateofBirth, Mob, RateSystem, RatingScore, activationCode;
        private bool IsToggled;
        private int id, IsMailEnabled;

        public EnableViewModel ViewModel = null;

        public AccountActivation(string email, string UserName, string UserImage, bool IsToggled, int id, string FirstName, string LastName, string password, string DateofBirth, string Mob, string RateSystem, string RatingScore, string activationCode, int IsMailEnabled)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            #region LoginFrame

            var semiTransparentColor = new Color(1, 1, 1, 0.1);
            //ForgotPwdFrame.BackgroundColor = semiTransparentColor;
            //LoginFrame.Opacity = 0.1;
            //ForgotPwdFrame.OutlineColor = Color.White;
            //ForgotPwdFrame.CornerRadius = (float)5.0;

            #endregion LoginFrame

            this.email = email;
            this.UserName = UserName;
            this.UserImage = UserImage;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.password = password;
            this.DateofBirth = DateofBirth;
            this.Mob = Mob;
            this.IsToggled = IsToggled;
            this.id = id;
            this.IsMailEnabled = IsMailEnabled;
            this.activationCode = activationCode?.Trim();
            this.RatingScore = RatingScore;
            ViewModel = new EnableViewModel();

            BindingContext = ViewModel;
        }

        private async void CancelClick(object sender, EventArgs args)
        {
            indicator.IsVisible = true;
            await Navigation.PopModalAsync(true);
            indicator.IsVisible = false;
        }

        private async void ActivateClicked(object sender, EventArgs args)
        {
            indicator.IsVisible = true;
            if (bhvActivationCode.IsValid)
            {
                ViewModel.IsEnabled = false;

                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    string activationCodeEntry = entryActivationCode.Text.Trim();
                    int SalonId = 0;

                    using (SalonViewModel obj = new SalonViewModel())
                    {
                        SalonId = obj.GetSalonId();
                    }

                    if (activationCodeEntry == activationCode)
                    {
                        using (LoginServices obj = new LoginServices())
                        {
                            string query = "UPDATE CustomersRegisteredTable set Activated='Y' WHERE Email='" + email + "' AND SalonId=" + SalonId;
                            //string result = await obj.ActivateAccount(query);

                            //if (!string.IsNullOrWhiteSpace(result))
                            //{
                            //    await DisplayAlert("Success!", "Your account has been activated successfully.", "Ok");
                            //    //await Navigation.PopModalAsync(true);
                            //    using (LoginViewModel loginViewModel = new LoginViewModel())
                            //    {
                            //        loginViewModel.StoreLoginCredData(email, UserName, UserImage, IsToggled, id, FirstName, LastName, password, DateofBirth, Mob, RateSystem, IsMailEnabled);
                            //    }
                            //    App.IsUserLoggedIn = true;
                            //    App.GoToRoot();
                            //}
                            //else
                            //{
                            //    await DisplayAlert("Error!", "Activation failed. Please try again later.", "Ok");
                            //}
                            ApiResult result = await obj.ActivateAccountRaw(query);

                            if (result?.StatusCode == 200)
                            {
                                await DisplayAlert("Success!", "Your account has been activated successfully.", "Ok");
                                //await Navigation.PopModalAsync(true);
                                using (LoginViewModel loginViewModel = new LoginViewModel())
                                {
                                    loginViewModel.StoreLoginCredData(email, UserName, UserImage, IsToggled, id, FirstName, LastName, password, DateofBirth, Mob, RateSystem, RatingScore, IsMailEnabled);
                                }
                                App.IsUserLoggedIn = true;
                                App.GoToRoot();
                            }
                            else
                            {
                                await DisplayAlert("Error!", "Activation failed. Please try again later.", "Ok");
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Activation code mismatch.", "Ok");
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
                bhvActivationCode.IsShow = true;
                string validationMessage = string.Empty;
                if (!bhvActivationCode.IsValid)
                {
                    validationMessage = $"{bhvActivationCode.Message}";
                }

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    App.toastConfig.Message = validationMessage;
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
            }
            indicator.IsVisible = false;
        }




        private async void ResendActivationCode(object sender, EventArgs args)
        {
            try
            {
                indicator.IsVisible = true;
                using (EmailService obj = new EmailService())
                {
                    RegistrationEmailEL emailModel = new RegistrationEmailEL()
                    {
                        activationcode = activationCode,
                        email = email.Trim(),
                        firstname = FirstName,
                        lastname = LastName,
                        password = password,
                        username = FirstName + " " + LastName,
                        vSalonname = "us",
                        //vSalonTel = registerViewModel.Salon_TelephoneNo,
                        //vSalonWeb = registerViewModel.Salon_Link
                    };

                    var x = await obj.RegistrationEmail(emailModel);
                    indicator.IsVisible = false;
                    await DisplayAlert("Success!", "An email has been sent to " + email.Trim() + " with the Activation Code, please check your inbox or junkbox", "Ok");
                   
                }
            }
            catch
            {
                indicator.IsVisible = false;
                await DisplayAlert("Alert!", "Please try again!", "Ok");
            }

        }

    }
}