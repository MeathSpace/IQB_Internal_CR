using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ViewModel.AdminManagement
{
    public class SalonTextViewModel : BaseViewModel
    {
        private BarberSalonService barberSalonService;

        public SalonTextViewModel()
        {
            IsRefreshing = false;
            IsEnabled = true;
            IsBackIconEnabled = true;
            barberSalonService = new BarberSalonService();
            // PopulateServiceByServiceId(serviceId);
            if (Application.Current.Properties.ContainsKey("SalonText"))
                
                    SalonText = Convert.ToString(Application.Current.Properties["SalonText"]);
            else
                SalonText = "";
        }

        public Command UpdateSalonTextCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    IsEnabled = false;
                    using (SalonViewModel obj = new SalonViewModel())
                    {
                        SalonID = obj.GetSalonId();
                    }

                    ApiResult res = new ApiResult();
                    using (BarberSalonService obj = new BarberSalonService())
                    {
                        res = await obj.PostUpdateSalonText(SalonID, SalonText);
                        if (res != null && res.StatusCode == 200)
                        {
                            await App.Current.MainPage.DisplayAlert("Salon Text Updated", res.StatusMessage, "OK");
                            Application.Current.Properties["SalonText"] = SalonText;
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                    }


                    IsBackIconEnabled = true;
                    IsEnabled = true;
                    IsRefreshing = false;
                });
            }
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

        private string _SalonText;

        public string SalonText
        {
            get { return _SalonText; }
            set { _SalonText = value; OnPropertyChanged("SalonText"); }
        }

        private int _SalonID;

        public int SalonID
        {
            get { return _SalonID; }
            set { _SalonID = value; OnPropertyChanged("SalonID"); }
        }
    }
}
