using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ViewModel.HomeManagement
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {

        }
       
    }
    public class RatingModel : BaseViewModel
    {
        private string m_username;
        public string username
        {
            get { return m_username; }
            set { m_username = value; OnPropertyChanged("username"); }
        }

        private string m_ratesystem;
        public string ratesystem
        {
            get { return m_ratesystem; }
            set { m_ratesystem = value; OnPropertyChanged("ratesystem"); }
        }

        private string m_ratingscore;
        public string ratingscore
        {
            get { return m_ratingscore; }
            set { m_ratingscore = value; OnPropertyChanged("ratingscore"); }
        }

        private int m_salonid;
        public int salonid
        {
            get { return m_salonid; }
            set { m_salonid = value; OnPropertyChanged("salonid"); }
        }

        private string m_personname;
        public string personname
        {
            get { return m_personname; }
            set { m_personname = value; OnPropertyChanged("personname"); }
        }

        private string m_salonname;
        public string salonname
        {
            get { return m_salonname; }
            set { m_salonname = value; OnPropertyChanged("salonname"); }
        }

        private string m_daterated;
        public string daterated
        {
            get { return m_daterated; }
            set { m_daterated = value; OnPropertyChanged("daterated"); }
        }
    }
}
