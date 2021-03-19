using IQB.EntityLayer.Common;
using IQB.IQBServices.HomeServices;
using IQB.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IQB.ViewModel.AuthViewModel
{
    public class HelpViewModel : BaseViewModel, IDisposable
    {
        private bool isFetchingLoad;
        public bool IsFetchingLoad
        {
            get
            {
                return isFetchingLoad;
            }
            set
            {
                isFetchingLoad = value;
                OnPropertyChanged("IsFetchingLoad");
            }
        }

        public HelpViewModel()
        {
            GetInfo();
        }

        public async void GetInfo()
        {
            ApiResult res = new ApiResult();
            IsFetchingLoad = true;

            using (BarberService obj = new BarberService())
            {
                res = await obj.GetSupportInfo("1");
            }

            if (res != null && res.StatusCode == 200)
            {
                //HelpViewModel data = UtilityEL.ToModel<HelpViewModel>(res.Response);
                HelpdataModel data = UtilityEL.ToModel<HelpdataModel>(res.Response);

                if (data != null)
                {
                    // id = data.id;
                    //ContactTel = "Support Telephone: " + data.ContactTel;
                    //EmailSupport = "Support Email: " + data.EmailSupport;
                    ContactTel = data.ContactTel;
                    EmailSupport = data.EmailSupport;
                    HelpText = "If you are finding any difficulties using this app please contact support using the following contact details.";
                   // EmailSales = data.EmailSales;
                }
                IsFetchingLoad = false;
            }
        }

        private string m_id;
        public string id
        {
            get { return m_id; }
            set { m_id = value; OnPropertyChanged("id"); }
        }
        private string m_ContactTel;
        public string ContactTel
        {
            get { return m_ContactTel; }
            set { m_ContactTel = value; OnPropertyChanged("ContactTel"); }
        }

        private string m_EmailSupport;
        public string EmailSupport
        {
            get { return m_EmailSupport; }
            set { m_EmailSupport = value; OnPropertyChanged("EmailSupport"); }
        }

        private string m_EmailSales;
        public string EmailSales
        {
            get { return m_EmailSales; }
            set { m_EmailSales = value; OnPropertyChanged("EmailSales"); }
        }

        private string m_HelpText;
        public string HelpText
        {
            get { return m_HelpText; }
            set { m_HelpText = value; OnPropertyChanged("HelpText"); }
        }


        #region Dispose

        public void Dispose()
        {
        }

        #endregion Dispose
    }

    public class HelpdataModel : BaseViewModel
    {

        private string m_ContactTel;
        public string ContactTel
        {
            get { return m_ContactTel; }
            set { m_ContactTel = value; OnPropertyChanged("ContactTel"); }
        }

        private string m_EmailSupport;
        public string EmailSupport
        {
            get { return m_EmailSupport; }
            set { m_EmailSupport = value; OnPropertyChanged("EmailSupport"); }
        }
        
    }
}
