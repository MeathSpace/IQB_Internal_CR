using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using IQB.Views.Home;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static IQB.EntityLayer.AuthModel.BarberModel;

namespace IQB.ViewModel.HomeManagement
{
    public class GroupJoinViewModel : BaseViewModel
    {
        public ObservableCollection<string> BarberList = null;
        public ObservableCollection<string> BarberServiceList = null;
        private ObservableCollection<string> BarberServices = null;
        public GroupJoinViewModel()
        {
            GroupJoinIsEnabled = false;
            GroupJoinList = new ObservableCollection<GroupListModel>();

            BindBarbersList();
            SetBarberList();
            BindBarbersServiceList();
            SetDefaultMenuList();
        }

        private List<string> _BindBarberName;

        public List<string> BindBarberName
        {
            get { return _BindBarberName; }
            set { _BindBarberName = value; OnPropertyChanged(nameof(BindBarberName)); }
        }
        public bool CheckIfJoinQueueEnabled()
        {
            if (GroupJoinList != null && GroupJoinList.Count() > 0)
            {
                if (GroupJoinList.Where(t => t.IsAdminEnabled == true && t.IsSelected == true).Count() > 0 && GroupJoinList.Where(t => t.IsAdminEnabled == false && t.IsSelected == true).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public void SetDefaultMenuList()
        {
            LoginViewModel objLogin = new LoginViewModel();
            GroupListModel User1 = new GroupListModel()
            {
                Id = 1,
                UserName = objLogin.GetFirstName() + " " + objLogin.GetLastName(),
                BarberName = "Select Barber",

                //gray_tick.png
                DefaultTickImage = "grey_tick.png",
                HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg",
                GreenTickImage = "green_tick.png",
                //DefaultJoinImage = "green_icon.png",
                DefaultJoinImage = "green_user.png",
                AddJoinColor = "#0DA5B7",
                RemoveJoinColor = "Gray",
                BarberServiceName = SelectServiceText.ToString(),
                //add_user.png
                JoinedImage = "add_user.png",
                //deactivegray.png.png
                DefaultRemoveJoinImage = "removeusergray.png",
                //red_icon.png
                RedRemoveJoinImage = "remove_user.png",
                JoinIsEnabled = true,
                RemoveJoinIsEnabled = false,
                IsSelected = false,
                BarberList = BarberList,
                BarberServiceList = BarberServiceList,
                NameIsEnabled = false,
                IsAdminEnabled = true,
                BarberNameForPanel = "",
                ServiceNameForPanel = "",
                ServicePriceForPanel = ""
            };
            GroupListModel User2 = new GroupListModel()
            {
                Id = 2,
                UserName = "",
                BarberName = SelectText.ToString(),
                BarberServiceName = SelectServiceText.ToString(),
                DefaultTickImage = "grey_tick.png",
                HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg",
                AddJoinColor = "#0DA5B7",
                RemoveJoinColor = "Gray",
                GreenTickImage = "green_tick.png",
                DefaultJoinImage = "green_user.png",
                JoinedImage = "add_user.png",
                DefaultRemoveJoinImage = "removeusergray.png",
                RedRemoveJoinImage = "remove_user.png",
                JoinIsEnabled = true,
                RemoveJoinIsEnabled = false,
                IsSelected = false,
                BarberList = BarberList,
                BarberServiceList = BarberServiceList,
                NameIsEnabled = true,
                IsAdminEnabled = false,
                BarberNameForPanel = "",
                ServiceNameForPanel = "",
                ServicePriceForPanel = ""
            };
            GroupListModel User3 = new GroupListModel()
            {
                Id = 3,
                UserName = "",
                BarberName = SelectText.ToString(),
                BarberServiceName = SelectServiceText.ToString(),
                DefaultTickImage = "grey_tick.png",
                HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg",
                AddJoinColor = "#0DA5B7",
                RemoveJoinColor = "Gray",
                GreenTickImage = "green_tick.png",
                DefaultJoinImage = "green_user.png",
                JoinedImage = "add_user.png",
                DefaultRemoveJoinImage = "removeusergray.png",
                RedRemoveJoinImage = "remove_user.png",
                JoinIsEnabled = true,
                RemoveJoinIsEnabled = false,
                IsSelected = false,
                BarberList = BarberList,
                BarberServiceList = BarberServiceList,
                NameIsEnabled = true,
                IsAdminEnabled = false,
                BarberNameForPanel = "",
                ServiceNameForPanel = "",
                ServicePriceForPanel = ""
            };
            GroupListModel User4 = new GroupListModel()
            {
                Id = 4,
                UserName = "",
                BarberName = SelectText.ToString(),
                BarberServiceName = SelectServiceText.ToString(),
                DefaultTickImage = "grey_tick.png",
                HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg",
                AddJoinColor = "#0DA5B7",
                RemoveJoinColor = "Gray",
                GreenTickImage = "green_tick.png",
                DefaultJoinImage = "green_user.png",
                JoinedImage = "add_user.png",
                DefaultRemoveJoinImage = "removeusergray.png",
                RedRemoveJoinImage = "remove_user.png",
                JoinIsEnabled = true,
                RemoveJoinIsEnabled = false,
                IsSelected = false,
                BarberList = BarberList,
                BarberServiceList = BarberServiceList,
                NameIsEnabled = true,
                IsAdminEnabled = false,
                BarberNameForPanel = "",
                ServiceNameForPanel = "",
                ServicePriceForPanel = ""
            };
            GroupListModel User5 = new GroupListModel()
            {
                Id = 5,
                UserName = "",
                BarberName = SelectText.ToString(),
                BarberServiceName = SelectServiceText.ToString(),
                DefaultTickImage = "grey_tick.png",
                HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg",
                AddJoinColor = "#0DA5B7",
                RemoveJoinColor = "Gray",
                GreenTickImage = "green_tick.png",
                DefaultJoinImage = "green_user.png",
                JoinedImage = "add_user.png",
                DefaultRemoveJoinImage = "removeusergray.png",
                RedRemoveJoinImage = "remove_user.png",
                JoinIsEnabled = true,
                RemoveJoinIsEnabled = false,
                IsSelected = false,
                BarberList = BarberList,
                BarberServiceList = BarberServiceList,
                NameIsEnabled = true,
                IsAdminEnabled = false,
                BarberNameForPanel = "",
                ServiceNameForPanel = "",
                ServicePriceForPanel = ""
            };
            GroupJoinList.Add(User1);
            GroupJoinList.Add(User2);
            GroupJoinList.Add(User3);
            GroupJoinList.Add(User4);
            GroupJoinList.Add(User5);
        }

        public void ChangeGroupJoinList(int id, string jointype)
        {
            for (int i = 0; i < GroupJoinList.Count(); i++)
            {
                if (GroupJoinList[i].Id == id)
                {
                    if (jointype == "join")
                    {
                        GroupJoinList[i].DefaultTickImage = GroupJoinList[i].GreenTickImage;
                        GroupJoinList[i].HexDefaultTickImage = "resource://IQB.Resources.Image.correctPrimary.svg";
                        GroupJoinList[i].AddJoinColor = "Gray";
                        GroupJoinList[i].RemoveJoinColor = "#0DA5B7";
                        GroupJoinList[i].DefaultJoinImage = GroupJoinList[i].JoinedImage;
                        GroupJoinList[i].DefaultRemoveJoinImage = GroupJoinList[i].RedRemoveJoinImage;
                        GroupJoinList[i].JoinIsEnabled = false;
                        GroupJoinList[i].RemoveJoinIsEnabled = true;
                        GroupJoinList[i].IsSelected = true;
                        GroupJoinList[i].NameIsEnabled = false; //added for non editable text after groupjoin
                    }
                    if (jointype == "remove")
                    {
                        GroupJoinList[i].DefaultTickImage = "grey_tick.png";
                        GroupJoinList[i].HexDefaultTickImage = "resource://IQB.Resources.Image.correct.svg";
                        GroupJoinList[i].AddJoinColor = "#0DA5B7";
                        GroupJoinList[i].RemoveJoinColor = "Gray";
                        GroupJoinList[i].DefaultJoinImage = "green_user.png";
                        GroupJoinList[i].DefaultRemoveJoinImage = "removeusergray.png";
                        GroupJoinList[i].JoinIsEnabled = true;
                        GroupJoinList[i].RemoveJoinIsEnabled = false;
                        GroupJoinList[i].IsSelected = false;
                        GroupJoinList[i].NameIsEnabled = true;
                        if (id == 1)
                        {
                            //GroupJoinList[i].BarberName = "Select Barber";
                            GroupJoinList[i].BarberName = SelectText.ToString();


                        }
                        else
                        {
                            //GroupJoinList[i].BarberName = "Select Barber";
                            GroupJoinList[i].BarberName = SelectText.ToString();
                            GroupJoinList[i].UserName = "";
                        }
                    }
                }
            }
        }
        public bool ValidateUserNameandBarber(int id)
        {
            bool ValidateUser = false;
            for (int i = 0; i < GroupJoinList.Count(); i++)
            {
                if (GroupJoinList[i].Id == id)
                {
                    if (!string.IsNullOrWhiteSpace(GroupJoinList[i].UserName) && GroupJoinList[i].BarberNameForPanel != "Select Barber" && GroupJoinList[i].BarberNameForPanel != "")
                    {
                        ValidateUser = true;
                    }
                }
            }
            return ValidateUser;
        }
        public void SetBarberName(int id, string BarberName)
        {
            for (int i = 0; i < GroupJoinList.Count(); i++)
            {
                if (GroupJoinList[i].Id == id)
                {
                    if (BarberName != null)
                    {
                        GroupJoinList[i].BarberName = BarberName;
                    }
                }
            }
        }
        public async void BindBarbersList()
        {
            BarberList = new ObservableCollection<string>();
            int salonId = 0;
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
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
                BarberList.Add("Any Barber");
                foreach (var item in result.Where(x => x.BarberOnline == "Y"))
                {
                    BarberList.Add(item.BarberName);
                    BarberList.Add(item.BarberId.ToString());
                }
                BindBarberName = BarberList.ToList();
            }
        }

        public async void SetBarberList()
        {

            int salonId = 0;

            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
            }

            ApiResult res = new ApiResult();
            using (BarberService obj = new BarberService())
            {
                res = await obj.BarberSelect(salonId);
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberReturnResult> result = UtilityEL.ToModelList<BarberReturnResult>(res.Response);
                BarberListForPopup = new ObservableCollection<BarbersListModel>();
                BarberListForPopup.Add(new BarbersListModel { Id = 0, BarberId = 777777, BarberImage = "noimage.png", BarberName = "Any Barber", EstimatedTime = null });
                foreach (var item in result)
                {
                    BarbersListModel Barber = new BarbersListModel();
                    Barber.Id = item.id;
                    Barber.BarberId = item.BarberId;
                    if (string.IsNullOrEmpty(item.BarberPic))
                    {
                        Barber.BarberImage = "noimage.png";
                    }
                    else
                    {
                        Barber.BarberImage = CommonEL.BarberImageLocation + "barbers_profile_pics" + "/" + item.BarberPic;
                    }

                    if (item.BarberOnline == "N")
                    {
                        //Barber.IsBarberOnline = false;
                        Barber.IsBarberOffline = true;
                        Barber.CircleImageBorderColor = "Red";
                    }
                    else
                    {
                      //  Barber.IsBarberOnline = true;
                        Barber.IsBarberOffline = false;
                        Barber.CircleImageBorderColor = "Green";
                    }

                    Barber.BarberName = item.BarberName;
                    Barber.EstimatedTime = item.EWT;
                    BarberListForPopup.Add(Barber);
                }

            }
        }
        public async void BindBarbersServiceList()
        {
            BarberServiceList = new ObservableCollection<string>();
            int salonId = 0;
            using (SalonViewModel objSalon = new SalonViewModel())
            {
                salonId = objSalon.GetSalonId();
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
                BarberServiceList.Add("Any Service");
                foreach (var item in result.Where(x => x.BarberOnline == "Y"))
                {
                    BarberServiceList.Add(item.Position);
                }
            }
        }

        private ObservableCollection<GroupListModel> m_GroupJoinList;

        public ObservableCollection<GroupListModel> GroupJoinList
        {
            get { return m_GroupJoinList; }
            set { m_GroupJoinList = value; OnPropertyChanged("GroupJoinList"); }
        }
        private bool m_GroupJoinIsEnabled;
        public bool GroupJoinIsEnabled
        {
            get { return m_GroupJoinIsEnabled; }
            set { m_GroupJoinIsEnabled = value; OnPropertyChanged("GroupJoinIsEnabled"); }
        }

        public FormattedString HintText
        {
            get
            {
                return new FormattedString
                {
                    Spans =
            {
                new Span
                {
                    Text ="Hint:\nEnter full name, press select option and choose a barber and service then press '+' button to confirm the entry, once all guests have been added press the Join Queue button at the end",
                }
            }
                };
            }
        }

        private bool m_IsLoaderEnabled;
        public bool IsLoaderEnabled
        {
            get { return m_IsLoaderEnabled; }
            set { m_IsLoaderEnabled = value; OnPropertyChanged("IsLoaderEnabled"); }
        }
        //
        private bool _PriceBlankOrNot;

        public bool PriceBlankOrNot
        {
            get { return _PriceBlankOrNot; }
            set { _PriceBlankOrNot = value; OnPropertyChanged("PriceBlankOrNot"); }
        }

        private int _BarberIdForPanel;

        public int BarberIdForPanel
        {
            get { return _BarberIdForPanel; }
            set { _BarberIdForPanel = value; OnPropertyChanged("BarberIdForPanel"); }
        }


        private string _EstimatedWaitTime;

        public string EstimatedWaitTime
        {
            get { return _EstimatedWaitTime; }
            set { _EstimatedWaitTime = value; OnPropertyChanged("EstimatedWaitTime"); }
        }

        private string _SelectBarberName;

        public string SelectBarberName
        {
            get { return _SelectBarberName; }
            set { _SelectBarberName = value; OnPropertyChanged("SelectBarberName"); }
        }

       

        private string _btnText = "Select Service";

        public string btnText
        {
            get { return _btnText; }
            set { _btnText = value; OnPropertyChanged("btnText"); }
        }

        private string _SelectedServicePrice = "0.00";

        public string SelectedServicePrice
        {
            get { return _SelectedServicePrice; }
            set { _SelectedServicePrice = value; OnPropertyChanged("SelectedServicePrice"); }
        }

        private int _SelectedServiceId;

        public int SelectedServiceId
        {
            get { return _SelectedServiceId; }
            set { _SelectedServiceId = value; OnPropertyChanged("SelectedServiceId"); }
        }

        private string _barberBtnText = "Select Barber";

        public string barberBtnText
        {
            get { return _barberBtnText; }
            set { _barberBtnText = value; OnPropertyChanged("barberBtnText"); }
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }

        //private bool _IsPanelVisible;

        //public bool IsPanelVisible
        //{
        //    get { return _IsPanelVisible; }
        //    set { _IsPanelVisible = value; OnPropertyChanged("IsPanelVisible"); }
        //}

        private ObservableCollection<BarbersListModel> m_BarberListForPopup = new ObservableCollection<BarbersListModel>();

        public ObservableCollection<BarbersListModel> BarberListForPopup
        {
            get { return m_BarberListForPopup; }
            set { m_BarberListForPopup = value; OnPropertyChanged("BarberListForPopup"); }
        }

        public FormattedString SelectText
        {
            get
            {
                return new FormattedString
                {
                    Spans =
            {
                new Span
                {
                    Text ="Select Barber",
                    ForegroundColor= Color.White
                }
            }
                };
            }
        }
        public FormattedString SelectServiceText
        {
            get
            {
                return new FormattedString
                {
                    Spans =
            {
                new Span
                {
                    Text ="Select Service",
                    ForegroundColor= Color.White
                }
            }
                };
            }
        }

    }

    public class GroupListModel : BaseViewModel
    {
        private ObservableCollection<string> m_BarberList;
        public ObservableCollection<string> BarberList
        {
            get { return m_BarberList; }
            set { m_BarberList = value; OnPropertyChanged("BarberList"); }
        }

        private int m_Id;
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; OnPropertyChanged("Id"); }
        }

        private string m_UserName;
        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; OnPropertyChanged("UserName"); }
        }

        private string m_BarberNameForPanel = "NA";
        public string BarberNameForPanel
        {
            get { return m_BarberNameForPanel; }
            set { m_BarberNameForPanel = value; OnPropertyChanged("BarberNameForPanel"); }
        }

        private string m_ServicePriceForPanel = "NA";
        public string ServicePriceForPanel
        {
            get { return m_ServicePriceForPanel; }
            set { m_ServicePriceForPanel = value; OnPropertyChanged("ServicePriceForPanel"); }
        }

        private string m_ServiceNameForPanel = "NA";
        public string ServiceNameForPanel
        {
            get { return m_ServiceNameForPanel; }
            set { m_ServiceNameForPanel = value; OnPropertyChanged("ServiceNameForPanel"); }
        }

        private int _ServiceIdForPanel;

        public int ServiceIdForPanel
        {
            get { return _ServiceIdForPanel; }
            set { _ServiceIdForPanel = value; OnPropertyChanged("ServiceIdForPanel"); }
        }


        private bool m_IsPanelVisible;
        public bool IsPanelVisible
        {
            get { return m_IsPanelVisible; }
            set { m_IsPanelVisible = value; OnPropertyChanged("IsPanelVisible"); }
        }

        private int _BarberID;

        public int BarberID
        {
            get { return _BarberID; }
            set { _BarberID = value; OnPropertyChanged("BarberID"); }
        }

        private string m_BarberName;
        public string BarberName
        {
            get { return m_BarberName; }
            set { m_BarberName = value; OnPropertyChanged("BarberName"); }
        }

        private string m_BarberServiceName;
        public string BarberServiceName
        {
            get { return m_BarberServiceName; }
            set { m_BarberServiceName = value; OnPropertyChanged("BarberServiceName"); }
        }


        private string m_HexDefaultTickImage;
        public string HexDefaultTickImage
        {
            get { return m_HexDefaultTickImage; }
            set { m_HexDefaultTickImage = value; OnPropertyChanged("HexDefaultTickImage"); }
        }

        private string _AddJoinColor;

        public string AddJoinColor
        {
            get { return _AddJoinColor; }
            set { _AddJoinColor = value; OnPropertyChanged("AddJoinColor"); }
        }

        private string _RemoveJoinColor;

        public string RemoveJoinColor
        {
            get { return _RemoveJoinColor; }
            set { _RemoveJoinColor = value; OnPropertyChanged("RemoveJoinColor"); }
        }



        private ImageSource m_DefaultTickImage;
        public ImageSource DefaultTickImage
        {
            get { return m_DefaultTickImage; }
            set { m_DefaultTickImage = value; OnPropertyChanged("DefaultTickImage"); }
        }

        private ImageSource m_GreenTickImage;
        public ImageSource GreenTickImage
        {
            get { return m_GreenTickImage; }
            set { m_GreenTickImage = value; OnPropertyChanged("GreenTickImage"); }
        }

        private ImageSource m_DefaultJoinImage;
        public ImageSource DefaultJoinImage
        {
            get { return m_DefaultJoinImage; }
            set { m_DefaultJoinImage = value; OnPropertyChanged("DefaultJoinImage"); }
        }
        private ImageSource m_JoinedImage;
        public ImageSource JoinedImage
        {
            get { return m_JoinedImage; }
            set { m_JoinedImage = value; OnPropertyChanged("JoinedImage"); }
        }

        private ImageSource m_DefaultRemoveJoinImage;
        public ImageSource DefaultRemoveJoinImage
        {
            get { return m_DefaultRemoveJoinImage; }
            set { m_DefaultRemoveJoinImage = value; OnPropertyChanged("DefaultRemoveJoinImage"); }
        }
        private ImageSource m_RedRemoveJoinImage;
        public ImageSource RedRemoveJoinImage
        {
            get { return m_RedRemoveJoinImage; }
            set { m_RedRemoveJoinImage = value; OnPropertyChanged("RedRemoveJoinImage"); }
        }
        //private ObservableCollection<string> m_BarberList;
        //public ObservableCollection<string> BarberList
        //{
        //    get { return m_BarberList; }
        //    set { m_BarberList = value; OnPropertyChanged("BarberList"); }
        //}
        private ObservableCollection<string> m_BarberServiceList;
        public ObservableCollection<string> BarberServiceList
        {
            get { return m_BarberServiceList; }
            set { m_BarberServiceList = value; OnPropertyChanged("BarberServiceList"); }
        }
        private bool m_JoinIsEnabled;
        public bool JoinIsEnabled
        {
            get { return m_JoinIsEnabled; }
            set { m_JoinIsEnabled = value; OnPropertyChanged("JoinIsEnabled"); }
        }
        private bool m_RemoveJoinIsEnabled;
        public bool RemoveJoinIsEnabled
        {
            get { return m_RemoveJoinIsEnabled; }
            set { m_RemoveJoinIsEnabled = value; OnPropertyChanged("RemoveJoinIsEnabled"); }
        }

        private bool m_IsSelected;
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value; OnPropertyChanged("IsSelected"); }
        }
        private bool m_NameIsEnabled;
        public bool NameIsEnabled
        {
            get { return m_NameIsEnabled; }
            set { m_NameIsEnabled = value; OnPropertyChanged("NameIsEnabled"); }
        }
        private bool m_IsAdminEnabled;
        public bool IsAdminEnabled
        {
            get { return m_IsAdminEnabled; }
            set { m_IsAdminEnabled = value; OnPropertyChanged("IsAdminEnabled"); }
        }

        private int _BarberIDForPanel;

        public int BarberIDForPanel
        {
            get { return _BarberIDForPanel; }
            set { _BarberIDForPanel = value; OnPropertyChanged("BarberIDForPanel"); }
        }

    }
}
