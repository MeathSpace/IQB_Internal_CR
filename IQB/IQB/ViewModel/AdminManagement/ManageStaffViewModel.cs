using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.ViewModel.AdminManagement
{
    public class ManageStaffViewModel : BaseViewModel, INotifyPropertyChanged
    {
        //private ObservableCollection<string> ManageStaffServices = null;
        private BarberSalonService barberSalonService;
      
        public ManageStaffViewModel()
        {
         
                BarberList = new ObservableCollection<BarbersListModel>();
                IsRefreshing = true;
                IsEnabled = true;
                IsBackIconEnabled = true;
                BindStaffList();
                barberSalonService = new BarberSalonService();
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

       


        private ObservableCollection<BarbersListModel> m_BarberList;

        public ObservableCollection<BarbersListModel> BarberList
        {
            get { return m_BarberList; }
            set { m_BarberList = value; OnPropertyChanged("BarberList"); }
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

        private string m_StaffCount;

        public string StaffCount
        {
            get { return m_StaffCount; }
            set { m_StaffCount = value; OnPropertyChanged("StaffCount"); }
        }

        private string m_LabelData;

        public string LabelData
        {
            get { return m_LabelData; }
            set { m_LabelData = value; OnPropertyChanged("LabelData"); }
        }


        public Command PageAppearing1
        {
            get
            {
                return new Command(() =>
                {
                    BindStaffList();
                });
            }
        }

        public async void BindStaffList()
        {
            
            IsRefreshing = true;
            int salonId = 0;
            //string ftpfolderpath = string.Empty;
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
              //  ftpfolderpath = objSalon.GetSalonBarberFolder();

            }

            ApiResult res = new ApiResult();
            using (BarberSalonService obj = new BarberSalonService())
            {
                // res = await obj.BarberSelect(salonId);
                res = await obj.GetBarberBySalonId(salonId);
                if (res != null && res.StatusCode == 200)
                {
                    List<BarberReturnResult> result = UtilityEL.ToModelList<BarberReturnResult>(res.Response);

                    StaffCount = "(" + Convert.ToString(result.Count) + ")";
                    LabelData = "Manage Staff " + StaffCount;
                    BarberList = new ObservableCollection<BarbersListModel>();

                    foreach (var item in result)
                    {
                        BarbersListModel Barber = new BarbersListModel();
                        Barber.BarberImage = new UriImageSource
                        {
                            Uri = new Uri(item.BarberPic),
                            CachingEnabled = false,
                        };
                        Barber.BarberName = item.BarberName;
                        Barber.BarberID = item.BarberId;
                        Barber.CircleImageBorderColor = "#000000";

                        //Barber.EditStaff = new Command( async(barberid) => 
                        //{
                        //    int BarberID = Convert.ToInt32(barberid);
                        //    Application.Current.Properties["Barber_Id"] = BarberID;
                        //  await Application.Current.MainPage.Navigation.PushModalAsync(new AddStaffMember());                       
                        //});
                        Barber.DeleteStaff = new Command(async (barberid) =>
                        {
                            int BarberID = Convert.ToInt32(barberid);
                            var _BarberName = BarberList.Where(x => x.BarberID == BarberID).Select(p => p.BarberName).FirstOrDefault();
                            var _Result1 = await App.Current.MainPage.DisplayAlert("Warning", "Are you sure want to delete " + _BarberName + "?", "Ok", "Cancel");
                            if (_Result1)
                            {
                                using (StaffServices obj1 = new StaffServices())
                                {
                                    var _Result = await obj1.DeleteStaff(salonId, BarberID);
                                    string msg = _Result.StatusMessage;
                                    if (_Result.StatusCode == 200)
                                    {

                                        await App.Current.MainPage.DisplayAlert("Info", "Successfully deleted"+" "+ _BarberName + "", "Ok");
                                        var itemToRemove = BarberList.Single(r => r.BarberID == BarberID);
                                        BarberList.Remove(itemToRemove);
                                        if((result.Count - BarberList.Count())==1)
                                        {
                                            StaffCount = "(" + Convert.ToString(result.Count-1) + ")";
                                            LabelData = "Manage Staff " + StaffCount;
                                        }

                                    }
                                    else
                                    {
                                        await App.Current.MainPage.DisplayAlert("Warning", "Cannot delete"+" "+ _BarberName +" "+"because is currently on duty!", "Ok");
                                    }
                                }
                            }
                        });
                        
                        BarberList.Add(Barber);
                    }
                }
            }

          
            IsBackIconEnabled = true;
            IsRefreshing = false;
            
        }
    }

    public class BarbersListModel : BaseViewModel
    {
        private UriImageSource m_BarberImage;

        public UriImageSource BarberImage
        {
            get { return m_BarberImage; }
            set { m_BarberImage = value; OnPropertyChanged("BarberImage"); }
        }
        //OnPropertyChanged("BarberImage");
        private string m_BarberName;

        public string BarberName
        {
            get { return m_BarberName; }
            set { m_BarberName = value; OnPropertyChanged("BarberName"); }
        }

        private string m_ActiveImage;

        public string ActiveImage
        {
            get { return m_ActiveImage; }
            set { m_ActiveImage = value; OnPropertyChanged("ActiveImage"); }
        }

        private string m_CircleImageBorderColor;

        public string CircleImageBorderColor
        {
            get { return m_CircleImageBorderColor; }
            set { m_CircleImageBorderColor = value; OnPropertyChanged("CircleImageBorderColor"); }
        }

        private int _BarberID;

        public int BarberID
        {
            get { return _BarberID; }
            set { _BarberID = value; OnPropertyChanged("BarberID"); }
        }

        private ICommand _EditStaff;

        public ICommand EditStaff
        {
            get { return _EditStaff; }
            set { _EditStaff = value;OnPropertyChanged("EditStaff"); }
        }

        private ICommand _DeleteStaff;

        public ICommand DeleteStaff
        {
            get { return _DeleteStaff; }
            set { _DeleteStaff = value;OnPropertyChanged("DeleteStaff"); }
        }

    }
}
