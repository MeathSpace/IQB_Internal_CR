using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.SalonModels;
using IQB.IQBServices;
using IQB.IQBServices.SalonServices;
using IQB.ViewModel.AuthViewModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.Views.SalonManagement
{
    public partial class CreateSalon : ContentPage
    {
        private string uName;
        public string TWLink { get; set; }
        public string FBLink { get; set; }
        public string INSTALink { get; set; }
        public bool IsImageUploaded { get; private set; }
        private int SalonID;
        private SalonServices objSalonService = new SalonServices();
        private string fileName;
        private byte[] upfilebytes;
        private string fName;
        private SalonModel salonDetails = new SalonModel();
        private List<Country> countries = new List<Country>();
        private List<Currency> currencies = new List<Currency>();
        private Country selectedCountry = new Country();
        private Currency selectedCurrency = new Currency();

        public CreateSalon(string salonID = "0", bool editMode = false)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SalonID = Convert.ToInt32(salonID);
            //Countries = new List<Country>() {
            //    new Country
            //    {
            //        Name="TestCountry1",
            //        Code="TC1"
            //    },
            //    new Country
            //    {
            //        Name="India",
            //        Code="IN"
            //    },
            //    new Country
            //    {
            //        Name="TestCountry2",
            //        Code="TC2"
            //    },
            //    new Country
            //    {
            //        Name="TestCountry3",
            //        Code="TC3"
            //    }
            //};
            GetCountryList();
            GetCurrencyList();
            ProfileImage.Source = ImageSource.FromFile("noimage.png");
            if (!editMode)
            {
                gdMain.IsEnabled = false;
            }
            else
                gdMain.IsEnabled = true;
            if (SalonID > 0)
            {
                PopulateExistingData();
                btnRegister.Text = "Update";
                btnRegister.Clicked += BtnUpdate_Clicked;
            }
            else
            {
                this.Title = "Create Salon";
                btnRegister.Text = "Register";
                btnRegister.Clicked += BtnRegister_Clicked;
                pickerCountry.ItemsSource = countries;
                pickerCurrency.ItemsSource = currencies;


                var currency = currencies.FirstOrDefault(x => x.symbol == "£");
                if(currency == null)
                {
                    currency = currencies.FirstOrDefault(x => x.currency_string == "GBP - British Pound");
                }
                if (currency != null)
                {
                    pickerCurrency.SelectedItem = currency;
                    selectedCurrency = currency;
                    lblCurrency.Text = currency.currency_string;
                }
            }
        }

        private void GetCountryList()
        {
            try
            {
                var countriesResult = Task.Run(async () => await objSalonService.GetCountryList()).Result;
                if (countriesResult?.StatusCode == 200)
                {
                    countries = countriesResult?.Response?.ToList();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetCurrencyList()
        {
            try
            {
                var currenciesResult = Task.Run(async () => await objSalonService.GetCurrencyList()).Result;
                if (currenciesResult?.StatusCode == 200)
                {
                    currencies = currenciesResult?.Response?.ToList();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void PopulateExistingData()
        {
            try
            {
                var salonDetailsResponse = await objSalonService.GetSalonDetailsByID($"{SalonID}");
                if (salonDetailsResponse?.StatusCode == 200)
                {
                    if (salonDetailsResponse.Response != null)
                    {
                        salonDetails = salonDetailsResponse.Response;
                        if (!countries.Any(x => x.CountryName == salonDetails.county))
                        {
                            var country = new Country
                            {
                                CountryName = salonDetails.county,
                                CountryCode = salonDetails.CountryCode
                            };
                            countries.Add(country);
                            pickerCountry.ItemsSource = countries;
                            pickerCountry.SelectedItem = country;
                            selectedCountry = country;
                        }
                        else
                        {
                            var country = countries.Where(x => x.CountryName == salonDetails.county).FirstOrDefault();
                            pickerCountry.ItemsSource = countries;
                            pickerCountry.SelectedItem = country;
                            selectedCountry = country;
                        }

                        if (!currencies.Any(x => x.symbol == salonDetails.Currency))
                        {
                            var currency = currencies.FirstOrDefault(x => x.symbol == "£");

                            if (currency == null)
                            {
                                currency = currencies.FirstOrDefault(x => x.currency_string == "GBP - British Pound");
                            }
                            if (currency != null)
                            {
                                pickerCurrency.ItemsSource = currencies;
                                pickerCurrency.SelectedItem = currency;
                                selectedCurrency = currency;
                                lblCurrency.Text = currency.currency_string;
                            }
                        }
                        else
                        {
                            var currency = currencies.FirstOrDefault(x => x.symbol == salonDetails.Currency);
                            pickerCurrency.ItemsSource = currencies;
                            pickerCurrency.SelectedItem = currency;
                            selectedCurrency = currency;
                            lblCurrency.Text = currency.currency_string;
                        }


                        ProfileImage.Source = new UriImageSource
                        {
                            Uri = new Uri(CommonEL.SalonImageLocation + salonDetails.SalonAppIcon),
                            CachingEnabled = false,
                        };
                        //         ProfileImage.Source = CommonEL.SalonImageLocation + salonDetails.SalonAppIcon;
                        entrySalonName.Text = salonDetails.SalonName;
                        entryCustomerName.Text = salonDetails.CustomerName;
                        uName = salonDetails.UserName;
                        entryEmail.Text = salonDetails.EmailAddress;
                        entryCity.Text = salonDetails.city;
                        entryAddr.Text = salonDetails.Address;
                        lblCountry.Text = salonDetails.county;

                        entryZip.Text = salonDetails.PostCode;
                        entryTel.Text = salonDetails.ContactTel;
                        entrySalonWebsite.Text = salonDetails.SalonWebsite;
                        TWLink = salonDetails.SalonTwitter;
                        FBLink = salonDetails.SalonFacebook;
                        INSTALink = salonDetails.SalonInstagram;
                        if (salonDetails.PaymentGatewaySettings[0].merchant_email != null)
                            MerchantEmail.Text = salonDetails.PaymentGatewaySettings[0].merchant_email;
                        //MerchnatId.Text = salonDetailsResponse.PaymentGatewaySettings.merchant_id;
                        //PrivateKey.Text = salonDetailsResponse.PaymentGatewaySettings.private_key;
                        //PublicKey.Text = salonDetailsResponse.PaymentGatewaySettings.public_key;
                        //TokenizationKey.Text = salonDetailsResponse.PaymentGatewaySettings.tokenization_key;

                    }
                }

            }
            catch(Exception ex)
            {

            }
          
        }

        private async void CancelRegistration(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            //using (LoginViewModel objLogin = new LoginViewModel())
            try
            {
                uName = Convert.ToString(Application.Current.Properties["UserName"]);
                if (bhvSalonName.IsValid && bhvCustomerName.IsValid && bhvIsValidEmail.IsValid && bhvMobileNo.IsValid && bhvCity.IsValid && bhvZip.IsValid && bhvAddress.IsValid)
                {
                    indicator.IsVisible = true;
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        SalonModel requestModel = new SalonModel
                        {
                            SalonAppIcon = "",
                            SalonName = entrySalonName.Text,
                            CustomerName = entryCustomerName.Text,
                            UserName = uName,
                            EmailAddress = entryEmail.Text,
                            city = entryCity.Text,
                            county = selectedCountry.CountryName,
                            Address = entryAddr.Text,//todo

                            PostCode = entryZip.Text,
                            ContactTel = entryTel.Text,
                            SalonWebsite = entrySalonWebsite.Text,
                            SalonTwitter = TWLink,
                            SalonFacebook = FBLink,
                            SalonInstagram = INSTALink,
                            SalonOnStop = "N",
                            FtpFolderName = "",
                            AccountExpiryDate = "",
                            CountryCode = selectedCountry.CountryCode,
                            UpdatedBy = "",
                            UpdatedOn = "",
                            Currency = selectedCurrency.symbol,
                            CreatedBy = uName,
                            CreatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            PaymentGatewaySettings = new List<PaymentGatewayData>(new[] { new PaymentGatewayData { merchant_email = MerchantEmail.Text } }),
                            //           PaymentGatewaySettings = new List<PaymentGatewayData>(new[] { new PaymentGatewayData { merchant_id = MerchnatId.Text, private_key = PrivateKey.Text, public_key = PublicKey.Text, tokenization_key = TokenizationKey.Text } }),
                        };
                        SalonResponseModel result = await objSalonService.CreateSalon(requestModel);
                        //to map
                        if (result != null && result.StatusCode == 200)
                        {
                            SalonID = result?.Response?.SalonId ?? 0;
                            IsImageUploaded = Task.Run(async () => await UploadProfileImage()).Result;
                            if (IsImageUploaded)
                            {
                                #region MapUserSalon
                                MapUserSalonResponseModel mapresult = await objSalonService.MapSalonUser(new MapUserSalon
                                {
                                    UserId = uName,
                                    SalonId = $"{SalonID}",
                                    CreatedBy = uName
                                });
                                if (mapresult != null && mapresult.StatusCode == 200)
                                {
                                    requestModel.SalonId = $"{SalonID}";
                                    requestModel.SalonAppIcon = fName;
                                    requestModel.UpdatedBy = uName;
                                    requestModel.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                                    SalonResponseModel finalResult = await objSalonService.UpdateSalon(requestModel);
                                    if (finalResult != null && finalResult.StatusCode == 200)
                                    {
                                        await DisplayAlert("Success!", "Salon Created successfully", "Ok");
                                        await Navigation.PopAsync(true);
                                    }
                                }
                                else
                                    await DisplayAlert("Error!", "Failed to create Salon. Please try again some time later.", "Ok");
                                #endregion
                            }
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
                    if (!bhvSalonName.IsValid)
                    {
                        validationMessage = $"{bhvSalonName.Message}";
                    }
                    else if (!bhvCustomerName.IsValid)
                    {
                        validationMessage = $"{bhvCustomerName.Message}";
                    }
                    else if (!bhvIsValidEmail.IsValid)
                    {
                        validationMessage = $"{bhvIsValidEmail.Message}";
                    }
                    else if (!bhvMobileNo.IsValid)
                    {
                        validationMessage = $"{bhvMobileNo.Message}";
                    }
                    else if (!bhvAddress.IsValid)
                    {
                        validationMessage = $"{bhvAddress.Message}";
                    }
                    else if (!bhvCity.IsValid)
                    {
                        validationMessage = $"{bhvCity.Message}";
                    }
                    else if (!bhvZip.IsValid)
                    {
                        validationMessage = $"{bhvZip.Message}";
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
                await DisplayAlert("Error!", "Failed to create Salon. Please try again some time later.", "Ok");
                indicator.IsVisible = false;
            }
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (bhvSalonName.IsValid && bhvCustomerName.IsValid && bhvIsValidEmail.IsValid && bhvMobileNo.IsValid && bhvCity.IsValid && bhvZip.IsValid && bhvAddress.IsValid)
                {
                    indicator.IsVisible = true;
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        {
                            uName = Convert.ToString(Application.Current.Properties["UserName"]);
                        }
                        if(salonDetails.AccountExpiryDate == "0000-00-00")
                        {
                            salonDetails.AccountExpiryDate = "";
                        }

                        SalonModel requestModel = new SalonModel
                        {
                            SalonId = $"{SalonID}",
                            SalonAppIcon = salonDetails.SalonAppIcon,
                            SalonName = entrySalonName.Text,
                            CustomerName = entryCustomerName.Text,
                            UserName = uName,
                            EmailAddress = entryEmail.Text,
                            city = entryCity.Text,
                            county = selectedCountry.CountryName,
                            Address = entryAddr.Text,//todo

                            PostCode = entryZip.Text,
                            ContactTel = entryTel.Text,
                            SalonWebsite = entrySalonWebsite.Text,
                            SalonTwitter = TWLink,
                            SalonFacebook = FBLink,
                            SalonInstagram = INSTALink,
                            SalonOnStop = "N",
                            FtpFolderName = salonDetails.FtpFolderName,
                            AccountExpiryDate = salonDetails.AccountExpiryDate,
                            CountryCode = selectedCountry.CountryCode,
                            UpdatedBy = uName,
                            UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            Currency = selectedCurrency.symbol,
                            CreatedBy = salonDetails.CreatedBy,
                            CreatedOn = salonDetails.CreatedOn,
                            PaymentGatewaySettings = new List<PaymentGatewayData>(new[] { new PaymentGatewayData { merchant_email = MerchantEmail.Text } }),
                            //    PaymentGatewaySettings = new List<PaymentGatewayData>(new[] { new PaymentGatewayData { merchant_id = MerchnatId.Text, private_key = PrivateKey.Text, public_key = PublicKey.Text, tokenization_key = TokenizationKey.Text } }),
                        };
                        IsImageUploaded = Task.Run(async () => await UploadProfileImage()).Result;
                        if (IsImageUploaded)
                        {
                            if (!string.IsNullOrEmpty(fName))
                                requestModel.SalonAppIcon = fName;
                            SalonResponseModel finalResult = await objSalonService.UpdateSalon(requestModel);
                            //to map
                            if (finalResult != null && finalResult.StatusCode == 200)
                            {
                                await DisplayAlert("Success!", "Salon Updated successfully", "Ok");

                                SalonViewModel sv = new SalonViewModel();
                                if (requestModel.SalonId == sv.SalonId.ToString())
                                {
                                    sv.StoreSalonCode(sv.SCode, requestModel.SalonName, sv.SalonId, CommonEL.SalonImageLocation + requestModel.SalonAppIcon, requestModel.ContactTel, requestModel.SalonWebsite, requestModel.Address, sv.state, requestModel.city, requestModel.PostCode, sv.FtpFolderPath);
                                }


                                await Navigation.PopAsync(true);
                            }
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
                    if (!bhvSalonName.IsValid)
                    {
                        validationMessage = $"{bhvSalonName.Message}";
                    }
                    else if (!bhvCustomerName.IsValid)
                    {
                        validationMessage = $"{bhvCustomerName.Message}";
                    }
                    else if (!bhvIsValidEmail.IsValid)
                    {
                        validationMessage = $"{bhvIsValidEmail.Message}";
                    }
                    else if (!bhvMobileNo.IsValid)
                    {
                        validationMessage = $"{bhvMobileNo.Message}";
                    }
                    else if (!bhvAddress.IsValid)
                    {
                        validationMessage = $"{bhvAddress.Message}";
                    }
                    else if (!bhvCity.IsValid)
                    {
                        validationMessage = $"{bhvCity.Message}";
                    }
                    else if (!bhvZip.IsValid)
                    {
                        validationMessage = $"{bhvZip.Message}";
                    }
                    if (!string.IsNullOrEmpty(validationMessage))
                    {
                        App.toastConfig.Message = validationMessage;
                        UserDialogs.Instance.Toast(App.toastConfig);
                    }
                }
            }
            catch (Exception)
            {

                await DisplayAlert("Error!", "Failed to update Salon. Please try again some time later.", "Ok");
                indicator.IsVisible = false;
            }
        }

        private void SelectCountry_Tapped(object sender, EventArgs e)
        {
            pickerCountry.Focus();
            pickerCountry.SelectedIndexChanged += (s, ev) =>
            {
                selectedCountry = (Country)pickerCountry.SelectedItem;
                lblCountry.Text = ((Country)pickerCountry.SelectedItem).CountryName;
                lblCountry.TextColor = Color.FromHex("#FFFFFF");
            };
        }

        private void SelectCurrency_Tapped(object sender, EventArgs e)
        {
            pickerCurrency.Focus();
            pickerCurrency.SelectedIndexChanged += (s, ev) =>
            {
                selectedCurrency = (Currency)pickerCurrency.SelectedItem;
                lblCurrency.Text = ((Currency)pickerCurrency.SelectedItem).currency_string;
                lblCurrency.TextColor = Color.FromHex("#FFFFFF");
            };
        }

        #region SocialMediaLinks Add/Update
        private void ChangeLINK(PromptResult res, SocialMediaType socialMediaType)
        {
            if (res.Ok)
            {
                switch (socialMediaType)
                {
                    case SocialMediaType.Twitter:
                        TWLink = res.Value;
                        break;
                    case SocialMediaType.Facebook:
                        FBLink = res.Value;
                        break;
                    case SocialMediaType.Instagram:
                        INSTALink = res.Value;
                        break;
                }
            }
        }
        private void UploadTwitterLinkTapped(object sender, EventArgs e)
        {
            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Enter link to Twitter Profile",
                Text = TWLink,
                OnAction = (res) => ChangeLINK(res, SocialMediaType.Twitter)
            });
        }
        private void UploadFacebookLinkTapped(object sender, EventArgs e)
        {
            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Enter link to Facebook Profile",
                Text = FBLink,
                OnAction = (res) => ChangeLINK(res, SocialMediaType.Facebook)
            });
        }
        private void UploadInstagramLinkTapped(object sender, EventArgs e)
        {
            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Enter link to Instagram Profile",
                Text = INSTALink,
                OnAction = (res) => ChangeLINK(res, SocialMediaType.Instagram)
            });
        }
        #endregion

        private async void PickPictureTapped(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "Pick photo not supported", "OK");
                return;
            }

            MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                CustomPhotoSize = 50
            });
            if (photo == null)
            {
                return;
            }
            string path = photo.Path;


            if (!string.IsNullOrWhiteSpace(path))
            {
                string[] partPaths = path.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (partPaths != null && partPaths.Length > 0)
                {
                    fileName = partPaths[partPaths.Length - 1];
                }

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    upfilebytes = await ReadFully(photo.GetStream());
                    int length = upfilebytes.Length;
                    IsImageUploaded = true;
                    ProfileImage.Source = ImageSource.FromStream(() =>
                    {
                        var stream = photo.GetStream();
                        photo.Dispose();
                        return stream;
                    });
                }
            }
        }

        public async Task<byte[]> ReadFully(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task<bool> UploadProfileImage()
        {
            //variable
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("UploadSalonAppIcon.php");
            try
            {
                if (upfilebytes != null)
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
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return false;
            }
        }

        
    }
    public enum SocialMediaType
    {
        Twitter,
        Facebook,
        Instagram
    }
}


