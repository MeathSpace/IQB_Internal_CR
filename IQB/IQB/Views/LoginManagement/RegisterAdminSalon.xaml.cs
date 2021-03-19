using Acr.UserDialogs;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.GenericModels;
using IQB.EntityLayer.SalonModels;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.IQBServices.GenericServices;
using IQB.IQBServices.SalonServices;
using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.Settings;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace IQB.Views.LoginManagement
{
    public partial class RegisterAdminSalon : ContentPage
    {
        private RegisterViewModel registerViewModel = null;
        RegistrationService objRegService = new RegistrationService();
        private string uName;

        private List<Country> countries = new List<Country>();
        private Country selectedCountry = new Country();

        private List<Currency> currencies = new List<Currency>();
        private Currency selectedCurrency = new Currency();

        public string TWLink { get; set; }
        public string FBLink { get; set; }
        public string INSTALink { get; set; }
        public bool IsImageUploaded { get; private set; }
        private int SalonID = 0;
        private SalonServices objSalonService = new SalonServices();
        private string fileName;
        private byte[] upfilebytes;
        private string fName;

        public RegisterAdminSalon(string city)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            registerViewModel = new RegisterViewModel();

            BindingContext = registerViewModel;

            GetCountryList();
            GetCurrencyList();
            entryCity.Text = city;
            entryCity.IsEnabled = false;

            pickerCountry.ItemsSource = countries;
            pickerCurrency.ItemsSource = currencies;

            var currency = currencies.FirstOrDefault(x => x.symbol == "£");
            if (currency == null)
            {
                currency = currencies.FirstOrDefault(x => x.currency_string == "GBP - British Pound");
            }
            if (currency != null)
            {
                pickerCurrency.SelectedItem = currency;
                selectedCurrency = currency;
                lblCurrency.Text = currency.currency_string;
            }

            ProfileImage.Source = ImageSource.FromFile("noimage.png");
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

        private async void CancelRegistration(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync(true);
        }
        
        #region Salon
        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            if (bhvSalonName.IsValid && bhvCustomerName.IsValid && bhvIsValidEmail.IsValid && bhvMobileNo.IsValid && bhvCity.IsValid && bhvZip.IsValid)
            {
                //gdCreateSalon.IsVisible = false;
                //gdMain.IsVisible = true;
                //todoNav
                SalonModel salonModel = new SalonModel
                {
                    SalonAppIcon = "",
                    SalonName = entrySalonName.Text,
                    CustomerName = entryCustomerName.Text,
                    UserName = uName,
                    EmailAddress = entryEmail.Text,
                    city = entryCity.Text,
                    county = selectedCountry.CountryName,
                    PostCode = entryZip.Text,
                    Address = entryAddr.Text,//todo
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
                    PaymentGatewaySettings = new List<PaymentGatewayData>(new[]{ new PaymentGatewayData { merchant_email = MerchantEmail.Text } }),
                //    PaymentGatewaySettings = new List<PaymentGatewayData>(new[]{ new PaymentGatewayData { merchant_id = MerchnatId.Text, private_key = PrivateKey.Text, public_key = PublicKey.Text, tokenization_key = TokenizationKey.Text } }),

                };
                    

                await Navigation.PushAsync(new RegisterAdmin(salonModel, upfilebytes,fileName,fName));
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
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                 status = await StorageUtil.CheckPermissions(Permission.Storage);
                }

                if (status==PermissionStatus.Granted)
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
            }

            catch(Exception ex)
            {

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
        #endregion

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (bool.Parse(Application.Current.Properties["IsAdminUserSalonRegistered"].ToString()))
            {
                await Navigation.PopAsync(true);
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
