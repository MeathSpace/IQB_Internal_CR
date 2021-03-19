using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using IQB.Views.AdminManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ViewModel.AdminManagement
{
    public class CreateServiceViewModel : BaseViewModel
    {
        private ObservableCollection<string> CreateServices = null;
        private BarberSalonService barberSalonService;
        public CreateServiceViewModel()
        {
            IsRefreshing = true;
            IsEnabled = true;
            IsVisible = false;
            IsBackIconEnabled = true;
            barberSalonService = new BarberSalonService();
        }

        public Command AddServiceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsVisible = true;
                    if (!string.IsNullOrEmpty(ServiceName) && !string.IsNullOrEmpty(ServicePrice))
                    {
                        using (SalonViewModel obj = new SalonViewModel())
                        {
                            SalonID = obj.GetSalonId();
                        };

                        ServiceModel objServiceModel = new ServiceModel();
                        objServiceModel.SalonId = SalonID;
                        objServiceModel.ServiceName = ServiceName;
                        objServiceModel.ServicePrice = ServicePrice;
                        var _Result = await barberSalonService.PostCreateService(objServiceModel);
                        string msg = _Result.StatusMessage;
                        if (_Result.StatusCode == 200)
                        {
                            await App.Current.MainPage.DisplayAlert("Service Added", msg, "OK");
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Error", msg, "OK");
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert !", "Field(s) cannot be empty ", "OK");
                    }
                    IsVisible = false ;

                    App.Current.MainPage.Navigation.PopAsync();
                });
            }
        }

        //private Command<ServiceModel> _AddServiceCommand;

        //public Command<ServiceModel> AddServiceCommand
        //{
        //    get { return _AddServiceCommand; }
        //    set { _AddServiceCommand = value; }
        //}


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

        private bool m_IsVisible;

        public bool IsVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; OnPropertyChanged("IsVisible"); }
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
