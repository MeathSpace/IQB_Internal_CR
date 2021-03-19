using IQB.EntityLayer.Common;
using IQB.EntityLayer.CustomerModels;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace IQB.ViewModel.Customer
{
    public class CustomerListViewModel : BaseViewModel
    {
        private CustomerServices customerServices;



        private ObservableCollection<CustomerListModel> m_CustomerList;

        public ObservableCollection<CustomerListModel> CustomerList
        {
            get { return m_CustomerList; }
            set { m_CustomerList = value; OnPropertyChanged("CustomerList"); }
        }

        private List<CustomerListModel> m_MoreCustomerList;

        public List<CustomerListModel> MoreCustomerList
        {
            get { return m_MoreCustomerList; }
            set { m_MoreCustomerList = value; OnPropertyChanged("MoreCustomerList"); }
        }


        private bool m_IsProfileApiRunning;

        public bool IsProfileApiRunning
        {
            get { return m_IsProfileApiRunning; }
            set { m_IsProfileApiRunning = value; OnPropertyChanged("IsProfileApiRunning"); }
        }

        private List<CustomerListModel> m_TargetCustomerList;

        public List<CustomerListModel> TargetCustomerList
        {
            get { return m_TargetCustomerList; }
            set { m_TargetCustomerList = value; OnPropertyChanged("TargetCustomerList"); }
        }

        private List<ListCustomerID> m_IdList;

        public List<ListCustomerID> IdList
        {
            get { return m_IdList; }
            set { m_IdList = value; OnPropertyChanged("IdList"); }
        }

        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; OnPropertyChanged("Count"); }
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private bool m_IsMainSwitchOn;

        public bool IsMainSwitchOn
        {
            get { return m_IsMainSwitchOn; }
            set { m_IsMainSwitchOn = value; OnPropertyChanged("IsMainSwitchOn"); }
        }

        private bool m_IsBackIconEnabled;
        public bool IsBackIconEnabled
        {
            get { return m_IsBackIconEnabled; }
            set { m_IsBackIconEnabled = value; OnPropertyChanged("IsBackIconEnabled"); }
        }

        private int _SalonID;

        public int SalonID
        {
            get { return _SalonID; }
            set { _SalonID = value; OnPropertyChanged("SalonID"); }
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set { m_FirstName = value; OnPropertyChanged("FirstName"); }
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value; OnPropertyChanged("LastName"); }
        }

        private int m_CustomerCount1;

        public int CustomerCount1
        {
            get { return m_CustomerCount1; }
            set { m_CustomerCount1 = value; OnPropertyChanged("CustomerCount1"); }
        }

        private string m_CustomerCount;

        public string CustomerCount
        {
            get { return m_CustomerCount; }
            set { m_CustomerCount = value; OnPropertyChanged("CustomerCount"); }
        }


        private string m_LabelData;

        public string LabelData
        {
            get { return m_LabelData; }
            set { m_LabelData = value; OnPropertyChanged("LabelData"); }
        }

        private string m_NoRecords;

        public string NoRecords
        {
            get { return m_NoRecords; }
            set { m_NoRecords = value; OnPropertyChanged("NoRecords"); }
        }

        private int m_Count;

        public int _Count
        {
            get { return m_Count; }
            set { m_Count = value; OnPropertyChanged("_Count"); }
        }

        private bool m_IsListVisible;

        public bool IsListVisible
        {
            get { return m_IsListVisible; }
            set { m_IsListVisible = value; OnPropertyChanged("IsListVisible"); }
        }


        private bool m_NoRecordsVisible;

        public bool NoRecordsVisible
        {
            get { return m_NoRecordsVisible; }
            set { m_NoRecordsVisible = value; OnPropertyChanged("NoRecordsVisible"); }
        }


        public CustomerListViewModel()
        {
            CustomerList = new ObservableCollection<CustomerListModel>();
            IsRefreshing = true;
            IsListVisible = true;

            Application.Current.Properties["CustomerFName"] = "";
            Application.Current.Properties["CustomerLName"] = "";
            Application.Current.Properties["IsProfileViewed"] = "";

            BindCustomerList("", "");
        }

        public async void BindCustomerList(string CusFirstName, string CusLastName)
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
                if (_Count == 0)
                {
                    IsProfileApiRunning = true;
                    res = await obj.GetCustomerBySalonId(salonId, Convert.ToString(Application.Current.Properties["CustomerFName"]), Convert.ToString(Application.Current.Properties["CustomerLName"]), _Count, 100);
                }
                else
                {
                    res = await obj.GetCustomerBySalonId(salonId, Convert.ToString(Application.Current.Properties["CustomerFName"]), Convert.ToString(Application.Current.Properties["CustomerLName"]), _Count + 1, 100);
                }
                    
                if (res != null && res.StatusCode == 200)
                {
                    List<CustomerModel> result = UtilityEL.ToModelList<CustomerModel>(res.Response);
                    Count = res.Cnt;
                   _Count += result.Count;
                    //CustomerCount = "(" + Convert.ToString(res.Cnt) + ")";
                    CustomerCount = "(" + Convert.ToString(_Count) + "/" + Count + ")";
                    LabelData = "My Customers " + CustomerCount;
                    CustomerCount1 = result.Count;
                    CustomerList = new ObservableCollection<CustomerListModel>();
                    IdList = new List<ListCustomerID>();

                    foreach (var item in result)
                    {
                        CustomerListModel objCustomerListModel = new CustomerListModel();

                        objCustomerListModel.CustomerID = item.CustomerId;
                        objCustomerListModel.CustomerFirstName = item.CustomerFName;
                        objCustomerListModel.CustomerLastName = item.CustomerLName;
                        objCustomerListModel.CustomerImage = item.CustomerImage;
                        objCustomerListModel.marketingMail = item.MaketingEmails;

                        if (item.MaketingEmails == "0")
                        {
                            objCustomerListModel.IsSwitchEnabled = true;
                            objCustomerListModel.IsSendMailEnabled = false;
                            //objCustomerListModel.swOpacity = 0.3;
                        }
                        else
                        {
                            objCustomerListModel.IsSwitchEnabled = false;
                            objCustomerListModel.IsSendMailEnabled = true;
                            //objCustomerListModel.swOpacity = 1;
                        }

                        if (IsMainSwitchOn)
                            objCustomerListModel.IsSwitchToggled = true;

                        IdList.Add(new ListCustomerID { CustomerID = item.CustomerId });
                        Application.Current.Properties["CustomerIDList"] = IdList;
                        Application.Current.Properties["CustomerCountToSendMail"] = IdList.Count;
                        CustomerList.Add(objCustomerListModel);
                        IsProfileApiRunning = false;
                    }
                  //  _Count = CustomerList.Count;


                    if (Application.Current.Properties["IsFilterApplied"] == "true")
                    {
                        MoreCustomerList = CustomerList.ToList();
                    }
                    else
                    {
                        if (MoreCustomerList != null)
                        {
                            string q = "";
                            foreach (var item in result)
                            {
                                for (int i = 0; i < MoreCustomerList.Count; i++)
                                {
                                    if (item.CustomerId == MoreCustomerList[i].CustomerID)
                                    {
                                        q = "yes";
                                    }
                                }
                            }

                            if (q == "yes")
                            {
                                MoreCustomerList = CustomerList.ToList();
                            }
                            else
                            {
                                MoreCustomerList = MoreCustomerList.Concat(CustomerList.ToList()).ToList();
                            }
                        }
                        else
                        {
                            MoreCustomerList = CustomerList.ToList();

                        }
                    }
                    MessagingCenter.Send(this, "Popped1");
                }
                else
                {
                    IsListVisible = false;
                    NoRecordsVisible = true;
                    NoRecords = "No records found";
                }
            }


            IsBackIconEnabled = true;
            IsRefreshing = false;
        }

        //public Command PageAppearing
        //{
        //    get
        //    {
        //        return new Command(() =>
        //        {
        //            if(Application.Current.Properties["IsProfileViewed"] != "true")
        //            BindCustomerList("", "");
        //            //Application.Current.Properties["CustomerID"] = new List<ListCustomerID>();
        //        });
        //    }
        //}

        public Command FilterCustomer
        {
            get
            {
                return new Command(async () =>
                {
                    CustomerModel objCustomerModel = new CustomerModel();
                    objCustomerModel.CustomerFName = FirstName;
                    objCustomerModel.CustomerLName = LastName;

                    if (Convert.ToString(objCustomerModel.CustomerFName) == "" && Convert.ToString(objCustomerModel.CustomerLName) == "")
                        await App.Current.MainPage.DisplayAlert("", "Please provide first name or last name", "Ok");
                    else
                    {
                        Application.Current.Properties["CustomerFName"] = Convert.ToString(objCustomerModel.CustomerFName).Trim();
                        Application.Current.Properties["CustomerLName"] = Convert.ToString(objCustomerModel.CustomerLName).Trim();
                        Application.Current.Properties["IsFilterApplied"] = "true";
                        _Count = 0;
                        BindCustomerList(Convert.ToString(objCustomerModel.CustomerFName).Trim(), Convert.ToString(objCustomerModel.CustomerLName).Trim());
                    }
                });
            }
        }
    }

    public class CustomerListModel : BaseViewModel
    {
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

        private string m_CustomerImage;

        public string CustomerImage
        {
            get { return m_CustomerImage; }
            set { m_CustomerImage = value; OnPropertyChanged("CustomerImage"); }
        }

        private int _CustomerID;

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; OnPropertyChanged("CustomerID"); }
        }

        private string _marketingMail;

        public string marketingMail
        {
            get { return _marketingMail; }
            set { _marketingMail = value; OnPropertyChanged("marketingMail"); }
        }

        private bool m_IsSwitchToggled;

        public bool IsSwitchToggled
        {
            get { return m_IsSwitchToggled; }
            set { m_IsSwitchToggled = value; OnPropertyChanged("IsSwitchToggled"); }
        }

        private bool m_IsSwitchEnabled;

        public bool IsSwitchEnabled
        {
            get { return m_IsSwitchEnabled; }
            set { m_IsSwitchEnabled = value; OnPropertyChanged("IsSwitchEnabled"); }
        }

        private bool m_IsSendMailEnabled;

        public bool IsSendMailEnabled
        {
            get { return m_IsSendMailEnabled; }
            set { m_IsSendMailEnabled = value; OnPropertyChanged("IsSendMailEnabled"); }
        }

        private ObservableCollection<ListCustomerID> _CustomerIDList;

        public ObservableCollection<ListCustomerID> CustomerIDList
        {
            get { return _CustomerIDList; }
            set { _CustomerIDList = value; OnPropertyChanged("CustomerIDList"); }
        }

        private Color _swOpacity;

        public Color swOpacity
        {
            get { return _swOpacity; }
            set { _swOpacity = value; OnPropertyChanged("swOpacity"); }
        }

    }

    public class ListCustomerID : BaseViewModel
    {
        private int _CustomerID;

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; OnPropertyChanged("CustomerID"); }
        }
    }
}
