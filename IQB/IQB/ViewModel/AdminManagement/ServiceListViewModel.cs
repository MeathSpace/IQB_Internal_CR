using IQB.EntityLayer.Common;
using IQB.EntityLayer.ServiceModels;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using IQB.Views.AdminManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static IQB.EntityLayer.ServiceModels.ServiceModel;

namespace IQB.ViewModel.AdminManagement
{
    public class ServiceListViewModel : BaseViewModel
    {
        private ObservableCollection<string> BarberServices = null;

        private ObservableCollection<ServiceListModel> m_ServiceList;

        public ObservableCollection<ServiceListModel> ServiceList
        {
            get { return m_ServiceList; }
            set { m_ServiceList = value; OnPropertyChanged("ServiceList"); }
        }


        private string m_ServiceCount;

        public string ServiceCount
        {
            get { return m_ServiceCount; }
            set { m_ServiceCount = value; OnPropertyChanged("ServiceCount"); }
        }



        private string m_LabelData;

        public string LabelData
        {
            get { return m_LabelData; }
            set { m_LabelData = value; OnPropertyChanged("LabelData"); }
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private bool m_IsBackIconEnabled;
        public bool IsBackIconEnabled
        {
            get { return m_IsBackIconEnabled; }
            set { m_IsBackIconEnabled = value; OnPropertyChanged("IsBackIconEnabled"); }
        }
        public ServiceListViewModel()
        {
            ServiceList = new ObservableCollection<ServiceListModel>();
            IsRefreshing = true;
            BindServiceList();

        }

        public Command PageAppearing
        {
            get
            {
                return new Command(async () =>
                {
                    BindServiceList();
                });
            }
        }

        private async void BindServiceList()
        {
            int salonId = 0;
            IsRefreshing = true;

            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            ApiResult res = new ApiResult();
            using (BarberSalonService obj = new BarberSalonService())
            {
                res = await obj.GetServicesBySalonId(salonId);
            }

            if (res != null && res.StatusCode == 200)
            {
                List<ServiceModel> result = UtilityEL.ToModelList<ServiceModel>(res.Response);

                ServiceCount = "(" + Convert.ToString(result.Count) + ")";
                LabelData = "Manage Service " + ServiceCount;

                ServiceList = new ObservableCollection<ServiceListModel>();
                foreach (var item in result)
                {
                    ServiceListModel Service = new ServiceListModel();

                    Service.ServiceName = item.ServiceName;
                    Service.ServicePrice = item.ServicePrice;
                    Service.ServiceID = item.ServiceId;

                    Service.DeleteService = new Command<ServiceListModel>(async (serviceId) =>
                    {
                        int ServiceId = Convert.ToInt16(serviceId.ServiceID);
                        var ServiceName = ServiceList.Where(p => p.ServiceID == ServiceId).Select(x => x.ServiceName).FirstOrDefault();
                        var _Result = await App.Current.MainPage.DisplayAlert("Warning", "Are you sure want to delete- " + ServiceName + "?", "Ok", "Cancel");

                        if (_Result)
                        {
                            using (BarberSalonService obj = new BarberSalonService())
                            {
                                res = await obj.DeleteService(salonId, ServiceId);
                            }

                            if (res != null && res.StatusCode == 200)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", res.StatusMessage, "Ok");

                                var itemToRemove = ServiceList.FirstOrDefault(r => r.ServiceID == ServiceId);
                                ServiceList.Remove(itemToRemove);
                                ServiceCount = "(" + Convert.ToString(ServiceList.Count) + ")";
                                LabelData = "Manage Service " + ServiceCount;

                            }
                            else if (res.StatusCode == 201)
                            {
                                await App.Current.MainPage.DisplayAlert("Alert", res.StatusMessage, "Ok");
                            }
                        }
                    });

                    ServiceList.Add(Service);
                }
            }
            IsBackIconEnabled = true;
            IsRefreshing = false;
        }


    }

    public class ServiceListModel : BaseViewModel
    {
        private string m_ServiceName;

        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; OnPropertyChanged("ServiceName"); }
        }

        private string _ServicePrice;

        public string ServicePrice
        {
            get { return _ServicePrice; }
            set { _ServicePrice = value; OnPropertyChanged("ServicePrice"); }
        }

        private int _ServiceID;

        public int ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; OnPropertyChanged("ServiceID"); }
        }

        private string _ServiceEstimatedTime;

        public string ServiceEstimatedTime
        {
            get { return _ServiceEstimatedTime; }
            set { _ServiceEstimatedTime = value; OnPropertyChanged("ServiceEstimatedTime"); }
        }

        public string _least_EST_wait_time;
        public string least_EST_wait_time
        {
            get { return _least_EST_wait_time; }
            set { _least_EST_wait_time = value; OnPropertyChanged("least_EST_wait_time"); }
        }

        private string m_Status;

        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; OnPropertyChanged("Status"); }
        }

        private ICommand _DeleteService;

        public ICommand DeleteService
        {
            get { return _DeleteService; }
            set { _DeleteService = value; OnPropertyChanged("DeleteService"); }
        }
        private string _BarberLName;

        public string BarberLName
        {
            get { return _BarberLName; }
            set { _BarberLName = value; OnPropertyChanged("BarberLName"); }
        }

        private int _SelectBarberID;

        public int SelectBarberID
        {
            get { return _SelectBarberID; }
            set { _SelectBarberID = value; OnPropertyChanged("SelectBarberID"); }
        }



        private bool _IsServiceChecked;

        public bool IsServiceChecked
        {
            get { return _IsServiceChecked; }
            set { _IsServiceChecked = value; OnPropertyChanged("IsServiceChecked"); }
        }




        private string m_SourceTickImage;

        public string SourceTickImage
        {
            get { return m_SourceTickImage; }
            set { m_SourceTickImage = value; OnPropertyChanged("SourceTickImage"); }
        }

        private Color _LabelColor;

        public Color LabelColor
        {
            get { return _LabelColor; }
            set { _LabelColor = value; OnPropertyChanged("LabelColor"); }
        }
    }
}
