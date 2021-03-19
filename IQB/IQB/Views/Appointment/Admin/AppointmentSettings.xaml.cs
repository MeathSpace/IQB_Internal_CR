using Acr.UserDialogs;
using IQB.EntityLayer.AppointmentModels;
using IQB.IQBServices.AppointmentServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentSettings : ContentPage
    {
        private TimeSpan StartTime = TimeSpan.Zero;
        private TimeSpan EndTime = TimeSpan.Zero;

        string AppointmentSettingsId = string.Empty;
        public string numofDays { get; set; }

        public string AdvanceBookTime { get; set; }

        public string RescheduleTime { get; set; }

        public string TimeSlotSpanTime { get; set; }

        private AppointmentServices _appointmentServices = new AppointmentServices();
        public AppointmentSettings ()
		{
			InitializeComponent ();


            GetAllSettings();

        }

        private async void GetAllSettings()
        {
            indicator.IsVisible = true;
            var result = await _appointmentServices.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]));
            if (result != null && result.StatusCode == 200)
            {
                AppointmentSettingsId = Convert.ToString(result.Response.AppoinmentSettingsID);
                //entryNumofDyas.Text = result.Response.NumberOfDaysAfterAppdate;
                entryNumofDays.Value = result.Response?.NumberOfDaysAfterAppdate;
                entryAdvanceBkTime.Value= result.Response?.AdvanceBookingTime;
                //entryReschudleTime.Value = result.Response?.ReScheduleEnableTime;
                entryTSSlotTime.Value = result.Response?.TimeSlotGap;
                lblStartTime.Text= result.Response?.SalonStartTime;
                lblEndTime.Text = result.Response?.SalonEndTime;
                entryAdvancePayment.Value= result.Response?.AdvancePaymentPercentage;
                StartTime = Convert.ToDateTime(result.Response?.SalonStartTime).TimeOfDay;
                EndTime = Convert.ToDateTime(result.Response?.SalonEndTime).TimeOfDay;
            }
            indicator.IsVisible = false;
        }
        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
           

            numofDays = Convert.ToString(e.Value);
        }

        private async void btnAddClicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            if (lblStartTime.Text!= "Start Time" && lblEndTime.Text!= "End Time" && int.Parse($"{entryNumofDays.Value}")>0 && int.Parse($"{entryAdvanceBkTime.Value}") > 0  && int.Parse($"{entryTSSlotTime.Value}") > 0)
            {
                //&& int.Parse($"{entryReschudleTime.Value}") > 0
                var timediff = DateTime.Today.Add(EndTime).CompareTo(DateTime.Today.Add(StartTime));
                if (timediff>0)
                {
                    AppoinmentSettingsRequestModel _appoinmentSettingsRequestModel = new AppoinmentSettingsRequestModel();
                    _appoinmentSettingsRequestModel.Id = (!string.IsNullOrEmpty(AppointmentSettingsId)) ? AppointmentSettingsId : "";
                    _appoinmentSettingsRequestModel.SalonId = Convert.ToString(Application.Current.Properties["_SalonId"]);
                    _appoinmentSettingsRequestModel.UserName = Convert.ToString(Application.Current.Properties["UserName"]);
                    //_appoinmentSettingsRequestModel.NumberOfDaysAfterAppdate = numofDays;
                    //_appoinmentSettingsRequestModel.AdvanceBookingTime = AdvanceBookTime;
                    //_appoinmentSettingsRequestModel.ReScheduleEnableTime = RescheduleTime;
                    //_appoinmentSettingsRequestModel.TimeSlotGap = TimeSlotSpanTime;

                    _appoinmentSettingsRequestModel.NumberOfDaysAfterAppdate = $"{entryNumofDays.Value}";
                    _appoinmentSettingsRequestModel.AdvanceBookingTime = $"{entryAdvanceBkTime.Value}";
                    //_appoinmentSettingsRequestModel.ReScheduleEnableTime = $"{entryReschudleTime.Value}";
                    _appoinmentSettingsRequestModel.ReScheduleEnableTime = "1440";
                    _appoinmentSettingsRequestModel.TimeSlotGap = $"{entryTSSlotTime.Value}";

                    _appoinmentSettingsRequestModel.SalonStartTime = lblStartTime.Text;
                    _appoinmentSettingsRequestModel.SalonEndTime = lblEndTime.Text;
                    _appoinmentSettingsRequestModel.AdvancePaymentPercentage = $"{entryAdvancePayment.Value}";
                    var result = await _appointmentServices.SetAppointmentSettings(_appoinmentSettingsRequestModel);

                    if (result.Response.AppoinmentSettingsID > 0)
                    {
                        await DisplayAlert("Success!", "You have successfully added/updated the appointment settings", "Ok");
                    } 
                }
                else
                {
                    await DisplayAlert("Error!", "Start time can't be greater than or equals End Time!", "Ok");
                }

            }
            else
            {
                string validationMessage = string.Empty;
                if (int.Parse($"{entryNumofDays.Value}")<=0)
                {
                    validationMessage = "Please fill up the Future Booking";
                }
                else if (int.Parse($"{entryAdvanceBkTime.Value}") <= 0)
                {
                    validationMessage = "Please fill up the Advance Booking";
                }

                //else if (int.Parse($"{entryReschudleTime.Value}") <= 0)
                //{
                //    validationMessage = "Please fill up the Rescheduling";
                //}

                else if (int.Parse($"{entryTSSlotTime.Value}") <= 0)
                {
                    validationMessage = "Please fill up the TimeSlot Interval";
                }

                else if (lblStartTime.Text != "Start Time" && lblStartTime.Text != null)
                {
                    validationMessage = "Please fill up the Business Hours Start Time";
                }

                else if (lblEndTime.Text != "End Time" && lblEndTime.Text != null)
                {
                    validationMessage = "Please fill up the Business Hours End Time";
                }

                if (!string.IsNullOrEmpty(validationMessage))
                {
                    App.toastConfig.Message = validationMessage;
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
                // await DisplayAlert("Error!", "Please fill up the all fields with valid values!", "Ok");
            }
            indicator.IsVisible = false;

        }

        private void AdvanceBkTime_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            AdvanceBookTime = Convert.ToString(e.Value);
        }

        private void ReschudleTime_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            RescheduleTime = Convert.ToString(e.Value);

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
                StartTime = startTime.SelectedTime;
                DateTime time = DateTime.Today.Add(StartTime);
                lblStartTime.Text = time.ToString("hh:mm tt");
            }
        }

        private async  void SelectEndTime_Tapped(object sender, EventArgs e)
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
                EndTime = endTime.SelectedTime;
                DateTime time = DateTime.Today.Add(EndTime);
                lblEndTime.Text = time.ToString("hh:mm tt");
            }
        }

        private async void FBTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Future Booking (in Days)", "Set the maximum period (in Days) from today onwards to schedule an appointment.", "Ok");
        }

        private async void ABTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Advance Booking (in Mins)", "Set the maximum time period (in Minutes) to schedule an appointment on the preferred day.", "Ok");
        }

        private async void ReTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Rescheduling (in Mins)", "Set the maximum time period (in Minutes) before an appointment can be rescheduled or cancelled.", "Ok");
        }

        private async void BHTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Business Hours", "Set the Salon's daily business hours with respect to Salon's opening time and Salon's closing time.", "Ok");
        }

        private async void TSTapped(object sender, EventArgs e)
        {
            await DisplayAlert("TimeSlot Interval (in Mins)", "Set the Salon's Timeslot interval time.", "Ok");
        }

        private void entryTSSlotTime_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            TimeSlotSpanTime = Convert.ToString(e.Value);
        }

        private async void APTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Advance Payment (in %)", "Set an advance amount in (%) that users can enter to pay to book an appointment.", "Ok");
        }
    }
}