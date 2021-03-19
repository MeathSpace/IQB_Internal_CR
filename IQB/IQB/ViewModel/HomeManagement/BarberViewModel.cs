using IQB.EntityLayer.Common;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.ViewModel.HomeManagement
{
    public class BarberViewModel : BaseViewModel
    {

        private string m_BarberNameP;

        public string BarberNameP
        {
            get { return m_BarberNameP; }
            set { m_BarberNameP = value; OnPropertyChanged("BarberName"); }
        }

        private ObservableCollection<string> BarberServices = null;

        private ObservableCollection<BarbersResultListModel> _br = null;

        //int CountSwitchOn;

        public ObservableCollection<BarbersResultListModel> BrResult
        {
            get { return _br; }
            set { _br = value; OnPropertyChanged("BrResult"); }
        }


        public BarberViewModel()
        {
            BarberList = new ObservableCollection<BarbersListModel>();
            IsRefreshing = true;
            IsEnabled = true;
            IsBackIconEnabled = false;
            SetBarberServices();
            SetDefaultMenuList();
            SetServicePopup();
        }

        public Command Dc
        {
            get
            {
                return new Command(async (aa) =>
                {

                    var a = aa as BarbersResultListModel;

                    //foreach (var item in _br)
                    //{
                    //    if (item.Istoggled == true)
                    //    {
                    //        await App.Current.MainPage.DisplayAlert("Service Selected", "", "Ok", "");
                    //        item.Istoggled = false;
                    //    }
                    //}
                    //return ;

                    //var q = a.IsSwitchOn;

                    //if (CountSwitchOn > 0)
                    //{
                    //    var answer = await App.Current.MainPage.DisplayAlert("", "You can select only one service", "Ok", "Cancel");

                    //    if (answer == true)
                    //    {
                    //        CountSwitchOn--;
                    //        SetServicePopup();
                    //    }

                    //}

                    //else
                    //{
                    //    CountSwitchOn++;
                    //}
                });
            }
        }

        private void SetServicePopup()
        {
            ObservableCollection<BarberReturnResult> objList = new ObservableCollection<BarberReturnResult>();

            objList.Add(new BarberReturnResult() { id = 1, BarberName = "Half Fade", Position = Application.Current.Properties["_Currency"].ToString() + "10.00" });
        
            objList.Add(new BarberReturnResult() { id = 2, BarberName = "Clipper Cut", Position = Application.Current.Properties["_Currency"].ToString() + " 15.00" });
            objList.Add(new BarberReturnResult() { id = 3, BarberName = "High Fade Quiff", Position = Application.Current.Properties["_Currency"].ToString() + " 30.00" });
            objList.Add(new BarberReturnResult() { id = 4, BarberName = "Messy Undercut", Position = Application.Current.Properties["_Currency"].ToString() + " 35.00" });
            BrResult = new ObservableCollection<BarbersResultListModel>();
            foreach (var item in objList)
            {
                BarbersResultListModel b = new BarbersResultListModel();
                b.ID = item.id;
                b.Name = item.BarberName;
                b.Posititon = item.Position;
                b.Istoggled = false;
                //b.DemoCommand = new Command(()=> {
                //    var a = ""+item.id;
                //});
                b.DemoCommand = Dc;
                BrResult.Add(b);
            }

        }

        private void SetBarberServices()
        {
            BarberServices = new ObservableCollection<string> { "Service1", "Service2", "Service3", "Service4", "Service5" };
        }

        private async void SetDefaultMenuList()
        {
            int salonId = 0;
            string ftpfolderpath = string.Empty;
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
                ftpfolderpath = objSalon.GetSalonBarberFolder();
            }

            //List<BarberReturnResult> result = new List<BarberReturnResult>();
            ApiResult res = new ApiResult();

            using (BarberService obj = new BarberService())
            {
                res = await obj.BarberSelect(salonId);
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberReturnResult> result = UtilityEL.ToModelList<BarberReturnResult>(res.Response);
                //TotalNoActiveBarbers = "List showing " + result.Count + " barbers on duty";
                //if (result[0].TotalQueue != "0")
                if (result[0].BarberOnDutyCnt != 0)
                {
                    IsTitleVisible = true;
                    //TotalNoActiveBarbers = result.Count + " Barber(s) on Duty";
                    TotalNoActiveBarbers = result[0].BarberOnDutyCnt + " Barber(s) Available";
                }
                else
                {
                    IsTitleVisible = false;
                    TotalNoActiveBarbers = "No Barber Available";
                }
                foreach (var item in result)
                {
                    BarbersListModel Barber = new BarbersListModel();
                    Barber.BarberId = item.BarberId;
                    Barber.Id = item.id;
                    if (string.IsNullOrEmpty(item.BarberPic))
                    {
                        //Barber.BarberImage = "noimage.png";
                        Barber.BarberImage = CommonEL.BarberImageLocation + "barbers_profile_pics" + "/" + "noimage.jpg";
                    }
                    else
                    {
                        Barber.BarberImage = CommonEL.BarberImageLocation + "barbers_profile_pics" + "/" + item.BarberPic;
                    }
                    Barber.BarberName = item.BarberName;
                    //Barber.Queuing = "Queuing: " + item.TotalQueue;
                    Barber.Queuing = item.TotalQueue;
                    int nextAvailPos = Convert.ToInt32(item.TotalQueue) + 1;
                    //Barber.NextAvailPos = "Next available position: " + nextAvailPos;
                    Barber.NextAvailPos = nextAvailPos.ToString();
                    string EstimatedTime = string.Empty;
                    if (!string.IsNullOrEmpty(item.EWT))
                    {
                        TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToInt32(item.EWT));
                        EstimatedTime = timeSpan.ToString(@"hh\:mm");

                        //int hours = Convert.ToInt32(item.EWT) * (Convert.ToInt32(item.TotalQueue)) / 60;
                        //int Minutes = Convert.ToInt32(item.EWT) * (Convert.ToInt32(item.TotalQueue));
                        //Minutes = Minutes % 60;
                        //EstimatedTime = string.Format("{0:00}", hours) + ":" + string.Format("{0:00}", Minutes);
                    }
                    else
                    {
                        EstimatedTime = "Next";
                    }

                    //Barber.EstimatedTime = "Estimated Time: " + EstimatedTime;
                    Barber.EstimatedTime = EstimatedTime;

                    if (item.BarberOnline == "Y")
                    {
                        Barber.ActiveImage = "online.png";
                        Barber.CircleImageBorderColor = "Green";
                        Barber.BarberOnline = "yes";
                    }
                    else
                    {
                        Barber.ActiveImage = "offline.png";
                        Barber.CircleImageBorderColor = "Red";
                        Barber.BarberOnline = "no";
                    }
                    Barber.BarberServices = this.BarberServices;
                 //   if(Barber.BarberOnline == "yes")
                    BarberList.Add(Barber);
                }
            }
            IsBackIconEnabled = true;
            IsRefreshing = false;
        }

        private ObservableCollection<BarbersListModel> m_BarberList;

        public ObservableCollection<BarbersListModel> BarberList
        {
            get { return m_BarberList; }
            set { m_BarberList = value; OnPropertyChanged("BarberList"); }
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        private bool m_IsLoaderEnabled;

        public bool IsLoaderEnabled
        {
            get { return m_IsLoaderEnabled; }
            set { m_IsLoaderEnabled = value; OnPropertyChanged("IsLoaderEnabled"); }
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

        private bool _IsTitleVisible = false;

        public bool IsTitleVisible
        {
            get { return _IsTitleVisible; }
            set { _IsTitleVisible = value; OnPropertyChanged("IsTitleVisible"); }
        }

        private bool _isVisible = false;

        public bool isVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; OnPropertyChanged("isVisible"); }
        }

    }

    public class BarbersListModel : BaseViewModel
    {
        public ICommand DemoCommand { get; set; }

        private int m_Id;

        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; OnPropertyChanged("Id"); }
        }

        private int m_BarberId;

        public int BarberId
        {
            get { return m_BarberId; }
            set { m_BarberId = value; OnPropertyChanged("BarberId"); }
        }

        private string m_Status = "unselected";

        public string Status
        {
            get { return m_Status; }
            set { m_Status = value; OnPropertyChanged("Status"); }
        }

        private string m_BarberImage;

        public string BarberImage
        {
            get { return m_BarberImage; }
            set { m_BarberImage = value; OnPropertyChanged("BarberImage"); }
        }

        private string m_BarberName;

        public string BarberName
        {
            get { return m_BarberName; }
            set { m_BarberName = value; OnPropertyChanged("BarberName"); }
        }

        private bool m_IsBarberOnline;

        public bool IsBarberOnline
        {
            get { return m_IsBarberOnline; }
            set { m_IsBarberOnline = value; OnPropertyChanged("IsBarberOnline"); }
        }

        private bool m_IsBarberOffline;

        public bool IsBarberOffline
        {
            get { return m_IsBarberOffline; }
            set { m_IsBarberOffline = value; OnPropertyChanged("IsBarberOffline"); }
        }

        private string m_Queuing;

        public string Queuing
        {
            get { return m_Queuing; }
            set { m_Queuing = value; OnPropertyChanged("Queuing"); }
        }

        private string m_EstimatedTime;

        public string EstimatedTime
        {
            get { return m_EstimatedTime; }
            set { m_EstimatedTime = value; OnPropertyChanged("EstimatedTime"); }
        }

        private string m_NextAvailPos;

        public string NextAvailPos
        {
            get { return m_NextAvailPos; }
            set { m_NextAvailPos = value; OnPropertyChanged("NextAvailPos"); }
        }

        private string m_ActiveImage;

        public string ActiveImage
        {
            get { return m_ActiveImage; }
            set { m_ActiveImage = value; OnPropertyChanged("ActiveImage"); }
        }

        private string m_BarberOnline;

        public string BarberOnline
        {
            get { return m_BarberOnline; }
            set { m_BarberOnline = value; OnPropertyChanged("BarberOnline"); }
        }

        private string m_CircleImageBorderColor;

        public string CircleImageBorderColor
        {
            get { return m_CircleImageBorderColor; }
            set { m_CircleImageBorderColor = value; OnPropertyChanged("CircleImageBorderColor"); }
        }

        private ObservableCollection<string> _barberServices;

        public ObservableCollection<string> BarberServices
        {
            get { return _barberServices; }
            set { _barberServices = value; OnPropertyChanged("BarberServices"); }
        }

    }

    public class BarbersResultListModel : BaseViewModel
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }
        private string _pos;

        public string Posititon
        {
            get { return _pos; }
            set { _pos = value; OnPropertyChanged("Posititon"); }
        }

        public ICommand DemoCommand { get; set; }

        private bool _Istoggled;

        public bool Istoggled
        {
            get { return _Istoggled; }
            set
            {
                if (_Istoggled != value)
                {
                    _Istoggled = value;
                    OnPropertyChanged("Istoggled");
                }
            }
        }

        //private bool _IsSwitchOn;

        //public bool IsSwitchOn
        //{
        //    get { return _IsSwitchOn; }
        //    set
        //    {
        //        if (_IsSwitchOn != value)
        //        {
        //            _IsSwitchOn = value;
        //            OnPropertyChanged("IsSwitchOn");
        //        }
        //    }
        //}
    }
}