namespace IQB.ViewModel.AuthViewModel
{
    using Base;
    using System;
    using System.Globalization;
    using System.Text;

    public class MyProfileViewModel : BaseViewModel
    {

        private IFormatProvider provider = new CultureInfo("en-US");

        #region Methods

        public void GetProfileInfo()
        {
            IsEnabled = true;
            using (LoginViewModel obj = new LoginViewModel())
            {
                FirstName = obj.GetFirstName();
                LastName = obj.GetLastName();
                Password = obj.GetPassword();
                MobileNo = obj.GetMobileNo();
                RateSystem = obj.GetRateSystem();
                RatingScore = obj.GetRateScore();
                EmailEnabled = obj.GetMailInfo();
          //    FullName = "Hello " + obj.GetFirstName() + " " + obj.GetLastName();
                FullName = obj.GetFirstName() + " " + obj.GetLastName();
                string DateOfBirth = obj.GetDOB();

                //DOB = !string.IsNullOrWhiteSpace(DateOfBirth) ? Convert.ToDateTime(DateOfBirth, provider) : Convert.ToDateTime(DateTime.Now, provider);
                if (!string.IsNullOrWhiteSpace(DateOfBirth))
                {
                    isDateVisible = true;
                    DOB = Convert.ToDateTime(DateOfBirth, provider);
                }
                else
                {
                    DOBvisible = true;
                }

                ProfileImage = obj.GetUserProfileImageName();
                Email = obj.GetUserName();
                UserId = obj.GetUserId();

                NewDobFormat = GetFormattedDate(DateOfBirth);
            }

            using (SalonViewModel objsalon = new SalonViewModel())
            {
                SalonId = objsalon.GetSalonId();
            }

          

            IsProfileApiRunning = false;
            IsVisibleProfileDetails = true;
            PropertyMaximumDate = DateTime.Now;
            PropertyMinimumDate = DateTime.Now.AddYears(-80);
        }

        private string GetFormattedDate(string dt)
        {
            if(dt != null)
            {
                StringBuilder sb;
                //switch()
                //{
                //    case 1:
                //        sb = 
                //}
                return "";
            }
            else
            return "";
        }

        #endregion Methods

        #region Controls

        private int m_EmailEnabled;

        public int EmailEnabled
        {
            get { return m_EmailEnabled; }
            set { m_EmailEnabled = value; OnPropertyChanged("EmailEnabled"); }
        }

        private int m_UserId;

        public int UserId
        {
            get { return m_UserId; }
            set { m_UserId = value; OnPropertyChanged("UserId"); }
        }

        private int m_SalonId;

        public int SalonId
        {
            get { return m_SalonId; }
            set { m_SalonId = value; OnPropertyChanged("SalonId"); }
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set { m_FirstName = value; OnPropertyChanged("FirstName"); }
        }


        private bool m_DOBvisible;

        public bool DOBvisible
        {
            get { return m_DOBvisible; }
            set { m_DOBvisible = value; OnPropertyChanged("DOBvisible"); }
        }


        private bool m_isDateVisible;

        public bool isDateVisible
        {
            get { return m_isDateVisible; }
            set { m_isDateVisible = value; OnPropertyChanged("isDateVisible"); }
        }
        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value; OnPropertyChanged("LastName"); }
        }

        private string m_RateSystem;

        public string RateSystem
        {
            get { return m_RateSystem; }
            set { m_RateSystem = value; OnPropertyChanged("RateSystem"); }
        }
        private string m_RatingScore;

        public string RatingScore
        {
            get { return m_RatingScore; }
            set { m_RatingScore = value; OnPropertyChanged("RatingScore"); }
        }
        private string m_Password;


        private string new_DOB_Formatted;

        public string NewDobFormat
        {
            get { return new_DOB_Formatted; }
            set { new_DOB_Formatted = value; OnPropertyChanged("NewDobFormat"); }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; OnPropertyChanged("Password"); }
        }

        private string m_MobileNo;

        public string MobileNo
        {
            get { return m_MobileNo; }
            set { m_MobileNo = value; OnPropertyChanged("MobileNo"); }
        }

        private DateTime? m_DOB;

        public DateTime? DOB
        {
            get { return m_DOB; }
            set { m_DOB = value; OnPropertyChanged("DOB"); }
        }

        private bool m_IsClearClicked;

        public bool IsClearClicked
        {
            get { return m_IsClearClicked; }
            set { m_IsClearClicked = value; OnPropertyChanged("IsClearClicked"); }
        }

        private string m_ProfileImage;

        public string ProfileImage
        {
            get { return m_ProfileImage; }
            set { m_ProfileImage = value; OnPropertyChanged("ProfileImage"); }
        }

        private string m_Email;

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; OnPropertyChanged("Email"); }
        }

        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }

        private bool m_IsProfileApiRunning;

        public bool IsProfileApiRunning
        {
            get { return m_IsProfileApiRunning; }
            set { m_IsProfileApiRunning = value; OnPropertyChanged("IsProfileApiRunning"); }
        }
        private bool m_IsVisibleProfileDetails;

        public bool IsVisibleProfileDetails
        {
            get { return m_IsVisibleProfileDetails; }
            set { m_IsVisibleProfileDetails = value; OnPropertyChanged("IsVisibleProfileDetails"); }
        }
        private DateTime m_PropertyMaximumDate;

        public DateTime PropertyMaximumDate
        {
            get { return m_PropertyMaximumDate; }
            set { m_PropertyMaximumDate = value; OnPropertyChanged("PropertyMaximumDate"); }
        }
        private DateTime m_PropertyMinimumDate;

        public DateTime PropertyMinimumDate
        {
            get { return m_PropertyMinimumDate; }
            set { m_PropertyMinimumDate = value; OnPropertyChanged("PropertyMinimumDate"); }
        }
        private string m_FullName;

        public string FullName
        {
            get { return m_FullName; }
            set { m_FullName = value; OnPropertyChanged("FullName"); }
        }
        #endregion Controls
    }
}