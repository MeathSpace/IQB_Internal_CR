using IQB.EntityLayer.ReportModels;
using IQB.IQBServices.ReportServices;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportGraphDetail : ContentPage
    {
        ReportResponse obj = new ReportResponse();
        string QueryString, ReportType;
        int IsAdmin;
        string NewFlag;
        ColumnSeries columnSeries;

        public ReportGraphDetail(string selectedGraphType)
        {
            InitializeComponent();
            this.Title = selectedGraphType;
            StartDatePicker.Date = DateTime.Now.Date;
            switch (selectedGraphType)
            {
                case "Customers Served":
                    {
                        ChartTitle.Text = "Number of Cuts";
                        FirstColSeries.Label = "Customers (in numbers)";
                        LoadGraphdata("NumberOfCuts", 2);
                        break;
                    }

                case "Average Cut Time":
                    {
                        ChartTitle.Text = "Average Cut Time";
                        FirstColSeries.Label = "Cut Time (in minutes)";
                        LoadGraphdata("AverageCutTime", 2);
                        break;
                    }

                case "Queuing Method":
                    {
                        ChartTitle.Text = "Queuing Method";
                        FirstColSeries.Label = "Desktop";
                        LoadGraphdata("QueuingMethods", 2);
                        break;
                    }

                case "Cancellation Report":
                    {
                        ChartTitle.Text = "No of Cancellations";
                        FirstColSeries.Label = "Cancellations (in numbers)";
                        LoadGraphdata("NoOfCancellation", 1);
                        break;
                    }

                case "Total Cut Report":
                    {
                        ChartTitle.Text = "Customers Served";
                        FirstColSeries.Label = "Total Cuts (in numbers)";
                        LoadGraphdata("CustomersServed", 1);
                        break;
                    }

                case "Salon Queuing Method":
                    {
                        ChartTitle.Text = "Queuing Method";
                        FirstColSeries.Label = "Desktop";
                        LoadGraphdata("SalonQueuingMethods", 1);
                        break;
                    }

                case "Busy Hours Report":
                    {
                        LoadGraphdata("NumberOfCuts", 1);
                        break;
                    }

                case "Booked Appointment Report":
                    {
                        LoadGraphdata("AppointmentBarberWise", 3);
                        NewFlag = "booked";
                        FirstColSeries.Label = "Booked Appointments";
                        break;
                    }

                case "Appointment Cancellation Report":
                    {
                        LoadGraphdata("AppointmentBarberWise", 3);
                        NewFlag = "canceled";
                        FirstColSeries.Label = "Cancelled Appointments";
                        break;
                    }

                case "Rescheduled Appointment Report":
                    {
                        LoadGraphdata("AppointmentBarberWise", 3);
                        NewFlag = "rescheduled";
                        FirstColSeries.Label = "Rescheduled Appointments";
                        break;
                    }

                case "Completed Appointment Report":
                    {
                        LoadGraphdata("AppointmentBarberWise", 3);
                        NewFlag = "completed";
                        FirstColSeries.Label = "Completed Appointments";
                        break;
                    }

                case "Monthly Booked Amount":
                    {
                        LoadGraphdata("PaymentMonthWise", 4);
                        NewFlag = "booked";
                        break;
                    }

                case "Monthly Cancelled Amount":
                    {
                        LoadGraphdata("PaymentMonthWise", 4);
                        NewFlag = "canceled";
                        break;
                    }

                case "Monthly Rescheduled Amount":
                    {
                        LoadGraphdata("PaymentMonthWise", 4);
                        NewFlag = "rescheduled";
                        break;
                    }

                case "Monthly Completed Amount":
                    {
                        LoadGraphdata("PaymentMonthWise", 4);
                        NewFlag = "completed";
                        break;
                    }
            }
        }

        private void Handle_SelectionChanged(object sender, Syncfusion.XForms.TabView.SelectionChangedEventArgs e)
        {
            if (SfReportTabContainer.SelectedIndex == 0)
            {
                StartDatePicker.Date = EndDatePicker.Date;
            }

            if (SfReportTabContainer.SelectedIndex == 1)
            {
                StartDatePicker.Date = EndDatePicker.Date.AddDays(-7);
            }

            if (SfReportTabContainer.SelectedIndex == 2)
            {
                StartDatePicker.Date = EndDatePicker.Date.AddMonths(-1);
            }
        }

        private void DateChanged(object sender, DateChangedEventArgs e)
        {
            LoadGraphdata(ReportType, IsAdmin);
        }

        private async void LoadGraphdata(string reportType, int isAdmin)
        {
            if (StartDatePicker.Date <= EndDatePicker.Date)
            {
                ReportType = reportType;
                IsAdmin = isAdmin;

                if (ReportType == "NoOfCancellation" || ReportType == "CustomersServed" || ReportType == "SalonQueuingMethods") /// Customers Served  is total cut report
                {
                    QueryString = ($"ReportType={ReportType}&StartDate={StartDatePicker.Date.Year}-" +
                     $"{StartDatePicker.Date.Month}-{StartDatePicker.Date.Day}&EndDate={EndDatePicker.Date.Year}-" +
                     $"{EndDatePicker.Date.Month}-{EndDatePicker.Date.Day}&SalonId=" +
                     $"{Application.Current.Properties["_SalonId"]}&Flag=Salon");
                }

                else
                {
                    if (IsAdmin == 1)
                    {
                        QueryString = ($"ReportType={ReportType}&StartDate={StartDatePicker.Date.Year}-" +
                            $"{StartDatePicker.Date.Month}-{StartDatePicker.Date.Day}&EndDate={EndDatePicker.Date.Year}" +
                            $"-{EndDatePicker.Date.Month}-{EndDatePicker.Date.Day}&UserId=" +
                            $"{Application.Current.Properties["UserName"]}&Flag=Salon");
                    }

                    if (IsAdmin == 2)
                    {
                        QueryString = ($"ReportType={ReportType}&StartDate={StartDatePicker.Date.Year}-" +
                      $"{StartDatePicker.Date.Month}-{StartDatePicker.Date.Day}&EndDate={EndDatePicker.Date.Year}-" +
                      $"{EndDatePicker.Date.Month}-{EndDatePicker.Date.Day}&SalonId=" +
                      $"{Application.Current.Properties["_SalonId"]}&Flag=Barber");

                    }

                    if (IsAdmin == 3 || IsAdmin == 4)
                    {
                        QueryString = ($"ReportType={ReportType}&StartDate={StartDatePicker.Date.Year}-" +
                     $"{StartDatePicker.Date.Month}-{StartDatePicker.Date.Day}&EndDate={EndDatePicker.Date.Year}-" +
                     $"{EndDatePicker.Date.Month}-{EndDatePicker.Date.Day}&SalonId=" +
                     $"{Application.Current.Properties["_SalonId"]}&Flag={NewFlag}");
                    }

                }

                using (ReportService obj1 = new ReportService())
                {
                    obj = await obj1.GetReport(QueryString, isAdmin);
                    MySyncChart.IsVisible = true;
                    if (obj.StatusCode == 201)
                    {
                        await DisplayAlert("Alert", "No Records Found!", "OK");
                    }

                    else
                    {
                        if (obj.Response == null)
                        {
                            MySyncChart.IsVisible = false;
                            await DisplayAlert("Alert", obj.StatusMessage, "OK");
                        }
                        else
                        {
                            this.BindingContext = obj;

                            if (ReportType == "QueuingMethods" || ReportType == "SalonQueuingMethods")
                            {
                                MySyncChart.Series.Remove(columnSeries);

                                columnSeries = new ColumnSeries()
                                {
                                    ItemsSource = obj.Response,
                                    XBindingPath = "xN",
                                    YBindingPath = "y1"
                                };

                                columnSeries.Label = "Mobile";
                                MySyncChart.Series.Add(columnSeries);
                            }
                        }

                    }

                }
            }

            else
            {
                await DisplayAlert("Alert", "Start Date cannot be more than End Date", "OK");
            }

        }
    }


}


