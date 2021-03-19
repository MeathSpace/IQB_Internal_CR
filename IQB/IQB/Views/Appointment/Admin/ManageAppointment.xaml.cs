using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.Common;
using IQB.IQBServices.AppointmentServices;
using IQB.Views.Appointment.Customer;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageAppointment : ContentView
    {
        public ObservableCollection<AppointmentModel> appointments { get; set; } = new ObservableCollection<AppointmentModel>();
        private AppointmentServices objAppointmentService = new AppointmentServices();
        bool isbusy = false;
        private List<EntityLayer.AppointmentModels.Appointment> appointmentList { get; set; } = new List<EntityLayer.AppointmentModels.Appointment>();
        public string SalonOpeningTime = string.Empty;
        public string SalonClosingTime = string.Empty;

        public ManageAppointment()
        {
            InitializeComponent();


            if (Device.RuntimePlatform == Device.iOS)
            {
                AddApptBtn.CornerRadius = 20;
                AddReserBtn.CornerRadius = 20;
                UpcomAppBtn.CornerRadius = 20;
            }
            try
            {
                GetAppointmentList(Convert.ToString(Application.Current.Properties["UserName"]), Convert.ToString(Application.Current.Properties["UserLevel"]));
                indicator.IsVisible = false;
                MessagingCenter.Subscribe<string>(this, "AppointmentStatusSelected", (statuses) =>
                {
                    if (!string.IsNullOrEmpty(statuses))
                    {
                        var Statuses = statuses.Split(',');
                        FilerApointmentList(Statuses[0]);
                    }
                });
            }
            catch (Exception ex)
            {

            }

            schedule.DataSource = appointments;
        }

        private void FilerApointmentList(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                schedule.DataSource = status != "All" ? appointments.Where(x => x.AppointmentStatus == status) : appointments;
            }
            else
            {
                schedule.DataSource = appointments;
            }

        }

        private async void CalenderBusinessHourSet()
        {
            var resultDay = Task.Run(async () => await objAppointmentService.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            if (resultDay != null && resultDay.StatusCode == 200)
            {

                SalonOpeningTime = !string.IsNullOrEmpty(resultDay.Response.SalonStartTime) ? resultDay.Response.SalonStartTime : CommonEL.TimeSlotStartTime;
                SalonClosingTime = !string.IsNullOrEmpty(resultDay.Response.SalonEndTime) ? resultDay.Response.SalonEndTime : CommonEL.TimeSlotEndTime;
                DayViewSettings dayViewSettings = new DayViewSettings();
               // DayLabelSettings dayLabelSettings = new DayLabelSettings();
                //dayLabelSettings.TimeFormat = "h tt";
                //dayLabelSettings.TimeLabelColor = Color.White;
                //dayLabelSettings.TimeLabelSize = 12;
                dayViewSettings.WorkStartHour = Convert.ToDateTime(SalonOpeningTime).TimeOfDay.TotalHours;
                dayViewSettings.WorkEndHour = Convert.ToDateTime(SalonClosingTime).TimeOfDay.TotalHours;
                //dayViewSettings.DayLabelSettings = dayLabelSettings;
                schedule.DayViewSettings = dayViewSettings;



               
            }
        }
        private async void GetAppointmentList(string userName, string userLevel)
        {
            try
            {

                //appointmentList = new List<EntityLayer.AppointmentModels.Appointment>();
                //var salonListResponse = await objSalonService.GetSalonList(userName);
                var appointmentListResponse = await objAppointmentService.GetUserAppointmentMapList(userName, userLevel);
                appointmentList.Clear();
                if (appointmentListResponse?.StatusCode == 200)
                {

                    if (appointmentListResponse?.Response?.Appointment?.Count() > 0)
                        appointmentList.AddRange(appointmentListResponse.Response.Appointment);

                    foreach (var item in appointmentList)
                    {
                        if (item.Services != null)
                        {
                            foreach (var itemdata in item.Services)
                            {
                                var serviceStartTime = UtilityEL.GetExactData(itemdata.From, "hh:mm tt").TimeOfDay;
                                var serviceEndTime = UtilityEL.GetExactData(itemdata.To, "hh:mm tt").TimeOfDay;

                                var startDateTime = UtilityEL.GetExactData(item.Date, "dd/MM/yyyy").Add(serviceStartTime);
                                var endDateTime = UtilityEL.GetExactData(item.Date, "dd/MM/yyyy").Add(serviceEndTime);

                                //var appointmentSubject = string.Empty;
                                //if (!bool.Parse(item.IsReserved))
                                //{
                                //    appointmentSubject =
                                //        $"Status: {item.Status}{Environment.NewLine}{startDateTime:hh:mm tt}-{endDateTime:hh:mm tt}{Environment.NewLine}{(!string.IsNullOrEmpty(item.WalkInCustomerName) ? item.WalkInCustomerName : item.CustomerName)}{Environment.NewLine}{itemdata.BarberName}:{itemdata.ServiceName}{Environment.NewLine}£{itemdata.Price}";
                                //    if (!string.IsNullOrEmpty(item.UpdatedBy))
                                //        appointmentSubject =
                                //            $"{appointmentSubject}{Environment.NewLine}Updated By:{item.UpdatedBy}{Environment.NewLine}Updated Time:{item.UpdatedDateTime}";
                                //}
                                //else
                                //    appointmentSubject = $"{startDateTime:hh:mm tt}-{endDateTime:hh:mm tt}{Environment.NewLine}{item.ReserveReason}";
                                var appointmentSubject = string.Empty;
                                if (!bool.Parse(item.IsReserved))
                                {
                                    appointmentSubject =
                                        $"{(!string.IsNullOrEmpty(item.WalkInCustomerName) ? item.WalkInCustomerName : item.CustomerName)}{Environment.NewLine}{itemdata.BarberName}{Environment.NewLine}{itemdata.ServiceName}";
                                    //if (!string.IsNullOrEmpty(item.UpdatedBy))
                                    //    appointmentSubject =
                                    //        $"{appointmentSubject}{Environment.NewLine}Updated By:{item.UpdatedBy}{Environment.NewLine}Updated Time:{item.UpdatedDateTime}";
                                }
                                else
                                    appointmentSubject = $"{item.ReserveReason}";

                                appointments?.Add(new AppointmentModel
                                {
                                    AppointmentId = item.AppointmentID,
                                    AppointmentStatus = item.Status,
                                    AppointmentUpdatedBy = item.UpdatedBy != null ? item.UpdatedBy : "",
                                    AppointmentUpdatedTime = item.UpdatedDateTime != null ? item.UpdatedDateTime : "",
                                    AppointmentStartTime = startDateTime,
                                    AppointmentEndTime = endDateTime,
                                    //AppointmentSubject = item.IsReserved == "true" ? item.ReserveReason : ((!string.IsNullOrEmpty(item.WalkInCustomerName)) ? item.WalkInCustomerName : item.CustomerName) + "," + itemdata.ServiceName + "," + itemdata.Price + "£" + (!string.IsNullOrEmpty(item.UpdatedBy) ? ",Updated By:" + item.UpdatedBy : "") + ((item.UpdatedBy != null && item.UpdatedBy != "") ? ",Updated Time:" + item.UpdatedDateTime : ""),
                                    AppointmentSubject = appointmentSubject,
                                    IsReservedAppointment = item.IsReserved == "true" ? true : false,
                                   // AppointmentColor = item.IsReserved == "true" ? (Color)App.CurrentApp.Resources["grey_700"] : (Color)App.CurrentApp.Resources["AppPrimary"],
                                    AppointmentColor = item.IsReserved == "true" ? (Color)App.CurrentApp.Resources["grey_700"] : item.Status=="Cancelled"? (Color)App.CurrentApp.Resources["ValidationBackground"] : (Color)App.CurrentApp.Resources["AppPrimary"],
                                    AppointmentLocation = "",
                                    Services = item.Services
                                });



                            }
                        }
                    }

                }
               
                
                CalenderBusinessHourSet();
                // schedule.DataSource = appointments;

            }
            catch (Exception ex)
            {

            }

        }

        //public DateTime GetExactData(string datetime, string format)
        //{
        //    DateTime tempDate = DateTime.Now;
        //    //

        //    if (Device.RuntimePlatform == Device.iOS)
        //    {
        //        tempDate = DateTime.Parse(datetime);
        //    }
        //    if (Device.RuntimePlatform == Device.Android)
        //    {
        //        tempDate = DateTime.ParseExact(datetime, format, System.Globalization.CultureInfo.InvariantCulture);
        //    }

        //    return tempDate;
        //}

        private async void Schedule_OnCellTapped(object sender, CellTappedEventArgs e)
        {
            dynamic selectedAppointment = e.Appointment;
            indicator.IsVisible = true;
            if (selectedAppointment != null)
            {
                //navigate to edit appointment
                if (selectedAppointment.IsReservedAppointment)
                    await Navigation.PushAsync(new ManageReservation(selectedAppointment));
                else
                {
                    var AppointmentID = selectedAppointment.AppointmentId;
                    var appointmentDetailDataResponse = await objAppointmentService.GetAppointmentById(AppointmentID);
                    var status = appointmentDetailDataResponse?.Response?.Status;

                    if (status == "Booked")
                    {
                        if (CheckAdvanceScheduleStatus(appointmentDetailDataResponse?.Response?.Date,
                        appointmentDetailDataResponse?.Response?.Services))
                            await Navigation.PushAsync(new CreateAppointment(appointmentDetailDataResponse));
                        else
                            await Navigation.PushAsync(new AppointmentDetails(appointmentDetailDataResponse?.Response));
                    }
                    else
                    {
                        await Navigation.PushAsync(new AppointmentDetails(appointmentDetailDataResponse?.Response));
                    }

                        
                }
            }
            indicator.IsVisible = false;
        }
        private bool CheckAdvanceScheduleStatus(string date, Service[] services)
        {
            try
            {
                bool checkstatus = true;
                foreach (var item in services)
                {
                    DateTime tempDate = UtilityEL.GetExactData(date + " " + item.From, "dd/MM/yyyy hh:mm tt");

                    if (DateTime.Compare(tempDate, DateTime.Now) <= 0)
                    {
                        checkstatus = false;
                        break;
                    }
                }
                return checkstatus;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void Filter_OnTapped(object sender, EventArgs e)
        {

        }

        private void Schedule_OnHeaderTapped(object sender, HeaderTappedEventArgs e)
        {
            GoToDate(e.DateTime);
        }

        private void Schedule_OnViewHeaderTapped(object sender, ViewHeaderTappedEventArgs e)
        {
            GoToDate(e.DateTime);
        }

        private async void GoToDate(DateTime dt)
        {
            var result = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig()
            {
                OkText = "Go to date",
                SelectedDate = dt
            });
            if (result.Ok)
                schedule.MoveToDate = result.SelectedDate;
        }

        private async void AddAppointment_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            var resultDay = Task.Run(async () => await objAppointmentService.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            if (resultDay != null && resultDay.StatusCode == 200)
            {
                await Navigation.PushAsync(new Clients());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Sorry!", "This appointment settings is not available for this salon.You need to configure the appointment settings of this salon.", "Ok");
            }

            
            indicator.IsVisible = false;
        }

        private async void AddReservation_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            var resultDay = Task.Run(async () => await objAppointmentService.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;
            if (resultDay != null && resultDay.StatusCode == 200)
            {
                await Navigation.PushAsync(new ManageReservation());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Sorry!", "This appointment service is not available for this salon.You need to configure the appointment settings of this salon.", "Ok");
            }
            indicator.IsVisible = false;
            
        }

        private async void OpenBookingOptionsTapped(object sender, EventArgs e)
        {
            if (!isbusy)
            {
                isbusy = true;
                if (AddReservationStack.Opacity == 0)
                {
                    SelectServicesStack.IsVisible = true;
                    schedule.Opacity = 0.1;
                    SelectBookingsFrame.Source = "resource://IQB.Resources.Image.cross.svg";
                    await UpcomingAppointmentStack.FadeTo(1, 300, Easing.CubicIn);
                    await AddAppointmentStack.FadeTo(1, 300, Easing.CubicIn);
                    await AddReservationStack.FadeTo(1, 300, Easing.CubicIn);
                }




                else
                {
                    SelectBookingsFrame.Source = "resource://IQB.Resources.Image.addIcon.svg";
                    await AddReservationStack.FadeTo(0, 300, Easing.CubicOut);
                    await AddAppointmentStack.FadeTo(0, 300, Easing.CubicOut);
                    await UpcomingAppointmentStack.FadeTo(0, 300, Easing.CubicOut);
                    SelectServicesStack.IsVisible = false;
                    schedule.Opacity = 1;
                }
                isbusy = false;
            }
        }

        private async void AddAppointmentFilter_Tapped(object sender, EventArgs e)
        {


            //await Navigation.PushModalAsync(new AppointmentFilter());

            //SelectBookingsFrame.Source = "resource://IQB.Resources.Image.addIcon.svg";
            //await AddReservationStack.FadeTo(0, 300, Easing.CubicOut);
            //await AddAppointmentStack.FadeTo(0, 300, Easing.CubicOut);
            //await AddAppointmentFilter.FadeTo(1, 300, Easing.CubicOut);
            //SelectServicesStack.IsVisible = false;
            //schedule.Opacity = 1;
        }

        private async void UpcomingAppointmentFilter_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            await Navigation.PushAsync(new AdminUpcomingAppointment());
            indicator.IsVisible = false;
        }
    }
}