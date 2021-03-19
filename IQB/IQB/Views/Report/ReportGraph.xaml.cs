using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportGraph : ContentPage
    {
        public ObservableCollection<ReportTypeModel> ReportBarbersTypeList = new ObservableCollection<ReportTypeModel>();
        ObservableCollection<ReportTypeModel> ReportSalonsTypeList = new ObservableCollection<ReportTypeModel>();

        public ReportGraph()
        {
            InitializeComponent();
            ReportBarbersTypeList.Add(new ReportTypeModel() { ReportTypeName = "Customers Served" });
            ReportBarbersTypeList.Add(new ReportTypeModel() { ReportTypeName = "Average Cut Time" });
            ReportBarbersTypeList.Add(new ReportTypeModel() { ReportTypeName = "Queuing Method" });

            ReportBarberTypeListView.ItemsSource = ReportBarbersTypeList;

            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Cancellation Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Total Cut Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Salon Queuing Method" });
            // ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Busy Hours Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Booked Appointment Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Appointment Cancellation Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Rescheduled Appointment Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Completed Appointment Report" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Monthly Booked Amount" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Monthly Cancelled Amount" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Monthly Rescheduled Amount" });
            ReportSalonsTypeList.Add(new ReportTypeModel() { ReportTypeName = "Monthly Completed Amount" });

            ReportSalonTypeListView.ItemsSource = ReportSalonsTypeList;
        }


        public class ReportTypeModel
        {
            public string ReportTypeName { get; set; }
        }

        private async void ReportsItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void NewListItemTapped(object sender, EventArgs e)
        {
            if (Application.Current.Properties["ModuleAvailability"].ToString() != "0")
            {
                if (Application.Current.Properties["ModuleAvailability"].ToString() == "1")
                {
                    if((((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text.Contains("Appointment") || (((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text.Contains("Amount"))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
                    }
                    else
                    {
                        await Navigation.PushAsync(new ReportGraphDetail((((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text));
                    }
                }
                if (Application.Current.Properties["ModuleAvailability"].ToString() == "2")
                {
                    if ((((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text.Contains("Queuing"))
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "The module is not available for this salon!!", "OK");
                    }
                    else
                    {
                        await Navigation.PushAsync(new ReportGraphDetail((((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text));
                    }
                }
            }
            else
            {
                await Navigation.PushAsync(new ReportGraphDetail((((sender as StackLayout).Children[1] as StackLayout).Children[0] as Label).Text));
            }

        }
    }
}