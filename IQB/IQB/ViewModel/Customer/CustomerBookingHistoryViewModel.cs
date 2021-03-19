using IQB.EntityLayer.Common;
using IQB.EntityLayer.CustomerModels;
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
using static IQB.EntityLayer.CustomerModels.CustomerHistoryModel;

namespace IQB.ViewModel.Customer
{
    public class CustomerBookingHistoryViewModel : BaseViewModel
    {
        private CustomerServices customerServices;
        CustomerBookingHistoryModel objCustomerBookingHistoryModel = new CustomerBookingHistoryModel();
        private ObservableCollection<CustomerBookingHistoryModel> m_CustomerBookingHistoryList;

        public ObservableCollection<CustomerBookingHistoryModel> CustomerBookingHistoryList
        {
            get { return m_CustomerBookingHistoryList; }
            set { m_CustomerBookingHistoryList = value; OnPropertyChanged("CustomerBookingHistoryList"); }
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

        private string m_CustomerFirstName;

        public string CustomerFirstName
        {
            get { return m_CustomerFirstName; }
            set { m_CustomerFirstName = value; OnPropertyChanged("CustomerFirstName"); }
        }

        private string m_CustomerLastName;

        public string CustomerLastName
        {
            get { return m_CustomerLastName; }
            set { m_CustomerLastName = value; OnPropertyChanged("CustomerLastName"); }
        }

        private bool m_NoRecord;

        public bool NoRecord
        {
            get { return m_NoRecord; }
            set { m_NoRecord = value; OnPropertyChanged("NoRecord"); }
        }

        private bool m_IsListVisible = true;

        public bool IsListVisible
        {
            get { return m_IsListVisible; }
            set { m_IsListVisible = value; OnPropertyChanged("IsListVisible"); }
        }

        private string _CustomerID;

        public string CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; OnPropertyChanged("CustomerID"); }
        }

        public CustomerBookingHistoryViewModel(int customerId)
        {
            CustomerBookingHistoryList = new ObservableCollection<CustomerBookingHistoryModel>();
            IsRefreshing = true;
            Application.Current.Properties["fromDate"] = "";
            Application.Current.Properties["toDate"] = "";
            BindCustomerBookingHistoryList(Convert.ToString(customerId),"","");
        }

        public async void BindCustomerBookingHistoryList(string customerId, string startDate, string endDate)
        {
            int salonId = 0;
            IsRefreshing = true;

            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            ApiResult res = new ApiResult();
            using (CustomerServices obj = new CustomerServices())
            {
                res = await obj.GetCustomerBookingHistory(customerId, startDate, endDate);
                if (res != null && res.StatusCode == 200)
                {
                    try
                    {
                        List<RootObject> result = UtilityEL.ToModelListNew<RootObject>(res.Response);
                        CustomerBookingHistoryList = new ObservableCollection<CustomerBookingHistoryModel>();

                        CustomerID = result[0].CustomerId;
                        CustomerFirstName = result[0].CustomerFName;
                        CustomerLastName = result[0].CustomerLName;

                        if (result[0].History == null)
                        {
                            NoRecord = true;
                            IsListVisible = false;
                        }
                        else
                        {
                            foreach (var item1 in result[0].History)
                            {
                                objCustomerBookingHistoryModel = new CustomerBookingHistoryModel();

                                objCustomerBookingHistoryModel.BarberName = item1.BarberName;
                                objCustomerBookingHistoryModel.ServiceName = item1.ServiceName;
                                objCustomerBookingHistoryModel.DateOfBooking = item1.DateOfBooking;

                                CustomerBookingHistoryList.Add(objCustomerBookingHistoryModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            IsBackIconEnabled = true;
            IsRefreshing = false;
        }


        public Command CustomerHistoryByDate
        {
            get
            {
                return new Command(async () =>
                {
                    BindCustomerBookingHistoryList(CustomerID, Convert.ToString(Application.Current.Properties["fromDate"]), Convert.ToString(Application.Current.Properties["toDate"]));
                });
            }
        }
    }

    public class CustomerBookingHistoryModel : BaseViewModel
    {
        private string m_BarberName;
        public string BarberName
        {
            get { return m_BarberName; }
            set { m_BarberName = value; OnPropertyChanged("BarberName"); }
        }

        private string m_ServiceName;
        public string ServiceName
        {
            get { return m_ServiceName; }
            set { m_ServiceName = value; OnPropertyChanged("ServiceName"); }
        }

        private string m_Type;
        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; OnPropertyChanged("Type"); }
        }

        private string m_DateOfBooking;
        public string DateOfBooking
        {
            get { return m_DateOfBooking; }
            set { m_DateOfBooking = value; OnPropertyChanged("DateOfBooking"); }
        }
    }
}
