using Acr.UserDialogs;
using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.AppointmentServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.TabPagers;
using Syncfusion.ListView.XForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IQB.Views.AdminManagement;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IQB.EntityLayer.AuthModel.BarberModel;
using IQB.Utility;
using IQB.IQBServices.SalonServices;

namespace IQB.Views.Appointment.Customer
{
  
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAppointment : ContentPage
    {

        private BarberSalonService objBarberSalonService = new BarberSalonService();
        private AppointmentServices objAppointmentService = new AppointmentServices();
        private GetEngageBarberTimeSlotResponseModel _getEngageBarberTimeSlotResponseModel = new GetEngageBarberTimeSlotResponseModel();
        private GetEngageReserveTimeBySalonidModel _getEngageReserveTimeBySalonidModel = new GetEngageReserveTimeBySalonidModel();
        private SalonServices objSalonService = new SalonServices();
        public static ToastConfig toastConfig { get; set; }
        //private List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
        private List<TimeSlot> _lstTime = new List<TimeSlot>();
        WriteErrorLog errLog = new WriteErrorLog();
        // public IEnumerable<TimeCheckList> newlistcheck { get; set; }
        public string ReasonForCancel { get; set; }
        public string ReasonForReschedule { get; set; }

        private int TimeLimit;
        private Int64 AdvanceReScheduleLimit;

        //private List<MappedAppointment> appointmentList = null;
        private List<ServiceModel> result = null;
        public List<ServiceData> _lstSelectedService { get; set; } = new List<ServiceData>();
        //public List<ServiceData> _lstSelectedServicenew { get; set; } = new List<ServiceData>();



        // public string ServiceTimeDuration = string.Empty;
        private List<string> datedata { get; set; } = new List<string>();
        private List<EntityLayer.AppointmentModels.Service> _servieListData { get; set; } = new List<EntityLayer.AppointmentModels.Service>();

        private BarberService objBarberService = new BarberService();
        private List<Mappedbarber> barberList = null;
        private AppointmentServices _appointmentServices = new AppointmentServices();
        BarberSalonService _BarberSalonService = new BarberSalonService();
        string AppointmentDate = string.Empty;
        string SelectedTimeSlot = string.Empty;
        string AppointmentId = string.Empty;
        string BarberID = string.Empty;
        public string SalonOpeningTime = string.Empty;
        public string SalonClosingTime = string.Empty;
        public string SalonTimeSlotSpanTime = string.Empty;
        public double SalonAdvancePayPercentange = 100D;
        private string WalkinCustomerName = string.Empty;
        private bool IsCheckServiceEdit = true;
        private bool _checkCancelStatus = true;
        private int DaysLimit;
        ApiResult res = new ApiResult();
        AppointmentRequestModel _appointmentRequestModel = new AppointmentRequestModel();
        int salonId = 0;
        private int _customerID;
        public CreateAppointment(int serviceId, int BarberId, int customerID = 0)
        {


            InitializeComponent();
            indicator.IsVisible = true;
            picker.IsEnabled = true;
            IsCheckServiceEdit = true;
            _customerID = customerID;
            btnCancel.IsVisible = false;
            btnAppointment.SetValue(Grid.ColumnSpanProperty, 2);
            gdPay.BackgroundColor = new Color(1, 1, 1, 0.75);
            toastConfig = new ToastConfig(string.Empty);
            toastConfig.SetDuration(5000);
            toastConfig.SetBackgroundColor((Color)App.CurrentApp.Resources["AppPrimary"]);
            toastConfig.Position = ToastPosition.Top;

            //entryPayment.Culture = new CultureInfo("en-GB");

            GetBarberEngageAndSalonReserveTimeList(Convert.ToString(Application.Current.Properties["_SalonId"]));
            MessagingCenter.Subscribe<string>(this, "SelectBarber", (st) =>
            {
                if (st != null)
                {
                    Navigation.PushModalAsync(new SelectBarber(st, Convert.ToString(salonId), true));
                }

            });//
            MessagingCenter.Subscribe<string>(this, "BarberSelected", (ids) =>
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    var IDS = ids.Split(',');
                    indicator.IsVisible = true;
                    SelectedListBinding(int.Parse(IDS[0]), int.Parse(IDS[1]), true);
                    indicator.IsVisible = false;
                }

            });

            //TimeSlotBinding();

            GetAppointmentSettingNew();
            //var resultDay = Task.Run(async () => await _appointmentServices.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            //if (resultDay != null && resultDay.StatusCode == 200)
            //{
            //    DaysLimit = Convert.ToInt32(resultDay.Response.NumberOfDaysAfterAppdate);
            //    TimeLimit = Convert.ToInt32(resultDay.Response.AdvanceBookingTime);
            //    AdvanceReScheduleLimit = Convert.ToInt64(resultDay.Response.ReScheduleEnableTime);
            //    SalonOpeningTime = !string.IsNullOrEmpty(resultDay.Response.SalonStartTime) ? resultDay.Response.SalonStartTime : CommonEL.TimeSlotStartTime;
            //    SalonClosingTime = !string.IsNullOrEmpty(resultDay.Response.SalonEndTime) ? resultDay.Response.SalonEndTime : CommonEL.TimeSlotEndTime;
            //    SalonAdvancePayPercentange = !string.IsNullOrEmpty(resultDay.Response.AdvancePaymentPercentage) ? Convert.ToDouble(resultDay.Response.AdvancePaymentPercentage) : 100D;
            //    var s = !string.IsNullOrEmpty(resultDay.Response.TimeSlotGap) ? resultDay.Response.TimeSlotGap : CommonEL.TimeSlotInterValInMinute;

            //    if ((s != null) && (s != "0"))
            //    {
            //        int totalMinutes = Convert.ToInt32(s);
            //        TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
            //        SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);


            //    }
            //    else
            //    {
            //        int totalMinutes = Convert.ToInt32(CommonEL.TimeSlotInterValInMinute);
            //        TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
            //        SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            //    }


            //}

            SelectedListBinding(serviceId, BarberId);
            ObservableCollection<object> todaycollection = new ObservableCollection<object>();

            //Select today dates
            todaycollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Date.Month)
                .Substring(0, 3));
            if (DateTime.Now.Date.Day < 10)
                todaycollection.Add("0" + DateTime.Now.Date.Day);
            else
                todaycollection.Add(DateTime.Now.Date.Day.ToString());
            todaycollection.Add(DateTime.Now.Date.Year.ToString());
            picker.SelectedItem = todaycollection;


        }
        public CreateAppointment(GetAppointmentByIdResponseModel _getAppointmentByIdResponseModel)
        {

            InitializeComponent();



            webView_Pay.HeightRequest = App.Current.MainPage.Height;
            webView_Pay.WidthRequest = App.Current.MainPage.Width;

            toastConfig = new ToastConfig(string.Empty);
            toastConfig.SetDuration(5000);
            toastConfig.SetBackgroundColor((Color)App.CurrentApp.Resources["AppPrimary"]);
            toastConfig.Position = ToastPosition.Top;


            picker.IsEnabled = true;
            IsCheckServiceEdit = false;
            indicator.IsVisible = true;
            _customerID = Convert.ToInt32(_getAppointmentByIdResponseModel.Response.UserName);
            btnCancel.IsEnabled = true;
            PageHeadingLabel.Text = "Reschedule Appointment";
            btnAppointment.Text = "UPDATE";
            gdPay.BackgroundColor = new Color(1, 1, 1, 0.75);
            GetBarberEngageAndSalonReserveTimeList(Convert.ToString(Application.Current.Properties["_SalonId"]));
            //entryPayment.Culture = new CultureInfo("en-GB");
            if (!string.IsNullOrEmpty(_getAppointmentByIdResponseModel?.Response?.WalkInCustomerName))
                WalkinCustomerName = _getAppointmentByIdResponseModel?.Response?.WalkInCustomerName;
            MessagingCenter.Subscribe<string>(this, "SelectBarber", (st) =>
            {
                if (st != null)
                {
                    //Navigation.PushModalAsync(new SelectBarber(st, _getAppointmentByIdResponseModel.Response.SalonID, true));
                    Navigation.PushModalAsync(new SelectBarber(st, Convert.ToString(Application.Current.Properties["_SalonId"]), true));
                }

            });

            GetAppointmentSettingNew();
            //var resultDay = Task.Run(async () => await _appointmentServices.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            // if (resultDay != null && resultDay.StatusCode == 200)
            // {
            //     DaysLimit = Convert.ToInt32(resultDay.Response.NumberOfDaysAfterAppdate);
            //     TimeLimit = Convert.ToInt32(resultDay.Response.AdvanceBookingTime);
            //     AdvanceReScheduleLimit = Convert.ToInt64(resultDay.Response.ReScheduleEnableTime);
            //     SalonOpeningTime = !string.IsNullOrEmpty(resultDay.Response.SalonStartTime) ? resultDay.Response.SalonStartTime : CommonEL.TimeSlotStartTime;
            //     SalonClosingTime = !string.IsNullOrEmpty(resultDay.Response.SalonEndTime) ? resultDay.Response.SalonEndTime : CommonEL.TimeSlotEndTime;
            //     SalonAdvancePayPercentange = !string.IsNullOrEmpty(resultDay.Response.AdvancePaymentPercentage) ? Convert.ToDouble(resultDay.Response.AdvancePaymentPercentage) : 100D;
            //     var s = !string.IsNullOrEmpty(resultDay.Response.TimeSlotGap) ? resultDay.Response.TimeSlotGap : CommonEL.TimeSlotInterValInMinute;
            //     if ((s != null) && (s != "0"))
            //     {
            //         int totalMinutes = Convert.ToInt32(s);
            //         TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
            //         SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);


            //     }
            //     else
            //     {
            //         int totalMinutes = Convert.ToInt32(CommonEL.TimeSlotInterValInMinute);
            //         TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
            //         SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            //     }
            // }


            if (_getAppointmentByIdResponseModel?.Response?.Services?.Count() > 0)
            {
                _servieListData.AddRange(_getAppointmentByIdResponseModel.Response.Services);
                AppointmentId = _getAppointmentByIdResponseModel.Response.AppointmentID;
                //SelectedExistingListBinding(_servieListData, Convert.ToInt32(_getAppointmentByIdResponseModel.Response.SalonID));
                SelectedExistingListBinding(_servieListData, Convert.ToInt32(Convert.ToString(Application.Current.Properties["_SalonId"])));




            }



            DateTime startDate;

            DateTime.TryParseExact(_getAppointmentByIdResponseModel?.Response?.Date.Replace(".", "/"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startDate);

            ObservableCollection<object> todaycollection = new ObservableCollection<object>();

            //Select API dates


            todaycollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startDate.Month)
                .Substring(0, 3));

            if (startDate.Day < 10)
                todaycollection.Add("0" + startDate.Day);
            else
                todaycollection.Add(startDate.Day.ToString());
            //todaycollection.Add(startDate.Day.ToString());
            todaycollection.Add(startDate.Year.ToString());

            picker.SelectedItem = todaycollection;


            ServiceList.IsEnabled = false;
            imgAddService.IsVisible = false;



            var _rescheduleStatus = CheckAdvanceScheduleStatus(_getAppointmentByIdResponseModel?.Response?.Date, _servieListData);
            picker.IsEnabled = _rescheduleStatus;
            MessagesListView.IsEnabled = _rescheduleStatus;
            btnAppointment.IsEnabled = _rescheduleStatus;
            btnCancel.IsEnabled = _rescheduleStatus;
            //ServiceList.IsEnabled = _rescheduleStatus;
            //imgAddService.IsVisible = _rescheduleStatus;

            if (!_rescheduleStatus)
            {
                toastConfig.Message = "You can not reschedule this appointment as because you crossed maximum amount of time (" + AdvanceReScheduleLimit + " mins) before an appointment can be rescheduled or cancelled.";
                UserDialogs.Instance.Toast(toastConfig);
                // UserDialogs.Instance.Alert(new AlertConfig { Message = "You can not reschedule this appointment as because you crossed maximum amount of time (" + AdvanceReScheduleLimit + " mins) before an appointment can be rescheduled or cancelled.", Title = "Sorry!" });
            }



            //Code for cancel logic
            _checkCancelStatus = CheckCancelScheduleStatus(_getAppointmentByIdResponseModel?.Response?.Date, _servieListData);
            //btnCancel.IsEnabled = _checkCancelStatus;
            //Code for cancel logic
            salonId = Convert.ToInt32(_getAppointmentByIdResponseModel.Response.SalonID);


        }
        public async void GetAppointmentSettingNew()
        {
            var resultDay = Task.Run(async () => await _appointmentServices.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            if (resultDay != null && resultDay.StatusCode == 200)
            {
                DaysLimit = Convert.ToInt32(resultDay.Response.NumberOfDaysAfterAppdate);
                TimeLimit = Convert.ToInt32(resultDay.Response.AdvanceBookingTime);
                AdvanceReScheduleLimit = Convert.ToInt64(resultDay.Response.ReScheduleEnableTime);
                SalonOpeningTime = !string.IsNullOrEmpty(resultDay.Response.SalonStartTime) ? resultDay.Response.SalonStartTime : CommonEL.TimeSlotStartTime;
                SalonClosingTime = !string.IsNullOrEmpty(resultDay.Response.SalonEndTime) ? resultDay.Response.SalonEndTime : CommonEL.TimeSlotEndTime;
                SalonAdvancePayPercentange = !string.IsNullOrEmpty(resultDay.Response.AdvancePaymentPercentage) ? Convert.ToDouble(resultDay.Response.AdvancePaymentPercentage) : 100D;
                var s = !string.IsNullOrEmpty(resultDay.Response.TimeSlotGap) ? resultDay.Response.TimeSlotGap : CommonEL.TimeSlotInterValInMinute;
                if ((s != null) && (s != "0"))
                {
                    int totalMinutes = Convert.ToInt32(s);
                    TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
                    SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);


                }
                else
                {
                    int totalMinutes = Convert.ToInt32(CommonEL.TimeSlotInterValInMinute);
                    TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
                    SalonTimeSlotSpanTime = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
        }

        public void BindAppointmentDate()
        {
            try
            {
                var selectedDate = ((IEnumerable)picker.SelectedItem).Cast<object>().ToList();
                var startDateString = $"{selectedDate[0]} {selectedDate[1]},{selectedDate[2]}";
                //var startDateTime = DateTime.ParseExact(startDateString, "MMM dd,yyyy", CultureInfo.InvariantCulture);
                var startDateTime = UtilityEL.GetExactData(startDateString, "MMM dd,yyyy");
                AppointmentDate = startDateTime.ToString("dd/MM/yyyy");


            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "BindAppointmentDate");
            }

        }

        private bool CheckAdvanceScheduleStatus(string date, List<EntityLayer.AppointmentModels.Service> _servieList)
        {
            try
            {
                bool checkstatus = true;
                foreach (var item in _servieList)
                {
                    DateTime tempDate = UtilityEL.GetExactData(date + " " + item.From, "dd/MM/yyyy hh:mm tt");

                    if (DateTime.Compare(tempDate, DateTime.Now) > 0)
                    {
                        var s = DateTime.Now.Subtract(tempDate);
                        var y = Math.Abs(s.TotalMinutes);
                        if (Math.Abs(s.TotalMinutes) < AdvanceReScheduleLimit)
                        {
                            checkstatus = false;
                        }
                    }
                    else
                    {
                        checkstatus = false;
                    }
                }

                return checkstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private bool CheckCancelScheduleStatus(string date, List<EntityLayer.AppointmentModels.Service> _servieList)
        {
            try
            {
                bool checkstatus = true;
                foreach (var item in _servieList)
                {
                    DateTime tempDate = UtilityEL.GetExactData(date + " " + item.From, "dd/MM/yyyy hh:mm tt");

                    if (DateTime.Compare(tempDate, DateTime.Now) > 0)
                    {
                        var s = DateTime.Now.Subtract(tempDate);
                        var y = Math.Abs(s.TotalMinutes);
                        if (Math.Abs(s.TotalMinutes) < 24 * 60)
                        {
                            checkstatus = false;
                        }
                    }
                    else
                    {
                        checkstatus = false;
                    }
                }

                return checkstatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {

        }

        public int CheckPresentDate()
        {
            try
            {
                DateTime tempAppoDate = UtilityEL.GetExactData(AppointmentDate, "dd/MM/yyyy");
                int result = DateTime.Compare(tempAppoDate, DateTime.Now.Date);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void TimeSlotBinding()
        {
            try
            {
                if (CheckPresentDate() >= 0)
                {
                    var selectedTimeSlot = !string.IsNullOrEmpty(SelectedTimeSlot) ? Convert.ToDateTime(SelectedTimeSlot).ToString("hh:mm tt") : SalonClosingTime;
                    DateTime tempDatecheck = UtilityEL.GetExactData(AppointmentDate + " " + selectedTimeSlot, "dd/MM/yyyy hh:mm tt");


                    if (DateTime.Compare(tempDatecheck, DateTime.Now) >= 0)
                    {
                        _lstTime.Clear();
                        DateTime dt = Convert.ToDateTime(SalonOpeningTime);
                        DateTime tempDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", dt).ToUpper(), "dd/MM/yyyy hh:mm tt");
                        DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                        if (DateTime.Compare(tempEndDate, tempDate) > 0)
                        {
                            int s = DateTime.Compare(tempEndDate, DateTime.Now.AddMinutes(TimeLimit));

                            //if (DateTime.Compare(tempDate, DateTime.Now.AddMinutes(TimeLimit)) > 0)
                            if (DateTime.Compare(tempEndDate, DateTime.Now.AddMinutes(TimeLimit)) > 0)

                                _lstTime.Add(new TimeSlot
                                {
                                    TimeSlotData = string.Format("{0:hh:mm tt}", dt).ToUpper(),
                                    TemplateBackground = Color.FromHex("#0D1013"),
                                    TimeTextColor = Color.FromHex("#0DA5B7")

                                });
                        }

                        // while (string.Format("{0:hh:mm:ss tt}", dt).ToUpper() != CommonEL.TimeSlotEndTime)
                        while (DateTime.Compare(tempEndDate, tempDate) > 0)
                        {


                            //dt += TimeSpan.Parse(CommonEL.TimeSlotInterVal);
                            dt += TimeSpan.Parse(SalonTimeSlotSpanTime);



                            tempDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", dt).ToUpper(), "dd/MM/yyyy hh:mm tt");
                            //if (DateTime.Compare(tempDate, DateTime.Now) > 0)
                            if (DateTime.Compare(tempEndDate, tempDate) > 0)
                            {

                                //if (DateTime.Compare(tempDate, DateTime.Now.AddMinutes(TimeLimit)) > 0)
                                if (DateTime.Compare(tempEndDate, DateTime.Now.AddMinutes(TimeLimit)) > 0)
                                    _lstTime.Add(new TimeSlot
                                    {
                                        TimeSlotData = string.Format("{0:hh:mm tt}", dt).ToUpper(),
                                        TemplateBackground = Color.FromHex("#0D1013"),
                                        TimeTextColor = Color.FromHex("#0DA5B7")

                                    });



                            }


                        }
                        MessagesListView.ItemsSource = null;
                        MessagesListView.ItemsSource = _lstTime;
                    }
                    //else
                    //{
                    //    //await DisplayAlert("Error!", "You can not book appointment on back date", "Ok");
                    //    //UserDialogs.Instance.Alert(new AlertConfig { Message = "You can not book appointment on this date", Title = "Message" });

                    //}
                }
                else
                {
                    _lstTime.Clear();
                    DateTime dt = Convert.ToDateTime(SalonOpeningTime);
                    DateTime tempDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", dt).ToUpper(), "dd/MM/yyyy hh:mm tt");
                    DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                    if (DateTime.Compare(tempEndDate, tempDate.AddMinutes(TimeLimit)) > 0)

                        _lstTime.Add(new TimeSlot
                        {
                            TimeSlotData = string.Format("{0:hh:mm tt}", dt).ToUpper(),
                            TemplateBackground = Color.FromHex("#0D1013"),
                            TimeTextColor = Color.FromHex("#0DA5B7")

                        });
                    while (DateTime.Compare(tempEndDate, tempDate) > 0)
                    {


                        //dt += TimeSpan.Parse(CommonEL.TimeSlotInterVal);
                        dt += TimeSpan.Parse(SalonTimeSlotSpanTime);


                        tempDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", dt).ToUpper(), "dd/MM/yyyy hh:mm tt");
                        //if (DateTime.Compare(tempDate, DateTime.Now) > 0)
                        if (DateTime.Compare(tempEndDate, tempDate) > 0)
                        {

                            //if (DateTime.Compare(tempDate, DateTime.Now.AddMinutes(TimeLimit)) > 0)
                            //if (DateTime.Compare(tempEndDate, tempDate.AddMinutes(TimeLimit)) > 0)
                            _lstTime.Add(new TimeSlot
                            {
                                TimeSlotData = string.Format("{0:hh:mm tt}", dt).ToUpper(),
                                TemplateBackground = Color.FromHex("#0D1013"),
                                TimeTextColor = Color.FromHex("#0DA5B7")

                            });



                        }


                    }
                    MessagesListView.ItemsSource = null;
                    MessagesListView.ItemsSource = _lstTime;


                }

                indicator.IsVisible = false;


            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "TimeSlotBinding");
            }

        }

        public bool CheckSaveAppointmentStatus(List<ServiceData> _lstTime)
        {
            bool CheckStatus = true;
            GetBarberEngageAndSalonReserveTimeList(Convert.ToString(Application.Current.Properties["_SalonId"]));

            var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
            {
                BarberId = x.BarberId,
                Date = x.Date,
                From = x.From,
                To = x.To,
                AppoDate = x.AppoDate
            }) ?? Enumerable.Empty<TimeCheckList>();
            var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
            {
                BarberId = x.BarberId,
                Date = x.Date,
                From = x.From,
                To = x.To,
                AppoDate = x.AppoDate
            }) ?? Enumerable.Empty<TimeCheckList>();
            var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId != "0").OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
            {
                BarberId = x.BarberId,
                Date = x.Date,
                From = x.From,
                To = x.To,
                AppoDate = x.AppoDate
            }) ?? Enumerable.Empty<TimeCheckList>();

            //Code for User check Start//
            List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
            string userdata = string.Empty;
            if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
            {
                if (_customerID != 0)
                {
                    userdata = $"{_customerID}";

                }
            }
            else
            {
                userdata = Convert.ToString(Application.Current.Properties["UserName"]);
            }
            if (!string.IsNullOrEmpty(userdata))
                _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

            var _userdata = _lstuser?.Select(x => new TimeCheckList
            {
                BarberId = x.BarberId,
                Date = x.Date,
                From = x.From,
                To = x.To,
                AppoDate = x.AppoDate
            }) ?? Enumerable.Empty<TimeCheckList>();


            //Code for User check End//
            DateTime dtnew;
            DateTime dtend;
            DateTime dtfrom;
            DateTime dtto;

            if (_lstTime?.Count() > 0)
            {
                foreach (var item in _lstTime)
                {
                    dtnew = Convert.ToDateTime(item.ServiceTime.Split('-')[0]);
                    dtend = Convert.ToDateTime(item.ServiceTime.Split('-')[1]);

                    var newlistcheck = (res?.Where(x => x.BarberId == item.BarberId)?.Concat(resReserve)?.Concat(resbarberReserve?.Where(s => s.BarberId == item.BarberId))?.Concat(_userdata))?
                            .GroupBy(m => new { m.BarberId, m.Date, m.From, m.To })?.Select(g => g.First())?.OrderBy(x => x.AppoDate.TimeOfDay);



                    //Added to serach on the Check list Star
                    if (newlistcheck != null && newlistcheck.Count() > 0)
                    {
                        foreach (var itemCheckdata in newlistcheck)
                        {
                            dtfrom = Convert.ToDateTime(itemCheckdata.From);
                            dtto = Convert.ToDateTime(itemCheckdata.To);
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                CheckStatus = false;
                            }
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                CheckStatus = false;
                            }
                            if ((dtnew <= dtfrom) && (dtend >= dtto))
                            {
                                //match found
                                CheckStatus = false;
                            }
                        }
                    }


                    //Added to serach on the Check listEnd

                }
            }


            return CheckStatus;
        }

        private async void btnAppointment_OnClicked(object sender, EventArgs e)
        {
            try
            {
                //bool appostatus = true;
                //DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                //foreach (var item in _lstSelectedService)
                //{
                //    string[] timedata;
                //    timedata = item.ServiceTime.Split('-');
                //    DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[1].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");
                //    if (DateTime.Compare(tempEndDate, appoendtime) < 0)
                //    {
                //        appostatus = false;
                //        break;
                //    }

                //}

                bool appostatus = true;
                DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                DateTime tempStartDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonOpeningTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                foreach (var item in _lstSelectedService)
                {
                    string[] timedata;
                    timedata = item.ServiceTime.Split('-');
                    DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[1].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");
                    DateTime appostarttime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[0].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");

                    if (DateTime.Compare(tempEndDate, appostarttime) <= 0)
                    {
                        appostatus = false;
                        break;
                    }
                    if (DateTime.Compare(tempEndDate, appoendtime) < 0)
                    {
                        appostatus = false;
                        break;
                    }
                    if ((DateTime.Compare(tempEndDate, appoendtime) >= 0) && (DateTime.Compare(tempStartDate, appoendtime) >= 0))
                    {
                        appostatus = false;
                        break;
                    }


                }

                if (appostatus)
                {

                    //if(CheckSaveAppointmentStatus(_lstSelectedService))
                    //{
                    var answer = await DisplayAlert("Save", AppointmentId != "" ? "Do you want to reschedule the Appointment?" : "Do you want to book the Appointment?", "Yes", "No");
                    if (answer)
                    {
                        var selectedTimeSlot = !string.IsNullOrEmpty(SelectedTimeSlot) ? Convert.ToDateTime(SelectedTimeSlot).ToString("HH:mm") : DateTime.Now.ToString("HH:mm");
                        DateTime tempDate = UtilityEL.GetExactData(AppointmentDate + " " + selectedTimeSlot, "dd/MM/yyyy HH:mm");

                        int result = DateTime.Compare(tempDate, DateTime.Now);
                        if (result > 0)
                        {

                            var sD = DateTime.Now;
                            var eD = UtilityEL.GetExactData(AppointmentDate + " " + selectedTimeSlot, "dd/MM/yyyy HH:mm");
                            var thirtyMin = TimeSpan.FromDays(DaysLimit > 0 ? DaysLimit : 30).TotalMinutes;
                            var diff = Math.Abs(eD.Subtract(sD).TotalMinutes);
                            if (diff < thirtyMin)
                            {
                                //Reason coding start here
                                if (!string.IsNullOrEmpty(AppointmentId))
                                {

                                    UserDialogs.Instance.Prompt(new PromptConfig
                                    {
                                        Title = "Please enter the Reason",
                                        Text = ReasonForReschedule,
                                        OnAction = ChangeAppointmentLINK

                                    });

                                }
                                else
                                {


                                    indicator.IsVisible = true;
                                    _appointmentRequestModel.ID = AppointmentId != null ? AppointmentId : "0";
                                    _appointmentRequestModel.SalonID = Convert.ToString(salonId);
                                    _appointmentRequestModel.Date = AppointmentDate;
                                    _appointmentRequestModel.StatusChangeReason = "";
                                    if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                                    {
                                        if (_customerID != 0)
                                        {
                                            _appointmentRequestModel.UserName = $"{_customerID}";
                                            _appointmentRequestModel.WalkInCustomerName = "";
                                        }
                                        else
                                        {
                                            _appointmentRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                                            if (!string.IsNullOrEmpty(Convert.ToString(Application.Current.Properties["_WalKinClientName"])))
                                                _appointmentRequestModel.WalkInCustomerName = Convert.ToString(Application.Current.Properties["_WalKinClientName"]);
                                            Application.Current.Properties["_WalKinClientName"] = string.Empty;
                                        }

                                    }

                                    else
                                    {
                                        _appointmentRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                                        _appointmentRequestModel.WalkInCustomerName = "";
                                    }

                                    _appointmentRequestModel.Status = "0";
                                    _appointmentRequestModel.IsReserved = "N";

                                    List<ServiceRequest> _lstServiceRequest = new List<ServiceRequest>();
                                    foreach (var item in _lstSelectedService)
                                    {
                                        string[] timedata;
                                        timedata = item.ServiceTime.Split('-');

                                        _lstServiceRequest.Add(new ServiceRequest
                                        {
                                            BarberId = item.BarberId,
                                            ServiceId = item.ServiceId,
                                            From = timedata[0].ToString(),
                                            To = timedata[1].ToString()
                                        });
                                    }
                                    _appointmentRequestModel.Services = _lstServiceRequest;



                                    var totalprice = _lstSelectedService.Sum(x => x.ServicePrice);
                                    var minAmount = totalprice * (SalonAdvancePayPercentange / 100);
                                    entryPayment.Minimum = minAmount;
                                    entryPayment.Maximum = totalprice;
                                    entryPayment.Value = minAmount;
                                    _appointmentRequestModel.TotalAmount = totalprice.ToString();

                                    var salonDetailsResponse = await objSalonService.GetSalonDetailsByID($"{salonId}");


                                    if (salonDetailsResponse?.StatusCode == 200)
                                    {
                                        if (salonDetailsResponse.Response != null)
                                        {

                                            //if (!string.IsNullOrEmpty(salonDetailsResponse?.PaymentGatewaySettings?.merchant_id) &&
                                            //    !string.IsNullOrEmpty(salonDetailsResponse?.PaymentGatewaySettings?.private_key) &&
                                            //    !string.IsNullOrEmpty(salonDetailsResponse?.PaymentGatewaySettings?.public_key) &&
                                            //    !string.IsNullOrEmpty(salonDetailsResponse?.PaymentGatewaySettings?.tokenization_key))
                                            //{
                                            if (!string.IsNullOrEmpty(salonDetailsResponse?.PaymentGatewaySettings?.merchant_email))
                                            {
                                                if (minAmount > 0)
                                                {
                                                    indicator.IsVisible = false;
                                                    gdPay.IsVisible = true;//to be opened after completion of payment gateway
                                                }
                                                else
                                                {
                                                    _appointmentRequestModel.pId = "0";
                                                    PaymentSuccessCallback(true);

                                                }
                                            }


                                            //   }

                                            else
                                            {

                                                var resultConfirm = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                                                {
                                                    Message = "Payment configuration for this salon is not present. Do you want to proceed without paying now?",
                                                    OkText = "Yes",
                                                    CancelText = "No"
                                                });
                                                if (resultConfirm)
                                                {
                                                    _appointmentRequestModel.pId = "0";
                                                    PaymentSuccessCallback(true);
                                                }
                                                else
                                                {
                                                    indicator.IsVisible = false;
                                                }

                                                //_appointmentRequestModel.pId = "0";
                                                //PaymentSuccessCallback(true);
                                            }
                                        }
                                    }



                                    //PaymentSuccessCallback(true);//to be deleted after completion of payment gateway
                                }
                                //Reason coding end  here
                            }
                            else
                            {
                                //await DisplayAlert("Message", "You can not book appointment more than 30 days advance", "Ok");
                                await DisplayAlert("Message", "You can only book an appointment up to " + DaysLimit.ToString() + " days in advance", "Ok");

                            }
                        }
                        else
                        {
                            await DisplayAlert("Message", "You can not book appointment on back date", "Ok");
                        }
                    }
                    //}
                    //else
                    //{

                    //     UserDialogs.Instance.Alert(new AlertConfig { Message = "The barbers may be engazed with thw others.So you need to change the Date or Time,", Title = "Message" });

                    //}




                }
                else
                {
                    UserDialogs.Instance.Alert(new AlertConfig { Message = "The service(s) you selected already crossed the closing time(" + SalonClosingTime + ") of the salon.So you need to change the Date or Time or need to remove that service which crossed the closing time.", Title = "Message" });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Some Error Occured", "Ok");
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "btnAppointment_OnClicked");
                indicator.IsVisible = false;
                webView_Pay.IsVisible = false;
            }
            finally
            {
                //indicator.IsVisible = false;
                //webView_Pay.IsVisible = false;
            }

        }

        private async void ChangeAppointmentLINK(PromptResult obj)
        {
            try
            {
                if (obj.Ok)
                {

                    indicator.IsVisible = true;
                    _appointmentRequestModel.ID = AppointmentId != null ? AppointmentId : "0";
                    _appointmentRequestModel.SalonID = Convert.ToString(salonId);
                    _appointmentRequestModel.Date = AppointmentDate;
                    _appointmentRequestModel.StatusChangeReason = obj.Text;
                    if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                    {
                        if (_customerID != 0)
                        {
                            _appointmentRequestModel.UserName = $"{_customerID}";
                            _appointmentRequestModel.WalkInCustomerName = "";
                        }
                        else
                        {
                            _appointmentRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                            _appointmentRequestModel.WalkInCustomerName = !string.IsNullOrEmpty(WalkinCustomerName) ? WalkinCustomerName : null;

                        }

                    }

                    else
                    {
                        _appointmentRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                        _appointmentRequestModel.WalkInCustomerName = "";
                    }

                    _appointmentRequestModel.Status = "0";
                    _appointmentRequestModel.IsReserved = "N";

                    List<ServiceRequest> _lstServiceRequest = new List<ServiceRequest>();
                    foreach (var item in _lstSelectedService)
                    {
                        string[] timedata;
                        timedata = item.ServiceTime.Split('-');

                        _lstServiceRequest.Add(new ServiceRequest
                        {
                            BarberId = item.BarberId,
                            ServiceId = item.ServiceId,
                            From = timedata[0].ToString(),
                            To = timedata[1].ToString()
                        });
                    }
                    _appointmentRequestModel.Services = _lstServiceRequest;
                    _appointmentRequestModel.TotalAmount = (_lstSelectedService.Sum(x => x.ServicePrice)).ToString();
                    var rescheduleResponse = await _appointmentServices.SetAppointment(_appointmentRequestModel);
                    if (rescheduleResponse != null && rescheduleResponse.StatusCode == 200)
                    {
                        await DisplayAlert("Success!", "Appointment Rescheduled successfully", "Ok");
                        indicator.IsVisible = false;

                    }
                    else
                        await DisplayAlert("Failure!", "Appointment not Rescheduled. Please try again.", "Ok");

                    App.Current.MainPage = new NavigationPage(new MainTabPage(true));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Some error occured. Please try again.", "Ok");
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "ChangeAppointmentLINK");
                indicator.IsVisible = false;
            }
            finally
            {
                //indicator.IsVisible = false;
            }
        }

        private async void btnCancel_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (_checkCancelStatus)
                {
                    if (!string.IsNullOrEmpty(AppointmentId))
                    {
                        var answer = await DisplayAlert("Cancel", "Do you want to cancel the Appointment?", "Yes", "No");
                        if (answer)
                        {


                            UserDialogs.Instance.Prompt(new PromptConfig
                            {
                                Title = "Please enter the Reason",
                                Text = ReasonForCancel,
                                OnAction = ChangeLINK

                            });

                            //var appointmentDeleteDataResponse = await objAppointmentService.DeleteAppoinmentStatus(Convert.ToInt32(AppointmentId));
                            //App.Current.MainPage = new NavigationPage(new MainTabPage(true));
                            //        await Navigation.PushAsync(new ManageAppointment());
                        }
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Message", "You can not cancel the appointment as it is within the 24 hours", "Ok");
                }



            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "btnCancel_OnClicked");
            }
        }

        private async void ChangeLINK(PromptResult res)
        {
            try
            {
                if (res.Ok)
                {
                    indicator.IsVisible = true;
                    string s = res.Text;
                    //Application.Current.Properties["_WalKinClientName"] = res.Value;
                    //await Navigation.PushAsync(new SelectService());


                    var appointmentDeleteDataResponse = await objAppointmentService.DeleteAppoinmentStatus(Convert.ToInt32(AppointmentId), res.Text);
                    if (appointmentDeleteDataResponse?.StatusCode == 200)
                    {
                        await DisplayAlert("Success!", "Appointment Cancelled successfully", "Ok");


                    }
                    else
                        await DisplayAlert("Failure!", "Appointment can't be cancelled. Please try again.", "Ok");

                    indicator.IsVisible = false;

                    App.Current.MainPage = new NavigationPage(new MainTabPage(true));

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Some error occured. Please try again.", "Ok");
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "ChangeLINK");
                indicator.IsVisible = false;

            }
            finally
            {
                // indicator.IsVisible = false;
            }
        }

        //public async void SelectedListBinding(int serviceId, int BarberId, bool isCallback = false)
        public void SelectedListBinding(int serviceId, int BarberId, bool isCallback = false)
        {
            try
            {
                BarberID = Convert.ToString(BarberId);

                #region ServiceList
                //using (SalonViewModel objSalon = new SalonViewModel())
                //{
                //    salonId = objSalon.GetSalonId();
                //}
                //var res = Task.Run(async () => await objBarberSalonService.GetServicesBySalonId(salonId)).Result;
                //if (res?.StatusCode == 200)
                //{
                //    result = UtilityEL.ToModelList<ServiceModel>(res.Response);
                //}

                #endregion


                #region BarberList
                //barberList = new List<Mappedbarber>();
                //var appointmentListResponse = Task.Run(async () => await objBarberService.GetBarberListBySalonServiceID(serviceId.ToString(), salonId.ToString())).Result;
                //if (appointmentListResponse?.StatusCode == 200)
                //{
                //    if (appointmentListResponse?.Response?.Count() > 0)
                //        barberList.AddRange(appointmentListResponse.Response);
                //}
                #endregion
                //var ServiceName = result.Where(x => x.ServiceId == serviceId).FirstOrDefault().ServiceName;
                //var ServiceTimeDuration = result.Where(x => x.ServiceId == serviceId).FirstOrDefault().Estimated_wait_time;
                //var SerVicePrice = result.Where(x => x.ServiceId == serviceId).FirstOrDefault().ServicePrice;
                //var BarberName = barberList.Where(x => x.BarberId == BarberId.ToString()).FirstOrDefault().BarberName;


                #region BarberAndService
                using (SalonViewModel objSalon = new SalonViewModel())
                {
                    salonId = objSalon.GetSalonId();
                }
                List<Rootobject> result = new List<Rootobject>();

                var res = Task.Run(async () => await _BarberSalonService.GetStaffDetailsBySalonIdStaffId(BarberId)).Result;
                if (res != null && res.StatusCode == 200)
                {
                    result = UtilityEL.ToModelListNew<Rootobject>(res.Response);


                }
                #endregion

                var BarberServiceList = result.Select(x => x.Services);

                var ServiceName = BarberServiceList.Select(z => z.FirstOrDefault(u => u.ServiceId == serviceId.ToString()).ServiceName)?.FirstOrDefault();
                var ServiceTimeDuration = BarberServiceList.Select(z => z.FirstOrDefault(u => u.ServiceId == serviceId.ToString()).ServiceTime)?.FirstOrDefault();
                var SerVicePrice = BarberServiceList.Select(z => z.FirstOrDefault(u => u.ServiceId == serviceId.ToString()).ServicePrice)?.FirstOrDefault();
                //var BarberName = result.Where(u => u.BarberId == BarberId).FirstOrDefault().FirstName + " " + result.Where(u => u.BarberId == BarberId).FirstOrDefault().LastName;
                var BarberName = result.Where(u => u.BarberId == BarberId).FirstOrDefault().PreferredName;


                string ServiceTimeData = string.Empty;
                if ((ServiceTimeDuration != null) && (ServiceTimeDuration != "0"))
                {
                    int totalMinutes = Convert.ToInt32(ServiceTimeDuration);
                    TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
                    ServiceTimeData = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    ServiceTimeData = CommonEL.DefaultServiceDeuarion;
                }

                //_lstSelectedService.Clear();
                if (!_lstSelectedService.Any(s => s.ServiceId == $"{serviceId}"))
                {
                    if (isCallback)
                    {

                        //if (!string.IsNullOrEmpty(GetAvailableServiceTimeDuration($"{BarberId}", $"{serviceId}", ServiceTimeData)))//Newly Added
                        //{
                        //    var s = GetAvailableServiceTimeDuration($"{BarberId}", $"{serviceId}", ServiceTimeData);
                        //    _lstSelectedService.Add(new ServiceData
                        //    {
                        //        BarberName = BarberName,
                        //        ServiceName = ServiceName,
                        //        //"01:00:00"
                        //        ServiceDuration = ServiceTimeData,
                        //        ServiceId = Convert.ToString(serviceId),
                        //        BarberId = Convert.ToString(BarberId),
                        //        ServicePrice = Int64.Parse(SerVicePrice, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                        //        ServicePriceData = "£" + SerVicePrice,
                        //        ServiceTime = s,
                        //        ServiceStarttime = Convert.ToDateTime(s.Split('-')[0])
                        //    });
                        //}
                        //var servicetime = GetAvailableServiceTimeDuration($"{BarberId}", $"{serviceId}", ServiceTimeData);
                        var servicetime = GetAvailableServiceTimeDurationOptimized($"{BarberId}", $"{serviceId}", ServiceTimeData);


                        if (!string.IsNullOrEmpty(servicetime))//Newly Added
                        {

                            _lstSelectedService.Add(new ServiceData
                            {
                                BarberName = BarberName,
                                ServiceName = ServiceName,
                                //"01:00:00"
                                ServiceDuration = ServiceTimeData,
                                ServiceId = Convert.ToString(serviceId),
                                BarberId = Convert.ToString(BarberId),
                                //ServicePrice = Int64.Parse(SerVicePrice, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                                ServicePrice = Double.Parse(SerVicePrice),
                                //ServicePriceData = "£" + SerVicePrice,
                                ServicePriceData = Application.Current.Properties["_Currency"].ToString() + SerVicePrice,
                                ServiceTime = servicetime,
                                ServiceStarttime = Convert.ToDateTime(servicetime.Split('-')[0])
                            });
                        }
                    }
                    else
                    {
                        _lstSelectedService.Add(new ServiceData
                        {
                            BarberName = BarberName,
                            ServiceName = ServiceName,
                            ServiceDuration = ServiceTimeData,
                            ServiceId = Convert.ToString(serviceId),
                            BarberId = Convert.ToString(BarberId),
                            //ServicePrice = Int64.Parse(SerVicePrice, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                            ServicePrice = Double.Parse(SerVicePrice),
                            ServicePriceData = Application.Current.Properties["_Currency"].ToString() + SerVicePrice,
                            ServiceTime = null
                        });

                    }
                }
                else
                {
                    //await DisplayAlert("Duplicate!", "You have already selected this service.", "Ok");
                    //Task.Run(async () => await DisplayAlert("Duplicate!", "You have already selected this service.", "Ok"));
                    toastConfig.Message = "Duplicate! You have already selected this service.";
                    UserDialogs.Instance.Toast(toastConfig);
                }





                /***********************For TestTing Sample Data Add Start*********************************************/
                //var ServiceName2 = result.Where(x => x.ServiceId == 56).FirstOrDefault().ServiceName;
                //var ServiceTimeDuration2 = result.Where(x => x.ServiceId == 56).FirstOrDefault().Estimated_wait_time;
                //var SerVicePrice2 = result.Where(x => x.ServiceId == 56).FirstOrDefault().ServicePrice;
                //var BarberName2 = barberList.Where(x => x.BarberId == "2").FirstOrDefault().BarberName;
                //_lstSelectedService.Add(new ServiceData
                //{
                //    BarberName = BarberName2,
                //    ServiceName = ServiceName2,
                //    ServiceDuration = ServiceTimeDuration2,
                //    ServiceId = Convert.ToString(56),
                //    BarberId = Convert.ToString("2"),
                //    ServicePrice = Int64.Parse(SerVicePrice2, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                //    ServicePriceData = "£" + SerVicePrice2
                //    //ServiceTime = "10:00AM-11:00AM"
                //});
                /***********************For TestTing Sample Data Add End*********************************************/
                //if (!isCallback)
                //{

                //}
                //else
                //{
                //    ServiceList.ItemsSource = null;
                //    ServiceList.ItemsSource = _lstSelectedService.OrderBy(x => x.ServiceStarttime.TimeOfDay);
                //    var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
                //    lblTotal.Text = "£" + totalprice + ".00";
                //}



                if (isCallback)
                {
                    ServiceList.ItemsSource = null;
                    ServiceList.ItemsSource = _lstSelectedService.OrderBy(x => x.ServiceStarttime.TimeOfDay);
                    var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
                    //lblTotal.Text = "£" + totalprice + ".00";
                    lblTotal.Text = Application.Current.Properties["_Currency"].ToString() + totalprice;
                }


            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "SelectedListBinding");
            }
        }

        //public async void GetBarberEngageAndSalonReserveTimeList(string SalonId)
        public void GetBarberEngageAndSalonReserveTimeList(string SalonId)
        {

            try
            {


                {
                    #region BaberEngageList

                    // _getEngageBarberTimeSlotResponseModel = Task.Run(async () => await objAppointmentService.GetEngageBarberTimeSlot(Convert.ToString(salonId))).Result;
                    _getEngageBarberTimeSlotResponseModel = Task.Run(async () => await objAppointmentService.GetEngageBarberTimeSlot(SalonId)).Result;

                    #endregion


                    #region SalonReserveTime

                    //  _getEngageReserveTimeBySalonidModel = await objAppointmentService.GetEngageReserveTimeBySalonid(Convert.ToString(salonId));
                    //_getEngageReserveTimeBySalonidModel = Task.Run(async () => await objAppointmentService.GetEngageReserveTimeBySalonid(Convert.ToString(salonId))).Result;
                    _getEngageReserveTimeBySalonidModel = Task.Run(async () => await objAppointmentService.GetEngageReserveTimeBySalonid(SalonId)).Result;

                    #endregion
                    // await DisplayAlert(_getEngageReserveTimeBySalonidModel.Response.Count().ToString(), _getEngageReserveTimeBySalonidModel.Response.Count().ToString(), "OK");
                }
            }
            catch (Exception ex)
            {

                // await DisplayAlert("GetBarberEngageAndSalonReserveTimeList!", ex.StackTrace, "Ok");
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "GetBarberEngageAndSalonReserveTimeList");
            }

        }

        // public async void SelectedExistingListBinding(List<EntityLayer.AppointmentModels.Service> _servieList, int salonID)
        public void SelectedExistingListBinding(List<EntityLayer.AppointmentModels.Service> _servieList, int salonID)
        {
            try
            {
                //#region ServiceList

                //salonId = salonID;
                //var res = Task.Run(async () => await objBarberSalonService.GetServicesBySalonId(salonId)).Result;


                //if (res?.StatusCode == 200)
                //{
                //    result = UtilityEL.ToModelList<ServiceModel>(res.Response);
                //}
                //#endregion


                //#region BarberList

                //List<BarberReturnResult> resultbarber = new List<BarberReturnResult>();
                //var resBarber = Task.Run(async () => await objBarberSalonService.GetBarberBySalonId(salonId)).Result;
                //if (resBarber != null && resBarber.StatusCode == 200)
                //{
                //    resultbarber = UtilityEL.ToModelList<BarberReturnResult>(resBarber.Response);
                //}

                ///****************************************Below code is for the satic data.When The data will be dynamic please comment the below line and uncomment the above start********************************************/
                ////barberList = new List<Mappedbarber>();
                ////var appointmentListResponse = Task.Run(async () => await objBarberService.GetBarberListBySalonServiceID("0", "0")).Result;

                ////if (appointmentListResponse?.StatusCode == 200)
                ////{
                ////    barberList.Clear();
                ////    if (appointmentListResponse?.Response?.Count() > 0)
                ////        barberList.AddRange(appointmentListResponse.Response);
                ////}
                ///****************************************Below code is for the satic data.When The data will be dynamic please comment the below line and uncomment the above start*******************************************/
                //#endregion




                _lstSelectedService.Clear();
                foreach (var item in _servieList)
                {

                    //#region BarberAndService

                    //List<Rootobject> resultsaffservice = new List<Rootobject>();

                    //var res1 = Task.Run(async () => await _BarberSalonService.GetStaffDetailsBySalonIdStaffId(Convert.ToInt16(item.BarberId))).Result;
                    //if (res1 != null && res1.StatusCode == 200)
                    //{
                    //    resultsaffservice = UtilityEL.ToModelListNew<Rootobject>(res1.Response);
                    //}
                    //#endregion

                    //var BarberServiceList = resultsaffservice.Select(x => x.Services);


                    //var ServiceTimeDuration = BarberServiceList?.Select(z => z.FirstOrDefault(u => u.ServiceId == item.ServiceId.ToString())?.ServiceTime)?.FirstOrDefault();

                    //string ServiceTimeData = string.Empty;


                    //if ((ServiceTimeDuration != null) && (ServiceTimeDuration != "0"))
                    //{
                    //    int totalMinutes = Convert.ToInt32(ServiceTimeDuration);
                    //    TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
                    //    ServiceTimeData = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

                    //}
                    //else
                    //{
                    //    ServiceTimeData = CommonEL.DefaultServiceDeuarion;
                    //}

                    //var ServiceName = result.Where(x => x.ServiceId == Convert.ToInt16(item.ServiceId)).FirstOrDefault().ServiceName;
                    ////var ServiceTimeDuration = result.Where(x => x.ServiceId == Convert.ToInt16(item.ServiceId)).FirstOrDefault().Estimated_wait_time;
                    //var BarberName = resultbarber.Where(x => x.BarberId == Convert.ToInt16(item.BarberId)).FirstOrDefault().BarberName;


                    string ServiceTimeData = string.Empty;

                    if ((item.EstWaitTime != null) && (item.EstWaitTime != "0"))
                    {
                        int totalMinutes = Convert.ToInt32(item.EstWaitTime);
                        TimeSpan ts = TimeSpan.FromMinutes(totalMinutes);
                        ServiceTimeData = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

                    }
                    else
                    {
                        ServiceTimeData = CommonEL.DefaultServiceDeuarion;
                    }




                    _lstSelectedService.Add(new ServiceData
                    {
                        BarberName = item.BarberName,
                        ServiceId = item.ServiceId,
                        ServiceDuration = ServiceTimeData,
                        BarberId = item.BarberId,
                        ServiceName = item.ServiceName,
                        ServicePrice = Double.Parse(item.Price),
                        //ServicePrice = Int64.Parse(item.Price, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                        ServicePriceData = Application.Current.Properties["_Currency"].ToString() + item.Price,
                        //ServiceTime = "10:00AM - 11:00AM"
                        ServiceTime = item.From + "-" + item.To
                    });


                }



                ServiceList.ItemsSource = _lstSelectedService;
                var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
                //lblTotal.Text = "£" + totalprice + ".00";
                lblTotal.Text = Application.Current.Properties["_Currency"].ToString() + totalprice;

                //indicator.IsVisible = false;





            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "SelectedExistingListBinding");

            }
        }

        private async void ViewService_Tapped(object sender, EventArgs e)
        {
            try
            {
                indicator.IsVisible = true;
                //    GetBarberEngageAndSalonReserveTimeList(Convert.ToString(salonId));
                var slecteddata = ((TimeSlot)((ItemSelectionChangedEventArgs)e).AddedItems?.FirstOrDefault())?.TimeSlotData;
                DateTime tempDate = UtilityEL.GetExactData(AppointmentDate + " " + slecteddata, "dd/MM/yyyy hh:mm tt");
                int result = DateTime.Compare(tempDate, DateTime.Now.AddMinutes(TimeLimit));
                if (result > 0)
                {
                    foreach (var item in _lstSelectedService)
                    {
                        item.ServiceTime = null;
                    }
                    SelectedTimeSlot = slecteddata;
                    //ServiceTimeBindIng(SelectedTimeSlot);
                    ServiceTimeBindIngNew(SelectedTimeSlot);
                }
                else
                {
                    UserDialogs.Instance.Alert(new AlertConfig { Message = "You can not select this time slot.", Title = "Sorry!" });

                }
                string minTimespot = setneartimeslot(GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0]);
                if (!string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    if (SelectedTimeSlot != minTimespot)
                    {
                        SelectedTimeSlot = minTimespot;
                    }
                    MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == SelectedTimeSlot).FirstOrDefault();
                }

                if (MessagesListView.SelectedItem != null)
                {
                    int i = ((List<TimeSlot>)MessagesListView.ItemsSource).IndexOf(_lstTime.Where(x => x.TimeSlotData == ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData).FirstOrDefault());
                    MessagesListView.LayoutManager.ScrollToRowIndex(i, true);

                }



                indicator.IsVisible = false;

            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "ViewService_Tapped");
            }


        }

        //public void ServiceTimeBindIng(string StartSlotTime)
        //{
        //    try
        //    {

        //        var StartTime = StartSlotTime;
        //        //var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).ToList();
        //        // var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).ToList();
        //        var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

        //        var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId != "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();

        //        //Code for User check Start//
        //        List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
        //        string userdata = string.Empty;
        //        if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
        //        {
        //            if (_customerID != 0)
        //            {
        //                userdata = $"{_customerID}";

        //            }
        //        }
        //        else
        //        {
        //            userdata = Convert.ToString(Application.Current.Properties["UserName"]);
        //        }
        //        if (!string.IsNullOrEmpty(userdata))
        //            _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        //Code for User check End//



        //        DateTime dtnew;
        //        DateTime dtend;
        //        DateTime dtfrom;
        //        DateTime dtto;

        //        if (_lstSelectedService?.Count() > 0)
        //        {
        //            if (res != null && res?.Count() > 0)
        //            {
        //                foreach (var item in _lstSelectedService)
        //                {
        //                    dtnew = Convert.ToDateTime(StartTime);

        //                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                    else
        //                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                    //Added to serach on the same list Start
        //                    foreach (var itemservicedata in _lstSelectedService)
        //                    {


        //                        dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
        //                        dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

        //                        //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                        if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }

        //                        if ((dtend > dtfrom) && (dtend <= dtto))

        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }
        //                    }

        //                    //Added to serach on the same listEnd

        //                    //For Service Start Time Logic
        //                    foreach (var itemEngageRsult in res)
        //                    {
        //                        if (item.BarberId == itemEngageRsult.BarberId)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
        //                            dtto = Convert.ToDateTime(itemEngageRsult.To);

        //                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))

        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                        }

        //                    }




        //                    //Code for Same user Appointment Check Start
        //                    if (_lstuser != null && _lstuser.Count() > 0)
        //                    {
        //                        foreach (var itemuser in _lstuser)
        //                        {
        //                            dtfrom = Convert.ToDateTime(itemuser.From);
        //                            dtto = Convert.ToDateTime(itemuser.To);

        //                            // if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))

        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //Code for Same user Appointment Check End

        //                    //code for barber reservation check start

        //                    if (resbarberReserve != null && resbarberReserve.Count() > 0)
        //                    {
        //                        foreach (var itemBarberReserve in resbarberReserve)
        //                        {
        //                            if (item.BarberId == itemBarberReserve.BarberId)
        //                            {
        //                                dtfrom = Convert.ToDateTime(itemBarberReserve.From);
        //                                dtto = Convert.ToDateTime(itemBarberReserve.To);

        //                                if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }

        //                                if ((dtend > dtfrom) && (dtend <= dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }
        //                            }


        //                        }
        //                    }
        //                    //code for barber reservation check end

        //                    //code for reservation check start
        //                    if (resReserve != null && resReserve.Count() > 0)
        //                    {
        //                        foreach (var itemReserve in resReserve)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemReserve.From);
        //                            dtto = Convert.ToDateTime(itemReserve.To);

        //                            //if ((dtnew > dtfrom) && (dtnew < dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            // if ((dtend > dtfrom) && (dtend < dtto))
        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //code for reservation check end





        //                    StartTime = string.Format("{0:hh:mm tt}", dtnew).ToUpper();
        //                    var finatTime = string.Format("{0:hh:mm tt}", dtend).ToUpper();
        //                    item.ServiceStarttime = Convert.ToDateTime(StartTime);
        //                    item.ServiceTime = StartTime + "-" + finatTime;

        //                    //StartTime = finatTime;
        //                    StartTime = StartSlotTime;


        //                }
        //            }
        //            else
        //            {
        //                foreach (var item in _lstSelectedService)
        //                {
        //                    dtnew = Convert.ToDateTime(StartTime);

        //                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                    else
        //                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


        //                    //Added to search on the same list Start
        //                    foreach (var itemservicedata in _lstSelectedService)
        //                    {


        //                        dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
        //                        dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

        //                        //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                        if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }

        //                        // if ((dtend >= dtfrom) && (dtend <= dtto))
        //                        if ((dtend > dtfrom) && (dtend <= dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }
        //                    }

        //                    //Added to search on the same listEnd



        //                    //Code for Same user Appointment Check Start
        //                    if (_lstuser != null && _lstuser.Count() > 0)
        //                    {
        //                        foreach (var itemuser in _lstuser)
        //                        {
        //                            dtfrom = Convert.ToDateTime(itemuser.From);
        //                            dtto = Convert.ToDateTime(itemuser.To);

        //                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //Code for Same user Appointment Check End

        //                    //code for barber reservation check start

        //                    if (resbarberReserve != null && resbarberReserve.Count() > 0)
        //                    {
        //                        foreach (var itemBarberReserve in resbarberReserve)
        //                        {
        //                            if (item.BarberId == itemBarberReserve.BarberId)
        //                            {
        //                                dtfrom = Convert.ToDateTime(itemBarberReserve.From);
        //                                dtto = Convert.ToDateTime(itemBarberReserve.To);

        //                                if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }

        //                                if ((dtend > dtfrom) && (dtend <= dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }
        //                            }


        //                        }
        //                    }
        //                    //code for barber reservation check end


        //                    //code for reservation check start
        //                    if (resReserve != null && resReserve.Count() > 0)
        //                    {
        //                        foreach (var itemReserve in resReserve)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemReserve.From);
        //                            dtto = Convert.ToDateTime(itemReserve.To);

        //                            // if ((dtnew > dtfrom) && (dtnew < dtto)).
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            //if ((dtend > dtfrom) && (dtend < dtto))
        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //code for reservation check end



        //                    StartTime = string.Format("{0:hh:mm tt}", dtnew).ToUpper();
        //                    var finatTime = string.Format("{0:hh:mm tt}", dtend).ToUpper();
        //                    item.ServiceStarttime = Convert.ToDateTime(StartTime);
        //                    item.ServiceTime = StartTime + "-" + finatTime;

        //                    StartTime = finatTime;
        //                    //StartTime = StartSlotTime;

        //                }
        //            }

        //        }

        //        ServiceList.ItemsSource = null;
        //        ServiceList.ItemsSource = _lstSelectedService.OrderBy(x => x.ServiceStarttime.TimeOfDay);

        //        var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
        //        lblTotal.Text = "£" + totalprice + ".00";

        //        bool appostatus = true;
        //        DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //        DateTime tempStartDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonOpeningTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");

        //        DateTime tempDayEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(CommonEL.DayTimeEndTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //        foreach (var item in _lstSelectedService)
        //        {
        //            string[] timedata;
        //            timedata = item.ServiceTime.Split('-');
        //            DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[1].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //            DateTime appostarttime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[0].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");

        //            if (DateTime.Compare(tempEndDate, appostarttime) <= 0)
        //            {
        //                appostatus = false;
        //                break;
        //            }
        //            if (DateTime.Compare(tempEndDate, appoendtime) < 0)
        //            {
        //                appostatus = false;
        //                break;
        //            }
        //            //int x = DateTime.Compare(tempEndDate, appoendtime);
        //            //int y = DateTime.Compare(tempStartDate, appoendtime);


        //            if ((DateTime.Compare(tempEndDate, appoendtime) >= 0) && (DateTime.Compare(tempStartDate, appoendtime) >= 0))
        //            {
        //                appostatus = false;
        //                break;
        //            }



        //        }
        //        if (appostatus)
        //        {
        //            btnAppointment.IsEnabled = true;

        //            bool b = _lstSelectedService.Any(s => s.ServiceTime.Contains(StartSlotTime));
        //            if (!b)
        //            {
        //                toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
        //                UserDialogs.Instance.Toast(toastConfig);
        //                //UserDialogs.Instance.Alert(new AlertConfig { Message = "The selected service time is changed due to unavialability of the barber at that time.", Title = "Alert!" });

        //            }

        //        }
        //        else
        //        {
        //            btnAppointment.IsEnabled = false;
        //            if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
        //            {

        //                toastConfig.Message = "You can not book appointment for this day because the day is off.So you need to change the Date.";
        //                UserDialogs.Instance.Toast(toastConfig);

        //            }


        //            else
        //            {
        //                toastConfig.Message = "You can not book appointment for this day because it already crossed the closing time (" + SalonClosingTime + ").So you need to change the Date";
        //                UserDialogs.Instance.Toast(toastConfig);
        //            }

        //        }




        //    }
        //    catch (Exception ex)
        //    {
        //        //UserDialogs.Instance.Prompt(new PromptConfig { Message = "Some Error occured", Title = "Error!" });
        //        errLog.WinrtErrLogTest(ex, "CreateAppointment", "ServiceTimeBindIng");

        //    }

        //}

        //public void ServiceTimeBindIngBKP(string StartSlotTime)
        //{
        //    try
        //    {

        //        var StartTime = StartSlotTime;
        //        //var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).ToList();
        //        // var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).ToList();
        //        var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

        //        var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId != "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();

        //        //Code for User check Start//
        //        List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
        //        string userdata = string.Empty;
        //        if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
        //        {
        //            if (_customerID != 0)
        //            {
        //                userdata = $"{_customerID}";

        //            }
        //        }
        //        else
        //        {
        //            userdata = Convert.ToString(Application.Current.Properties["UserName"]);
        //        }
        //        if (!string.IsNullOrEmpty(userdata))
        //            _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
        //        //Code for User check End//



        //        DateTime dtnew;
        //        DateTime dtend;
        //        DateTime dtfrom;
        //        DateTime dtto;

        //        if (_lstSelectedService?.Count() > 0)
        //        {
        //            if (res != null && res?.Count() > 0)
        //            {
        //                foreach (var item in _lstSelectedService)
        //                {
        //                    dtnew = Convert.ToDateTime(StartTime);

        //                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                    else
        //                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                    //Added to serach on the same list Start
        //                    foreach (var itemservicedata in _lstSelectedService)
        //                    {


        //                        dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
        //                        dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

        //                        //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                        if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }

        //                        if ((dtend > dtfrom) && (dtend <= dtto))

        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }
        //                    }

        //                    //Added to serach on the same listEnd

        //                    //For Service Start Time Logic
        //                    foreach (var itemEngageRsult in res)
        //                    {
        //                        if (item.BarberId == itemEngageRsult.BarberId)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
        //                            dtto = Convert.ToDateTime(itemEngageRsult.To);

        //                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))

        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                        }

        //                    }

        //                    //code for reservation check start
        //                    if (resReserve != null && resReserve.Count() > 0)
        //                    {
        //                        foreach (var itemReserve in resReserve)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemReserve.From);
        //                            dtto = Convert.ToDateTime(itemReserve.To);

        //                            //if ((dtnew > dtfrom) && (dtnew < dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            // if ((dtend > dtfrom) && (dtend < dtto))
        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //code for reservation check end

        //                    //code for barber reservation check start

        //                    if (resbarberReserve != null && resbarberReserve.Count() > 0)
        //                    {
        //                        foreach (var itemBarberReserve in resbarberReserve)
        //                        {
        //                            if (item.BarberId == itemBarberReserve.BarberId)
        //                            {
        //                                dtfrom = Convert.ToDateTime(itemBarberReserve.From);
        //                                dtto = Convert.ToDateTime(itemBarberReserve.To);

        //                                if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }

        //                                if ((dtend > dtfrom) && (dtend <= dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }
        //                            }


        //                        }
        //                    }
        //                    //code for barber reservation check end


        //                    //Code for Same user Appointment Check Start
        //                    if (_lstuser != null && _lstuser.Count() > 0)
        //                    {
        //                        foreach (var itemuser in _lstuser)
        //                        {
        //                            dtfrom = Convert.ToDateTime(itemuser.From);
        //                            dtto = Convert.ToDateTime(itemuser.To);

        //                            // if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))

        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //Code for Same user Appointment Check End



        //                    StartTime = string.Format("{0:hh:mm tt}", dtnew).ToUpper();
        //                    var finatTime = string.Format("{0:hh:mm tt}", dtend).ToUpper();
        //                    item.ServiceStarttime = Convert.ToDateTime(StartTime);
        //                    item.ServiceTime = StartTime + "-" + finatTime;

        //                    //StartTime = finatTime;
        //                    StartTime = StartSlotTime;


        //                }
        //            }
        //            else
        //            {
        //                foreach (var item in _lstSelectedService)
        //                {
        //                    dtnew = Convert.ToDateTime(StartTime);

        //                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                    else
        //                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


        //                    //Added to search on the same list Start
        //                    foreach (var itemservicedata in _lstSelectedService)
        //                    {


        //                        dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
        //                        dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

        //                        //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                        if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }

        //                        // if ((dtend >= dtfrom) && (dtend <= dtto))
        //                        if ((dtend > dtfrom) && (dtend <= dtto))
        //                        {
        //                            //match found
        //                            dtnew = dtto;

        //                            if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                            else
        //                                dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                        }
        //                    }

        //                    //Added to search on the same listEnd

        //                    //code for reservation check start
        //                    if (resReserve != null && resReserve.Count() > 0)
        //                    {
        //                        foreach (var itemReserve in resReserve)
        //                        {

        //                            dtfrom = Convert.ToDateTime(itemReserve.From);
        //                            dtto = Convert.ToDateTime(itemReserve.To);

        //                            // if ((dtnew > dtfrom) && (dtnew < dtto)).
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            //if ((dtend > dtfrom) && (dtend < dtto))
        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //code for reservation check end

        //                    //code for barber reservation check start

        //                    if (resbarberReserve != null && resbarberReserve.Count() > 0)
        //                    {
        //                        foreach (var itemBarberReserve in resbarberReserve)
        //                        {
        //                            if (item.BarberId == itemBarberReserve.BarberId)
        //                            {
        //                                dtfrom = Convert.ToDateTime(itemBarberReserve.From);
        //                                dtto = Convert.ToDateTime(itemBarberReserve.To);

        //                                if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }

        //                                if ((dtend > dtfrom) && (dtend <= dtto))
        //                                {
        //                                    //match found
        //                                    dtnew = dtto;

        //                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                    else
        //                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                                }
        //                            }


        //                        }
        //                    }
        //                    //code for barber reservation check end

        //                    //Code for Same user Appointment Check Start
        //                    if (_lstuser != null && _lstuser.Count() > 0)
        //                    {
        //                        foreach (var itemuser in _lstuser)
        //                        {
        //                            dtfrom = Convert.ToDateTime(itemuser.From);
        //                            dtto = Convert.ToDateTime(itemuser.To);

        //                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
        //                            if ((dtnew >= dtfrom) && (dtnew < dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }

        //                            if ((dtend > dtfrom) && (dtend <= dtto))
        //                            {
        //                                //match found
        //                                dtnew = dtto;

        //                                if (!string.IsNullOrEmpty(item.ServiceDuration))
        //                                    dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
        //                                else
        //                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

        //                            }
        //                        }
        //                    }
        //                    //Code for Same user Appointment Check End


        //                    StartTime = string.Format("{0:hh:mm tt}", dtnew).ToUpper();
        //                    var finatTime = string.Format("{0:hh:mm tt}", dtend).ToUpper();
        //                    item.ServiceStarttime = Convert.ToDateTime(StartTime);
        //                    item.ServiceTime = StartTime + "-" + finatTime;

        //                    StartTime = finatTime;
        //                    //StartTime = StartSlotTime;

        //                }
        //            }

        //        }

        //        ServiceList.ItemsSource = null;
        //        ServiceList.ItemsSource = _lstSelectedService.OrderBy(x => x.ServiceStarttime.TimeOfDay);

        //        var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
        //        lblTotal.Text = "£" + totalprice + ".00";

        //        bool appostatus = true;
        //        DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //        DateTime tempStartDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonOpeningTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");

        //        DateTime tempDayEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(CommonEL.DayTimeEndTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //        foreach (var item in _lstSelectedService)
        //        {
        //            string[] timedata;
        //            timedata = item.ServiceTime.Split('-');
        //            DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[1].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");
        //            DateTime appostarttime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[0].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");

        //            if (DateTime.Compare(tempEndDate, appostarttime) <= 0)
        //            {
        //                appostatus = false;
        //                break;
        //            }
        //            if (DateTime.Compare(tempEndDate, appoendtime) < 0)
        //            {
        //                appostatus = false;
        //                break;
        //            }
        //            //int x = DateTime.Compare(tempEndDate, appoendtime);
        //            //int y = DateTime.Compare(tempStartDate, appoendtime);


        //            if ((DateTime.Compare(tempEndDate, appoendtime) >= 0) && (DateTime.Compare(tempStartDate, appoendtime) >= 0))
        //            {
        //                appostatus = false;
        //                break;
        //            }



        //        }
        //        if (appostatus)
        //        {
        //            btnAppointment.IsEnabled = true;

        //            bool b = _lstSelectedService.Any(s => s.ServiceTime.Contains(StartSlotTime));
        //            if (!b)
        //            {
        //                toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
        //                UserDialogs.Instance.Toast(toastConfig);
        //                //UserDialogs.Instance.Alert(new AlertConfig { Message = "The selected service time is changed due to unavialability of the barber at that time.", Title = "Alert!" });

        //            }

        //        }
        //        else
        //        {
        //            btnAppointment.IsEnabled = false;
        //            if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
        //            {

        //                toastConfig.Message = "You can not book appointment for this day because the day is off.So you need to change the Date.";
        //                UserDialogs.Instance.Toast(toastConfig);

        //            }


        //            else
        //            {
        //                toastConfig.Message = "You can not book appointment for this day because it already crossed the closing time (" + SalonClosingTime + ").So you need to change the Date";
        //                UserDialogs.Instance.Toast(toastConfig);
        //            }

        //        }




        //    }
        //    catch (Exception ex)
        //    {
        //        //UserDialogs.Instance.Prompt(new PromptConfig { Message = "Some Error occured", Title = "Error!" });
        //        errLog.WinrtErrLogTest(ex, "CreateAppointment", "ServiceTimeBindIng");

        //    }

        //}


        public void ServiceTimeBindIngNew(string StartSlotTime)
        {
            try
            {

                var StartTime = StartSlotTime;
                var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();
                var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();
                var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId != "0").OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();

                //Code for User check Start//
                List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
                string userdata = string.Empty;
                if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                {
                    if (_customerID != 0)
                    {
                        userdata = $"{_customerID}";

                    }
                }
                else
                {
                    userdata = Convert.ToString(Application.Current.Properties["UserName"]);
                }
                if (!string.IsNullOrEmpty(userdata))
                    _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

                var _userdata = _lstuser?.Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();


                //Code for User check End//


                DateTime dtnew;
                DateTime dtend;
                DateTime dtfrom;
                DateTime dtto;
                bool timenotchange = true;
                if (_lstSelectedService?.Count() > 0)
                {
                    foreach (var item in _lstSelectedService)
                    {
                        dtnew = Convert.ToDateTime(StartTime);

                        if (!string.IsNullOrEmpty(item.ServiceDuration))
                            dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                        //Added to serach on the same list Start
                        var _selfdataCheck = CheckSelfTimeList(_lstSelectedService, dtnew, dtend);
                        dtnew = _selfdataCheck.Item1;
                        dtend = _selfdataCheck.Item2;
                        //Added to serach on the same listEnd

                        var newlistcheck = (res?.Where(x => x.BarberId == item.BarberId)?.Concat(resReserve)?.Concat(resbarberReserve?.Where(s => s.BarberId == item.BarberId))?.Concat(_userdata))?
                             .GroupBy(m => new { m.BarberId, m.Date, m.From, m.To })?.Select(g => g.First())?.OrderBy(x => x.AppoDate.TimeOfDay);



                        //Added to serach on the Check list Star
                        if (newlistcheck != null && newlistcheck.Count() > 0)
                        {
                            foreach (var itemCheckdata in newlistcheck)
                            {


                                dtfrom = Convert.ToDateTime(itemCheckdata.From);
                                dtto = Convert.ToDateTime(itemCheckdata.To);


                                //if ((dtnew >= dtfrom) && (dtnew <= dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    timenotchange = false;
                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))

                                {
                                    //match found
                                    dtnew = dtto;
                                    timenotchange = false;
                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                                //New Added
                                if ((dtnew <= dtfrom) && (dtend >= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    timenotchange = false;
                                    if (!string.IsNullOrEmpty(item.ServiceDuration))
                                        dtend = dtnew + TimeSpan.Parse(item.ServiceDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                                }
                            }
                        }


                        //Added to serach on the Check listEnd


                        StartTime = string.Format("{0:hh:mm tt}", dtnew).ToUpper();
                        var finatTime = string.Format("{0:hh:mm tt}", dtend).ToUpper();
                        item.ServiceStarttime = Convert.ToDateTime(StartTime);
                        item.ServiceTime = StartTime + "-" + finatTime;

                        //StartTime = finatTime;
                        StartTime = StartSlotTime;

                    }

                }



                ServiceList.ItemsSource = null;
                ServiceList.ItemsSource = _lstSelectedService.OrderBy(x => x.ServiceStarttime.TimeOfDay);

                var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
                //lblTotal.Text = "£" + totalprice + ".00";
                lblTotal.Text = Application.Current.Properties["_Currency"].ToString() + totalprice;

                bool appostatus = true;
                DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                DateTime tempStartDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonOpeningTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");

                DateTime tempDayEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(CommonEL.DayTimeEndTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
                foreach (var item in _lstSelectedService)
                {
                    string[] timedata;
                    timedata = item.ServiceTime.Split('-');
                    DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[1].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");
                    DateTime appostarttime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(timedata[0].ToString())).ToUpper(), "dd/MM/yyyy hh:mm tt");

                    if (DateTime.Compare(tempEndDate, appostarttime) <= 0)
                    {
                        appostatus = false;
                        break;
                    }
                    if (DateTime.Compare(tempEndDate, appoendtime) < 0)
                    {
                        appostatus = false;
                        break;
                    }


                    if ((DateTime.Compare(tempEndDate, appoendtime) >= 0) && (DateTime.Compare(tempStartDate, appoendtime) >= 0))
                    {
                        appostatus = false;
                        break;
                    }
                }

                if (appostatus)
                {
                    btnAppointment.IsEnabled = true;
                    if (!timenotchange)
                    {
                        toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
                        UserDialogs.Instance.Toast(toastConfig);
                    }
                }
                else
                {
                    btnAppointment.IsEnabled = false;
                    if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
                    {

                        toastConfig.Message = "You cannot book an appointment for this day because the salon is closed, please choose a different date.";
                        UserDialogs.Instance.Toast(toastConfig);
                    }
                    else
                    {
                        toastConfig.Message = "You can not book appointment for this day because it already crossed the closing time (" + SalonClosingTime + ").";
                        UserDialogs.Instance.Toast(toastConfig);
                    }

                }




            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "ServiceTimeBindIng");

            }

        }

        public Tuple<DateTime, DateTime> CheckSelfTimeList(List<ServiceData> _lstSelectedService, DateTime dtnew, DateTime dtend)
        {
            DateTime dtfrom;
            DateTime dtto;

            //Added to serach on the same list Start
            foreach (var itemservicedata in _lstSelectedService)
            {
                dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                //if ((dtnew >= dtfrom) && (dtnew <= dtto))
                if ((dtnew >= dtfrom) && (dtnew < dtto))
                {
                    //match found
                    dtnew = dtto;

                    if (!string.IsNullOrEmpty(itemservicedata.ServiceDuration))
                        dtend = dtnew + TimeSpan.Parse(itemservicedata.ServiceDuration);
                    else
                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                }

                if ((dtend > dtfrom) && (dtend <= dtto))

                {
                    //match found
                    dtnew = dtto;

                    if (!string.IsNullOrEmpty(itemservicedata.ServiceDuration))
                        dtend = dtnew + TimeSpan.Parse(itemservicedata.ServiceDuration);
                    else
                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                }
            }
            return Tuple.Create(dtnew, dtend);
            //Added to serach on the same listEnd
        }
        public void ChangeStartSlotTime()
        {
            try
            {
                /////////////////////
                List<string> timeslot = new List<string>();
                foreach (var item in _lstSelectedService)
                {
                    timeslot.Add(item.ServiceTime.Split('-')[0]);
                }
                var restime = GetEarlierSatrtTimeSlot(timeslot);
                string slottime = string.Empty;


                foreach (var item in _lstTime)
                {
                    DateTime tempDateslot1 = UtilityEL.GetExactData(AppointmentDate + " " + item.TimeSlotData, "dd/MM/yyyy hh:mm tt");
                    DateTime tempDateslot2 = UtilityEL.GetExactData(AppointmentDate + " " + restime, "dd/MM/yyyy hh:mm tt");

                    int resultslot = DateTime.Compare(tempDateslot1, tempDateslot2);

                    if (resultslot >= 0 && String.IsNullOrEmpty(slottime))
                    {
                        slottime = item.TimeSlotData;

                    }

                }
                MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == slottime).FirstOrDefault();
                SelectedTimeSlot = ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData;


                ////////////////////
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "ChangeStartSlotTime");
            }

        }

        public string GetEarlierSatrtTimeSlot(List<string> timeString)
        {
            try
            {
                List<double> _lst = new List<double>();
                List<TimeSpan> _lsts = new List<TimeSpan>();

                foreach (var item in timeString)
                {
                    var x = UtilityEL.GetExactData(item, "hh:mm tt").TimeOfDay;
                    _lsts.Add(x);
                    _lst.Add(x.TotalMinutes);
                }
                var res = timeString[_lst.IndexOf(_lst.Min())];
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetAvailableServiceTimeDuration(string BarberId, string ServiceId, string ServiceTimeDuration)
        {

            try
            {
                string Timeslot = string.Empty;
                //var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).ToList();
                //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).ToList();

                var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

                var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();


                //Code for User check Start//
                List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
                string userdata = string.Empty;
                if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                {
                    if (_customerID != 0)
                    {
                        userdata = $"{_customerID}";

                    }
                }
                else
                {
                    userdata = Convert.ToString(Application.Current.Properties["UserName"]);
                }
                if (!string.IsNullOrEmpty(userdata))
                    _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                //Code for User check End//

                DateTime dtnew;
                DateTime dtend;
                DateTime dtfrom;
                DateTime dtto;
                bool ServiceTimeStatus = true;

                if (string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    var lstserviceData = _lstSelectedService.LastOrDefault();

                    if (res != null && res?.Count() > 0)
                    {
                        dtnew = Convert.ToDateTime(lstserviceData.ServiceTime.Split('-')[1]);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }

                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew > dtfrom) && (dtnew <= dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd



                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);

                            // if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                // if ((dtnew >= dtfrom) && (dtnew < dtto))

                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                        }




                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //   if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //code for reservation check end

                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {
                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    if ((dtnew > dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }
                        }
                        //code for barber reservation check end

                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End
                    }
                    else
                    {
                        dtnew = Convert.ToDateTime(lstserviceData.ServiceTime.Split('-')[1]);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                    }

                }
                else
                {

                    if (res != null && res?.Count() > 0)
                    {
                        dtnew = Convert.ToDateTime(SelectedTimeSlot);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);



                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd





                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                // if ((dtnew >== dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            // if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtend > dtfrom) && (dtend <= dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }


                        }


                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))

                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                // if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend >= dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                                }
                            }
                        }
                        //code for reservation check end

                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {

                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }

                        }

                        //code for barber reservation check end

                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End


                    }
                    else
                    {
                        dtnew = Convert.ToDateTime(SelectedTimeSlot);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //    if ((dtnew >= dtfrom) && (dtnew < dtto))
                                // { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                //  { ServiceTimeStatus = false; }
                                dtnew = dtto;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd


                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                // if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    if ((dtnew > dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //code for reservation check end
                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {

                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }

                        }

                        //code for barber reservation check end



                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //    if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    //{ ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //  if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //    if ((dtend > dtfrom) && (dtend <= dtto))
                                    //  { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End
                    }

                }



                //Need to check the end time here
                DateTime tempEndDate = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "hh:mm tt");
                DateTime LastTime = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", dtend).ToUpper(), "hh:mm tt");
                if (DateTime.Compare(tempEndDate, LastTime) > 0)
                {
                    Timeslot = string.Format("{0:hh:mm tt}", dtnew).ToUpper() + "-" + string.Format("{0:hh:mm tt}", dtend).ToUpper();
                }
                else
                {
                    Timeslot = "";
                }

                //ServiceTimeStatus = false;
                if (!ServiceTimeStatus)
                {
                    toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
                    UserDialogs.Instance.Toast(toastConfig);

                }
                if (string.IsNullOrEmpty(Timeslot))
                {
                    //resReserve

                    if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
                    {
                        toastConfig.Message = "You cannot book an appointment for this day because the salon is closed, please choose a different date.";
                        UserDialogs.Instance.Toast(toastConfig);
                        // SelectedTimeSlot = SalonOpeningTime;
                    }


                    else
                    {
                        toastConfig.Message = "The service you can not select because it already crossed the closing time (" + SalonClosingTime + ").Please change the date or time.";
                        UserDialogs.Instance.Toast(toastConfig);
                    }

                }
                return Timeslot;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string GetAvailableServiceTimeDurationbkp(string BarberId, string ServiceId, string ServiceTimeDuration)
        {

            try
            {
                string Timeslot = string.Empty;
                //var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).ToList();
                //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).ToList();

                var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

                var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();


                //Code for User check Start//
                List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
                string userdata = string.Empty;
                if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                {
                    if (_customerID != 0)
                    {
                        userdata = $"{_customerID}";

                    }
                }
                else
                {
                    userdata = Convert.ToString(Application.Current.Properties["UserName"]);
                }
                if (!string.IsNullOrEmpty(userdata))
                    _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                //Code for User check End//

                DateTime dtnew;
                DateTime dtend;
                DateTime dtfrom;
                DateTime dtto;
                bool ServiceTimeStatus = true;

                if (string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    var lstserviceData = _lstSelectedService.LastOrDefault();

                    if (res != null && res?.Count() > 0)
                    {
                        dtnew = Convert.ToDateTime(lstserviceData.ServiceTime.Split('-')[1]);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }

                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew > dtfrom) && (dtnew <= dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd



                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);

                            // if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                // if ((dtnew >= dtfrom) && (dtnew < dtto))

                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                        }




                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //   if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //code for reservation check end

                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {
                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    if ((dtnew > dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }
                        }
                        //code for barber reservation check end

                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End
                    }
                    else
                    {
                        dtnew = Convert.ToDateTime(lstserviceData.ServiceTime.Split('-')[1]);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                    }

                }
                else
                {

                    if (res != null && res?.Count() > 0)
                    {
                        dtnew = Convert.ToDateTime(SelectedTimeSlot);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);



                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                //{ ServiceTimeStatus = false; }
                                dtnew = dtto;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                //{ ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd





                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                // if ((dtnew >== dtfrom) && (dtnew < dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            // if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //if ((dtend > dtfrom) && (dtend <= dtto))
                                { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }


                        }



                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End


                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {

                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }

                        }

                        //code for barber reservation check end


                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))

                                {
                                    //match found
                                    // if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                // if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend >= dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtend >= dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                                }
                            }
                        }
                        //code for reservation check end


                    }
                    else
                    {
                        dtnew = Convert.ToDateTime(SelectedTimeSlot);

                        if (!string.IsNullOrEmpty(ServiceTimeDuration))
                            dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                        else
                            dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                        //Added to serach on the same list Start
                        foreach (var itemservicedata in _lstSelectedService)
                        {

                            dtfrom = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[0]);
                            dtto = Convert.ToDateTime(itemservicedata.ServiceTime?.Split('-')[1]);

                            //if ((dtnew > dtfrom) && (dtnew < dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //    if ((dtnew >= dtfrom) && (dtnew < dtto))
                                // { ServiceTimeStatus = false; }
                                dtnew = dtto;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            //if ((dtend > dtfrom) && (dtend < dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                //match found
                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                //  { ServiceTimeStatus = false; }
                                dtnew = dtto;

                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }
                        }

                        //Added to serach on the same listEnd





                        //Code for Same user Appointment Check Start
                        if (_lstuser != null && _lstuser.Count() > 0)
                        {
                            foreach (var itemuser in _lstuser)
                            {
                                dtfrom = Convert.ToDateTime(itemuser.From);
                                dtto = Convert.ToDateTime(itemuser.To);

                                // if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //    if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                //  if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    //    if ((dtend > dtfrom) && (dtend <= dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //Code for Same user Appointment Check End



                        //code for barber reservation check start
                        if (resbarberReserve != null && resbarberReserve.Count() > 0)
                        {

                            foreach (var itemReserveRsult in resbarberReserve)
                            {
                                dtfrom = Convert.ToDateTime(itemReserveRsult.From);
                                dtto = Convert.ToDateTime(itemReserveRsult.To);

                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    dtnew = dtto;
                                    ServiceTimeStatus = false;

                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                            }

                        }

                        //code for barber reservation check end

                        //code for reservation check start
                        if (resReserve != null && resReserve.Count() > 0)
                        {
                            foreach (var itemReserve in resReserve)
                            {

                                dtfrom = Convert.ToDateTime(itemReserve.From);
                                dtto = Convert.ToDateTime(itemReserve.To);

                                //if ((dtnew > dtfrom) && (dtnew < dtto))
                                if ((dtnew >= dtfrom) && (dtnew < dtto))
                                {
                                    //match found
                                    //if ((dtnew > dtfrom) && (dtnew < dtto))
                                    // if ((dtnew >= dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }

                                // if ((dtend > dtfrom) && (dtend < dtto))
                                if ((dtend > dtfrom) && (dtend <= dtto))
                                {
                                    //match found
                                    if ((dtnew > dtfrom) && (dtnew < dtto))
                                    { ServiceTimeStatus = false; }
                                    dtnew = dtto;


                                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                    else
                                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                                }
                            }
                        }
                        //code for reservation check end

                    }

                }



                //Need to check the end time here
                DateTime tempEndDate = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "hh:mm tt");
                DateTime LastTime = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", dtend).ToUpper(), "hh:mm tt");
                if (DateTime.Compare(tempEndDate, LastTime) > 0)
                {
                    Timeslot = string.Format("{0:hh:mm tt}", dtnew).ToUpper() + "-" + string.Format("{0:hh:mm tt}", dtend).ToUpper();
                }
                else
                {
                    Timeslot = "";
                }

                //ServiceTimeStatus = false;
                if (!ServiceTimeStatus)
                {
                    toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
                    UserDialogs.Instance.Toast(toastConfig);

                }
                if (string.IsNullOrEmpty(Timeslot))
                {
                    //resReserve

                    if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
                    {
                        toastConfig.Message = "You cannot book an appointment for this day because the salon is closed, please choose a different date.";
                        UserDialogs.Instance.Toast(toastConfig);
                        // SelectedTimeSlot = SalonOpeningTime;
                    }


                    else
                    {
                        toastConfig.Message = "The service you can not select because it already crossed the closing time (" + SalonClosingTime + ").Please change the date or time.";
                        UserDialogs.Instance.Toast(toastConfig);
                    }

                }
                return Timeslot;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public string GetAvailableServiceTimeDurationOptimized(string BarberId, string ServiceId, string ServiceTimeDuration)
        {

            try
            {
                string Timeslot = string.Empty;

                var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();
                var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();
                var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();

                //Code for User check Start//
                List<GetEngageBarberTimeSlotResponse> _lstuser = new List<GetEngageBarberTimeSlotResponse>();
                string userdata = string.Empty;
                if (int.Parse(Convert.ToString(Application.Current.Properties["UserLevel"])) == 2)
                {
                    if (_customerID != 0)
                    {
                        userdata = $"{_customerID}";

                    }
                }
                else
                {
                    userdata = Convert.ToString(Application.Current.Properties["UserName"]);
                }
                if (!string.IsNullOrEmpty(userdata))
                    _lstuser = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.UserName == userdata).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                //Code for User check End//



                var _userdata = _lstuser?.Select(x => new TimeCheckList
                {
                    BarberId = x.BarberId,
                    Date = x.Date,
                    From = x.From,
                    To = x.To,
                    AppoDate = x.AppoDate
                }) ?? Enumerable.Empty<TimeCheckList>();

                var newlistcheck = (res?.Concat(resReserve)?.Concat(resbarberReserve)?.Concat(_userdata))?
                           .GroupBy(m => new { m.BarberId, m.Date, m.From, m.To })?.Select(g => g.First())?.OrderBy(x => x.AppoDate.TimeOfDay);



                bool ServiceTimeStatus = true;


                DateTime dtnew;
                DateTime dtend;
                DateTime dtfrom;
                DateTime dtto;




                if (string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    var lstserviceData = _lstSelectedService.LastOrDefault();
                    dtnew = Convert.ToDateTime(lstserviceData.ServiceTime.Split('-')[1]);

                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                    else
                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


                    var _selfdataCheck = CheckSelfTimeList(_lstSelectedService, dtnew, dtend);
                    dtnew = _selfdataCheck.Item1;
                    dtend = _selfdataCheck.Item2;

                    //Added to serach on the Check list Star
                    if (newlistcheck != null && newlistcheck.Count() > 0)
                    {
                        foreach (var itemCheckdata in newlistcheck)
                        {


                            dtfrom = Convert.ToDateTime(itemCheckdata.From);
                            dtto = Convert.ToDateTime(itemCheckdata.To);


                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            if ((dtend > dtfrom) && (dtend <= dtto))

                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;
                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            if ((dtnew <= dtfrom) && (dtend >= dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;
                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                            }
                        }
                    }


                    //Added to serach on the Check listEnd
                }
                else
                {
                    dtnew = Convert.ToDateTime(SelectedTimeSlot);

                    if (!string.IsNullOrEmpty(ServiceTimeDuration))
                        dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                    else
                        dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);


                    var _selfdataCheck = CheckSelfTimeList(_lstSelectedService, dtnew, dtend);
                    dtnew = _selfdataCheck.Item1;
                    dtend = _selfdataCheck.Item2;


                    //Added to serach on the Check list Star
                    if (newlistcheck != null && newlistcheck.Count() > 0)
                    {
                        foreach (var itemCheckdata in newlistcheck)
                        {


                            dtfrom = Convert.ToDateTime(itemCheckdata.From);
                            dtto = Convert.ToDateTime(itemCheckdata.To);


                            //if ((dtnew >= dtfrom) && (dtnew <= dtto))
                            if ((dtnew >= dtfrom) && (dtnew < dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;


                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            if ((dtend > dtfrom) && (dtend <= dtto))

                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;
                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);

                            }

                            if ((dtnew <= dtfrom) && (dtend >= dtto))
                            {
                                //match found
                                dtnew = dtto;
                                ServiceTimeStatus = false;
                                if (!string.IsNullOrEmpty(ServiceTimeDuration))
                                    dtend = dtnew + TimeSpan.Parse(ServiceTimeDuration);
                                else
                                    dtend = dtnew + TimeSpan.Parse(CommonEL.DefaultServiceDeuarion);
                            }
                        }
                    }


                    //Added to serach on the Check listEnd
                }






                //Need to check the end time here
                DateTime tempEndDate = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "hh:mm tt");
                DateTime LastTime = UtilityEL.GetExactData(string.Format("{0:hh:mm tt}", dtend).ToUpper(), "hh:mm tt");
                if (DateTime.Compare(tempEndDate, LastTime) > 0)
                {
                    Timeslot = string.Format("{0:hh:mm tt}", dtnew).ToUpper() + "-" + string.Format("{0:hh:mm tt}", dtend).ToUpper();
                }
                else
                {
                    Timeslot = "";
                }

                //ServiceTimeStatus = false;
                if (!ServiceTimeStatus)
                {
                    toastConfig.Message = "The selected service time is changed due to unavialability of the barber at that time.";
                    UserDialogs.Instance.Toast(toastConfig);

                }
                if (string.IsNullOrEmpty(Timeslot))
                {
                    //resReserve

                    if (resReserve.Where(g => g.From == SalonOpeningTime && g.To == SalonClosingTime).Count() > 0)
                    {
                        toastConfig.Message = "You cannot book an appointment for this day because the salon is closed, please choose a different date.";
                        UserDialogs.Instance.Toast(toastConfig);
                        // SelectedTimeSlot = SalonOpeningTime;
                    }


                    else
                    {
                        toastConfig.Message = "The service you can not select because it already crossed the closing time (" + SalonClosingTime + ").Please change the date or time.";
                        UserDialogs.Instance.Toast(toastConfig);
                    }

                }
                return Timeslot;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //private async void AppoDate_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        private async void AppoDate_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {

            try
            {
                BindAppointmentDate();
                //GetBarberEngageAndSalonReserveTimeList(Convert.ToString(salonId));
                TimeSlotBinding();
                string selectedslot = string.Empty;

                if (!string.IsNullOrEmpty(SelectedTimeSlot))
                {
                    foreach (var item in _lstSelectedService)
                    {
                        item.ServiceTime = null;
                    }

                    //ServiceTimeBindIng(SelectedTimeSlot);
                    ServiceTimeBindIngNew(SelectedTimeSlot);
                    string minTimespot = setneartimeslot(GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0]);

                    if (SelectedTimeSlot != minTimespot)
                    {
                        SelectedTimeSlot = minTimespot;
                    }
                    MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == SelectedTimeSlot).FirstOrDefault();



                }
                else
                {
                    if (_servieListData?.Count() == 0)
                    {
                        DateTime tempDate = UtilityEL.GetExactData(AppointmentDate, "dd/MM/yyyy");
                        int result = DateTime.Compare(tempDate, DateTime.Now);
                        if (result > 0)
                        {
                            //await DisplayAlert("Check!", "I am Here 1", "Ok");
                            //ServiceTimeBindIng(SalonOpeningTime);
                            ServiceTimeBindIngNew(SalonOpeningTime);
                            SelectedTimeSlot = setneartimeslot(GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0]);
                            MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == SelectedTimeSlot).FirstOrDefault();


                        }
                        else
                        {
                            foreach (var item in _lstTime)
                            {
                                DateTime tempDateslot = UtilityEL.GetExactData(AppointmentDate + " " + item.TimeSlotData, "dd/MM/yyyy hh:mm tt");

                                int resultslot = DateTime.Compare(tempDateslot, DateTime.Now.AddMinutes(TimeLimit));

                                if (resultslot > 0 && String.IsNullOrEmpty(selectedslot))
                                {
                                    selectedslot = item.TimeSlotData;
                                    //break;
                                }

                            }
                            if (!String.IsNullOrEmpty(selectedslot))
                            {
                                foreach (var item in _lstSelectedService)
                                {
                                    item.ServiceTime = null;
                                }

                                //ServiceTimeBindIng(selectedslot);
                                ServiceTimeBindIngNew(selectedslot);
                                if (_lstSelectedService?.Count() > 0)
                                {
                                    string timedata = GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0];
                                    string minTimespot = setneartimeslot(timedata);
                                    // string minTimespot = setneartimeslot(GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0]);
                                    if (!string.IsNullOrEmpty(selectedslot) && !string.IsNullOrEmpty(minTimespot))
                                    {
                                        if (selectedslot != minTimespot)
                                        {
                                            selectedslot = minTimespot;
                                            SelectedTimeSlot = minTimespot;
                                        }
                                        MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == SelectedTimeSlot).FirstOrDefault();
                                    }
                                }


                            }

                            else
                            {
                                toastConfig.Message = "You can not book appointment on this date.Please select any other date.";
                                UserDialogs.Instance.Toast(toastConfig);

                            }

                        }

                    }
                    else
                    {

                        selectedslot = GetMinFromTime(_servieListData).From;
                    }

                    if (string.IsNullOrEmpty(SelectedTimeSlot))
                    {
                        MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == selectedslot).FirstOrDefault();
                        SelectedTimeSlot = ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData;

                        if (SelectedTimeSlot == null)
                        {
                            //await DisplayAlert("Check!", "I am Here 2", "Ok");
                            string settimeslot = setneartimeslot(selectedslot);

                            MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == settimeslot).FirstOrDefault();
                            SelectedTimeSlot = ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData;
                        }
                        else
                        {
                            //await DisplayAlert("Check!", "I am Here 3", "Ok");
                            string settimeslot = setneartimeslot(SelectedTimeSlot);
                            MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == settimeslot).FirstOrDefault();
                            SelectedTimeSlot = ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData;
                        }

                    }



                }

                if (MessagesListView.SelectedItem != null)
                {
                    await Task.Delay(2000);
                    int i = ((List<TimeSlot>)MessagesListView.ItemsSource).IndexOf(_lstTime.Where(x => x.TimeSlotData == ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData).FirstOrDefault());
                    MessagesListView.LayoutManager.ScrollToRowIndex(i, true);

                }

                if (_lstSelectedService != null && _lstSelectedService.Count() > 0)
                {
                    if (!ChangeTimeSlotStartTime(_lstSelectedService?.OrderBy(x => x.ServiceStarttime.TimeOfDay).LastOrDefault().ServiceTime.Split('-')[1]))
                    {
                        SelectedTimeSlot = SalonOpeningTime;
                    }
                }


                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "AppoDate_SelectionChanged");
            }


        }

        private bool ChangeTimeSlotStartTime(string minTimespot)
        {
            bool appostatus = true;
            DateTime tempEndDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonClosingTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
            DateTime tempStartDate = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(SalonOpeningTime)).ToUpper(), "dd/MM/yyyy hh:mm tt");
            DateTime appoendtime = UtilityEL.GetExactData(AppointmentDate + " " + string.Format("{0:hh:mm tt}", Convert.ToDateTime(minTimespot)).ToUpper(), "dd/MM/yyyy hh:mm tt");
            if (DateTime.Compare(tempEndDate, appoendtime) < 0)
            {
                appostatus = false;

            }

            if ((DateTime.Compare(tempEndDate, appoendtime) >= 0) && (DateTime.Compare(tempStartDate, appoendtime) >= 0))
            {
                appostatus = false;

            }

            return appostatus;
        }

        public string setneartimeslot(string nearesttime)
        {
            try
            {
                string settimeslot = string.Empty;

                foreach (var item in _lstTime)
                {
                    DateTime tempDateslot1 = UtilityEL.GetExactData(AppointmentDate + " " + item.TimeSlotData, "dd/MM/yyyy hh:mm tt");
                    DateTime tempDateslot2 = UtilityEL.GetExactData(AppointmentDate + " " + nearesttime, "dd/MM/yyyy hh:mm tt");

                    int resultslot = DateTime.Compare(tempDateslot1, tempDateslot2);
                    //if(resultslot > 0)
                    // {
                    //     break;
                    // }
                    // settimeslot = item.TimeSlotData;
                    if (resultslot <= 0)
                    {
                        settimeslot = item.TimeSlotData;

                    }


                }
                return settimeslot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private EntityLayer.AppointmentModels.Service GetMinFromTime(List<EntityLayer.AppointmentModels.Service> _lstTime)
        {
            try
            {
                List<double> _lst = new List<double>();
                List<TimeSpan> _lsts = new List<TimeSpan>();

                foreach (var item in _lstTime)
                {
                    var x = UtilityEL.GetExactData(AppointmentDate, "hh:mm tt").TimeOfDay;
                    _lsts.Add(x);
                    _lst.Add(x.TotalMinutes);
                }
                //_lst.Sort();
                var res = _lstTime[_lst.IndexOf(_lst.Min())];
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        private ServiceData GetMinFromTimeData(List<ServiceData> _lstTime)
        {
            try
            {
                List<double> _lst = new List<double>();
                List<TimeSpan> _lsts = new List<TimeSpan>();

                foreach (var item in _lstTime)
                {
                    var x = UtilityEL.GetExactData(AppointmentDate, "hh:mm tt").TimeOfDay;
                    _lsts.Add(x);
                    _lst.Add(x.TotalMinutes);
                }
                //_lst.Sort();
                var res = _lstTime[_lst.IndexOf(_lst.Min())];
                return res;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        private void RemoveService_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (IsCheckServiceEdit)
                {
                    if (_lstSelectedService?.Count > 1)
                    {
                        var serviceID = (e as TappedEventArgs).Parameter?.ToString();
                        _lstSelectedService.RemoveAll(x => x.ServiceId == serviceID);
                        if (!string.IsNullOrEmpty(SelectedTimeSlot))
                        {
                            foreach (var item in _lstSelectedService)
                            {
                                item.ServiceTime = null;
                            }

                            //ServiceTimeBindIng(SelectedTimeSlot);
                            ServiceTimeBindIngNew(SelectedTimeSlot);

                            string minTimespot = setneartimeslot(GetMinFromTimeData(_lstSelectedService).ServiceTime.Split('-')[0]);

                            if (SelectedTimeSlot != minTimespot)
                            {
                                SelectedTimeSlot = minTimespot;
                            }
                            MessagesListView.SelectedItem = ((List<TimeSlot>)MessagesListView.ItemsSource)?.Where(x => x.TimeSlotData == SelectedTimeSlot).FirstOrDefault();


                            if (MessagesListView.SelectedItem != null)
                            {
                                int i = ((List<TimeSlot>)MessagesListView.ItemsSource).IndexOf(_lstTime.Where(x => x.TimeSlotData == ((TimeSlot)MessagesListView.SelectedItem)?.TimeSlotData).FirstOrDefault());
                                MessagesListView.LayoutManager.ScrollToRowIndex(i, true);

                            }
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Alert(new AlertConfig { Message = "Sorry! You can not delete this service.", Title = "Message" });
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert(new AlertConfig { Message = "Sorry! You can not delete the service.", Title = "Message" });
                }
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "RemoveService_Tapped");
            }
        }

        private async void imgAddService_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (IsCheckServiceEdit)
                    await Navigation.PushModalAsync(new SelectService(true));
                else
                    await DisplayAlert("Message", "You can not add any service.", "Ok");
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "imgAddService_Tapped");
            }
        }

        private void btnProceed_OnClicked(object sender, EventArgs e)
        {
            try
            {
                NavigationPage.SetHasNavigationBar(this, false);
                webView_Pay.Source = $@"{CommonEL.PayUrl}?salonID={salonId}&AppointmentID={ _appointmentRequestModel.ID}&Amount={entryPayment.Value}";
                webView_Pay.IsVisible = true;
                webView_Pay.Navigating -= OnUrlRedirect;
                webView_Pay.Navigating += OnUrlRedirect;
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "btnProceed_OnClicked");
            }

        }

        private void OnUrlRedirect(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                e.Cancel = false;
                indicator.IsVisible = false;
                if (e.Url.ToLower().Contains("success"))
                {
                    Uri payURI = new Uri(e.Url);
                    string pID = HttpUtility.ParseQueryString(payURI.Query).Get("pId").Replace("'", "");
                    _appointmentRequestModel.pId = pID;
                    PaymentSuccessCallback(true);
                }
                else if (e.Url.ToLower().Contains("failure"))
                {
                    //UserDialogs.Instance.Alert(new AlertConfig { Message = e.Url.ToString(), Title = "Message" });
                    PaymentSuccessCallback(false);
                }
                else if (e.Url.ToLower().Contains("usercancelled"))
                {
                    //UserDialogs.Instance.Alert(new AlertConfig { Message = e.Url.ToString(), Title = "Message" });
                    NavigationPage.SetHasNavigationBar(this, true);
                    gdPay.IsVisible = false;
                    webView_Pay.Source = null;
                    webView_Pay.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "OnUrlRedirect");
            }
        }

        private async void PaymentSuccessCallback(bool result)
        {
            try
            {
                indicator.IsVisible = true;
                NavigationPage.SetHasNavigationBar(this, true);
                webView_Pay.IsVisible = false;
                gdPay.IsVisible = false;
                if (result)
                {
                    var res = await _appointmentServices.SetAppointment(_appointmentRequestModel);

                    if (res != null && res.StatusCode == 200)
                    {
                        var a = UserDialogs.Instance.Alert(new AlertConfig
                        {
                            Title = "Success!",
                            Message = "You have made an appointment.",
                            OkText = "Ok",
                            OnAction = () =>
                            {
                                App.Current.MainPage = new NavigationPage(new MainTabPage(true));
                            }
                        });


                    }


                }
                else
                {

                    await DisplayAlert("Failed!", "Payment failed. Please try again.", "Ok");
                }
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "PaymentSuccessCallback");
            }
        }
        private void btnClose_OnClicked(object sender, EventArgs e)
        {
            try
            {
                indicator.IsVisible = false;
                gdPay.IsVisible = false;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex, "CreateAppointment", "btnClose_OnClicked");
            }
        }

        private void EntryPayment_OnFocused(object sender, FocusEventArgs e)
        {
            try
            {
                gdPayInner.Margin = new Thickness(0, -10, 0, 0);
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "EntryPayment_OnFocused");
            }
        }

        private void EntryPayment_OnUnfocused(object sender, FocusEventArgs e)
        {
            try
            {
                gdPayInner.Margin = new Thickness(0, 0, 0, 0);
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex, "CreateAppointment", "EntryPayment_OnUnfocused");
            }
        }

    }

    public class TimeSlot
    {
        public string TimeSlotData { get; set; }
        public Color TemplateBackground { get; set; }
        public Color TimeTextColor { get; set; }
    }

    public class ServiceData
    {
        public string ServiceName { get; set; }
        public string BarberName { get; set; }

        public string ServiceId { get; set; }


        public string BarberId { get; set; }

        public string ServiceTime { get; set; }


        public double ServicePrice { get; set; }

        public string ServicePriceData { get; set; }

        public string ServiceDuration { get; set; }

        public DateTime ServiceStarttime { get; set; }
    }

}