using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.IQBServices.AuthServices;
using IQB.Utility;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.AdminManagement;
using IQB.Views.ApplicationManagement;
using IQB.Views.LoginManagement;
using IQB.Views.QueueList;
using IQB.Views.Settings;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.XForms.Buttons;
using static IQB.EntityLayer.AuthModel.LoginModel;

namespace IQB.Views
{
    public partial class Login : ContentPage
    {
        private LoginViewModel loginViewModel = null;

        public Login()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                loginViewModel = new LoginViewModel();
                loginViewModel.SetSalonInfo();

                BindingContext = loginViewModel;
                Application.Current.Properties["UserName"] = string.Empty;
                Application.Current.Properties["RatingScore"] = "0";
                Application.Current.Properties["IsAlreadyRated"] = "0";

                if (!String.IsNullOrEmpty(loginViewModel.Salon_ImageName))
                    SalonImage.Source = new UriImageSource
                    {
                        Uri = new Uri(loginViewModel.Salon_ImageName),
                        CachingEnabled = false,
                    };
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "MainTabPage.xaml", "MainPage");
            }

            //Region commented due to design Change
            //#region LoginFrame 

            //var semiTransparentColor = new Color(1, 1, 1, 0.1);
            //LoginFrame.BackgroundColor = semiTransparentColor;
            ////LoginFrame.Opacity = 0.1;
            //LoginFrame.OutlineColor = Color.White;
            //LoginFrame.CornerRadius = (float)5.0;

            //#endregion LoginFrame

            //Rememberme.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }

        private async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                indicator.IsVisible = true;
                if (!loginViewModel.IsRegistrationTapped && loginViewModel.IsEnabled)
                {
                    loginViewModel.IsRegistrationTapped = true;
                    await Navigation.PushModalAsync(new Register(), true);
                    loginViewModel.IsRegistrationTapped = false;
                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
                indicator.IsVisible = false;
                loginViewModel.IsRegistrationTapped = false;
            }
        }

        //async void GetImage(string imageURI)
        //{
        //    System.Uri uri;
        //    System.Uri.TryCreate(imageURI, UriKind.Absolute, out uri);
        //    Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
        //    _companyImage.Source = await result;
        //}

        private async void btnLogin_Clicked(object sender, EventArgs args)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                indicator.IsVisible = true;
                if (bhvPassword.IsValid && bhvUserName.IsValid) //bhvIsValidEmail.IsValid &&
                {
                    loginViewModel.IsEnabled = false;
                    sw_RememberMe.IsEnabled = false;

                    string email = entryEmail.Text;
                    string password = entryPass.Text;

                    string devicetype = string.Empty;

                    if (Device.OS == TargetPlatform.Android)
                    {
                        devicetype = "android";
                    }
                    else
                    {
                        devicetype = "iOS";
                    }

                    ApiResult res = new ApiResult();

                    using (LoginServices obj = new LoginServices())
                    {
                        res = await obj.LoginUser(email.ToLower(), password, loginViewModel.SalonId, App.DeviceToken, devicetype);
                    }

                    if (res != null && res.StatusCode == 200)
                    {
                        //await Navigation.PushAsync(new About());
                        LoginReturnResult result = UtilityEL.ToModel<LoginReturnResult>(res.Response);

                        if (result != null && result.id > 0)
                        {
                            using (LoginServices obj = new LoginServices())
                            {
                                res = await obj.UserAuthentication(email, loginViewModel.SalonId);
                            }

                            if (res != null && res.StatusCode == 200)
                            {
                                UserAuthenticationResult result1 = UtilityEL.ToModelNew<UserAuthenticationResult>(res.Response);

                                if ((result1.UserLevel == 2) && ((result.AccountExpiryDate - DateTime.Now).Days < 90))
                                {
                                    Application.Current.Properties["ExpiringMsg"] = "Your License Expires in " + (((result.AccountExpiryDate - DateTime.Now).Days) + 1).ToString() + " days. Please contact support!!";
                                    await DisplayAlert("Alert", Application.Current.Properties["ExpiringMsg"].ToString(), "OK");
                                }
                                else
                                {
                                    Application.Current.Properties["ExpiringMsg"] = "";
                                }  

                                Application.Current.Properties["_SalonId"] = result.SalonId;
                                Application.Current.Properties["_Currency"] = (String.IsNullOrEmpty(result.Currency) || result.Currency == "Â£") ? "£" : result.Currency;
                                Application.Current.Properties["_UserEmail"] = result.Email;
                                Application.Current.Properties["UserName"] = result.UserName;
                                Application.Current.Properties["IsBarberRegistered"] = result.IsBarberRegistered;
                                Application.Current.Properties["UserLevel"] = result1.UserLevel;
                                Application.Current.Properties["ProfileBarber_Id"] = result1.StaffBarberId;
                                Application.Current.Properties["IsAdminBarber"] = result1.IsBarber;
                                if (String.IsNullOrEmpty(result.ModuleAvailability))
                                {
                                    Application.Current.Properties["ModuleAvailability"] = "0";
                                }
                                else
                                {
                                    Application.Current.Properties["ModuleAvailability"] = result.ModuleAvailability;
                                }

                            }

                            string UserImage = string.Empty;
                            if (!string.IsNullOrWhiteSpace(result.userIcon))
                            {
                                UserImage = CommonEL.ProfileImageLocation + result.userIcon;//"noimage.png"; + ".jpg"
                            }
                            else
                            {
                                UserImage = "noimage.png";
                            }
                            //try
                            //{
                            //    ImageSource img = ImageSource.FromUri(new Uri(CommonEL.ProfileImageLocation + result.id + ".png"));

                            //    if (img != null)
                            //    {
                            //        UserImage = CommonEL.ProfileImageLocation + result.id + ".png";
                            //    }
                            //    else
                            //    {
                            //        UserImage = "noimage.png";
                            //    }
                            //}
                            //catch
                            //{
                            //    UserImage = "noimage.png";
                            //}

                            //Uri uri;
                            //if(Uri.TryCreate(CommonEL.ProfileImageLocation + result.id + ".png", UriKind.Absolute, out uri))
                            //{
                            //    Task<ImageSource> resultImage = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(uri));
                            //    if (resultImage != null)
                            //    {
                            //        UserImage = CommonEL.ProfileImageLocation + result.id + ".png";
                            //    }
                            //}

                            string UserName = result.FirstName + " " + result.LastName;

                            if (!string.IsNullOrWhiteSpace(result.Activated) && result.Activated.ToUpper() == "Y")
                            {
                                //loginViewModel.StoreLoginCredData(email, UserName, UserImage, sw_RememberMe.IsToggled, result.id, result.FirstName, result.LastName, password, result.DateofBirth, result.Mob, result.RateSystem, result.MaketingEmails);
                                loginViewModel.StoreLoginCredData(email, UserName, UserImage, sw_RememberMe.IsOn.Value, Convert.ToInt32(result.UserName), result.FirstName, result.LastName, password, result.DateofBirth, result.Mob, result.RateSystem, result.RatingScore, result.MaketingEmails);
                                App.IsUserLoggedIn = true;
                                App.GoToRoot();
                            }
                            else
                            {
                                //await Navigation.PushModalAsync(new AccountActivation(email, UserName, UserImage, sw_RememberMe.IsToggled, result.id, result.FirstName, result.LastName, password, result.DateofBirth, result.Mob, result.RateSystem, result.ActivationCode, result.MaketingEmails), true);
                                await Navigation.PushModalAsync(new AccountActivation(email, UserName, UserImage, sw_RememberMe.IsOn.Value, result.id, result.FirstName, result.LastName, password, result.DateofBirth, result.Mob, result.RateSystem, result.RatingScore, result.ActivationCode, result.MaketingEmails), true);
                            }
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Invalid Email Id or Password!", "OK");
                        }
                    }
                    else
                    {
                        if (res != null && res.StatusCode == 201 && !string.IsNullOrWhiteSpace(res.StatusMessage))
                        {
                            await DisplayAlert("Error!", res.StatusMessage, "OK");
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Invalid Email Id or Password!", "OK");
                        }
                    }

                    loginViewModel.IsEnabled = true;
                    sw_RememberMe.IsEnabled = true;
                }
                else
                {
                    string validationMessage = string.Empty;
                    //if (!bhvIsValidEmail.IsValid)
                    //{
                    //    bhvIsValidEmail.IsShow = true;
                    //}
                    if (!bhvUserName.IsValid)
                    {
                        validationMessage = $"{bhvUserName.Message}";
                    }
                    else if (!bhvPassword.IsValid)
                    {
                        validationMessage = $"{bhvPassword.Message}";
                        bhvPassword.IsShow = true;
                    }

                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        App.toastConfig.Message = validationMessage;
                        UserDialogs.Instance.Toast(App.toastConfig);
                    }

                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
                await errLog.WinrtErrLogTest(ex);
                await DisplayAlert("Error!", "Some error occured!", "OK");
                loginViewModel.IsEnabled = true;
                sw_RememberMe.IsEnabled = true;
            }
        }

        private async void OnTapGestureRecognizerForgot(object sender, EventArgs args)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                indicator.IsVisible = true;
                if (!loginViewModel.IsForgotPasswordTapped && loginViewModel.IsEnabled)
                {
                    loginViewModel.IsForgotPasswordTapped = true;
                    await Navigation.PushModalAsync(new ForgotPassword(), true);
                    loginViewModel.IsForgotPasswordTapped = false;
                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void OnHelpClicked(object sender, EventArgs args)
        {
            indicator.IsVisible = true;
            await App.Current.MainPage.Navigation.PushAsync(new Help(), true);
            indicator.IsVisible = false;
        }

        private async void ChooseSalon_Tapped(object sender, EventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                indicator.IsVisible = true;
                var answer = await DisplayAlert("Alert!", "Are you sure you want to change the location?", "Yes", "No");
                if (answer)
                {
                    LoginViewModel.DestroyLoginCredData();
                    SalonViewModel.DestroySalonCodeData();
                    App.GoToMainRoot();
                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
                errLog.WinrtErrLogTest(ex);
            }
        }
    }
}