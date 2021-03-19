using Acr.UserDialogs;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.StaffModels;
using IQB.IQBServices;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.AuthServices;
using IQB.ViewModel.AuthViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IQB.EntityLayer.StaffModels.StaffModel;

namespace IQB.Views.LoginManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private MyProfileViewModel profileViewModel = null;
        BarberEditViewModel barberViewModel = null;
        bool IsBarber = false, isEnabled = true;

        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        Regex mobileRegex = new Regex(@"^[0-9]{11}$");

        Regex nameRegex = new Regex(@"^[a-zA-Z\s]+$");

        Regex prefferedNameRegex = new Regex(@"^[a-zA-Z0-9\s]+$");

        Regex passCodeRegex = new Regex(@"^[a-zA-Z0-9]+$");

        public EditProfilePage(bool isBarber, int barberId, string barberFullName, string barberPassCode, string barberEmailId, string barberEstimatedTime, string barberPrefferedNAme)
        {
            InitializeComponent();
            try
            {
                EditProfileDatePicker.MaximumDate = DateTime.Now.Date;
                IsBarber = isBarber;
                if (IsBarber)
                {
                    barberViewModel = new BarberEditViewModel { BarberID = barberId, BarberFullName = barberFullName, BarberEmailId = barberEmailId, BarberEstimatedTime = barberEstimatedTime, BarberPassCode = barberPassCode, BarberPrefferedName = barberPrefferedNAme };
                    this.BindingContext = barberViewModel;
                    CustomerEditProfile.IsVisible = false;
                    BarberEditProfile.IsVisible = true;
                }

                else
                {
                    profileViewModel = new MyProfileViewModel();
                    profileViewModel.GetProfileInfo();
                    BindingContext = profileViewModel;
                    if (profileViewModel.DOB.HasValue)
                        DOBEntry.Text = GetFormattedDateFormat(profileViewModel.DOB.Value.Date);
                    switchCanSendEmail.IsOn = profileViewModel.EmailEnabled == 1 ? true : false ;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void UpdateProfileClicked(object sender, EventArgs e)
        {
            try
            {
                if (SubmitModelValidation())
                {
                    indicator.IsVisible = true;
                    if (IsBarber)
                    {
                        StaffModel staffModel = new StaffModel();
                        using (SalonViewModel obj = new SalonViewModel())
                        {
                            staffModel.SalonId = obj.GetSalonId();
                        }

                        staffModel.LstService = new List<Services>();
                        if (barberViewModel.BarberID != 0)
                        {
                            staffModel.BarberId = barberViewModel.BarberID;
                        }

                        var BarberNameArray = barberViewModel.BarberFullName.Split(' ');

                        for (int j = 0; j < BarberNameArray.Length; j++)
                        {
                            if (j == BarberNameArray.Length - 1)
                            {
                                staffModel.LastName = BarberNameArray[j];
                            }

                            else
                            {
                                staffModel.FirstName += BarberNameArray[j];
                            }
                        }

                        staffModel.PreferredName = barberViewModel.BarberPrefferedName;
                        staffModel.EmployeePasscode = barberViewModel.BarberPassCode;
                        staffModel.EmailAddress = barberViewModel.BarberEmailId.ToLower();
                        staffModel.Estimatedwaittime = Convert.ToInt32(barberViewModel.BarberEstimatedTime);

                        StaffServices staffServices = new StaffServices();
                        var _Result = await staffServices.PostUpdateStaff(staffModel);

                        string msg = _Result.StatusMessage;
                        if (_Result.StatusCode == 200)
                        {
                            isEnabled = false;
                            await App.Current.MainPage.DisplayAlert("Sucess", msg, "Ok");
                            //App.GoToRoot(true);
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", msg, "Ok");
                        }
                    }

                    else
                    {
                        if (NetworkConnectionMgmt.IsConnectedToNetwork())
                        {
                            profileViewModel.IsProfileApiRunning = true;
                            profileViewModel.IsVisibleProfileDetails = false;
                            profileViewModel.IsEnabled = false;
                            var NameArray = profileViewModel.FullName.Split(' ');

                            if (!String.IsNullOrEmpty(profileViewModel.FullName))
                            {
                                profileViewModel.FirstName = String.Empty;
                                profileViewModel.LastName = String.Empty;
                            }

                            for (int i = 0; i < NameArray.Length; i++)
                            {
                                if (i == NameArray.Length - 1)
                                {
                                    profileViewModel.LastName = NameArray[i];
                                }

                                else
                                {
                                    profileViewModel.FirstName += NameArray[i];
                                }
                            }

                            MyProfileModel postModel = new MyProfileModel()
                            {
                                FirstName = profileViewModel.FirstName,
                                LastName = profileViewModel.LastName,
                                Email = profileViewModel.Email,
                                Password = profileViewModel.Password,
                                MobileNo = profileViewModel.MobileNo,
                                id = profileViewModel.UserId,
                                SalonId = profileViewModel.SalonId
                            };

                            if (profileViewModel.DOB.HasValue)
                            {
                                int DOBD = profileViewModel.DOB.Value.Day;
                                int DOBM = profileViewModel.DOB.Value.Month;
                                int DOBY = profileViewModel.DOB.Value.Year;

                                postModel.DOB = DOBY.ToString();
                                postModel.DOB += "-" + ((DOBM <= 9) ? ("0" + DOBM.ToString()) : DOBM.ToString());
                                postModel.DOB += "-" + ((DOBD <= 9) ? ("0" + DOBD.ToString()) : DOBD.ToString());
                            }

                            //if (switchCanSendEmail.IsToggled)

                            if (switchCanSendEmail.IsOn.Value)
                                postModel.IsMailEnabled = 1;
                            else
                                postModel.IsMailEnabled = 0;

                            MyProfileUpdateResultModelAPI result = new MyProfileUpdateResultModelAPI();
                            ApiResult res = new ApiResult();
                            using (MyProfileService obj = new MyProfileService())
                            {
                                res = await obj.UpdateProfile(postModel);
                            }

                            if (res != null && res.StatusCode == 200)
                            {
                                string UserImage = string.Empty;
                                await DisplayAlert("Success!", Convert.ToString(res.Response), "Ok");
                                LoginViewModel obj = new LoginViewModel();
                                UserImage = obj.GetUserProfileImageName();
                                string UserName = postModel.FirstName + " " + postModel.LastName;
                                obj.StoreLoginCredData(postModel.Email, UserName, UserImage, obj.GetIsRemembered(), postModel.id, postModel.FirstName, postModel.LastName, postModel.Password, postModel.DOB, postModel.MobileNo, profileViewModel.RateSystem, profileViewModel.RatingScore, postModel.IsMailEnabled);
                                //    App.GoToRoot(true);
                                await Navigation.PopModalAsync();
                            }
                            else
                            {
                                await DisplayAlert("Error!", "Failed to update user. Please try again after sometime .", "Ok");
                            }

                            profileViewModel.IsEnabled = true;
                            profileViewModel.IsProfileApiRunning = false;
                            profileViewModel.IsVisibleProfileDetails = true;
                        }
                        else
                        {
                            await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                        }
                    }

                    indicator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
            }
          
        }

        private void ChangePasswordTapped(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = false;
            PasswordEntry.Focus();
        }

        private void PasswordEntryUnfocussed(object sender, FocusEventArgs e)
        {
            PasswordEntry.IsPassword = true;
        }

        private void DOBSelected(object sender, DateChangedEventArgs e)
        {
            DateTime datevalue = ((DatePicker)sender).Date;

            profileViewModel.DOB = datevalue;

            //lblDOB.Text = datevalue.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            //((DatePicker)s).IsVisible = false;
            DOBEntry.Text = GetFormattedDateFormat(profileViewModel.DOB.Value.Date);

        }

        private string GetFormattedDateFormat(DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                switch (date.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        sb.Append($"{date.Day.ToString()}st ");
                        break;
                    case 2:
                    case 22:
                        sb.Append($"{date.Day.ToString()}nd ");
                        break;
                    case 3:
                    case 23:
                        sb.Append($"{date.Day.ToString()}rd ");
                        break;
                    default:
                        sb.Append($"{date.Day.ToString()}th ");
                        break;

                }
                //sb.Append($"{date.Day.ToString()}st ");
                sb.Append($"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month)} {date.Year}");

            }
            catch { }

            return sb.ToString();
        }

        private void DobFocusTapped(object sender, EventArgs e)
        {
            EditProfileDatePicker.Focus();
        }

        private async void CrossIconTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private bool SubmitModelValidation()
        {
            if (IsBarber)
            {
                if ((!nameRegex.IsMatch(barberViewModel.BarberFullName)) && !String.IsNullOrEmpty(barberViewModel.BarberFullName))
                {
                    App.toastConfig.Message = "    Special charactes are not allowed in Full Name";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }
                if ((!prefferedNameRegex.IsMatch(barberViewModel.BarberPrefferedName)) && !String.IsNullOrEmpty(barberViewModel.BarberPrefferedName))
                {
                    App.toastConfig.Message = "    Special charactes are not allowed in Preffered Name";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }
                if ((!passCodeRegex.IsMatch(barberViewModel.BarberPassCode)) && !String.IsNullOrEmpty(barberViewModel.BarberPassCode))
                {
                    App.toastConfig.Message = "    Special charactes or spaces are not allowed in PassCode";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }
                return true;
            }
            else
            {
                if ((!nameRegex.IsMatch(profileViewModel.FullName)) && !String.IsNullOrEmpty(profileViewModel.FullName))
                {
                    App.toastConfig.Message = "    Special charactes are not allowed in Name";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }

                if ((!mobileRegex.IsMatch(profileViewModel.MobileNo)) && !String.IsNullOrEmpty(profileViewModel.MobileNo))
                {
                    App.toastConfig.Message = "    Phone Number is not valid";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }

                if ((!emailRegex.IsMatch(profileViewModel.Email)) && !String.IsNullOrEmpty(profileViewModel.Email))
                {
                    App.toastConfig.Message = "    Please enter a valid email id";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }

                if (profileViewModel.Password.Contains(" ") || profileViewModel.Password.Length > 10 && !String.IsNullOrEmpty(profileViewModel.Password))
                {
                    App.toastConfig.Message = "    Password cannot contain spaces or more than 10 characters";
                    UserDialogs.Instance.Toast(App.toastConfig);
                    return false;
                }
                return true;
            }

        }

    }


    public class BarberEditViewModel
    {
        public int BarberID { get; set; }

        public string BarberFullName { get; set; }

        public string BarberPrefferedName { get; set; }

        public string BarberPassCode { get; set; }

        public string BarberEmailId { get; set; }

        public string BarberEstimatedTime { get; set; }
    }
}