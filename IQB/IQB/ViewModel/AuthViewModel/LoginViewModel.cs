namespace IQB.ViewModel.AuthViewModel
{
    using IQB.Utility;
    using Base;
    using EntityLayer.Common;
    using System;
    using System.Linq;
    using Xamarin.Auth;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel, IDisposable
    {
        private string UserName = string.Empty;
        private string Name = string.Empty;
        private string UserProfileImageName = string.Empty;
        private int UserId = default(int);
        private bool IsRemembered = false;

        //Might get deleted
        private string FirstName = string.Empty;

        private string LastName = string.Empty;
        private string password = string.Empty;
        private string DOB = string.Empty;
        private string MobileNo = string.Empty;
        private string RateSystem = string.Empty;
        private string RateScore = string.Empty;
        private int IsMailEnabled = 0;

        public LoginViewModel()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {


                IsEnabled = true;
                IsToggled = false;

                IsForgotPasswordTapped = false;
                IsRegistrationTapped = false;

                Account account = AccountStore.Create().FindAccountsForService(CommonEL.LoginServiceName).FirstOrDefault();
                //account.Properties["IsMailEnabled"] = "0";

                if (account != null)
                {
                    IsRemembered = Convert.ToBoolean(account.Properties["IsRemembered"]);
                    UserName = account.Username;
                    Name = account.Properties["Name"];
                    errLog.WinrtErrLogTest(Name);
                    UserProfileImageName = account.Properties["UserProfileImageName"];
                    UserId = Convert.ToInt32(account.Properties["UserId"]);

                    FirstName = account.Properties["FirstName"];
                    LastName = account.Properties["LastName"];
                    password = account.Properties["password"];
                    DOB = account.Properties["DOB"];
                    MobileNo = account.Properties["MobileNo"];
                    RateSystem = account.Properties["RateSystem"];
                    RateScore = account.Properties["RateScore"];
                    IsMailEnabled = !string.IsNullOrEmpty(account.Properties["IsMailEnabled"]) ? Convert.ToInt32(account.Properties["IsMailEnabled"]) : 0;

                    if (IsRemembered)
                    {
                        IsToggled = true;
                        UserEmail = UserName;
                        UserPass = password;
                    }
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        #region public Methods

        public bool IsLoginCredRemembered()
        {
            return IsRemembered;
        }

        public string GetUserName()
        {
            return UserName;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetUserProfileImageName()
        {
            return UserProfileImageName;
        }

        public int GetUserId()
        {
            return UserId;
        }

        public string GetFirstName()
        {
            return FirstName;
        }

        public string GetLastName()
        {
            return LastName;
        }
        public bool GetIsRemembered()
        {
            return IsRemembered;
        }

        public string GetPassword()
        {
            return password;
        }

        public string GetDOB()
        {
            return DOB;
        }

        public string GetRateSystem()
        {
            return RateSystem;
        }
        public string GetRateScore()
        {
            return RateScore;
        }
        public string GetMobileNo()
        {
            return MobileNo;
        }

        public int GetMailInfo()
        {
            return IsMailEnabled;
        }

        public void SetSalonInfo()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                using (SalonViewModel obj = new SalonViewModel())
                {
                    SalonId = obj.GetSalonId();
                    SalonName = "WELCOME TO " + obj.GetSalonName();
                    Salon_ImageName = obj.GetSalonLogo();
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        public void StoreLoginCredData(string Username, string Name, string UserProfileImageName, bool IsRemembered, int UserId)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                Account currentAccount = AccountStore.Create().FindAccountsForService(CommonEL.LoginServiceName).FirstOrDefault();

                if (currentAccount != null)
                {
                    AccountStore.Create().Delete(currentAccount, CommonEL.LoginServiceName);
                }

                Account account = new Account(Username);
                account.Properties.Add("Name", Name);
                account.Properties.Add("UserProfileImageName", UserProfileImageName);
                account.Properties.Add("IsRemembered", Convert.ToString(IsRemembered));
                account.Properties.Add("UserId", Convert.ToString(UserId));

                AccountStore.Create().Save(account, CommonEL.LoginServiceName);
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        //Might get deleted
        public void StoreLoginCredData(string Username, string Name, string UserProfileImageName, bool IsRemembered, int UserId, string FirstName, string LastName, string password, string DOB, string MobileNo, string RateSystem, string RateScore, int IsMailEnabled)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                Account currentAccount = AccountStore.Create().FindAccountsForService(CommonEL.LoginServiceName).FirstOrDefault();

                if (currentAccount != null)
                {
                    AccountStore.Create().Delete(currentAccount, CommonEL.LoginServiceName);
                }

                Account account = new Account(Username);
                account.Properties.Add("Name", Name);
                account.Properties.Add("UserProfileImageName", UserProfileImageName);
                account.Properties.Add("IsRemembered", Convert.ToString(IsRemembered));
                account.Properties.Add("UserId", Convert.ToString(UserId));

                account.Properties.Add("FirstName", Convert.ToString(FirstName ?? ""));
                account.Properties.Add("LastName", Convert.ToString(LastName ?? ""));
                account.Properties.Add("password", Convert.ToString(password ?? ""));
                account.Properties.Add("DOB", DOB??"");
                account.Properties.Add("MobileNo", MobileNo ?? "");
                account.Properties.Add("RateSystem", RateSystem ?? "");
                account.Properties.Add("RateScore", RateScore ?? "");
                account.Properties.Add("IsMailEnabled", Convert.ToString(IsMailEnabled));


                AccountStore.Create().Save(account, CommonEL.LoginServiceName);
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        public static void DestroyLoginCredData()
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                Account currentAccount = AccountStore.Create().FindAccountsForService(CommonEL.LoginServiceName).FirstOrDefault();

                if (currentAccount != null)
                {
                    AccountStore.Create().Delete(currentAccount, CommonEL.LoginServiceName);
                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
        }

        #endregion public Methods

        #region Controls

        private int m_SalonId;

        public int SalonId
        {
            get { return m_SalonId; }
            set { m_SalonId = value; OnPropertyChanged("SalonId"); }
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

        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }

        private bool m_IsForgotPasswordTapped;

        public bool IsForgotPasswordTapped
        {
            get { return m_IsForgotPasswordTapped; }
            set { m_IsForgotPasswordTapped = value; OnPropertyChanged("IsForgotPasswordTapped"); }
        }

        private bool m_IsRegistrationTapped;

        public bool IsRegistrationTapped
        {
            get { return m_IsRegistrationTapped; }
            set { m_IsRegistrationTapped = value; OnPropertyChanged("IsRegistrationTapped"); }
        }

        private string m_UserEmail;

        public string UserEmail
        {
            get { return m_UserEmail; }
            set { m_UserEmail = value; OnPropertyChanged("UserEmail"); }
        }

        private string m_UserPass;

        public string UserPass
        {
            get { return m_UserPass; }
            set { m_UserPass = value; OnPropertyChanged("UserPass"); }
        }

        private bool m_IsToggled;

        public bool IsToggled
        {
            get { return m_IsToggled; }
            set { m_IsToggled = value; OnPropertyChanged("IsToggled"); }
        }

        #endregion Controls

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose

        public FormattedString RegisterText
        {
            get
            {
                return new FormattedString
                {
                    Spans =
            {
                new Span
                {
                    Text ="Don't have an account yet? ",
                    ForegroundColor=Color.Black


                },
                new Span
                {
                    Text = "Register now!",
                    ForegroundColor=Color.Red
                }
            }
                };
            }
        }
    }
}