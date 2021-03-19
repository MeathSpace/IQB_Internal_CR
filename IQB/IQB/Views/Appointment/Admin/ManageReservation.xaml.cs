using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.AppointmentServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.TabPagers;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageReservation : ContentPage
    {
        private TimeSpan StartTime = TimeSpan.Zero;
        private TimeSpan EndTime = TimeSpan.Zero;

        string AppointmentDate = string.Empty;
        string SelectedTimeSlot = string.Empty;
        string AppointmentId = string.Empty;

        private AppointmentServices _appointmentServices = new AppointmentServices();
        //private AppointmentServices objAppointmentService = new AppointmentServices();
        private GetEngageBarberTimeSlotResponseModel _getEngageBarberTimeSlotResponseModel = new GetEngageBarberTimeSlotResponseModel();
        private GetEngageReserveTimeBySalonidModel _getEngageReserveTimeBySalonidModel = new GetEngageReserveTimeBySalonidModel();

        ApiResult res = new ApiResult();
        int salonId = 0;
        private string BarberId { get; set; }
        string SelectedBarberId;

        public string SalonOpeningTime = string.Empty;
        public string SalonClosingTime = string.Empty;
        public string SalonTimeSlotSpanTime = string.Empty;
        public double SalonAdvancePayPercentange = 100D;
        private string WalkinCustomerName = string.Empty;
        private bool IsCheckServiceEdit = true;

        private int TimeLimit;
        private Int64 AdvanceReScheduleLimit;

        private int DaysLimit;

        public ManageReservation(AppointmentModel appointment = null)
        {
            InitializeComponent();
            ObservableCollection<object> todaycollection = new ObservableCollection<object>();

            //SetbarberId();
            var resultDay = Task.Run(async () => await _appointmentServices.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            if (resultDay != null && resultDay.StatusCode == 200)
            {
                DaysLimit = Convert.ToInt32(resultDay.Response.NumberOfDaysAfterAppdate);
                TimeLimit = Convert.ToInt32(resultDay.Response.AdvanceBookingTime);
                AdvanceReScheduleLimit = Convert.ToInt64(resultDay.Response.ReScheduleEnableTime);
                SalonOpeningTime = !string.IsNullOrEmpty(resultDay.Response.SalonStartTime) ? resultDay.Response.SalonStartTime : CommonEL.TimeSlotStartTime;
                SalonClosingTime = !string.IsNullOrEmpty(resultDay.Response.SalonEndTime) ? resultDay.Response.SalonEndTime : CommonEL.TimeSlotEndTime;

            }

            if (appointment != null)
            {
                AppointmentId = appointment.AppointmentId;
                //select reservation date
                todaycollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(appointment.AppointmentStartTime.Date.Month)
                    .Substring(0, 3));
                if (appointment.AppointmentStartTime.Date.Day < 10)
                    todaycollection.Add("0" + appointment.AppointmentStartTime.Date.Day);
                else
                    todaycollection.Add(appointment.AppointmentStartTime.Date.Day.ToString());
                todaycollection.Add(appointment.AppointmentStartTime.Date.Year.ToString());
                //entryReasonForReservation.Text = appointment.AppointmentSubject;
                if (appointment.AppointmentSubject.Contains("\n"))
                    entryReasonForReservation.Text = appointment.AppointmentSubject.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[1];
                else
                    entryReasonForReservation.Text = appointment.AppointmentSubject;
                lblStartTime.Text = appointment.AppointmentStartTime.ToString("hh:mm tt");
                lblEndTime.Text = appointment.AppointmentEndTime.ToString("hh:mm tt");
                StartTime = appointment.AppointmentStartTime.TimeOfDay;
                EndTime = appointment.AppointmentEndTime.TimeOfDay;
                picker.SelectedItem = todaycollection;




                var selectedDate = ((IEnumerable)picker.SelectedItem).Cast<object>().ToList();
                var startDateString = $"{selectedDate[0]} {selectedDate[1]},{selectedDate[2]}";
                var endDateString = $"{selectedDate[0]} {selectedDate[1]},{selectedDate[2]}";
                var startDateTime = DateTime.ParseExact(startDateString, "MMM dd,yyyy", CultureInfo.InvariantCulture).Add(StartTime);
                AppointmentDate = startDateTime.ToString("dd/MM/yyyy");

                //Get The Barber ID at time of edit start

                if (appointment.Services?.Count() > 0)
                    BarberId = appointment.Services.ToList().FirstOrDefault().BarberId;
                else
                    BarberId = "0";
                //Get The Barber ID at time of edit end

                //DateTime tempAppoDate = UtilityEL.GetExactData(AppointmentDate, "dd/MM/yyyy");



                if (appointment.AppointmentStatus == "Cancelled")
                {
                    PageHeadingLabel.Text = "View Reservation";
                    btnReserve.IsVisible = false;
                    picker.IsEnabled = false;
                    StartTimeButton.IsVisible = false;
                    EndTimeButton.IsVisible = false;
                    lblStartTime.IsEnabled = false;
                    lblEndTime.IsEnabled = false;

                    entryReasonForReservation.IsEnabled = false;
                    StartDateGrid.IsEnabled = false;
                    EndDateGrid.IsEnabled = false;
                }
                else
                {
                    DateTime tempAppoDate = UtilityEL.GetExactData(AppointmentDate + " " + lblEndTime.Text, "dd/MM/yyyy hh:mm tt");
                    int result = DateTime.Compare(tempAppoDate, DateTime.Now);
                    //int result = DateTime.Compare(tempAppoDate, DateTime.Now.Date);
                    btnReserve.IsVisible = true;
                    PageHeadingLabel.Text = "Update Reservation";
                    btnReserve.Text = "UPDATE";
                    if (result < 0)
                    {
                        PageHeadingLabel.Text = "View Reservation";
                        btnReserve.IsVisible = false;
                        picker.IsEnabled = false;
                        StartTimeButton.IsVisible = false;
                        EndTimeButton.IsVisible = false;
                        lblStartTime.IsEnabled = false;
                        lblEndTime.IsEnabled = false;
                        entryReasonForReservation.IsEnabled = false;

                        StartDateGrid.IsEnabled = false;
                        EndDateGrid.IsEnabled = false;

                    }

                }

                //if (result < 0)
                //{
                //    PageHeadingLabel.Text = "View Reservation";
                //    btnReserve.IsVisible = false;
                //    picker.IsEnabled = false;
                //    StartTimeButton.IsVisible = false;
                //    EndTimeButton.IsVisible = false;
                //    lblStartTime.IsEnabled = false;
                //    lblEndTime.IsEnabled = false;


                //    StartDateGrid.IsEnabled = false;
                //    EndDateGrid.IsEnabled = false;

                //}


            }

            else
            {
                SetbarberId();
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

            if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "2")
            {
                SfProfileTabContainer.IsVisible = true;

                if (PageHeadingLabel.Text == "Update Reservation" || PageHeadingLabel.Text == "View Reservation")
                {
                    if (BarberId != "0")
                    {
                    //    SfProfileTabContainer.SelectedIndex = 1;
                        if (appointment.Services?.Count() > 0)
                        BarberTab.Title = appointment.Services.ToList().FirstOrDefault().BarberName;
                    }
                    SfProfileTabContainer.IsEnabled = false;
                }
                else
                {
                    GetBarberListForSelection();
                }
            }
        }

        private async void GetBarberListForSelection()
        {
            ApiResult res = new ApiResult();
            using (BarberSalonService obj = new BarberSalonService())
            {
                res = await obj.GetAppointmentAvlBarberBySalonId((Convert.ToInt32(Application.Current.Properties["_SalonId"])));
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberModel.BarberReturnResult> result = UtilityEL.ToModelList<BarberModel.BarberReturnResult>(res.Response);

                BarberListView.ItemsSource = result;
            }
        }

        private async void SetbarberId()
        {
            if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "2")

                if (SfProfileTabContainer.SelectedIndex == 1 && SelectedBarberId != "")
                {
                    BarberId = SelectedBarberId;
                }
                else
                {
                    BarberId = "0";
                }

            else
            {

                var res = await _appointmentServices.GetBarberIdByEmailId(Convert.ToString(Application.Current.Properties["_UserEmail"]), Convert.ToInt32(Application.Current.Properties["_SalonId"]));


                if (res != null && res.StatusCode == 200)
                {

                    BarberId = res.Response?.BarberId;
                }
            }
        }

        private async void SelectStartTime_Tapped(object sender, EventArgs e)
        {
            var startTime = await UserDialogs.Instance.TimePromptAsync(new TimePromptConfig
            {
                IsCancellable = false,
                OkText = "Confirm",
                CancelText = "",
                Use24HourClock = false,
                SelectedTime = (!string.IsNullOrEmpty(lblStartTime.Text) && lblStartTime.Text != "Start Time")
                    ? DateTime.ParseExact(lblStartTime.Text, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay
                    : DateTime.Now.TimeOfDay
            });
            if (startTime.Ok)
            {
                var serviceStartTime = UtilityEL.GetExactData(DateTime.Today.Add(startTime.SelectedTime).ToString("hh:mm tt"), "hh:mm tt").TimeOfDay;
                var SalonOpenTime = UtilityEL.GetExactData(SalonOpeningTime, "hh:mm tt").TimeOfDay;
                var SalonEndTime = UtilityEL.GetExactData(SalonClosingTime, "hh:mm tt").TimeOfDay;
                int x = TimeSpan.Compare(serviceStartTime, SalonOpenTime);
                int y = TimeSpan.Compare(serviceStartTime, SalonEndTime);
                if (x >= 0 && y < 0)
                {
                    StartTime = startTime.SelectedTime;
                    DateTime time = DateTime.Today.Add(StartTime);
                    lblStartTime.Text = time.ToString("hh:mm tt");
                }
                else
                {
                    await DisplayAlert("Error!", "Your selected time is not between salon opening time (" + SalonOpeningTime + ") and closing time (" + SalonClosingTime + ").Please change the selected time", "Ok");
                }

            }
        }

        private async void SelectEndTime_Tapped(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(lblEndTime.Text) && (lblStartTime.Text != "Start Time"))
            {
                var endTime = await UserDialogs.Instance.TimePromptAsync(new TimePromptConfig
                {
                    IsCancellable = false,
                    OkText = "Confirm",
                    CancelText = "",
                    Use24HourClock = false,
                    SelectedTime = (!string.IsNullOrEmpty(lblEndTime.Text) && lblEndTime.Text != "End Time")
                   ? DateTime.ParseExact(lblEndTime.Text, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay
                   : DateTime.Now.TimeOfDay
                });
                if (endTime.Ok)
                {

                    var serviceEndTime = UtilityEL.GetExactData(DateTime.Today.Add(endTime.SelectedTime).ToString("hh:mm tt"), "hh:mm tt").TimeOfDay;
                    var SalonOpenTime = UtilityEL.GetExactData(SalonOpeningTime, "hh:mm tt").TimeOfDay;
                    var SalonEndTime = UtilityEL.GetExactData(SalonClosingTime, "hh:mm tt").TimeOfDay;

                    var salonStartTime = UtilityEL.GetExactData(lblStartTime.Text, "hh:mm tt").TimeOfDay;

                    int starttimecheck = TimeSpan.Compare(serviceEndTime, salonStartTime);
                    if (starttimecheck > 0)
                    {
                        int x = TimeSpan.Compare(serviceEndTime, SalonOpenTime);
                        int y = TimeSpan.Compare(serviceEndTime, SalonEndTime);

                        if (x > 0 && y <= 0)
                        {
                            EndTime = endTime.SelectedTime;
                            DateTime time = DateTime.Today.Add(EndTime);
                            lblEndTime.Text = time.ToString("hh:mm tt");
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Your selected time is not between salon opening time (" + SalonOpeningTime + ") and closing time (" + SalonClosingTime + ").Please change the selected time", "Ok");

                        }
                    }
                    else
                    {
                        await DisplayAlert("Error!", "The end time must be grater that start time", "Ok");
                    }



                }
            }
            else
            {
                await DisplayAlert("Error!", "Please select the start time first", "Ok");

            }


        }

        public void GetBarberEngageAndSalonReserveTimeList(string SalonId)
        {
            #region BaberEngageList

            // _getEngageBarberTimeSlotResponseModel = await objAppointmentService.GetEngageBarberTimeSlot(Convert.ToString(salonId));
            _getEngageBarberTimeSlotResponseModel = Task.Run(async () => await _appointmentServices.GetEngageBarberTimeSlot(SalonId)).Result;
            #endregion


            #region SalonReserveTime

            //_getEngageReserveTimeBySalonidModel = await objAppointmentService.GetEngageReserveTimeBySalonid(Convert.ToString(salonId));
            _getEngageReserveTimeBySalonidModel = Task.Run(async () => await _appointmentServices.GetEngageReserveTimeBySalonid(SalonId)).Result;
            #endregion
        }


        public bool getReseveCheckStatus(DateTime dtstart, DateTime dtend)
        {
            try
            {
                GetBarberEngageAndSalonReserveTimeList($"{salonId}");
                bool Reservestatus = true;
                DateTime dtfrom;
                DateTime dtto;

                if (BarberId != "0")
                {
                    var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                    //var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0").OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                    var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == "0" && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

                    var resbarberReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.BarberId == BarberId && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();

                    if (res?.Count() > 0)
                    {
                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtstart >= dtfrom) && (dtstart <= dtto))
                            if ((dtstart >= dtfrom) && (dtstart < dtto))
                            {
                                Reservestatus = false;

                            }

                            //if ((dtend >= dtfrom) && (dtend <= dtto))
                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                Reservestatus = false;
                            }
                        }
                    }

                    if (resReserve?.Count() > 0)
                    {
                        foreach (var itemEngageRsult in resReserve)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtstart >= dtfrom) && (dtstart <= dtto))
                            //{
                            //    Reservestatus = false;

                            //}

                            //if ((dtend >= dtfrom) && (dtend <= dtto))
                            //{
                            //    Reservestatus = false;
                            //}

                            if ((dtstart >= dtfrom) && (dtstart < dtto))
                            {
                                Reservestatus = false;

                            }


                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                Reservestatus = false;
                            }
                        }
                    }


                    if (resbarberReserve?.Count() > 0)
                    {
                        foreach (var itemEngageRsult in resbarberReserve)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtstart >= dtfrom) && (dtstart <= dtto))
                            //{
                            //    Reservestatus = false;

                            //}

                            //if ((dtend >= dtfrom) && (dtend <= dtto))
                            //{
                            //    Reservestatus = false;
                            //}

                            if ((dtstart >= dtfrom) && (dtstart < dtto))
                            {
                                Reservestatus = false;

                            }


                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                Reservestatus = false;
                            }
                        }
                    }
                }
                else
                {
                    //var res = Task.Run(async () => await _appointmentServices.GetEngageBarberTimeSlot($"{salonId}")).Result?.Response?.Where(x => x.Date == AppointmentDate);
                    // var resReserve = Task.Run(async () => await _appointmentServices.GetEngageReserveTimeBySalonid($"{salonId}")).Result?.Response?.Where(x => x.Date == AppointmentDate && x.AppointmentId != AppointmentId);

                    var res = _getEngageBarberTimeSlotResponseModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/')).OrderBy(x => x.AppoDate.TimeOfDay).ToList();
                    var resReserve = _getEngageReserveTimeBySalonidModel.Response?.Where(x => x.Date == AppointmentDate.Replace('-', '/') && x.AppointmentId != AppointmentId).OrderBy(x => x.AppoDate.TimeOfDay).ToList();


                    if (res?.Count() > 0)
                    {
                        foreach (var itemEngageRsult in res)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtstart >= dtfrom) && (dtstart <= dtto))
                            //{
                            //    Reservestatus = false;

                            //}

                            //if ((dtend >= dtfrom) && (dtend <= dtto))
                            //{
                            //    Reservestatus = false;
                            //}

                            if ((dtstart >= dtfrom) && (dtstart < dtto))
                            {
                                Reservestatus = false;

                            }


                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                Reservestatus = false;
                            }
                        }
                    }

                    if (resReserve?.Count() > 0)
                    {
                        foreach (var itemEngageRsult in resReserve)
                        {
                            dtfrom = Convert.ToDateTime(itemEngageRsult.From);
                            dtto = Convert.ToDateTime(itemEngageRsult.To);
                            //if ((dtstart >= dtfrom) && (dtstart <= dtto))
                            //{
                            //    Reservestatus = false;

                            //}

                            //if ((dtend >= dtfrom) && (dtend <= dtto))
                            //{
                            //    Reservestatus = false;
                            //}

                            if ((dtstart >= dtfrom) && (dtstart < dtto))
                            {
                                Reservestatus = false;

                            }


                            if ((dtend > dtfrom) && (dtend <= dtto))
                            {
                                Reservestatus = false;
                            }
                        }
                    }

                }




                return Reservestatus;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public int CheckPresentDate()
        {
            //var selectedTimeSlot = !string.IsNullOrEmpty(lblStartTime.Text) ? Convert.ToDateTime(lblStartTime.Text).ToString("HH:mm") : DateTime.Now.ToString("HH:mm");
            DateTime tempAppoDate = UtilityEL.GetExactData(AppointmentDate + " " + Convert.ToDateTime(lblStartTime.Text).ToString("HH:mm"), "dd/MM/yyyy HH:mm");
            // DateTime tempDate = UtilityEL.GetExactData(AppointmentDate + " " + selectedTimeSlot, "dd/MM/yyyy HH:mm");
            //int result = DateTime.Compare(tempAppoDate, DateTime.Now.AddMinutes(TimeLimit));
            int result = DateTime.Compare(tempAppoDate, DateTime.Now);

            return result;
        }

        private async void BtnReserve_OnClicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            if (!string.IsNullOrEmpty(lblStartTime.Text) && !string.IsNullOrEmpty(lblEndTime.Text) && (lblStartTime.Text != "Start Time") && (lblEndTime.Text != "End Time"))
            {
                using (SalonViewModel objSalon = new SalonViewModel())
                {
                    salonId = objSalon.GetSalonId();
                }

                //GetBarberEngageAndSalonReserveTimeList(Convert.ToString(salonId));

                var selectedDate = ((IEnumerable)picker.SelectedItem).Cast<object>().ToList();
                var startDateString = $"{selectedDate[0]} {selectedDate[1]},{selectedDate[2]}";
                var endDateString = $"{selectedDate[0]} {selectedDate[1]},{selectedDate[2]}";
                var startDateTime = DateTime.ParseExact(startDateString, "MMM dd,yyyy", CultureInfo.InvariantCulture).Add(StartTime);
                var endDateTime = DateTime.ParseExact(endDateString, "MMM dd,yyyy", CultureInfo.InvariantCulture).Add(EndTime);
                var reason = entryReasonForReservation.Text;


                //AppointmentDate = startDateTime.ToString("dd/MM/yyyy hh:mm:tt");
                AppointmentDate = startDateTime.ToString("dd/MM/yyyy");

                int result = CheckPresentDate();

                AppointmentRequestModel _appointmentRequestModel = new AppointmentRequestModel();
                if (CheckPresentDate() >= 0)
                {


                    var sD = DateTime.Now;


                    // var eD = UtilityEL.GetExactData(AppointmentDate, "dd/MM/yyyy hh:mm:tt");
                    var eD = UtilityEL.GetExactData(AppointmentDate + " " + Convert.ToDateTime(lblStartTime.Text).ToString("HH:mm"), "dd/MM/yyyy HH:mm");



                    var thirtyMin = TimeSpan.FromDays(DaysLimit > 0 ? DaysLimit : 30).TotalMinutes;
                    var diff = Math.Abs(eD.Subtract(sD).TotalMinutes);

                    if (diff < thirtyMin)
                    {


                        if (getReseveCheckStatus(Convert.ToDateTime(string.Format("{0:hh:mm tt}", startDateTime).ToUpper()), Convert.ToDateTime(string.Format("{0:hh:mm tt}", endDateTime).ToUpper())))
                        {
                            _appointmentRequestModel.ID = _appointmentRequestModel.ID = AppointmentId != null ? AppointmentId : "0";
                            _appointmentRequestModel.SalonID = Convert.ToString(salonId);
                            //_appointmentRequestModel.Date = startDateTime.ToShortDateString();
                            _appointmentRequestModel.Date = AppointmentDate;
                            _appointmentRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                            //_appointmentRequestModel.Status = "1";
                            _appointmentRequestModel.Status = "0";
                            _appointmentRequestModel.IsReserved = "Y";
                            _appointmentRequestModel.ReserveReason = entryReasonForReservation.Text;
                            _appointmentRequestModel.WalkInCustomerName = "";

                            List<ServiceRequest> _lstServiceRequest = new List<ServiceRequest>();


                            _lstServiceRequest.Add(new ServiceRequest
                            {
                                // BarberId = "0",
                                BarberId = BarberId,
                                ServiceId = "0",
                                From = string.Format("{0:hh:mm tt}", startDateTime).ToUpper(),
                                To = string.Format("{0:hh:mm tt}", endDateTime).ToUpper(),
                            });

                            _appointmentRequestModel.Services = _lstServiceRequest;
                            var res = await _appointmentServices.SetAppointment(_appointmentRequestModel);
                            if (res?.StatusCode == 200)
                            {
                                await DisplayAlert("Success!", "Reservation Successful", "Ok");
                                App.Current.MainPage = new NavigationPage(new MainTabPage(true));
                            }
                            else
                                await DisplayAlert("Failure!", "Reservation failed. Please try again.", "Ok");
                        }

                        else
                            await DisplayAlert("Error!", "You can not reserve on this date/time.Please select any other date/time.", "Ok");

                    }
                    else
                    {
                        await DisplayAlert("Error!", "You can not make reservation more than " + DaysLimit.ToString() + " days advance", "Ok");
                    }




                }
                else
                {
                    UserDialogs.Instance.Alert(new AlertConfig { Message = "Please change the date/time.You can not make reservation on the this selected date/time.", Title = "Alert!" });
                }
            }

            else
            {
                UserDialogs.Instance.Alert(new AlertConfig { Message = "Please fill the start time and end time.", Title = "Alert!" });
            }

            indicator.IsVisible = false;
        }

        private async void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (e.Index == 1 && PageHeadingLabel.Text != "Update Reservation")
            {
                BarberGrid.IsVisible = true;
                // Show Barber Grid
            }
            //else
            //{
            //    await DisplayAlert("Alert", "You cannot change this selection!!", "OK");
            //}
        }

        private void BarberSelectListviewTapped(object sender, ItemTappedEventArgs e)
        {
            BarberGrid.IsVisible = false;
            SelectedBarberId = (e.Item as BarberModel.BarberReturnResult).BarberId.ToString();
            BarberId = SelectedBarberId;
            BarberTab.Title = (e.Item as BarberModel.BarberReturnResult).BarberName.ToString();
            (sender as ListView).SelectedItem = null;
        }
    }
}