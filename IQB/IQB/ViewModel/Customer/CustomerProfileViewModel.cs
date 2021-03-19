using IQB.EntityLayer.Common;
using IQB.EntityLayer.CustomerModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IQB.EntityLayer.CustomerModels.CustomerHistoryModel;

namespace IQB.ViewModel.Customer
{
    public class CustomerProfileViewModel : BaseViewModel
    {

        CustomerBookingHistoryModel objCustomerBookingHistoryModel = new CustomerBookingHistoryModel();
        private ObservableCollection<CustomerBookingHistoryModel> m_CustomerBookingHistoryList;

        public ObservableCollection<CustomerBookingHistoryModel> CustomerBookingHistoryList
        {
            get { return m_CustomerBookingHistoryList; }
            set { m_CustomerBookingHistoryList = value; OnPropertyChanged("CustomerBookingHistoryList"); }
        }

        public CustomerProfileViewModel(List<ListCustomerID> CustomerID)
        {
            GetCustomerDetails(CustomerID[0].CustomerID);
            //IsEnabled = true;
            // IsBackIconEnabled = true;

            CustomerBookingHistoryList = new ObservableCollection<CustomerBookingHistoryModel>();
            IsRefreshing = true;
            Application.Current.Properties["fromDate"] = "";
            Application.Current.Properties["toDate"] = "";
            BindCustomerBookingHistoryList(Convert.ToString(CustomerID[0].CustomerID), "", "");

        }

        private async void GetCustomerDetails(int CustomerId)
        {
            int salonId = 0;
            IsLoaderEnabled = true;
            IsGridVisible = true;
            IsMainGridVisible = false;

            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            ApiResult res = new ApiResult();
            using (CustomerServices obj = new CustomerServices())
            {
                res = await obj.GetCustomerDetailsByCustomerId(salonId, CustomerId);
                if (res != null && res.StatusCode == 200)
                {
                    CustomerModel result = UtilityEL.ToModelNew<CustomerModel>(res.Response);

                    CustomerFirstName = result.CustomerFName;
                    CustomerLastName = result.CustomerLName;
                    CustomerFullName = CustomerFirstName + " " + CustomerLastName;
                    CustomerContact = result.CustomerPhone == "" ? "Phone number" : result.CustomerPhone;
                    //CustomerDOB = result.CustomerDOB == "" ? "Date of Birth" : result.CustomerDOB;
                    CustomerDOB = string.IsNullOrEmpty(result.CustomerDOB) ? "Date of Birth" : UtilityEL.GetFormattedDateFormat(UtilityEL.GetExactData(result.CustomerDOB, "yyyy-mm-dd"));
                    CustomerEmail = result.CustomerEmail;
                    CustomerID = result.CustomerId;

                    CustomerImage = new UriImageSource
                    {
                        Uri = new Uri(result.CustomerImage),
                        CachingEnabled = false,
                    };


                    CustomerProfileImage = new UriImageSource
                    {
                        Uri = new Uri(result.CustomerImage),
                        CachingEnabled = false,
                    };
                }
            }

            IsBackIconEnabled = true;
            IsLoaderEnabled = false;
            IsGridVisible = false;
            IsMainGridVisible = true;
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

                        //CustomerID = Convert.ToInt32(result[0].CustomerId);
                        //CustomerFirstName = result[0].CustomerFName;
                        //CustomerLastName = result[0].CustomerLName;

                        if (result[0].History == null)
                        {
                            NoRecord = true;
                            IsListVisible = false;
                            IsFrmVisible = true;
                        }
                        else
                        {
                            IsListVisible = true;
                            IsFrmVisible = false;
                            foreach (var item1 in result[0].History)
                            {
                                objCustomerBookingHistoryModel = new CustomerBookingHistoryModel();

                                objCustomerBookingHistoryModel.BarberName = item1.BarberName;
                                objCustomerBookingHistoryModel.ServiceName = item1.ServiceName;
                                //objCustomerBookingHistoryModel.DateOfBooking = item1.DateOfBooking;
                                objCustomerBookingHistoryModel.DateOfBooking = UtilityEL.GetFormattedDateFormat(UtilityEL.GetExactData(item1.DateOfBooking, "dd/MM/yyyy"));

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



        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
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


        private bool m_IsFrmVisible = false;

        public bool IsFrmVisible
        {
            get { return m_IsFrmVisible; }
            set { m_IsFrmVisible = value; OnPropertyChanged("IsFrmVisible"); }
        }





        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }

        private bool m_IsLoaderEnabled;

        public bool IsLoaderEnabled
        {
            get { return m_IsLoaderEnabled; }
            set { m_IsLoaderEnabled = value; OnPropertyChanged("IsLoaderEnabled"); }
        }

        private bool m_IsGridVisible;

        public bool IsGridVisible
        {
            get { return m_IsGridVisible; }
            set { m_IsGridVisible = value; OnPropertyChanged("IsGridVisible"); }
        }

        private bool m_IsMainGridVisible;

        public bool IsMainGridVisible
        {
            get { return m_IsMainGridVisible; }
            set { m_IsMainGridVisible = value; OnPropertyChanged("IsMainGridVisible"); }
        }

        private bool m_IsBackIconEnabled;
        public bool IsBackIconEnabled
        {
            get { return m_IsBackIconEnabled; }
            set { m_IsBackIconEnabled = value; OnPropertyChanged("IsBackIconEnabled"); }
        }

        #region Entities
        private string _CustomerFirstName;

        public string CustomerFirstName
        {
            get { return _CustomerFirstName; }
            set { _CustomerFirstName = value; OnPropertyChanged("CustomerFirstName"); }
        }

        private string _CustomerLastName;

        public string CustomerLastName
        {
            get { return _CustomerLastName; }
            set { _CustomerLastName = value; OnPropertyChanged("CustomerLastName"); }
        }



        private string _CustomerFullName;

        public string CustomerFullName
        {
            get { return _CustomerFullName; }
            set { _CustomerFullName = value; OnPropertyChanged("CustomerFullName"); }
        }

        private int _CustomerID;

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; OnPropertyChanged("CustomerID"); }
        }

        private string _CustomerEmail;

        public string CustomerEmail
        {
            get { return _CustomerEmail; }
            set { _CustomerEmail = value; OnPropertyChanged("CustomerEmail"); }
        }

        private string _CustomerContact;

        public string CustomerContact
        {
            get { return _CustomerContact; }
            set { _CustomerContact = value; OnPropertyChanged("CustomerContact"); }
        }

        private ImageSource _CustomerImage;

        public ImageSource CustomerImage
        {
            get { return _CustomerImage; }
            set { _CustomerImage = value; OnPropertyChanged("CustomerImage"); }
        }



        private ImageSource _CustomerProfileImage;

        public ImageSource CustomerProfileImage
        {
            get { return _CustomerProfileImage; }
            set { _CustomerProfileImage = value; OnPropertyChanged("CustomerProfileImage"); }
        }

        private string _CustomerDOB;

        public string CustomerDOB
        {
            get { return _CustomerDOB; }
            set { _CustomerDOB = value; OnPropertyChanged("CustomerDOB"); }
        }
        #endregion
    }
}
