using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.GenericModels;
using IQB.IQBServices.AdminServices;
using IQB.IQBServices.GenericServices;
using IQB.ViewModel.AuthViewModel;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ViewModel.AdminManagement
{
    public class EmailViewModel : BaseViewModel
    {
        public EmailViewModel()
        {
            IsRefreshing = true;
            IsEnabled = true;
            IsBackIconEnabled = true;

            IsRefreshing = false;

            PageHeading = "Email";
        }
        private string m_PageHeading;

        public string PageHeading
        {
            get { return m_PageHeading; }
            set { m_PageHeading = value; OnPropertyChanged("PageHeading"); }
        }

        private bool m_IsRefreshing;

        public bool IsRefreshing
        {
            get { return m_IsRefreshing; }
            set { m_IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
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

        private string _SalonText;

        public string SalonText
        {
            get { return _SalonText; }
            set { _SalonText = value; OnPropertyChanged("SalonText"); }
        }

        private int _SalonID;

        public int SalonID
        {
            get { return _SalonID; }
            set { _SalonID = value; OnPropertyChanged("SalonID"); }
        }

        private string _emailBody;
        public string EmailBody
        {
            get { return _emailBody; }
            set { _emailBody = value; OnPropertyChanged("EmailBody"); }
        }
        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged("Subject"); }
        }
        private bool _sendToAdmin;
        public bool SendToAdmin
        {
            get { return _sendToAdmin; }
            set { _sendToAdmin = value; OnPropertyChanged("SendToAdmin"); }
        }
        private string _from;
        public string From
        {
            get { return _from; }
            set { _from = value; OnPropertyChanged("From"); }
        }
        private string _to;
        public string To
        {
            get { return _to; }
            set { _to = value; OnPropertyChanged("To"); }
        }
    }
}
