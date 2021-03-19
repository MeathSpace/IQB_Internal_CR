using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ViewModel.AdminManagement
{
    public class UpdateServiceViewModel:BaseViewModel
    {
        private BarberSalonService barberSalonService;
        public UpdateServiceViewModel(int serviceId)
        {
            IsRefreshing = true;
            IsEnabled = true;
            IsVisible = false;
            IsBackIconEnabled = true;
            barberSalonService = new BarberSalonService();
            PopulateServiceByServiceId(serviceId);
        }

        public Command UpdateServiceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsVisible = true;
                    using (SalonViewModel obj = new SalonViewModel())
                    {
                        SalonID = obj.GetSalonId();
                    }

                    ServiceModel aa = new ServiceModel();
                    aa.SalonId = SalonID;
                    aa.ServiceId = ServiceId;
                    aa.ServiceName = ServiceName;
                    aa.ServicePrice = ServicePrice;
                    var _Result = await barberSalonService.PostUpdateService(aa);
                   // string msg = _Result.StatusMessage;
                    if (_Result.StatusCode == 200)
                    {                       
                        await App.Current.MainPage.DisplayAlert("Service Updated", _Result.StatusMessage, "Ok");
                    }
                    else
                    {                      
                        await App.Current.MainPage.DisplayAlert("Service Updated", _Result.StatusMessage, "Ok");
                    }
                    IsVisible = false;
                    App.Current.MainPage.Navigation.PopAsync();

                });
            }
        }

        private async void PopulateServiceByServiceId(int selectedServiceID)
        {
            ApiResult res = new ApiResult();
            using (BarberSalonService obj = new BarberSalonService())
            {
                res = await obj.GetServiceDetailsBySalonIdServiceId(selectedServiceID);
                if (res != null && res.StatusCode == 200)
                {
                    ServiceListModel data = UtilityEL.ToModel<ServiceListModel>(res.Response);
                        ServiceId = data.ServiceID;
                        ServiceName = data.ServiceName;
                        ServicePrice = data.ServicePrice;
                    

                }
            }

           
            IsBackIconEnabled = true;
            IsRefreshing = false;
        }




        private bool m_IsVisible;

        public bool IsVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; OnPropertyChanged("IsVisible"); }
        }


        private int _ServiceId;

        public int ServiceId
        {
            get { return _ServiceId; }
            set { _ServiceId = value;OnPropertyChanged("ServiceId"); }
        }

        private int _SelectedServiceId;

        public int SelectedServiceId
        {
            get { return _SelectedServiceId; }
            set { _SelectedServiceId = value; OnPropertyChanged("SelectedServiceId"); }
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
    }
}
