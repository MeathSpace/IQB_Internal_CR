namespace IQB.ViewModel.Base
{
    using System.ComponentModel;

    public class BaseViewModel : INotifyPropertyChanged
    {
        private string m_Title = string.Empty;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; OnPropertyChanged("Title"); }
        }

        private string m_Subtitle = string.Empty;

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        public string Subtitle
        {
            get { return m_Subtitle; }
            set { m_Subtitle = value; OnPropertyChanged("Subtitle"); }
        }

        private string m_Icon;

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public string Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; OnPropertyChanged("Icon"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}