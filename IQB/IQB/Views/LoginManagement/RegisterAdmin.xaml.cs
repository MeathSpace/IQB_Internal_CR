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
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Syncfusion.Buttons.XForms;
using IQB.Utility;

namespace IQB.Views.LoginManagement
{
    public partial class RegisterAdmin : ContentPage
    {
        private RegisterViewModel registerViewModel = null;
        RegistrationService objRegService = new RegistrationService();
        WriteErrorLog errLog = new WriteErrorLog();
        private string uName;
        private List<Country> countries = new List<Country>();
        private Country selectedCountry = new Country();
        public string TWLink { get; set; }
        public string FBLink { get; set; }
        public string INSTALink { get; set; }
        public bool IsImageUploaded { get; private set; }
        private int SalonID = 0;
        private SalonServices objSalonService = new SalonServices();
        private string fileName;
        private byte[] upfilebytes;
        private string fName;
        private SalonModel salonModel;

        public RegisterAdmin()
        {
            //NavigationPage.SetHasNavigationBar(this, false);

        }

        public RegisterAdmin(SalonModel salonModel, byte[] upfilebytes, string fileName, string fName)
        {
            InitializeComponent();
            this.salonModel = salonModel;
            this.upfilebytes = upfilebytes;
            this.fileName = fileName;
            this.fName = fName;


            registerViewModel = new RegisterViewModel();

            BindingContext = registerViewModel;
        }

        private async void CancelRegistration(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void RegisterUser(object sender, EventArgs args)
        {

            try
            {
                if (bhvFirstNameAdmin.IsValid && bhvLastNameAdmin.IsValid && bhvValidPasswordAdmin.IsValid && bhvIsValidEmailAdmin.IsValid)
                {
                    registerViewModel.IsEnabled = false;
                    indicator.IsVisible = true;
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        RegistrationModel postModel = new RegistrationModel()
                        {
                            activated = "N",
                            activationcode = UtilityEL.GetRandomString(6),
                            email = registerViewModel.EmailAdmin.Trim(),
                            firstname = registerViewModel.FirstNameAdmin,
                            lastname = registerViewModel.LastNameAdmin,
                            loggedin = "N",
                            password = registerViewModel.PasswordAdmin,
                            registerdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            salonid = registerViewModel.SalonId,
                            IsBarber= registerViewModel.IsBarberAdmin
                        };
                        //RegistrationService objRegService = new RegistrationService();

                        ApiResult apiCheckExistence = await objRegService.CheckEmailExistence(postModel.email.Trim(), postModel.salonid);

                        if (apiCheckExistence == null || apiCheckExistence.StatusCode != 200)
                        {
                            EmailExistCheckModel checkExistence = UtilityEL.ToModel<EmailExistCheckModel>(apiCheckExistence.Response);

                            if (checkExistence == null || checkExistence.id <= 0)
                            {
                               // if (switchCanSendEmailAdmin.IsToggled)
                                if (switchCanSendEmailAdmin.IsOn.Value)
                                {
                                    postModel.canSendEmail = 1;
                                }
                                else
                                {
                                    postModel.canSendEmail = 0;
                                }
                                indicator.IsVisible = false;
                                gdPopupPolicy.IsVisible = true;
                                gdMain.IsVisible = false;

                                #region Unused
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
                                #endregion
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
                    if (!bhvFirstNameAdmin.IsValid)
                    {
                        validationMessage = $"{bhvFirstNameAdmin.Message}";
                    }
                    else if (!bhvLastNameAdmin.IsValid)
                    {
                        validationMessage = $"{bhvLastNameAdmin.Message}";
                    }
                    else if (!bhvIsValidEmailAdmin.IsValid)
                    {
                        validationMessage = $"{bhvIsValidEmailAdmin.Message}";
                    }
                    else if (!bhvValidPasswordAdmin.IsValid)
                    {
                        validationMessage = $"{bhvValidPasswordAdmin.Message}";
                    }
                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        App.toastConfig.Message = validationMessage;
                        UserDialogs.Instance.Toast(App.toastConfig);
                    }
                }

            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
            }
        }

        private void entryFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(entryFirstNameAdmin.Text))
            {
                entryFirstNameAdmin.Text = string.Empty;
            }
            else
            {
                char[] letters = entryFirstNameAdmin.Text.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                entryFirstNameAdmin.Text = new string(letters);
            }
            //Debug.WriteLine($"IsShow = {bhvFirstName.IsShow}\nMessage = {bhvFirstName.Message}");
            if (bhvFirstNameAdmin.IsShow)
            {
                App.toastConfig.Message = bhvFirstNameAdmin.Message;
                UserDialogs.Instance.Toast(App.toastConfig);
            }

        }
        private void entryLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(entryLastNameAdmin.Text))
            {
                entryLastNameAdmin.Text = string.Empty;
            }
            else
            {
                char[] letters = entryLastNameAdmin.Text.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                entryLastNameAdmin.Text = new string(letters);
            }
            //Debug.WriteLine($"IsShow = {bhvLastName.IsShow}\nMessage = {bhvLastName.Message}");
            if (bhvLastNameAdmin.IsShow)
            {
                App.toastConfig.Message = bhvLastNameAdmin.Message;
                UserDialogs.Instance.Toast(App.toastConfig);
            }
        }
        //private void OnEditSalonClicked(object sender, EventArgs args)
        //{
        //    gdCreateSalon.IsVisible = true;
        //    gdMain.IsVisible = false;
        //    //await Navigation.PushModalAsync(new Help(), true);
        //}
        private async void btnOk_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (NetworkConnectionMgmt.IsConnectedToNetwork())
                {
                    // (switchAgree.IsToggled)
                        if (switchAgree.IsOn.Value)
                        {
                        indicator.IsVisible = true;
                        RegistrationModel postModel = new RegistrationModel()
                        {
                            activated = "N",
                            activationcode = UtilityEL.GetRandomString(6),
                            email = registerViewModel.EmailAdmin.Trim(),
                            firstname = registerViewModel.FirstNameAdmin,
                            lastname = registerViewModel.LastNameAdmin,
                            loggedin = "N",
                            password = registerViewModel.PasswordAdmin,
                            registerdate = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            salonid = registerViewModel.SalonId,
                            IsBarber = registerViewModel.IsBarberAdmin,
                            UserLevel = 2//0-client,1-staff,2-admin
                        };
                        //SalonModel salonModel = new SalonModel
                        //{
                        //    SalonAppIcon = "",
                        //    SalonName = entrySalonName.Text,
                        //    CustomerName = entryCustomerName.Text,
                        //    UserName = uName,
                        //    EmailAddress = entryEmail.Text,
                        //    city = entryCity.Text,
                        //    county = selectedCountry.CountryName,
                        //    PostCode = entryZip.Text,
                        //    Address = entryAddr.Text,//todo

                        //    ContactTel = entryTel.Text,
                        //    SalonWebsite = "",
                        //    SalonTwitter = TWLink,
                        //    SalonFacebook = FBLink,
                        //    SalonInstagram = INSTALink,
                        //    SalonOnStop = "N",
                        //    FtpFolderName = "",
                        //    AccountExpiryDate = "",
                        //    CountryCode = selectedCountry.CountryCode,
                        //    UpdatedBy = "",
                        //    UpdatedOn = "",
                        //    Currency = "",
                        //    CreatedBy = uName,
                        //    CreatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                        //};
                        RegisterAdminResponseModel result = await objRegService.RegisterAdminUser(postModel, postModel.canSendEmail);
                        if (result != null && result.StatusCode == 200)
                        {
                            //errLog.WinrtErrLogTest("Admin registration successfull", "RegisterAdmin", "btnOk_Clicked");
                            //#region  send mail to user registered
                            //using (EmailService obj = new EmailService())
                            //{
                            //    RegistrationEmailEL emailModel = new RegistrationEmailEL()
                            //    {
                            //        activationcode = postModel.activationcode,
                            //        email = postModel.email.Trim(),
                            //        firstname = postModel.firstname,
                            //        lastname = postModel.lastname,
                            //        password = postModel.password,
                            //        username = postModel.firstname + " " + postModel.lastname,
                            //        vSalonname = registerViewModel.SalonName,
                            //        vSalonTel = registerViewModel.Salon_TelephoneNo,
                            //        vSalonWeb = registerViewModel.Salon_Link
                            //    };
                            //    await obj.RegistrationEmail(emailModel);
                            //}
                            //#endregion

                            #region create salon for the registered user
                            uName = $"{result.Response.USerName}";
                            salonModel.UserName = uName;
                            salonModel.CreatedBy = uName;
                            SalonResponseModel sresult = await objSalonService.CreateSalon(salonModel);
                            if (sresult != null && sresult.StatusCode == 200)
                            {
                                
                                SalonID = sresult?.Response?.SalonId ?? 0;
                                //errLog.WinrtErrLogTest("salon registration successfull and salonid="+ SalonID.ToString(), "RegisterAdmin", "btnOk_Clicked");
                                //check whether any Image is Uploaded 
                                //(if uploaded file byte is null then directly show success message else upload the image followed by Updating Salon AppIcon and finally showing success message)
                                #region MapUserSalon
                                MapUserSalonResponseModel mapresult = await objSalonService.MapSalonUser(new MapUserSalon
                                {
                                    UserId = uName,
                                    SalonId = $"{SalonID}",
                                    CreatedBy = uName
                                });
                                if (mapresult != null && mapresult.StatusCode == 200)
                                {
                                    //errLog.WinrtErrLogTest("salon map successfull and salonid=" + SalonID.ToString(), "RegisterAdmin", "btnOk_Clicked");

                                    #region  send mail to user registered
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
                                            vSalonname = salonModel.SalonName,
                                            vSalonTel = salonModel.ContactTel,
                                            vSalonWeb = salonModel.SalonWebsite
                                        };
                                        await obj.RegistrationEmail(emailModel);
                                    }
                                    #endregion

                                    #region Update Salon Image 
                                    if (upfilebytes != null)
                                    {
                                        IsImageUploaded = Task.Run(async () => await UploadProfileImage()).Result;
                                        if (IsImageUploaded)
                                        {
                                           // errLog.WinrtErrLogTest("Upload image done", "RegisterAdmin", "btnOk_Clicked");

                                            salonModel.SalonId = $"{SalonID}";
                                            salonModel.SalonAppIcon = fName ?? "";
                                            salonModel.UpdatedBy = uName;
                                            salonModel.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                                            SalonResponseModel finalResult = await objSalonService.UpdateSalon(salonModel);
                                            if (finalResult != null && finalResult.StatusCode == 200)
                                            {
                                                //errLog.WinrtErrLogTest("Update salon done.", "RegisterAdmin", "btnOk_Clicked");
                                                await ShowSuccessPopup(postModel.email.Trim());
                                            }
                                               
                                            else
                                                await DisplayAlert("Error!", "Registration process interrupted,Failed to update salon. Please try again some time later.", "Ok");
                                        }
                                        else
                                            await DisplayAlert("Error!", "Registration process interrupted,Failed to upload salon image. Please try again some time later.", "Ok");
                                    }
                                    else
                                        await ShowSuccessPopup(postModel.email.Trim());
                                    #endregion
                                }
                                else
                                {
                                    //Code for DeleteAdmin
                                    DeleteRegisteredUserResponseModel resultDeleteuser = await objRegService.DeleteRegisteredUser($"{result.Response.USerName}");
                                    
                                    //Code for DeleteAdmin
                                    await DisplayAlert("Error!", "Registration process interrupted,Failed to map salon. Please try again some time later.", "Ok");
                                }
                                #endregion
                            }
                            else
                                await DisplayAlert("Error!", "Registration process interrupted,Failed to create salon.  Please try again some time later.", "Ok");
                            #endregion
                        }
                        else
                        {
                            if (result.StatusMessage != String.Empty)
                                await DisplayAlert("Error!", result.StatusMessage, "Ok");
                            else
                                await DisplayAlert("Error!", "Failed to register user. Please try again some time later.", "Ok");
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
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Failed to register user. Please try again some time later.", "Ok");
                gdPopupPolicy.IsVisible = false;
                //gdMain.IsVisible = false;
                //gdCreateSalon.IsVisible = true;
                indicator.IsVisible = false;
                await Navigation.PopAsync(true);
            }
        }

        private async Task ShowSuccessPopup(string email)
        {
            await DisplayAlert("Admin User Registered Successfully!", "An email has been sent to " + email + " with the Activation Code, please check your inbox or junkbox", "Ok");
            gdPopupPolicy.IsVisible = false;
            gdMain.IsVisible = false;
            Application.Current.Properties["IsAdminUserSalonRegistered"] = true;
            //gdCreateSalon.IsVisible = true;
            await Navigation.PopAsync(true);
        }

        private async Task<bool> UploadProfileImage()
        {
            //variable
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("UploadSalonAppIcon.php");
            try
            {
                HttpClient client = new HttpClient();

                string boundary = "---8393774hhy37373773";
                MultipartFormDataContent content = new MultipartFormDataContent(boundary);

                var ImageContent = new ByteArrayContent(upfilebytes);

                ImageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

                string uploadedImageName = fileName;
                int index = uploadedImageName.LastIndexOf('.');
                var ext = uploadedImageName.Substring(index + 1);
                fName = $"{CommonEL.SalonAppIconBaseName}{SalonID}.{ext}";
                ImageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fName,
                    Name = "file",

                };
                content.Add(ImageContent);
                //content.Add(userIdContent, "userid");

                //upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, content);
                //ApiResult resultObject = new ApiResult();
                ////read response result as a string async into json var
                //var responsestr = response.Content.ReadAsStringAsync().Result;
                //if (!string.IsNullOrWhiteSpace(responsestr))
                //{
                //    resultObject = UtilityEL.ToApiModel(responsestr);
                //}
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return false;
            }
        }
    }
}