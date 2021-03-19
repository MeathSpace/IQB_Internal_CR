namespace IQB.ViewModel.AuthViewModel
{
    using Base;
    using System;

    public class RegisterViewModel : BaseViewModel, IDisposable
    {
        public RegisterViewModel()
        {
            IsEnabled = true;
            SetSalonId();
        }

        #region Methods

        public void SetSalonId()
        {
            using (SalonViewModel obj = new SalonViewModel())
            {
                SalonId = obj.GetSalonId();
                SalonName = obj.GetSalonName();
                Salon_TelephoneNo = obj.GetSalonTel();
                Salon_Link = obj.GetSalonLink();
            }
        }

        #endregion Methods

        #region Controls
        private int m_tabIndex;

        public int TabIndex
        {
            get { return m_tabIndex; }
            set { m_tabIndex = value; OnPropertyChanged("TabIndex"); }
        }
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

        private string m_Salon_TelephoneNo;

        public string Salon_TelephoneNo
        {
            get { return m_Salon_TelephoneNo; }
            set { m_Salon_TelephoneNo = value; OnPropertyChanged("Salon_TelephoneNo"); }
        }

        private string m_Salon_Link;

        public string Salon_Link
        {
            get { return m_Salon_Link; }
            set { m_Salon_Link = value; OnPropertyChanged("Salon_Link"); }
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

        private string m_Email;

        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; OnPropertyChanged("Email"); }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; OnPropertyChanged("Password"); }
        }

        #region AdminReg
        private bool m_IsBarberAdmin;

        public bool IsBarberAdmin
        {
            get { return m_IsBarberAdmin; }
            set { m_IsBarberAdmin = value; OnPropertyChanged("IsBarberAdmin"); }
        }
        private string m_FirstNameAdmin;

        public string FirstNameAdmin
        {
            get { return m_FirstNameAdmin; }
            set { m_FirstNameAdmin = value; OnPropertyChanged("FirstNameAdmin"); }
        }

        private string m_LastNameAdmin;

        public string LastNameAdmin
        {
            get { return m_LastNameAdmin; }
            set { m_LastNameAdmin = value; OnPropertyChanged("LastNameAdmin"); }
        }

        private string m_EmailAdmin;

        public string EmailAdmin
        {
            get { return m_EmailAdmin; }
            set { m_EmailAdmin = value; OnPropertyChanged("EmailAdmin"); }
        }

        private string m_PasswordAdmin;

        public string PasswordAdmin
        {
            get { return m_PasswordAdmin; }
            set { m_PasswordAdmin = value; OnPropertyChanged("PasswordAdmin"); }
        }
        #endregion
        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        private bool m_IsBarber;

        public bool IsBarber
        {
            get { return m_IsBarber; }
            set { m_IsBarber = value; OnPropertyChanged("IsBarber"); }
        }
        #endregion Controls

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}