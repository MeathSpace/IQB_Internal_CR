namespace IQB.ViewModel.AuthViewModel
{
    using Base;
    using System;

    public class EnableViewModel : BaseViewModel, IDisposable
    {
        public EnableViewModel()
        {
            IsEnabled = true;
        }

        private bool m_IsEnabled;
        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }

        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }
}
