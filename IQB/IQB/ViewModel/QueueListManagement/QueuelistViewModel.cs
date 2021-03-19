namespace IQB.ViewModel.QueueListManagement
{
    using AuthViewModel;
    using Base;
    using EntityLayer.QueueELS;
    using IQBServices.QueueServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QueuelistViewModel : BaseViewModel
    {
        private List<QueueEL> queueList = null;

        public QueuelistViewModel()
        {
            queueList = new List<QueueEL>();
            IsRunning = true;
            TimerRunning = true;
            IsEnabled = true;

            using (SalonViewModel obj = new SalonViewModel())
            {
                SalonId = obj.GetSalonId();
            }

            using (LoginViewModel obj = new LoginViewModel())
            {
                CurrentUserId = obj.GetUserId();
            }
        }

        #region Methods

        public List<QueueEL> GetQueueList()
        {
            return queueList;
        }

        public async Task SetQueueList()
        {
            using (QueueListService obj = new QueueListService())
            {
                int PageNo = 1;
                List<QueueListApiModel> list = await obj.GetQueueList(SalonId, PageNo);

                bool InQueueExist = false;

                queueList = new List<QueueEL>();

                List<QueueEL> tempqueueList = new List<QueueEL>();

                if (list != null && list.Count() > 0)
                {
                    if (list.Where(t => t.Username == Convert.ToString(CurrentUserId)).Count() > 0)
                    {
                        InQueueExist = true;
                    }

                    string QgCode = string.Empty;

                    string CurrentUserId1 = Convert.ToString(CurrentUserId) + "-1";
                    string CurrentUserId2 = Convert.ToString(CurrentUserId) + "-2";
                    string CurrentUserId3 = Convert.ToString(CurrentUserId) + "-3";
                    string CurrentUserId4 = Convert.ToString(CurrentUserId) + "-4";
                    string CurrentUserId5 = Convert.ToString(CurrentUserId) + "-5";
                    string CurrentUserId6 = Convert.ToString(CurrentUserId) + "-6";
                    string CurrentUserId7 = Convert.ToString(CurrentUserId) + "-7";
                    string CurrentUserId8 = Convert.ToString(CurrentUserId) + "-8";
                    string CurrentUserId9 = Convert.ToString(CurrentUserId) + "-9";
                    string CurrentUserId10 = Convert.ToString(CurrentUserId) + "-10";

                    if (list.Where(t => (t.Username == Convert.ToString(CurrentUserId)) || (t.Username == CurrentUserId1) || (t.Username == CurrentUserId2) || (t.Username == CurrentUserId3) || (t.Username == CurrentUserId4) || (t.Username == CurrentUserId5) || (t.Username == CurrentUserId6) || (t.Username == CurrentUserId7) || (t.Username == CurrentUserId8) || (t.Username == CurrentUserId9) || (t.Username == CurrentUserId10)).Count() > 0)
                    {
                        QgCode = list.Where(t => (t.Username == Convert.ToString(CurrentUserId)) || (t.Username == CurrentUserId1) || (t.Username == CurrentUserId2) || (t.Username == CurrentUserId3) || (t.Username == CurrentUserId4) || (t.Username == CurrentUserId5) || (t.Username == CurrentUserId6) || (t.Username == CurrentUserId7) || (t.Username == CurrentUserId8) || (t.Username == CurrentUserId9) || (t.Username == CurrentUserId10)).FirstOrDefault().QGCode;
                    }

                    if (!string.IsNullOrWhiteSpace(QgCode))
                    {
                        if (QgCode.ToLower() == "n/a")
                        {
                            QgCode = string.Empty;
                        }
                    }

                    foreach (QueueListApiModel item in list)
                    {
                        QueueEL temp = new QueueEL()
                        {
                            BarberName = item.BarberName,
                            Position = item.QPosition
                        };

                        if (string.IsNullOrWhiteSpace(QgCode))
                        {
                            if (item.Username == Convert.ToString(CurrentUserId))
                            {
                                temp.IsCurrent = true;
                                temp.Name = item.FirstLastName;
                            }
                            else
                            {
                                temp.IsCurrent = false;
                                temp.Name = "Client";
                            }
                        }
                        else
                        {
                            if (item.QGCode == QgCode)
                            {
                                temp.IsCurrent = true;
                                temp.Name = item.FirstLastName;
                            }
                            else
                            {
                                temp.IsCurrent = false;
                                temp.Name = "Client";
                            }
                        }

                        tempqueueList.Add(temp);
                    }


                    

                    if (tempqueueList != null && tempqueueList.Count()>0)
                    {
                        if (tempqueueList.Where(t => t.IsCurrent == true).Count() == 1)//In case of single joined record show only the records with that barber only
                        {
                            queueList = tempqueueList.Where(t => t.BarberName == (tempqueueList.Where(x => x.IsCurrent == true).FirstOrDefault().BarberName)).OrderBy(t => t.Position).ToList();
                        }
                        else
                        {
                            queueList = tempqueueList.OrderBy(t => t.Position).ToList();
                        }
                    }
                }
            }
        }

        #endregion Methods

        #region Controls

        private int m_CurrentUserId;

        public int CurrentUserId
        {
            get { return m_CurrentUserId; }
            set { m_CurrentUserId = value; OnPropertyChanged("CurrentUserId"); }
        }

        private int m_SalonId;

        public int SalonId
        {
            get { return m_SalonId; }
            set { m_SalonId = value; OnPropertyChanged("SalonId"); }
        }

        private bool m_IsRunning;

        public bool IsRunning
        {
            get { return m_IsRunning; }
            set { m_IsRunning = value; OnPropertyChanged("IsRunning"); }
        }

        private bool m_TimerRunning;

        public bool TimerRunning
        {
            get { return m_TimerRunning; }
            set { m_TimerRunning = value; OnPropertyChanged("TimerRunning"); }
        }

        private string m_LastRefreshed;

        public string LastRefreshed
        {
            get { return m_LastRefreshed; }
            set { m_LastRefreshed = value; OnPropertyChanged("LastRefreshed"); }
        }

        private bool m_IsEnabled;

        public bool IsEnabled
        {
            get { return m_IsEnabled; }
            set { m_IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }

        #endregion Controls
    }
}