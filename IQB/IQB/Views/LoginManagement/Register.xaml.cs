using Acr.UserDialogs;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.GenericModels;
using IQB.EntityLayer.SalonModels;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.IQBServices.GenericServices;
using IQB.IQBServices.SalonServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.Settings;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace IQB.Views.LoginManagement
{
    public partial class Register : ContentPage
    {
        private RegisterViewModel registerViewModel = null;
        RegistrationService objRegService = new RegistrationService();
        private SalonServices objSalonService = new SalonServices();
        private bool isCancellable = true;
        public Register()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            registerViewModel = new RegisterViewModel();

            BindingContext = registerViewModel;

            //if(registerViewModel.SalonId != 0)
            //{
            //    tabView.ItemSource.RemoveAt(1);
            //}

            #region RegisterFrame

            //var semiTransparentColor = new Color(1, 1, 1, 0.1);
            //RegisterFrame.BackgroundColor = semiTransparentColor;
            ////RegisterFrame.Opacity = 0.5;
            //RegisterFrame.OutlineColor = Color.White;
            //RegisterFrame.CornerRadius = (float)5.0;
            //var toastConfig = new ToastConfig("Toasting...");
            //toastConfig.SetDuration(3000);
            //toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));

            //UserDialogs.Instance.Toast(toastConfig);
            #endregion RegisterFrame

            tabView.PositionChanging += TabView_PositionChanging;

            tabView.PositionChanged += (s, e) =>
            {
                isCancellable = true;
                tabView.PositionChanging += TabView_PositionChanging;
            };
        }

        private void TabView_PositionChanging(object sender, Xam.Plugin.TabView.PositionChangingEventArgs e)
        {
            //if (registerViewModel.SalonId != 0)
            //{
            //    e.Canceled = isCancellable;
            //    UserDialogs.Instance.Alert(new AlertConfig
            //    {
            //        OkText = "OK",
            //        Title = "Alert",
            //        Message = "You cannot register as Admin for this Salon!"
            //    });
            //    //    OnAction = (isConfirmed) =>
            //    //    {
            //    //        if (isConfirmed)
            //    //        {
            //    //            ResetData(tabView.SelectedTabIndex);
            //    //            isCancellable = false;
            //    //            //e.NewPosition = tabView.SelectedTabIndex == 0 ? 1 : 0;
            //    //            tabView.PositionChanging -= TabView_PositionChanging;
            //    //            tabView.SetPosition(tabView.SelectedTabIndex == 0 ? 1 : 0);
            //    //        }
            //    //    }
            //    //});
            //}
            //else
            {
            e.Canceled = isCancellable;
            UserDialogs.Instance.Confirm(new ConfirmConfig
            {
                OkText = "Yes",
                CancelText = "No",
                Title = "Confirm",
                Message = "All data entered will be cleared. Do you want to continue?",
                OnAction = (isConfirmed) =>
                {
                    if (isConfirmed)
                    {
                        ResetData(tabView.SelectedTabIndex);
                        isCancellable = false;
                        //e.NewPosition = tabView.SelectedTabIndex == 0 ? 1 : 0;
                        tabView.PositionChanging -= TabView_PositionChanging;
                        tabView.SetPosition(tabView.SelectedTabIndex == 0 ? 1 : 0);
                    }
                }
            });
            }
        }

        private void ResetData(int selectedTabIndex)
        {
            if (selectedTabIndex == 0)
            {
                bhvFirstName.IsShow = false;
                bhvLastName.IsShow = false;
                bhvIsValidEmail.IsShow = false;
                bhvValidPassword.IsShow = false;
                registerViewModel.FirstName = null;
                registerViewModel.LastName = null;
                registerViewModel.Email = null;
                registerViewModel.Password = null;
                registerViewModel.IsBarber = false;
                switchCanSendEmail.IsOn = false;
            }
            //else
            //{
            //    bhvFirstNameAdmin.IsShow = false;
            //    bhvLastNameAdmin.IsShow = false;
            //    bhvIsValidEmailAdmin.IsShow = false;
            //    bhvValidPasswordAdmin.IsShow = false;
            //    registerViewModel.FirstNameAdmin = null;
            //    registerViewModel.LastNameAdmin = null;
            //    registerViewModel.EmailAdmin = null;
            //    registerViewModel.PasswordAdmin = null;
            //    registerViewModel.IsBarberAdmin = false;
            //    switchCanSendEmail.IsOn = false;
            //}
        }

        private async void CancelRegistration(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void RegisterUser(object sender, EventArgs args)
        {

            try
            {
                var adminTab = tabView.SelectedTabIndex;
                switch (adminTab)
                {
                    case 0:
                        {
                            if (bhvFirstName.IsValid && bhvLastName.IsValid && bhvValidPassword.IsValid && bhvIsValidEmail.IsValid)
                            {
                                registerViewModel.IsEnabled = false;
                                indicator.IsVisible = true;
                                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                                {
                                    RegistrationModel postModel = new RegistrationModel()
                                    {
                                        activated = "N",
                                        activationcode = UtilityEL.GetRandomString(6),
                                        email = registerViewModel.Email.Trim(),
                                        firstname = registerViewModel.FirstName,
                                        lastname = registerViewModel.LastName,
                                        loggedin = "N",
                                        password = registerViewModel.Password,
                                        registerdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                        salonid = registerViewModel.SalonId,
                                        IsBarber = registerViewModel.IsBarber
                                    };
                                    //RegistrationService objRegService = new RegistrationService();

                                    ApiResult apiCheckExistence = await objRegService.CheckEmailExistence(postModel.email.Trim(), postModel.salonid);

                                    if (apiCheckExistence == null || apiCheckExistence.StatusCode != 200)
                                    {
                                        EmailExistCheckModel checkExistence = UtilityEL.ToModel<EmailExistCheckModel>(apiCheckExistence.Response);

                                        if (checkExistence == null || checkExistence.id <= 0)
                                        {
                                            //if (switchCanSendEmail.IsToggled)
                                            if (switchCanSendEmail.IsOn.Value)
                                            {
                                                postModel.canSendEmail = 1;
                                            }
                                            else
                                            {
                                                postModel.canSendEmail = 0;
                                            }
                                            gdPopupPolicy.IsVisible = true;
                                            gdMain.IsVisible = false;

                                            //ApiResult result = await objRegService.RegisterUser(postModel, postModel.canSendEmail);

                                            //if (result != null && result.StatusCode == 200)
                                            //{
                                            //    await DisplayAlert("Success!", result.StatusMessage, "Ok");
                                            //    //Send email
                                            //    using (EmailService obj = new EmailService())
                                            //    {
                                            //        RegistrationEmailEL emailModel = new RegistrationEmailEL()
                                            //        {
                                            //            activationcode = postModel.activationcode,
                                            //            email = postModel.email.Trim(),
                                            //            firstname = postModel.firstname,
                                            //            lastname = postModel.lastname,
                                            //            password = postModel.password,
                                            //            username = postModel.firstname + " " + postModel.lastname,
                                            //            vSalonname = registerViewModel.SalonName,
                                            //            vSalonTel = registerViewModel.Salon_TelephoneNo,
                                            //            vSalonWeb = registerViewModel.Salon_Link
                                            //        };

                                            //        await obj.RegistrationEmail(emailModel);
                                            //    }

                                            //    await DisplayAlert("Success!", "An email has been sent to " + postModel.email.Trim() + " with the Activation Code, please check your inbox or junkbox", "Ok");
                                            //    await Navigation.PopModalAsync(true);
                                            //}
                                            //else
                                            //{
                                            //    await DisplayAlert("Error!", "Failed to register user. Please try again some time later.", "Ok");
                                            //}
                                        }
                                        else
                                        {
                                            await DisplayAlert("Error!", "Email id already exists. Please choose another email id.", "Ok");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Error!", "Email id already exists. Please choose another email id.", "Ok");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                                }
                                indicator.IsVisible = false;
                                registerViewModel.IsEnabled = true;
                            }
                            else
                            {
                                string validationMessage = string.Empty;
                                if (!bhvFirstName.IsValid)
                                {
                                    validationMessage = $"{bhvFirstName.Message}";
                                }
                                else if (!bhvLastName.IsValid)
                                {
                                    validationMessage = $"{bhvLastName.Message}";
                                }
                                else if (!bhvValidPassword.IsValid)
                                {
                                    validationMessage = $"{bhvValidPassword.Message}";
                                }
                                else if (!bhvIsValidEmail.IsValid)
                                {
                                    validationMessage = $"{bhvIsValidEmail.Message}";
                                }
                                if (!string.IsNullOrEmpty(validationMessage))
                                {
                                    App.toastConfig.Message = validationMessage;
                                    UserDialogs.Instance.Toast(App.toastConfig);
                                }
                            }
                        }
                        break;
                    //case 1:
                    //    {
                    //        if (bhvFirstNameAdmin.IsValid && bhvLastNameAdmin.IsValid && bhvValidPasswordAdmin.IsValid && bhvIsValidEmailAdmin.IsValid)
                    //        {
                    //            registerViewModel.IsEnabled = false;
                    //            indicator.IsVisible = true;
                    //            if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    //            {
                    //                RegistrationModel postModel = new RegistrationModel()
                    //                {
                    //                    activated = "N",
                    //                    activationcode = UtilityEL.GetRandomString(6),
                    //                    email = registerViewModel.EmailAdmin.Trim(),
                    //                    firstname = registerViewModel.FirstNameAdmin,
                    //                    lastname = registerViewModel.LastNameAdmin,
                    //                    loggedin = "N",
                    //                    password = registerViewModel.PasswordAdmin,
                    //                    registerdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                    //                    salonid = registerViewModel.SalonId,
                    //                    IsBarber = registerViewModel.IsBarberAdmin
                    //                };
                    //                //RegistrationService objRegService = new RegistrationService();

                    //                ApiResult apiCheckExistence = await objRegService.CheckEmailExistence(postModel.email.Trim(), postModel.salonid);

                    //                if (apiCheckExistence == null || apiCheckExistence.StatusCode != 200)
                    //                {
                    //                    EmailExistCheckModel checkExistence = UtilityEL.ToModel<EmailExistCheckModel>(apiCheckExistence.Response);

                    //                    if (checkExistence == null || checkExistence.id <= 0)
                    //                    {
                    //                        //if (switchCanSendEmail.IsToggled)
                    //                        if (switchCanSendEmail.IsOn.Value)
                    //                        {
                    //                            postModel.canSendEmail = 1;
                    //                        }
                    //                        else
                    //                        {
                    //                            postModel.canSendEmail = 0;
                    //                        }
                    //                        gdPopupPolicy.IsVisible = true;
                    //                        gdMain.IsVisible = false;

                    //                        //ApiResult result = await objRegService.RegisterUser(postModel, postModel.canSendEmail);

                    //                        //if (result != null && result.StatusCode == 200)
                    //                        //{
                    //                        //    await DisplayAlert("Success!", result.StatusMessage, "Ok");
                    //                        //    //Send email
                    //                        //    using (EmailService obj = new EmailService())
                    //                        //    {
                    //                        //        RegistrationEmailEL emailModel = new RegistrationEmailEL()
                    //                        //        {
                    //                        //            activationcode = postModel.activationcode,
                    //                        //            email = postModel.email.Trim(),
                    //                        //            firstname = postModel.firstname,
                    //                        //            lastname = postModel.lastname,
                    //                        //            password = postModel.password,
                    //                        //            username = postModel.firstname + " " + postModel.lastname,
                    //                        //            vSalonname = registerViewModel.SalonName,
                    //                        //            vSalonTel = registerViewModel.Salon_TelephoneNo,
                    //                        //            vSalonWeb = registerViewModel.Salon_Link
                    //                        //        };

                    //                        //        await obj.RegistrationEmail(emailModel);
                    //                        //    }

                    //                        //    await DisplayAlert("Success!", "An email has been sent to " + postModel.email.Trim() + " with the Activation Code, please check your inbox or junkbox", "Ok");
                    //                        //    await Navigation.PopModalAsync(true);
                    //                        //}
                    //                        //else
                    //                        //{
                    //                        //    await DisplayAlert("Error!", "Failed to register user. Please try again some time later.", "Ok");
                    //                        //}
                    //                    }
                    //                    else
                    //                    {
                    //                        await DisplayAlert("Error!", "Email id already exists. Please choose another email id.", "Ok");
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    await DisplayAlert("Error!", "Email id already exists. Please choose another email id.", "Ok");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    //            }
                    //            indicator.IsVisible = false;
                    //            registerViewModel.IsEnabled = true;
                    //        }
                    //        else
                    //        {
                    //            string validationMessage = string.Empty;
                    //            if (!bhvFirstNameAdmin.IsValid)
                    //            {
                    //                validationMessage = $"{bhvFirstNameAdmin.Message}";
                    //            }
                    //            else if (!bhvLastNameAdmin.IsValid)
                    //            {
                    //                validationMessage = $"{bhvLastNameAdmin.Message}";
                    //            }
                    //            else if (!bhvValidPasswordAdmin.IsValid)
                    //            {
                    //                validationMessage = $"{bhvValidPasswordAdmin.Message}";
                    //            }
                    //            else if (!bhvIsValidEmailAdmin.IsValid)
                    //            {
                    //                validationMessage = $"{bhvIsValidEmailAdmin.Message}";
                    //            }
                    //            if (!string.IsNullOrEmpty(validationMessage))
                    //            {
                    //                App.toastConfig.Message = validationMessage;
                    //                UserDialogs.Instance.Toast(App.toastConfig);
                    //            }
                    //        }
                    //    }
                    //    break;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void entryFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Admin = tabView.SelectedTabIndex;
            switch (Admin)
            {
                case 0:
                    {
                        if (string.IsNullOrEmpty(entryFirstName.Text))
                        {
                            entryFirstName.Text = string.Empty;
                        }
                        else
                        {
                            char[] letters = entryFirstName.Text.ToCharArray();
                            letters[0] = char.ToUpper(letters[0]);
                            entryFirstName.Text = new string(letters);
                        }
                        //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
                        if (bhvFirstName.IsShow)
                        {
                            App.toastConfig.Message = bhvFirstName.Message;
                            UserDialogs.Instance.Toast(App.toastConfig);
                        }
                    }
                    break;
                //case 1:
                //    {
                //        if (string.IsNullOrEmpty(entryFirstNameAdmin.Text))
                //        {
                //            entryFirstNameAdmin.Text = string.Empty;
                //        }
                //        else
                //        {
                //            char[] letters = entryFirstNameAdmin.Text.ToCharArray();
                //            letters[0] = char.ToUpper(letters[0]);
                //            entryFirstNameAdmin.Text = new string(letters);
                //        }
                //        //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
                //        if (bhvFirstNameAdmin.IsShow)
                //        {
                //            App.toastConfig.Message = bhvFirstNameAdmin.Message;
                //            UserDialogs.Instance.Toast(App.toastConfig);
                //        }
                //    }
                //    break;
            }


        }
        private void entryLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var Admin = tabView.SelectedTabIndex;
            switch (Admin)
            {
                case 0:
                    {
                        if (string.IsNullOrEmpty(entryLastName.Text))
                        {
                            entryLastName.Text = string.Empty;
                        }
                        else
                        {
                            char[] letters = entryLastName.Text.ToCharArray();
                            letters[0] = char.ToUpper(letters[0]);
                            entryLastName.Text = new string(letters);
                        }
                        //Debug.WriteLine($"IsShow = {bhvLastName.IsShow}\nMessage = {bhvLastName.Message}");
                        if (bhvLastName.IsShow)
                        {
                            App.toastConfig.Message = bhvLastName.Message;
                            UserDialogs.Instance.Toast(App.toastConfig);
                        }
                    }
                    break;
                //case 1:
                //    {
                //        if (string.IsNullOrEmpty(entryLastNameAdmin.Text))
                //        {
                //            entryLastNameAdmin.Text = string.Empty;
                //        }
                //        else
                //        {
                //            char[] letters = entryLastNameAdmin.Text.ToCharArray();
                //            letters[0] = char.ToUpper(letters[0]);
                //            entryLastNameAdmin.Text = new string(letters);
                //        }
                //        //Debug.WriteLine($"IsShow = {bhvLastName.IsShow}\nMessage = {bhvLastName.Message}");
                //        if (bhvLastNameAdmin.IsShow)
                //        {
                //            App.toastConfig.Message = bhvLastNameAdmin.Message;
                //            UserDialogs.Instance.Toast(App.toastConfig);
                //        }
                //    }
                //    break;
            }
        }
        private async void OnLoginClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
            //await Navigation.PushModalAsync(new Help(), true);
        }
        private async void btnOk_Clicked(object sender, EventArgs e)
        {
            if (NetworkConnectionMgmt.IsConnectedToNetwork())
            {
                //if (switchAgree.IsToggled)
                    if (switchAgree.IsOn.Value)
                    {
                    indicator.IsVisible = true;
                    RegistrationModel postModel = new RegistrationModel()
                    {
                        activated = "N",
                        activationcode = UtilityEL.GetRandomString(6),
                        email = tabView.SelectedTabIndex == 0 ? registerViewModel.Email.Trim() : registerViewModel.EmailAdmin.Trim(),
                        firstname = tabView.SelectedTabIndex == 0 ? registerViewModel.FirstName : registerViewModel.FirstNameAdmin,
                        lastname = tabView.SelectedTabIndex == 0 ? registerViewModel.LastName : registerViewModel.LastNameAdmin,
                        loggedin = "N",
                        password = tabView.SelectedTabIndex == 0 ? registerViewModel.Password : registerViewModel.PasswordAdmin,
                        registerdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                        salonid = registerViewModel.SalonId,
                        IsBarber = tabView.SelectedTabIndex == 0 ? registerViewModel.IsBarber : registerViewModel.IsBarberAdmin,
                        UserLevel = tabView.SelectedTabIndex == 0 ? (registerViewModel.IsBarber?1:0) : 2//0-client,1-staff,2-admin
                    };

                    if (switchCanSendEmail.IsOn.Value)
                    {
                        postModel.canSendEmail = 1;
                    }
                    else
                    {
                        postModel.canSendEmail = 0;
                    }

                    //if(tabView.SelectedTabIndex == 0)
                    //{
                    //    if (switchCanSendEmail.IsOn.Value)
                    //    {
                    //        postModel.canSendEmail = 1;
                    //    }
                    //    else
                    //    {
                    //        postModel.canSendEmail = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    if (switchCanSendEmailAdmin.IsOn.Value)
                    //    {
                    //        postModel.canSendEmail = 1;
                    //    }
                    //    else
                    //    {
                    //        postModel.canSendEmail = 0;
                    //    }
                    //}
                    RegisterAdminResponseModel result = await objRegService.RegisterUser(postModel, postModel.canSendEmail);
                    if (result != null && result.StatusCode == 200)
                    {
                        MapUserSalonResponseModel mapresult = await objSalonService.MapSalonUser(new MapUserSalon
                        {
                            UserId = $"{result.Response.USerName}",
                            SalonId = $"{postModel.salonid}",
                            CreatedBy = $"{result.Response.USerName}"
                        });
                        if (mapresult != null && mapresult.StatusCode == 200)
                        {

                            await DisplayAlert("Success!", result.StatusMessage, "Ok");
                            //Send email
                            using (EmailService obj = new EmailService())
                            {
                                RegistrationEmailEL emailModel = new RegistrationEmailEL()
                                {
                                    activationcode = postModel.activationcode,
                                    email = postModel.email.Trim(),
                                    firstname = postModel.firstname,
                                    lastname = postModel.lastname,
                                    password = postModel.password,
                                    username = postModel.firstname + " " + postModel.lastname,
                                    vSalonname = registerViewModel.SalonName,
                                    vSalonTel = registerViewModel.Salon_TelephoneNo,
                                    vSalonWeb = registerViewModel.Salon_Link
                                };

                                await obj.RegistrationEmail(emailModel);
                            }

                            await DisplayAlert("Success!", "An email has been sent to " + postModel.email.Trim() + " with the Activation Code, please check your inbox or junkbox", "Ok");
                            await Navigation.PopModalAsync(true);

                            gdPopupPolicy.IsVisible = false;
                            gdMain.IsVisible = true;
                        }                        //scMain.Opacity = 1;
                    }
                    else
                    {
                        await DisplayAlert("Error!", $"{result?.StatusMessage} Failed to register user. Please try again some time later.", "Ok");
                        gdPopupPolicy.IsVisible = false;
                        gdMain.IsVisible = true;
                    }
                    indicator.IsVisible = false;
                }
                else
                    await DisplayAlert("Alert !", "You must accept our terms and conditions if you want to use this application.", "OK");
            }
            else
            {
                await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
            }
        }

        private void EntryEmail_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var Admin = tabView.SelectedTabIndex;
            switch (Admin)
            {
                case 0:
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
                    break;
                //case 1:
                //    {
                //        if (string.IsNullOrEmpty(entryEmailAdmin.Text))
                //        {
                //            entryEmailAdmin.Text = string.Empty;
                //        }
                //        else
                //        {
                //            char[] letters = entryEmailAdmin.Text.ToCharArray();
                //            letters[0] = char.ToUpper(letters[0]);
                //            entryEmailAdmin.Text = new string(letters);
                //        }
                //        //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
                //        if (bhvIsValidEmailAdmin.IsShow)
                //        {
                //            App.toastConfig.Message = bhvIsValidEmailAdmin.Message;
                //            UserDialogs.Instance.Toast(App.toastConfig);
                //        }
                //    }
                //    break;
            }
        }

        private void EntryPassword_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var Admin = tabView.SelectedTabIndex;
            switch (Admin)
            {
                case 0:
                    {
                        if (string.IsNullOrEmpty(entryPassword.Text))
                        {
                            entryPassword.Text = string.Empty;
                        }
                        else
                        {
                            char[] letters = entryPassword.Text.ToCharArray();
                            letters[0] = char.ToUpper(letters[0]);
                            entryPassword.Text = new string(letters);
                        }
                        //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
                        if (bhvValidPassword.IsShow)
                        {
                            App.toastConfig.Message = bhvValidPassword.Message;
                            UserDialogs.Instance.Toast(App.toastConfig);
                        }
                    }
                    break;
                //case 1:
                //    {
                //        if (string.IsNullOrEmpty(entryPasswordAdmin.Text))
                //        {
                //            entryPasswordAdmin.Text = string.Empty;
                //        }
                //        else
                //        {
                //            char[] letters = entryPasswordAdmin.Text.ToCharArray();
                //            letters[0] = char.ToUpper(letters[0]);
                //            entryPasswordAdmin.Text = new string(letters);
                //        }
                //        //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
                //        if (bhvValidPasswordAdmin.IsShow)
                //        {
                //            App.toastConfig.Message = bhvValidPasswordAdmin.Message;
                //            UserDialogs.Instance.Toast(App.toastConfig);
                //        }
                //    }
                //    break;
            }
        }
    }
}