namespace IQB.ViewModel.AuthViewModel
{
    using Base;
    using EntityLayer.AuthModel;
    using EntityLayer.Common;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Auth;
    using Xamarin.Forms;

    public class SalonViewModel : BaseViewModel, IDisposable
    {
        private int MainSalonId = default(int);
        private string MainSalonName = string.Empty;
        private string SalonCode = string.Empty;
        private string MainSalonLogo = string.Empty;
        private string MainSalonTel = string.Empty;
        private string MainSalonWeb = string.Empty;
        private string MainSalonAddress = string.Empty;
        private string MainSalonState = string.Empty;
        private string MainSalonCity = string.Empty;
        private string MainSalonPostalCode = string.Empty;
        private bool IsAuthenticated = false;
        private string MainSalonBarberImageFolder = string.Empty;
        public AccountStore accountStore { get; set; }
        public SalonViewModel()
        {
            accountStore = AccountStore.Create();
            IsAuthenticated = false;
            IsConnectShowed = true;
            Account account = accountStore.FindAccountsForService(CommonEL.SelonServiceName).FirstOrDefault();

            if (account != null)
            {
                IsAuthenticated = true;
                SalonCode = account.Username;
                MainSalonName = account.Properties["MainSalonName"];
                MainSalonLogo = account.Properties["MainSalonLogo"];
                MainSalonTel = account.Properties["MainSalonTel"];
                MainSalonWeb = account.Properties["MainSalonWeb"];

                MainSalonAddress = account.Properties["MainSalonAddress"];
                MainSalonState = account.Properties["MainSalonState"];
                MainSalonCity = account.Properties["MainSalonCity"];
                MainSalonPostalCode = account.Properties["MainSalonPostalCode"];

                MainSalonId = Convert.ToInt32(account.Properties["MainSalonId"]);
                MainSalonBarberImageFolder= account.Properties["MainSalonBarberImageFolder"];


                SalonAuthModel info = new SalonAuthModel()
                {
                    address = MainSalonAddress,
                    city = MainSalonCity,
                    id = MainSalonId,
                    Link = MainSalonWeb,
                    PostalCode = MainSalonPostalCode,
                    SalonName = MainSalonName,
                    Salon_ImageName = MainSalonLogo,
                    SCode = SalonCode,
                    state = MainSalonState,
                    TelephoneNo = MainSalonTel,
                    BarberImageFtpFolder=MainSalonBarberImageFolder
                };

                this.SetSalonInfo(info,false);
            }
        }

        #region Methods

        public void StoreSalonCode(string salonCode, string MainSalonName, int MainSalonId, string MainSalonLogo, string MainSalonTel, string MainSalonWeb,
            string MainSalonAddress, string MainSalonState, string MainSalonCity, string MainSalonPostalCode,string MainSalonBarberImageFolder)
        {
            Account currentAccount = accountStore.FindAccountsForService(CommonEL.SelonServiceName).FirstOrDefault();

            if (currentAccount != null)
            {
                accountStore.Delete(currentAccount, CommonEL.SelonServiceName);
            }

            Account account = new Account(salonCode);
            account.Properties.Add("MainSalonName", MainSalonName);
            account.Properties.Add("MainSalonLogo", MainSalonLogo);
            account.Properties.Add("MainSalonTel", MainSalonTel);
            account.Properties.Add("MainSalonWeb", MainSalonWeb);
            account.Properties.Add("MainSalonId", Convert.ToString(MainSalonId));

            account.Properties.Add("MainSalonAddress", MainSalonAddress);
            account.Properties.Add("MainSalonState", MainSalonState);
            account.Properties.Add("MainSalonCity", MainSalonCity);
            account.Properties.Add("MainSalonPostalCode", MainSalonPostalCode);
            account.Properties.Add("MainSalonBarberImageFolder", MainSalonBarberImageFolder);

            accountStore.Save(account, CommonEL.SelonServiceName);
        }        
        public static void DestroySalonCodeData()
        {
            Account currentAccount = AccountStore.Create().FindAccountsForService(CommonEL.SelonServiceName).FirstOrDefault();

            if (currentAccount != null)
            {
                AccountStore.Create().Delete(currentAccount, CommonEL.SelonServiceName);
            }
        }

        public string GetSalonCode()
        {
            return SalonCode;
        }

        public string GetSalonName()
        {
            return MainSalonName;
        }

        public string GetSalonLogo()
        {
            return MainSalonLogo;
        }

        public int GetSalonId()
        {
            return MainSalonId;
        }

        public string GetSalonTel()
        {
            return MainSalonTel;
        }

        public string GetSalonLink()
        {
            return MainSalonWeb;
        }

        public string GetSalonBarberFolder()
        {
            return MainSalonBarberImageFolder;
        }

        public bool IsSalonAuthenticated()
        {
            return IsAuthenticated;
        }

        public void SetSalonInfo(SalonAuthModel info, bool setImage)
        {
            SalonId = info.id;
            address = info.address;
            city = info.city;
            Link = info.Link;
            PostalCode = info.PostalCode;
            state = info.state;
            TelephoneNo = info.TelephoneNo;
            SCode = info.SCode;
            SalonName = info.SalonName;
            if (setImage)
            {
                if (!string.IsNullOrWhiteSpace(info.Salon_ImageName))
                {
                    //try
                    //{
                    //    ImageSource img = ImageSource.FromUri(new Uri(CommonEL.SalonImageLocation + info.Salon_ImageName));

                    //    if (img != null)
                    //    {
                    //        Salon_ImageName = CommonEL.SalonImageLocation + info.Salon_ImageName;
                    //    }
                    //    else
                    //    {
                    //        Salon_ImageName = "iqb_logo.png";
                    //    }
                    //}
                    //catch
                    //{
                    //    Salon_ImageName = "iqb_logo.png";
                    //}
                    Salon_ImageName = CommonEL.SalonImageLocation + info.Salon_ImageName;
                }
                else
                {
                    Salon_ImageName = CommonEL.SalonImageLocation + "noimage.jpg";
                }
            }
            else
            {
                Salon_ImageName = info.Salon_ImageName;
            }
            FtpFolderPath = info.BarberImageFtpFolder;
        }

        public async Task ConnectSalon()
        {
            address = "New address";
        }

        #endregion Methods

        #region Controls

        private int m_SalonId;

        public int SalonId
        {
            get { return m_SalonId; }
            set { m_SalonId = value; OnPropertyChanged("SalonId"); }
        }

        private string m_SalonCode;

        public string SCode
        {
            get { return m_SalonCode; }
            set { m_SalonCode = value; OnPropertyChanged("SCode"); }
        }

        private string m_SalonName;

        public string SalonName
        {
            get { return m_SalonName; }
            set { m_SalonName = value; OnPropertyChanged("SalonName"); }
        }

        private string m_Salon_ImageName;

        public string Salon_ImageName
        {
            get { return m_Salon_ImageName; }
            set { m_Salon_ImageName = value; OnPropertyChanged("Salon_ImageName"); }
        }

        private string m_address;

        public string address
        {
            get { return m_address; }
            set { m_address = value; OnPropertyChanged("address"); }
        }

        private string m_city;

        public string city
        {
            get { return m_city; }
            set { m_city = value; OnPropertyChanged("city"); }
        }

        private string m_State;

        public string state
        {
            get { return m_State; }
            set { m_State = value; OnPropertyChanged("state"); }
        }

        private string m_PostalCode;

        public string PostalCode
        {
            get { return m_PostalCode; }
            set { m_PostalCode = value; OnPropertyChanged("PostalCode"); }
        }

        private string m_TelephoneNo;

        public string TelephoneNo
        {
            get { return m_TelephoneNo; }
            set { m_TelephoneNo = value; OnPropertyChanged("TelephoneNo"); }
        }

        private string m_Link;

        public string Link
        {
            get { return m_Link; }
            set { m_Link = value; OnPropertyChanged("Link"); }
        }

        private bool m_IsConnectShowed;

        public bool IsConnectShowed
        {
            get { return m_IsConnectShowed; }
            set { m_IsConnectShowed = value; OnPropertyChanged("IsConnectShowed"); }
        }

        private string m_FtpFolderPath;

        public string FtpFolderPath
        {
            get { return m_FtpFolderPath; }
            set { m_FtpFolderPath = value; OnPropertyChanged("FtpFolderPath"); }
        }

        private Command m_ConnectCommand;
        public ICommand ConnectCommand { get { return m_ConnectCommand ?? (m_ConnectCommand = new Command(async () => await ConnectSalon())); } }

        #endregion Controls

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}