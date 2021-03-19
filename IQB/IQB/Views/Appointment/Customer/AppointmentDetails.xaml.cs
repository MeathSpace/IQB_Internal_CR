using IQB.EntityLayer.AppointmentModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.AppointmentServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Service = IQB.EntityLayer.AppointmentModels.Service;

namespace IQB.Views.Appointment.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentDetails : ContentPage
    {
        private readonly GetAppointmentByIdResponse _appointmentDetails;
        private BarberSalonService objBarberSalonService = new BarberSalonService();
        private AppointmentServices objAppointmentService = new AppointmentServices();
        private List<ServiceModel> result = null;
        private List<ServiceData> _lstSelectedService = new List<ServiceData>();

        public AppointmentDetails(GetAppointmentByIdResponse appointmentDetails)
        {
            InitializeComponent();
            _appointmentDetails = appointmentDetails;
            PopulateDetails();
        }

        private void PopulateDetails()
        {
            txtCustomerName.Text = !string.IsNullOrEmpty(_appointmentDetails?.WalkInCustomerName) ? _appointmentDetails.WalkInCustomerName : _appointmentDetails?.CustomerName;
            txtAppointmentDate.Text = _appointmentDetails?.Date;
            txtAppointmentStatus.Text = _appointmentDetails?.Status;

            if (!string.IsNullOrEmpty(_appointmentDetails?.StatusChangeReason))
            {
                lblReason.Text = _appointmentDetails?.Status == "Cancelled" ? "Cancellation Reason" : "Reason for Rescheduling";
                txtAppointmentReason.Text = _appointmentDetails?.StatusChangeReason;
                lblReason.IsVisible = true;
                txtAppointmentReason.IsVisible = true;
                bdrReason.IsVisible = true;
            }
            else
            {
                lblReason.IsVisible = false;
                txtAppointmentReason.IsVisible = false;
                bdrReason.IsVisible = false;
            }

            SelectedExistingListBinding(_appointmentDetails?.Services?.ToList(), Convert.ToInt32(_appointmentDetails?.SalonID));
        }

        public async void SelectedExistingListBinding(List<Service> _servieList, int salonID)
        {
            try
            {
                //#region ServiceList

                //var res = Task.Run(async () => await objBarberSalonService.GetServicesBySalonId(salonID)).Result;
                //if (res?.StatusCode == 200)
                //{
                //    result = UtilityEL.ToModelList<ServiceModel>(res.Response);
                //}
                //#endregion
                //#region BarberList

                //List<BarberModel.BarberReturnResult> resultbarber = new List<BarberModel.BarberReturnResult>();
                //var resBarber = await objBarberSalonService.GetBarberBySalonId(salonID);
                //if (resBarber != null && resBarber.StatusCode == 200)
                //{
                //    resultbarber = UtilityEL.ToModelList<BarberModel.BarberReturnResult>(resBarber.Response);
                //}
                //#endregion
                _lstSelectedService.Clear();
                foreach (var item in _servieList)
                {
                    //var ServiceName = result.FirstOrDefault(x => x.ServiceId == Convert.ToInt16(item.ServiceId))?.ServiceName;
                    //var BarberName = resultbarber.FirstOrDefault(x => x.BarberId == Convert.ToInt16(item.BarberId))?.BarberName;
                    _lstSelectedService.Add(new ServiceData
                    {
                        BarberName = item.BarberName,
                        ServiceId = item.ServiceId,
                        BarberId = item.BarberId,
                        ServiceName = item.ServiceName,
                        ServicePrice = Double.Parse(item.Price),
                        // ServicePrice = Int64.Parse(item.Price, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo),
                        ServicePriceData = Application.Current.Properties["_Currency"].ToString() + item.Price,
                        //ServicePriceData = "£" + item.Price,
                        ServiceTime = item.From + "-" + item.To
                    });
                }

                ServiceList.ItemsSource = _lstSelectedService;
                var totalprice = _lstSelectedService.Sum(x => x.ServicePrice).ToString();
                txtTotalPrice.Text = Application.Current.Properties["_Currency"].ToString() + totalprice;
                //txtTotalPrice.Text = "£" + totalprice;
            }
            catch (Exception ex)
            {

            }
        }
    }
}