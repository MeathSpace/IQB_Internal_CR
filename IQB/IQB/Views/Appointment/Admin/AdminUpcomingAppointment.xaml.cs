using Acr.UserDialogs;
using IQB.EntityLayer.AppointmentModels;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.SalonModels;
using IQB.IQBServices.AppointmentServices;
using IQB.Views.Appointment.Admin;
using IQB.Views.Appointment.Customer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Appointment.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdminUpcomingAppointment : ContentPage
	{
        public string ReasonForCancel { get; set; }
        string AppointmentId = string.Empty;
        public ObservableCollection<string> Items { get; set; }
        private AppointmentServices objAppointmentService = new AppointmentServices();

        private List<EntityLayer.AppointmentModels.Appointment> appointmentList { get; set; } = new List<EntityLayer.AppointmentModels.Appointment>();

        private List<string> statuses = new List<string>();

        public AdminUpcomingAppointment()
        {
            InitializeComponent();
            //if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
            //{
            //    frmAppoAdd.IsVisible = true;
            //}
            //else
            //{
            //    frmAppoAdd.IsVisible = false;
            //}
            BindStatus();
            lblStartDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            lblEndDate.Text = DateTime.Now.AddDays(30).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            AppointmentList.RefreshCommand = new Command<string>((s) => GetAppointmentList(Convert.ToString(Application.Current.Properties["UserName"]), Convert.ToString(Application.Current.Properties["UserLevel"]), Convert.ToString(Application.Current.Properties["_SalonId"])));
            //MyListView.ItemsSource = Items;
            AppointmentList.BeginRefresh();
            indicator.IsVisible = false;
        }

        public AdminUpcomingAppointment(EntityLayer.AppointmentModels.Appointment objApp = null)
        {

            BindStatus();
            InitializeComponent();
            //if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
            //{
            //    frmAppoAdd.IsVisible = true;
            //}
            //else
            //{
            //    frmAppoAdd.IsVisible = false;
            //}
            if (objApp != null)
            {
                //appointmentList = new List<EntityLayer.AppointmentModels.Appointment>();
                appointmentList?.Add(objApp);
            }
            lblStartDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            lblEndDate.Text = DateTime.Now.AddDays(30).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            AppointmentList.RefreshCommand = new Command<string>((s) => GetAppointmentList(Convert.ToString(Application.Current.Properties["UserName"]), Convert.ToString(Application.Current.Properties["UserLevel"]), Convert.ToString(Application.Current.Properties["_SalonId"])));

            AppointmentList.BeginRefresh();
            indicator.IsVisible = false;
            //MyListView.ItemsSource = Items;
        }

        private void BindStatus()
        {
            statuses.Add("Booked");
            statuses.Add("Cancelled");
            statuses.Add("Completed");
            statuses.Add("ALL");
            pickerStatus.ItemsSource = statuses;
        }
        //private void ContentPage_Appearing(object sender, EventArgs e)
        //{
        //    AppointmentList.BeginRefresh();
        //}

        private void SelectStatus_Tapped(object sender, EventArgs e)
        {
            pickerStatus.Focus();
            pickerStatus.SelectedIndexChanged += (s, ev) =>
            {
                //selectedCountry = (Country)pickerStatus.SelectedItem;
                lblStatus.Text = ((string)pickerStatus.SelectedItem);
                lblStatus.TextColor = Color.FromHex("#FFFFFF");

                BindWithDateFilter();



            };


        }
        private async void GetAppointmentList(string userName, string userLevel, string salonID)
        {
            try
            {

                //appointmentList = new List<EntityLayer.AppointmentModels.Appointment>();
                //var salonListResponse = await objSalonService.GetSalonList(userName);
                var appointmentListResponse = await objAppointmentService.GetUserAppointmentMapList(userName, userLevel);
                appointmentList.Clear();
                if (appointmentListResponse?.StatusCode == 200)
                {
                    lblSwipe.IsVisible = true;
                //    lblNoRecord.IsVisible = false;

                    //*******************Code for barber Filter*******************
                    //var newList = appointmentListResponse.Response.Appointment.Where(sublist => sublist.Services.Any(item => item.BarberId == "347")).ToList();
                    //int u = 0;
                    //if (newList!=null)
                    //u = newList.Count();
                    //*******************Code for barber Filter*******************

                    if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
                    {
                        if (appointmentListResponse?.Response?.Appointment?.Where(x => x.IsReserved == "false" && x.SalonID == salonID).Count() > 0)
                            appointmentList.AddRange(appointmentListResponse.Response.Appointment.Where(x => x.IsReserved == "false" && x.SalonID == salonID));
                    }
                    else
                    {
                        appointmentList.AddRange(appointmentListResponse.Response.Appointment.Where(x => x.SalonID == salonID));
                    }

                    AppointmentList.ItemsSource = null;

                    //AppointmentList.ItemsSource = appointmentList.OrderByDescending(x=>x.AppoDate.Date);
                    BindWithDateFilter();
                    AppointmentList.EndRefresh();
                }
                else
                {
                    lblSwipe.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    AppointmentList.EndRefresh();
                }


            }
            catch (Exception ex)
            {

            }

        }



        //async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    await App.Current.MainPage.DisplayAlert("Item Tapped", "An item was tapped.", "OK");

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                AppointmentList.ItemsSource = appointmentList.OrderByDescending(x => x.AppoDate.Date);
            }

            else
            {
                AppointmentList.ItemsSource = appointmentList.Where(x => x.Status.StartsWith(e.NewTextValue)).OrderByDescending(x => x.AppoDate.Date);
            }
        }

        //private async void AddAppointment_Tapped(object sender, EventArgs e)
        //{
        //    ///Checking of Appointment Settings
        //    ///
        //    // var resultDay = Task.Run(async () => await objAppointmentService.GetAppoinmentSettingsBysalonId(_getAppointmentByIdResponseModel.Response.SalonID)).Result;
        //    var resultDay = Task.Run(async () => await objAppointmentService.GetAppoinmentSettingsBysalonId(Convert.ToString(Application.Current.Properties["_SalonId"]))).Result;

        //    if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
        //    {
        //        indicator.IsVisible = true;
        //        if (resultDay != null && resultDay.StatusCode == 200)
        //        {
        //            await Navigation.PushAsync(new SelectService());
        //        }
        //        else
        //        {
        //            await App.Current.MainPage.DisplayAlert("Sorry!", "The appointment settings is not configured for this salon.", "Ok");
        //        }


        //        indicator.IsVisible = false;
        //    }
        //    else
        //    {
        //        indicator.IsVisible = true;
        //        if (resultDay != null && resultDay.StatusCode == 200)
        //        {
        //            await Navigation.PushAsync(new ManageReservation());
        //        }
        //        else
        //        {
        //            await App.Current.MainPage.DisplayAlert("Sorry!", "The appointment settings is not configured for this salon.", "Ok");
        //        }
        //        indicator.IsVisible = false;
        //    }
        //}

        //private async void EditAppointment_Tapped(object sender, EventArgs e)
        //{
        //    //var salonID = (e as TappedEventArgs).Parameter?.ToString();
        //    //await Navigation.PushAsync(new CreateSalon(salonID, true));
        //}

        private async void DeleteAppointment_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            AppointmentId = (e as TappedEventArgs).Parameter?.ToString();
            //if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
            //{
                //var AppointmentID = (e as TappedEventArgs).Parameter?.ToString();

                var status = appointmentList.Where(x => x.AppointmentID == AppointmentId).FirstOrDefault().Status;

                if (status == "Booked")
                {
                    var appointmentDetailDataResponse = await objAppointmentService.GetAppointmentById(AppointmentId);
                    //CheckCancelScheduleStatus
                    if (appointmentDetailDataResponse.Response.IsReserved == "true" || CheckCancelScheduleStatus(appointmentDetailDataResponse?.Response?.Date,
                      appointmentDetailDataResponse?.Response?.Services))
                    {
                        var answer = await App.Current.MainPage.DisplayAlert("Cancel", "Do you want to cancel the Appointment?", "Yes", "No");
                        if (answer)
                        {
                            UserDialogs.Instance.Prompt(new PromptConfig
                            {
                                Title = "Please enter the Reason",
                                Text = ReasonForCancel,
                                OnAction = ChangeLINK

                            });

                            // var appointmentDeleteDataResponse = await objAppointmentService.DeleteAppoinmentStatus(Convert.ToInt32(AppointmentID),"");
                            //AppointmentList.BeginRefresh();
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Message", "You can not cancel the appointment as it is within the 24 hours", "Ok");
                    }

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Message", "You cannot delete this appointment.", "Ok");
                }
        //    }
            //else
            //{
            //    var Isreserved = appointmentList.Where(x => x.AppointmentID == AppointmentId).FirstOrDefault().IsReserved;
            //    if (Isreserved == "false")
            //        await App.Current.MainPage.DisplayAlert("Message", "You can not change the status of appointment.", "Ok");
            //    else
            //    {
            //        var answer = await App.Current.MainPage.DisplayAlert("Cancel", "Do you want to cancel the Reservation?", "Yes", "No");
            //        if (answer)
            //        {
            //            UserDialogs.Instance.Prompt(new PromptConfig
            //            {
            //                Title = "Please enter the Reason",
            //                Text = ReasonForCancel,
            //                OnAction = ChangeLINK

            //            });

            //            // var appointmentDeleteDataResponse = await objAppointmentService.DeleteAppoinmentStatus(Convert.ToInt32(AppointmentID),"");
            //            //AppointmentList.BeginRefresh();
            //        }
            //    }
            //}
            indicator.IsVisible = false;
        }

        private async void ChangeLINK(PromptResult res)
        {
            if (res.Ok)
            {
                indicator.IsVisible = true;
                var appointmentDeleteDataResponse = await objAppointmentService.DeleteAppoinmentStatus(Convert.ToInt32(AppointmentId), res.Text);
                indicator.IsVisible = false;
                AppointmentList.BeginRefresh();

            }
        }

        private async void ViewAppointment_Tapped(object sender, EventArgs e)
        {
            var AppointmentID = (e as TappedEventArgs)?.Parameter?.ToString();
            indicator.IsVisible = true;
            var appointmentDetailDataResponse = await objAppointmentService.GetAppointmentById(AppointmentID);

            if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
            {
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

            else
            {
                if (appointmentDetailDataResponse?.Response?.IsReserved == "true")
                {
                    var appointment = appointmentList.Where(x => x.AppointmentID == AppointmentID).FirstOrDefault();
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = DateTime.Now;

                    foreach (var itemdata in appointment.Services)
                    {
                        var serviceStartTime = UtilityEL.GetExactData(itemdata.From, "hh:mm tt").TimeOfDay;
                        var serviceEndTime = UtilityEL.GetExactData(itemdata.To, "hh:mm tt").TimeOfDay;

                        startDateTime = UtilityEL.GetExactData(appointment.Date, "dd/MM/yyyy").Add(serviceStartTime);
                        endDateTime = UtilityEL.GetExactData(appointment.Date, "dd/MM/yyyy").Add(serviceEndTime);

                    }
                    AppointmentModel objAppointmentModel = new AppointmentModel();
                    objAppointmentModel.AppointmentId = appointment.AppointmentID;
                    objAppointmentModel.AppointmentStatus = appointment.Status;
                    objAppointmentModel.AppointmentUpdatedBy = appointment.UpdatedBy != null ? appointment.UpdatedBy : "";
                    objAppointmentModel.AppointmentUpdatedTime = appointment.UpdatedDateTime != null ? appointment.UpdatedDateTime : "";
                    objAppointmentModel.AppointmentStartTime = startDateTime;
                    objAppointmentModel.AppointmentEndTime = endDateTime;
                    objAppointmentModel.AppointmentSubject = $"{appointment.ReserveReason}";
                    objAppointmentModel.IsReservedAppointment = true;
                    //objAppointmentModel.AppointmentColor = (Color)App.CurrentApp.Resources["grey_700"];
                    objAppointmentModel.AppointmentLocation = "";
                    objAppointmentModel.Services = appointment.Services;

                    if (objAppointmentModel != null)
                        await Navigation.PushAsync(new ManageReservation(objAppointmentModel));
                }
                else
                {
                    await Navigation.PushAsync(new AppointmentDetails(appointmentDetailDataResponse?.Response));
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

        private bool CheckCancelScheduleStatus(string date, Service[] services)
        {
            bool checkstatus = true;
            try
            {
                foreach (var item in services)
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
                return checkstatus;
            }

        }

        //private bool CheckCancelScheduleStatus(string date, Service[] services)
        //{
        //    try
        //    {
        //        bool checkstatus = true;
        //        foreach (var item in services)
        //        {
        //            DateTime tempDate = UtilityEL.GetExactData(date + " " + item.From, "dd/MM/yyyy hh:mm tt");

        //            if (DateTime.Compare(tempDate, DateTime.Now.AddMinutes(2000) <= 0)
        //            {
        //                checkstatus = false;
        //                break;
        //            }
        //        }
        //        return checkstatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        private void imgCloseHint_Tapped(object sender, EventArgs e)
        {
            frmHintList.IsVisible = false;
            frmHintList.HeightRequest = 0;
            frmHintList.Margin = new Thickness(0);
        }

        private void PickerStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            //var s = (string)pickerStatus.ItemsSource[selectedIndex];
            if (selectedIndex != -1)
            {
                AppointmentList.ItemsSource = appointmentList.Where(x => x.Status == (string)pickerStatus.ItemsSource[selectedIndex]).OrderByDescending(x => x.AppoDate.Date);
            }
            else
            {
                AppointmentList.ItemsSource = appointmentList.OrderByDescending(x => x.AppoDate.Date);
            }


        }

        private async void SelectStartDate_Tapped(object sender, EventArgs e)
        {
            var StartDate = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig
            {
                IsCancellable = false,
                OkText = "Confirm",
                CancelText = "",

            });
            if (StartDate.Ok)
            {

                lblStartDate.Text = StartDate.SelectedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }

        }

        private async void SelectEndDate_Tapped(object sender, EventArgs e)
        {
            var EndDate = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig
            {
                IsCancellable = false,
                OkText = "Confirm",
                CancelText = "",


            });
            if (EndDate.Ok)
            {

                lblEndDate.Text = EndDate.SelectedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
        }

        private async void SearchButton_Tapped(object sender, EventArgs e)
        {

            BindWithDateFilter();

        }

        private async void BindWithDateFilter()
        {
            //if (Convert.ToString(Application.Current.Properties["UserLevel"]) == "0")
            //{
            if (!string.IsNullOrEmpty(lblStartDate.Text) && !string.IsNullOrEmpty(lblEndDate.Text) && (lblStartDate.Text != "From Date") && (lblEndDate.Text != "To Date"))
            {
                var startSearchdate = UtilityEL.GetExactData(lblStartDate.Text, "dd/MM/yyyy");
                var endSearchdate = UtilityEL.GetExactData(lblEndDate.Text, "dd/MM/yyyy");
                //var x = appointmentList.Where(s => (s.AppoDate.Date >= startSearchdate.Date));
                //AppointmentList.ItemsSource = appointmentList.Where(s => (s.AppoDate.Date >= startSearchdate.Date)&&(s.AppoDate.Date <= endSearchdate.Date)).OrderByDescending(x => x.AppoDate.Date);


                if (!string.IsNullOrEmpty(lblStatus.Text) && (lblStatus.Text != "Status"))
                {

                    AppointmentList.ItemsSource = lblStatus.Text != "ALL" ? appointmentList.Where(x => x.Status == lblStatus.Text && x.AppoDate.Date >= startSearchdate.Date && x.AppoDate.Date <= endSearchdate.Date).OrderByDescending(x => x.AppoDate.Date) : appointmentList.Where(s => (s.AppoDate.Date >= startSearchdate.Date) && (s.AppoDate.Date <= endSearchdate.Date)).OrderByDescending(x => x.AppoDate.Date);


                }

                else
                {
                    AppointmentList.ItemsSource = appointmentList.Where(s => (s.AppoDate.Date >= startSearchdate.Date) && (s.AppoDate.Date <= endSearchdate.Date)).OrderByDescending(x => x.AppoDate.Date);

                }




            }
            else
            {
                // await App.Current.MainPage.DisplayAlert("Error", "Please fill date fields.", "Ok");
                if (!string.IsNullOrEmpty(lblStatus.Text))
                {

                    AppointmentList.ItemsSource = lblStatus.Text != "ALL" ? appointmentList.Where(x => x.Status == lblStatus.Text).OrderByDescending(x => x.AppoDate.Date) : appointmentList.OrderByDescending(x => x.AppoDate.Date);

                }

                else
                {
                    AppointmentList.ItemsSource = appointmentList.OrderByDescending(x => x.AppoDate.Date);
                }

            }
            //}
            //else
            //{

            //}

        }
    }
}
