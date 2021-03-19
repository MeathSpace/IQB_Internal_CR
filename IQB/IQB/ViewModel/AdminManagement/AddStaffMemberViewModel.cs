using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.EntityLayer.StaffModels;
using IQB.IQBServices.AdminServices;
using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberSaloonModel;
using static IQB.EntityLayer.StaffModels.StaffModel;

namespace IQB.ViewModel.AdminManagement
{

    public class AddStaffMemberViewModel : BaseViewModel
    {
        BarberSalonService _BarberSalonService = new BarberSalonService();
        StaffServices _StaffServices = new StaffServices();
        int value = 0;
        string fileName, fName = string.Empty;
        byte[] upfilebytes;
        bool IsImageUploaded = false;

        public AddStaffMemberViewModel(int barberid)
        {
            //if (Convert.ToString(Application.Current.Properties["IsGridEnabled"]) == "true")
            //{
            //    IsStaffGridEnabled = false;
            //    IsGridVisible = true;
            //}

            Application.Current.Properties["IsUpdateEnabled"] = "";
            Application.Current.Properties["Barber_Id"] = barberid;
            value = (int)Application.Current.Properties["Barber_Id"];
            IsRefreshing = true;
            IsEnabled = true;
            IsVisible = false;
            IsBackIconEnabled = true;
            BindServiceList();
            BindGetStaff(value);
            //BrResult = new List<Service> { new Service(), new Service(), new Service() };
            BrResult = new ObservableCollection<Service>();
            GetlistService = new List<Service>();
            IsRefreshing = false;

            //TextValue = "ADD MEMBER";
            TextValue = " ADD  +";
            isAddVisible = true;
            //PageHeading = "Add Staff Member";
            PageHeading = "Staff Member";
        }


        public async void BindGetStaff(int BarberID)
        {

            try
            {
                ApiResult res = new ApiResult();

                res = await _BarberSalonService.GetStaffDetailsBySalonIdStaffId(BarberID);
                if (res != null && res.StatusCode == 200)
                {
                    //TextValue = "UPDATE MEMBER";
                    TextValue = "UPDATE";
                    isUpdateVisible = true;
                    isAddVisible = false;
                    //PageHeading = "Update Staff Member";
                    PageHeading = "Staff Member";
                    List<Rootobject> result = UtilityEL.ToModelListNew<Rootobject>(res.Response);

                    foreach (var items in result)
                    {
                        FirstName = items.FirstName;
                        LastName = items.LastName;
                        FullName = FirstName + " " + LastName;
                        PreferredName = items.PreferredName;
                        EmployeePasscode = items.EmployeePasscode;
                        EmailAddress = items.EmailAddress;
                        Estimatedwaittime = items.Estimated_wait_time;
                        IsAvailableForAppointments = (items.AvailableForAppointments == "1") ? true : false;

                        //string url = items.BarberPic;

                        Application.Current.Properties["Services"] = items.Services;

                        ProfileImage = new UriImageSource
                        {
                            Uri = new Uri(items.BarberPic.ToString()),
                            CachingEnabled = false,
                        };



                        // ProfileImage = ImageSource.FromUri(new Uri(items.BarberPic.ToString(), UriKind.RelativeOrAbsolute));


                        if (items.Services != null)
                        {
                            foreach (var item in items.Services)
                            {
                                Service Services = new Service();

                                Services.ServiceId = Convert.ToInt32(item.ServiceId);
                                Services.ServiceName = item.ServiceName;
                                Services.ServiceIdTime = item.ServiceTime;
                                Services.Price = item.ServicePrice;

                                Services.IsToggled = true;
                                Services.SwitchIsEnabled = true;
                                //BrResult.Add(Services);
                                GetlistService.Add(Services);
                            }
                        }
                        //else
                        //{
                        int salonId = 0;

                        using (SalonViewModel objSalon = new SalonViewModel())
                        {
                            salonId = objSalon.GetSalonId();
                        }


                        using (BarberSalonService obj = new BarberSalonService())
                        {
                            res = await obj.GetServicesBySalonId(salonId);
                            if (res != null && res.StatusCode == 200)
                            {
                                List<ServiceModel> result1 = UtilityEL.ToModelList<ServiceModel>(res.Response);

                                foreach (var item in result1)
                                {
                                    Service Services = new Service();

                                    Services.ServiceId = item.ServiceId;
                                    
                                    Services.Price = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;
                                    //Services.Price = "£" + item.ServicePrice;
                                    Services.ServiceName = item.ServiceName;
                                    Services.ServiceIdTime = item.Estimated_wait_time;
                                    Services.SwitchIsEnabled = true;
                                    BrResult.Add(Services);
                                }

                                ServiceListViewHeight = (BrResult.Count * 100) + 25;

                                foreach (var item in GetlistService)
                                {
                                    foreach (var serviceitems in BrResult)
                                    {
                                        if (item.ServiceId == serviceitems.ServiceId)
                                        {
                                            serviceitems.IsToggled = true;
                                            serviceitems.ServiceIdTime = item.ServiceIdTime;
                                            var apiResult = await _StaffServices.ServiceStatusJoinQueueByServiceID(salonId, BarberID, serviceitems.ServiceId);
                                            int msg = Convert.ToInt32(apiResult.Response);
                                            if (msg == 1)
                                            {
                                                serviceitems.SwitchIsEnabled = false;
                                            }

                                        }

                                    }
                                }


                            }
                        }
                        IsBackIconEnabled = true;
                        IsRefreshing = false;

                        //}
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void BindServiceList()
        {
            if (value == 0)
            {
                int salonId = 0;

                using (SalonViewModel objSalon = new SalonViewModel())
                {
                    salonId = objSalon.GetSalonId();
                }

                ApiResult res = new ApiResult();
                using (BarberSalonService obj = new BarberSalonService())
                {
                    res = await obj.GetServicesBySalonId(salonId);
                    if (res != null && res.StatusCode == 200)
                    {
                        List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                        foreach (var item in result)
                        {
                            Service Services = new Service();

                            Services.ServiceId = item.ServiceId;
                            Services.Price = Application.Current.Properties["_Currency"].ToString() + item.ServicePrice;
                            //Services.Price = "£" + item.ServicePrice;
                            Services.ServiceName = item.ServiceName;
                            Services.ServiceIdTime = item.Estimated_wait_time;
                            Services.SwitchIsEnabled = true;
                            BrResult.Add(Services);
                        }
                    }
                }


                IsBackIconEnabled = true;
                IsRefreshing = false;
            }
        }

        public Command AddMemberCommand
        {

            get
            {
                return new Command(async () =>
                {
                    IsVisible = true;

                    try
                    {
                        if (String.IsNullOrEmpty(PreferredName) == false && String.IsNullOrEmpty(EmployeePasscode) == false && String.IsNullOrEmpty(Estimatedwaittime) == false && EmployeePasscode.Trim().Contains(" ") == false && Estimatedwaittime.All(char.IsNumber))
                        {
                            if (isEnabled == true)
                            {
                                using (SalonViewModel obj = new SalonViewModel())
                                {
                                    SalonID = obj.GetSalonId();
                                }


                                StaffModel staffModel = new StaffModel();
                                staffModel.LstService = new List<Services>();
                                if (value != 0)
                                {
                                    staffModel.BarberId = value;
                                }
                                if (!string.IsNullOrEmpty(FullName))
                                {
                                    var NameArray = FullName.Split(' ');
                                    for (int i = 0; i < NameArray.Length; i++)
                                    {
                                        if (i == NameArray.Length - 1)
                                        {
                                            staffModel.LastName = NameArray[i];
                                        }

                                        else
                                        {
                                            staffModel.FirstName += NameArray[i];
                                        }
                                    }
                                }



                                //     staffModel.FirstName = FirstName;
                                //      staffModel.LastName = LastName;
                                staffModel.PreferredName = PreferredName.Trim();
                                staffModel.EmployeePasscode = EmployeePasscode.ToUpper().Trim();
                                if (EmailAddress != null)
                                {
                                    staffModel.EmailAddress = EmailAddress.ToLower();
                                }
                                staffModel.Estimatedwaittime = Convert.ToInt32(Estimatedwaittime);

                                staffModel.SalonId = SalonID;

                                staffModel.AvailableForAppointments = (IsAvailableForAppointments == true) ? "1" : "0";


                                foreach (var items in BrResult)
                                {
                                    try
                                    {
                                        if (items.IsToggled)
                                        {
                                            Services service = new Services();
                                            service.ServiceId = items.ServiceId;
                                            service.ServiceTime = Convert.ToInt32(items.ServiceIdTime);
                                            staffModel.LstService.Add(service);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                                StaffServices staffServices = new StaffServices();
                                if (value == 0)
                                {
                                    var _Result = await staffServices.PostCreateStaff(staffModel);
                                    string msg = _Result.StatusMessage;

                                    if (_Result.StatusCode == 200)
                                    {
                                        isEnabled = false;
                                        // defaultcolor = Color.Gray;
                                        int CurrentBarberID = _Result.BarberID;
                                        await UploadProfileImage(CurrentBarberID);
                                        await App.Current.MainPage.DisplayAlert("Sucess", msg, "Ok");
                                        App.Current.MainPage.Navigation.PopAsync();
                                    }
                                    else if (_Result.StatusCode == 201)
                                    {
                                        await App.Current.MainPage.DisplayAlert("Error", msg, "Ok");
                                    }
                                }
                                else
                                {
                                    var _Result = await staffServices.PostUpdateStaff(staffModel);

                                    string msg = _Result.StatusMessage;
                                    if (_Result.StatusCode == 200)
                                    {
                                        isEnabled = false;
                                        // defaultcolor = Color.Gray;
                                        await UploadProfileImage(value);
                                        await App.Current.MainPage.DisplayAlert("Sucess", msg, "Ok");

                                        App.Current.MainPage.Navigation.PopAsync();
                                    }
                                    else
                                    {
                                        await App.Current.MainPage.DisplayAlert("Error", msg, "Ok");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(PreferredName) == true)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Preferred name cannot be blank", "Ok");
                            }
                            else if (String.IsNullOrEmpty(EmployeePasscode) == true || EmployeePasscode.Trim().Contains(" ") == true)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Employee passcode cannot contain space or be blank", "Ok");
                            }

                            else if (String.IsNullOrEmpty(Estimatedwaittime) == true || Estimatedwaittime.All(char.IsNumber) == false)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Estimated wait time is not valid", "Ok");
                            }
                            else if (String.IsNullOrEmpty(PreferredName) == false && String.IsNullOrEmpty(EmployeePasscode) == false)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Preferred name and Employee passcode cannot be blank", "Ok");
                            }
                            else if (String.IsNullOrEmpty(PreferredName) == false && String.IsNullOrEmpty(Estimatedwaittime) == false)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Estimated wait time and Preferred name cannot be blank", "Ok");
                            }
                            else if (String.IsNullOrEmpty(Estimatedwaittime) == false && String.IsNullOrEmpty(EmployeePasscode) == false)
                            {
                                await App.Current.MainPage.DisplayAlert("Failure", "Estimated wait time and Employee passcode cannot be blank", "Ok");
                            }

                        }
                    }
                    catch 
                    {

                        await App.Current.MainPage.DisplayAlert("Alert", "Please check the input provided!", "Ok");
                    }
                    IsVisible = false;
                    //App.Current.MainPage.Navigation.PopAsync();
                });
            }
        }

        public Command AddMemberPictureCommand
        {

            get
            {
                return new Command(async () =>
                {


                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                    if (status != PermissionStatus.Granted)
                    {
                        status = await StorageUtil.CheckPermissions(Permission.Storage);
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        IsEnabled = false;
                        await CrossMedia.Current.Initialize();

                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            await App.Current.MainPage.DisplayAlert("Oops", "Pick photo not supported", "OK");
                            IsEnabled = true;
                            return;
                        }

                        MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {
                            PhotoSize = PhotoSize.Small,
                            CustomPhotoSize = 50
                        });
                        if (photo == null)
                        {
                            IsEnabled = true;
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
                                ProfileImage = ImageSource.FromStream(() =>
                                {
                                    var stream = photo.GetStream();
                                    photo.Dispose();
                                    return stream;
                                });

                                //  ProfileImage = ImageSource.FromUri(new Uri("https://server.iqueuebarbers.com/~iqueue/userspics/18643.png",UriKind.RelativeOrAbsolute));
                            }
                        }
                        IsEnabled = true;
                        IsProfileApiRunning = false;
                    }


                });
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

        private async Task UploadProfileImage(int CurrentBarberID)
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

        private Color _defaultcolor = Color.Black;

        public Color defaultcolor
        {
            get { return _defaultcolor; }
            set { _defaultcolor = value; OnPropertyChanged("defaultcolor"); }
        }


        private bool m_IsVisible;

        public bool IsVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; OnPropertyChanged("IsVisible"); }
        }



        private bool _isUpdateVisible;

        public bool isUpdateVisible
        {
            get { return _isUpdateVisible; }
            set { _isUpdateVisible = value; OnPropertyChanged("isUpdateVisible"); }
        }

        private bool _isAddVisible;

        public bool isAddVisible
        {
            get { return _isAddVisible; }
            set { _isAddVisible = value; OnPropertyChanged("isAddVisible"); }
        }

        private bool _isEnabled = true;

        public bool isEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged("isEnabled"); }
        }

        private string _PageHeading;

        public string PageHeading
        {
            get { return _PageHeading; }
            set { _PageHeading = value; OnPropertyChanged("PageHeading"); }
        }



        private string _ButtonText;

        public string ButtonText
        {
            get { return _ButtonText; }
            set { _ButtonText = value; OnPropertyChanged("ButtonText"); }
        }


        private string _HeaderText;

        public string HeaderText
        {
            get { return _HeaderText; }
            set { _HeaderText = value; OnPropertyChanged("HeaderText"); }
        }
        private double _deviceHeight = Application.Current.MainPage.Height;

        public double deviceHeight
        {
            get { return _deviceHeight; }
            set { _deviceHeight = value; OnPropertyChanged("deviceHeight"); }
        }


        private bool _IsStaffGridEnabled = true;

        public bool IsStaffGridEnabled
        {
            get { return _IsStaffGridEnabled; }
            set { _IsStaffGridEnabled = value; OnPropertyChanged("IsStaffGridEnabled"); }
        }

        private bool _IsGridVisible;

        public bool IsGridVisible
        {
            get { return _IsGridVisible; }
            set { _IsGridVisible = value; OnPropertyChanged("IsGridVisible"); }
        }

        private string _ProfileImageURI;

        public string ProfileImageURI
        {
            get { return _ProfileImageURI; }
            set { _ProfileImageURI = value; OnPropertyChanged("ProfileImageURI"); }
        }


        private ImageSource _ProfileImage;

        public ImageSource ProfileImage
        {
            get { return _ProfileImage; }
            set { _ProfileImage = value; OnPropertyChanged("ProfileImage"); }
        }


        private bool m_IsProfileApiRunning;

        public bool IsProfileApiRunning
        {
            get { return m_IsProfileApiRunning; }
            set { m_IsProfileApiRunning = value; OnPropertyChanged("IsProfileApiRunning"); }
        }
        private string _FirstName;

        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _FullName;

        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
                OnPropertyChanged("FullName");
            }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; OnPropertyChanged("LastName"); }
        }

        private string _PreferredName;

        public string PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; OnPropertyChanged("PreferredName"); }
        }

        private String _EmployeePasscode;

        public String EmployeePasscode
        {
            get { return _EmployeePasscode; }
            set { _EmployeePasscode = value; OnPropertyChanged("EmployeePasscode"); }
        }

        private bool _IsAvailableForAppointments;

        public bool IsAvailableForAppointments
        {
            get
            {
                return _IsAvailableForAppointments;
            }
            set
            {
                _IsAvailableForAppointments = value;
                OnPropertyChanged("IsAvailableForAppointments");
            }
        }

        private double _ServiceListViewHeight;

        public double ServiceListViewHeight
        {
            get { return _ServiceListViewHeight; }
            set { _ServiceListViewHeight = value; OnPropertyChanged("ServiceListViewHeight"); }
        }


        private string _EmailAddress;

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; OnPropertyChanged("EmailAddress"); }
        }

        private string _Estimatedwaittime;

        public string Estimatedwaittime
        {
            get { return _Estimatedwaittime; }
            set { _Estimatedwaittime = value; OnPropertyChanged("Estimatedwaittime"); }
        }

        private string _ServiceName;

        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; OnPropertyChanged("ServiceName"); }
        }

        private string _ServicePrice;

        public string ServicePrice
        {
            get { return _ServicePrice; }
            set { _ServicePrice = value; OnPropertyChanged("ServicePrice"); }
        }

        private int _SalonID;

        public int SalonID
        {
            get { return _SalonID; }
            set { _SalonID = value; OnPropertyChanged("SalonID"); }
        }


        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }
        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private bool m_IsBackIconEnabled;
        public bool IsBackIconEnabled
        {
            get { return m_IsBackIconEnabled; }
            set { m_IsBackIconEnabled = value; OnPropertyChanged("IsBackIconEnabled"); }
        }

        private string m_TotalNoActiveBarbers { get; set; }
        public string TotalNoActiveBarbers
        {
            get { return m_TotalNoActiveBarbers; }
            set { m_TotalNoActiveBarbers = value; OnPropertyChanged("TotalNoActiveBarbers"); }
        }

        private string _TextValue;

        public string TextValue
        {
            get { return _TextValue; }
            set { _TextValue = value; OnPropertyChanged("TextValue"); }
        }

        private ObservableCollection<Service> _br = new ObservableCollection<Service>();
        public ObservableCollection<Service> BrResult
        {
            get { return _br; }
            set { _br = value; OnPropertyChanged("BrResult"); }
        }

        private List<Service> _GetlistService = new List<Service>();

        public List<Service> GetlistService
        {
            get { return _GetlistService; }
            set { _GetlistService = value; OnPropertyChanged("GetlistService"); }
        }

    }

    public class Service : BaseViewModel
    {
        private String _ServiceName;

        public String ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; OnPropertyChanged("ServiceName"); }
        }

        private int _serviceId;
        public int ServiceId
        {
            get { return _serviceId; }
            set { _serviceId = value; OnPropertyChanged(nameof(ServiceId)); }
        }

        private string _Price;

        public string Price
        {
            get { return _Price; }
            set { _Price = value; OnPropertyChanged("Price"); }
        }



        private string _ServiceIdTime;
        public string ServiceIdTime { get { return _ServiceIdTime; } set { _ServiceIdTime = value; OnPropertyChanged(nameof(ServiceIdTime)); } }

        private bool _IsToggled;

        public bool IsToggled
        {
            get { return _IsToggled; }
            set { _IsToggled = value; OnPropertyChanged("IsToggled"); }
        }

        private bool _SwitchIsEnabled;

        public bool SwitchIsEnabled
        {
            get { return _SwitchIsEnabled; }
            set { _SwitchIsEnabled = value; OnPropertyChanged("SwitchIsEnabled"); }
        }

    }
}
