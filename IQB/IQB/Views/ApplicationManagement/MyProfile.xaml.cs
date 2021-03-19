using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.AuthServices;
using IQB.Utility;
using IQB.ViewModel.AdminManagement;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.MenuManagement;
using IQB.Views.LoginManagement;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProfile : ContentPage
    {
        private MyProfileViewModel profileViewModel = null;
        private Rootobject barberViewModel = new Rootobject();
        public MasterDetailsViewModel MenuViewModel = null;
        bool IsImageUploaded = false;
        int _barberID;
        byte[] upfilebytes;
        bool BarberTabVisible;
        string fileName = string.Empty;
        string fName = string.Empty;
        bool isFirstLoad = true;
        public ManageStaffViewModel _ManageStaffViewModel = null;

        public MyProfile(MasterDetailsViewModel viewModel)
        {
            InitializeComponent();
        }

        private void ProfilePageAppearing(object sender, EventArgs e)
        {
            LaodPrimaryProfileData();
        }

        private void LaodPrimaryProfileData()
        {
            try
            {
                profileViewModel = new MyProfileViewModel();
                profileViewModel.GetProfileInfo();

                if (((Application.Current.Properties["ProfileBarber_Id"].ToString() != String.Empty) && (((Application.Current.Properties["UserLevel"]).ToString() == "2")) || ((Application.Current.Properties["UserLevel"]).ToString() == "1")))
                {
                    _barberID = Convert.ToInt32(Application.Current.Properties["ProfileBarber_Id"].ToString());
                    LaodBarberDetailsData(_barberID);
                    MainScrollView.Margin = new Thickness(0, 0, 0, 0);
                    SfProfileTabContainer.IsVisible = true;
                }

                else
                {
                    SfProfileTabContainer.IsVisible = false;
                    MainScrollView.Margin = new Thickness(0, -50, 0, 0);
                }

                if ((Application.Current.Properties["UserLevel"]).ToString() == "2")
                {
                    LinkSwitchStack.IsVisible = true;
                    BarberLinkSwitch.IsOn = (bool)Application.Current.Properties["IsAdminBarber"];
                }

                //else
                //{
                //    MainScrollView.Margin = new Thickness(0, -50, 0, 0);
                //}

                profileViewModel = new MyProfileViewModel();
                profileViewModel.GetProfileInfo();
                ProfileImageNew.Source = "";
                ProfileImageNew.Source = new UriImageSource
                {
                    Uri = new Uri(profileViewModel.ProfileImage),
                    CachingEnabled = false,
                };

                LabelFullName.Text = profileViewModel.FullName;
                if (profileViewModel.DOB.HasValue)
                    LabelDoBorEst.Text = GetFormattedDateFormat(profileViewModel.DOB.Value.Date);
               else
                    LabelDoBorEst.Text = "";
                MobileOrPassCode.Text = profileViewModel.MobileNo != "0" ? profileViewModel.MobileNo : "";
                LabelEmail.Text = profileViewModel.Email;

            }
            catch { }
            isFirstLoad = false;
        }

        private string GetFormattedDateFormat(DateTime date)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                switch (Convert.ToString((date.Day).ToString()[(date.Day).ToString().Length - 1]))
                {
                    case "1":
                        sb.Append($"{date.Day.ToString()}st ");
                        break;

                    case "2":
                        sb.Append($"{date.Day.ToString()}nd ");
                        break;

                    case "3":
                        sb.Append($"{date.Day.ToString()}rd ");
                        break;

                    default:
                        sb.Append($"{date.Day.ToString()}th ");
                        break;

                }

                sb.Append($"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month)} {date.Year}");

            }
            catch { }

            return sb.ToString();
        }

        private async void LaodBarberDetailsData(int BarberIdVal)
        {
            BarberSalonService _BarberSalonService = new BarberSalonService();

            try
            {
                ApiResult res = new ApiResult();

                res = await _BarberSalonService.GetStaffDetailsBySalonIdStaffId(BarberIdVal);
                if (res != null && res.StatusCode == 200)
                {
                    List<Rootobject> result = UtilityEL.ToModelListNew<Rootobject>(res.Response);

                    if (result[0] != null)
                        barberViewModel = result[0];

                    foreach (var items in result)
                    {
                        barberViewModel = items;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async Task BarberUploadProfileImage(int CurrentBarberID)
        {
            //variable
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("Barber_Upload_Pic.php");
            var Fullurl = url + "?BarberId=" + CurrentBarberID + "";
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
                fName = CurrentBarberID + "." + ext; //UserId + "." + ext;
                ImageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = CurrentBarberID + "." + ext,  //UserId + "." + ext,
                    Name = "file",

                };
                content.Add(ImageContent);
                //content.Add(userIdContent, "userid");

                //upload MultipartFormDataContent content async and store response in response var
                //  var response = await client.PostAsync(url, content);
                var response = await client.PostAsync(Fullurl, content);
                ApiResult resultObject = new ApiResult();
                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responsestr))
                {
                    resultObject = UtilityEL.ToApiModel(responsestr);
                }
                if (resultObject != null && resultObject.StatusCode == 200)
                {
                    await App.Current.MainPage.DisplayAlert("Success!", Convert.ToString(resultObject.Response), "Ok");
                    Application.Current.Properties["IsUpdateEnabled"] = "true";

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (barberViewModel.BarberPic != null)
                        {
                            ProfileImageNew.Source = new UriImageSource
                            {
                                Uri = new Uri(barberViewModel.BarberPic),
                                CachingEnabled = false
                            };
                        }
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error!", Convert.ToString(resultObject.Response), "Ok");
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return;
            }
        }

        private async void PickPictureTapped(object sender, EventArgs args)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    status = await StorageUtil.CheckPermissions(Permission.Storage);
                }

                if (status == PermissionStatus.Granted)
                {
                    profileViewModel.IsEnabled = false;
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Oops", "Pick photo not supported", "OK");
                        profileViewModel.IsEnabled = true;
                        return;
                    }

                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        CustomPhotoSize = 50
                    });
                    if (photo == null)
                    {
                        profileViewModel.IsEnabled = true;
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
                            ProfileImageNew.Source = ImageSource.FromStream(() =>
                            {
                                var stream = photo.GetStream();
                                photo.Dispose();
                                return stream;
                            });
                        }
                    }
                    profileViewModel.IsEnabled = true;
                    profileViewModel.IsProfileApiRunning = false;
                    UploadTabRespectiveProfilePicture();

                }
            }
            catch (Exception)
            {


            }


        }

        private async void UploadTabRespectiveProfilePicture()
        {
            if (BarberTabVisible && SfProfileTabContainer.IsVisible && SfProfileTabContainer.SelectedIndex == 1)

                await BarberUploadProfileImage(_barberID);
            else
                await UploadProfileImage();

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

        private async Task UploadProfileImage()
        {
            //variable
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("upload_profile_image.php");
            try
            {
                indicator.IsVisible = true;
                HttpClient client = new HttpClient();

                string boundary = "---8393774hhy37373773";
                MultipartFormDataContent content = new MultipartFormDataContent(boundary);

                var ImageContent = new ByteArrayContent(upfilebytes);

                ImageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

                string uploadedImageName = fileName;
                int index = uploadedImageName.LastIndexOf('.');
                var ext = uploadedImageName.Substring(index + 1);
                fName = profileViewModel.UserId + "." + ext;
                ImageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = profileViewModel.UserId + "." + ext,
                    Name = "file",

                };
                content.Add(ImageContent);
                //content.Add(userIdContent, "userid");

                //upload MultipartFormDataContent content async and store response in response var
                var response = await client.PostAsync(url, content);
                ApiResult resultObject = new ApiResult();
                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responsestr))
                {
                    resultObject = UtilityEL.ToApiModel(responsestr);
                }
                if (resultObject != null && resultObject.StatusCode == 200)
                {
                    indicator.IsVisible = false;
                    await DisplayAlert("Success!", Convert.ToString(resultObject.Response), "Ok");
                    if (profileViewModel.ProfileImage.Contains("noimage"))
                    {
                        Account account = AccountStore.Create().FindAccountsForService(CommonEL.LoginServiceName).FirstOrDefault();
                        //account.Properties["UserProfileImageName"] = CommonEL.ProfileImageLocation + Application.Current.Properties["UserName"] + ".jpg";
                        LoginViewModel obj = new LoginViewModel();
                        obj.StoreLoginCredData(Application.Current.Properties["_UserEmail"].ToString(), Application.Current.Properties["UserName"].ToString(), CommonEL.ProfileImageLocation + Application.Current.Properties["UserName"] + ".jpg", obj.GetIsRemembered(), Convert.ToInt32(account.Properties["UserId"]), account.Properties["FirstName"], account.Properties["LastName"], account.Properties["password"], account.Properties["DOB"], account.Properties["MobileNo"], profileViewModel.RateSystem, profileViewModel.RatingScore, Convert.ToInt32(account.Properties["IsMailEnabled"]));
                        profileViewModel.GetProfileInfo();
                    }

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        if (profileViewModel.ProfileImage != null)
                        {
                            ProfileImageNew.Source = new UriImageSource
                            {
                                Uri = new Uri(profileViewModel.ProfileImage),
                                CachingEnabled = false
                            };
                        }
                    }

                    MessagingCenter.Send("Updated", "MessagingProfileSender");
                }
                else
                {
                    indicator.IsVisible = false;
                    await DisplayAlert("Error!", Convert.ToString(resultObject.Response), "Ok");
                }
            }
            catch (Exception ex)
            {
                indicator.IsVisible = false;
                string Message = ex.Message;
                return;
            }
        }

        private async void UpdateIconTapped(object sender, EventArgs e)
        {
            // && ((Application.Current.Properties["UserLevel"]).ToString() == "2")
            if (SfProfileTabContainer.IsVisible && SfProfileTabContainer.SelectedIndex == 1)
            {
                await Navigation.PushModalAsync(new EditProfilePage(true, barberViewModel.BarberId, LabelFullName.Text, MobileOrPassCode.Text, LabelEmail.Text, barberViewModel.Estimated_wait_time, barberViewModel.PreferredName));
            }
            else
            {
                await Navigation.PushModalAsync(new EditProfilePage(false, 0, "", "", "", "", ""));
            }

        }

        private void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            indicator.IsVisible = true;
            if (SfProfileTabContainer.SelectedIndex == 1)
            {
                if ((Application.Current.Properties["UserLevel"]).ToString() == "1")
                {
                    UpdateStack.IsVisible = false;
                }

                BarberTabVisible = true;
                if (barberViewModel.BarberPic != null)
                {
                    ProfileImageNew.Source = new UriImageSource
                    {
                        Uri = new Uri(barberViewModel.BarberPic),
                        CachingEnabled = false
                    };
                }
                LinkSwitchStack.IsVisible = false;
                LabelFullName.Text = barberViewModel.FirstName + " " + barberViewModel.LastName;
                CakeOrEstImage.HeightRequest = 110;
                CakeOrEstImage.WidthRequest = 110;
                CakeOrEstImage.Source = "resource://IQB.Resources.Image.HEstWaitTime.svg";
                EmailIcon.Source = "resource://IQB.Resources.Image.ProfileMail.svg";
                //MobileOrPassCode.Text = barberViewModel.EmployeePasscode;
                MobileOrPassCode.Text = barberViewModel.PreferredName;
                LabelEmail.Text = barberViewModel.EmailAddress;
                LabelDoBorEst.Text = "Est. Wait Time : " + barberViewModel.Estimated_wait_time + "(in mins)";
            }

            else
            {
                UpdateStack.IsVisible = true;
                if ((Application.Current.Properties["UserLevel"]).ToString() == "2")
                {
                    LinkSwitchStack.IsVisible = true;

                }
                else
                {
                    LinkSwitchStack.IsVisible = false;
                }
                ProfileImageNew.Source = new UriImageSource
                {
                    Uri = new Uri(profileViewModel.ProfileImage),
                    CachingEnabled = false
                };
                LabelFullName.Text = profileViewModel.FullName;
                if (profileViewModel.DOB.HasValue)
                    LabelDoBorEst.Text = GetFormattedDateFormat(profileViewModel.DOB.Value.Date);
                else
                    LabelDoBorEst.Text = "";
                MobileOrPassCode.Text = profileViewModel.MobileNo;
                LabelEmail.Text = profileViewModel.Email;
                BarberTabVisible = false;
                CakeOrEstImage.HeightRequest = 60;
                CakeOrEstImage.WidthRequest = 60;
                CakeOrEstImage.Source = "resource://IQB.Resources.Image.ProfileCake.svg";
                EmailIcon.Source = "resource://IQB.Resources.Image.ProfileContact.svg";
            }
            indicator.IsVisible = false;
        }

        private async void LogoutIconTapped(object sender, EventArgs e)
        {
            var ans = await App.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to Logout?", "Yes", "No");
            if (ans)
                await Navigation.PushAsync(new Logout());
        }

        // private async void BarberLinkSwitchToggled(object sender, ToggledEventArgs e)
        private async void BarberLinkSwitchToggled(object sender, SwitchStateChangingEventArgs e)
        {
            if (!isFirstLoad && (e.OldValue != e.NewValue))
            {
                using (StaffServices obj = new StaffServices())
                {
                    indicator.IsVisible = true;
                    //var res = await obj.AdminLinkBarber(profileViewModel.Email, (bool)BarberLinkSwitch.IsToggled);
                    var res = await obj.AdminLinkBarber(profileViewModel.Email, profileViewModel.SalonId, (bool)e.NewValue);

                    //      IsAdminBarberResponse result = UtilityEL.ToModel<IsAdminBarberResponse>(res);

                    indicator.IsVisible = false;
                    await DisplayAlert("Message", res.StatusMessage, "OK");

                    if (res.StatusMessage == "Linked successfully")
                    {
                        Application.Current.Properties["IsAdminBarber"] = true;
                    }

                    if (res.StatusMessage == "Delinked successfully")
                    {
                        Application.Current.Properties["IsAdminBarber"] = false;
                    }

                    if (res.AdminBarberId != null)
                    {
                        Application.Current.Properties["ProfileBarber_Id"] = res.AdminBarberId;
                    }

                    else
                    {
                        Application.Current.Properties["ProfileBarber_Id"] = String.Empty;
                    }

                    isFirstLoad = true;
                    LaodPrimaryProfileData();
                    return;
                }
            }

            isFirstLoad = false;

        }
    }


    public class IsAdminBarberResponse
    {
        public int StatusCode { get; set; }

        public string AdminBarberId { get; set; }

        public string StatusMessage { get; set; }
    }
}
//Plugin
//private void MyDatePicker_Focused(object sender, FocusEventArgs e)
//{
//    myDatePicker.IsCleared = false;
//}

//void ClearDate(object sender, EventArgs args)
//{
//    myDatePicker.IsCleared = true;//Plugin
//    profileViewModel.IsClearClicked = true;
//    profileViewModel.DOB = null;

//    //profileViewModel.IsClearClicked = false;
//}

//private async void UpdateProfile(object sender, EventArgs args)
//{
//    if (bhvFirstName.IsValid && bhvLastName.IsValid && bhvPassword.IsValid && bhvMobileNo.IsValid)
//    {
//        if (NetworkConnectionMgmt.IsConnectedToNetwork())
//        {
//            profileViewModel.IsProfileApiRunning = true;
//            profileViewModel.IsVisibleProfileDetails = false;
//            profileViewModel.IsEnabled = false;
//            MyProfileModel postModel = new MyProfileModel()
//            {
//                FirstName = profileViewModel.FirstName,
//                LastName = profileViewModel.LastName,
//                Email = profileViewModel.Email,
//                Password = profileViewModel.Password,
//                MobileNo = profileViewModel.MobileNo,
//                //DOB = string.Format("{0:MM/dd/yyyy}",profileViewModel.DOB),
//                id = profileViewModel.UserId,
//                SalonId = profileViewModel.SalonId
//            };

//            if (profileViewModel.DOB.HasValue)
//            {
//                int DOBD = profileViewModel.DOB.Value.Day;
//                int DOBM = profileViewModel.DOB.Value.Month;
//                int DOBY = profileViewModel.DOB.Value.Year;

//                postModel.DOB = DOBY.ToString();
//                postModel.DOB += "-" + ((DOBM <= 9) ? ("0" + DOBM.ToString()) : DOBM.ToString());
//                postModel.DOB += "-" + ((DOBD <= 9) ? ("0" + DOBD.ToString()) : DOBD.ToString());
//            }

//            if (switchCanSendEmail.IsToggled)
//                postModel.IsMailEnabled = 1;
//            else
//                postModel.IsMailEnabled = 0;

//            //int DOBD = profileViewModel.DOB.Day;
//            //int DOBM = profileViewModel.DOB.Month;
//            //int DOBY = profileViewModel.DOB.Year;

//            //postModel.DOB = DOBY.ToString();
//            //postModel.DOB += "-" + ((DOBM <= 9) ? ("0" + DOBM.ToString()) : DOBM.ToString());
//            //postModel.DOB += "-" + ((DOBD <= 9) ? ("0" + DOBD.ToString()) : DOBD.ToString());


//            MyProfileUpdateResultModelAPI result = new MyProfileUpdateResultModelAPI();
//            ApiResult res = new ApiResult();

//            //ImageUpload
//            if (IsImageUploaded)
//            {
//                await UploadProfileImage();
//            }
//            //ImageUpload

//            using (MyProfileService obj = new MyProfileService())
//            {
//                res = await obj.UpdateProfile(postModel);
//            }

//            if (res != null && res.StatusCode == 200)
//            {
//                string UserImage = string.Empty;
//                await DisplayAlert("Success!", Convert.ToString(res.Response), "Ok");
//                LoginViewModel obj = new LoginViewModel();
//                if (IsImageUploaded)
//                {
//                    UserImage = CommonEL.ProfileImageLocation + fName;
//                }
//                else
//                {
//                    UserImage = obj.GetUserProfileImageName();
//                }
//                string UserName = postModel.FirstName + " " + postModel.LastName;
//                //obj.StoreLoginCredData(postModel.Email, UserName, "", obj.GetIsRemembered(), postModel.id, postModel.FirstName, postModel.LastName, postModel.Password, postModel.DOB, postModel.MobileNo, profileViewModel.RateSystem);
//                //MenuViewModel.SetProfileData();
//                obj.StoreLoginCredData(postModel.Email, UserName, UserImage, obj.GetIsRemembered(), postModel.id, postModel.FirstName, postModel.LastName, postModel.Password, postModel.DOB, postModel.MobileNo, profileViewModel.RateSystem, postModel.IsMailEnabled);
//                App.GoToRoot(true);
//                //MenuViewModel.SetProfileData1();
//                //MenuViewModel.SetProfileData();
//            }
//            else
//            {
//                await DisplayAlert("Error!", "Failed to update user. Please try again after sometime .", "Ok");
//            }

//            profileViewModel.IsEnabled = true;
//            profileViewModel.IsProfileApiRunning = false;
//            profileViewModel.IsVisibleProfileDetails = true;
//        }
//        else
//        {
//            await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
//        }
//    }
//    else
//    {
//        if (!bhvFirstName.IsValid)
//        {
//            bhvFirstName.IsShow = true;
//        }
//        if (!bhvLastName.IsValid)
//        {
//            bhvLastName.IsShow = true;
//        }
//        if (!bhvPassword.IsValid)
//        {
//            bhvPassword.IsShow = true;
//        }
//        if (!bhvMobileNo.IsValid)
//        {
//            bhvMobileNo.IsShow = true;
//        }
//    }
//}

//private void lblDOB_Tapped(object sender, EventArgs e)
//{
//    myDatePicker.Focus();
//}
//myDatePicker.Focused += MyDatePicker_Focused;//Plugin
//MenuViewModel = viewModel;


//myDatePicker.Unfocused += (s, e) =>
//{
//    DateTime datevalue = ((DatePicker)s).Date;

//    lblDOB.Text = datevalue.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
//    ((DatePicker)s).IsVisible = false;
//};