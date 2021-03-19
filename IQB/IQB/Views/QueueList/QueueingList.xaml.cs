using IQB.EntityLayer.Common;
using IQB.ViewModel.QueueListManagement;
using IQB.Views.MenuItems;
using IQB.Views.QueueList.QueueListGrid;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.Views.QueueList
{
    public partial class QueueingList : ContentView
    {
        public QueuelistViewModel viewModel;
        public QueueGridView listLayout = null;

        public QueueingList()
        {
            InitializeComponent();

            viewModel = new QueuelistViewModel();
            BindingContext = viewModel;

            //Title = "Queueing List";

            setQueueList();

            MenuMaster.QueueListTimerStat = true;
            //Create schedule to call list after every 30 seconds
            Device.StartTimer(new TimeSpan(0, 0, 30), callSetQueueList);
        }

        private async void btnRefreshClicked(object sender, EventArgs args)
        {
            viewModel.TimerRunning = false;
            await setQueueList();
            viewModel.TimerRunning = true;
        }

        public bool callSetQueueList()
        {
            if (viewModel.TimerRunning && MenuMaster.QueueListTimerStat && App.IsApplicationAwake)
            {
                setQueueList();
            }

            return MenuMaster.QueueListTimerStat;
        }

        public async Task setQueueList()
        {
            viewModel.IsEnabled = false;
            if (listLayout != null)
            {
                stckQueueList.Children.Remove(listLayout);
            }

            viewModel.IsRunning = false;

            //await viewModel.SetQueueList();

            listLayout = new QueueGridView(viewModel)
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = CommonEL.QueueGridListBackgroundColor
            };

            stckQueueList.Children.Add(listLayout);

            int PageNo = 1;
            bool IsRecordExists = true;

            listLayout.AddLoader();

            while (IsRecordExists)
            {
                IsRecordExists = await listLayout.GetGridQueueListData(PageNo++);
            }

            await listLayout.SetListViewNew(PageNo);

            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);
            //await listLayout.GetGridQueueListData(1);

            viewModel.IsRunning = false;

            int TimeHour = DateTime.Now.Hour;
            int TimeMinute = DateTime.Now.Minute;

            //viewModel.LastRefreshed = "Queueing list \n last refreshed: " + ((TimeHour < 10) ? ("0" + Convert.ToString(TimeHour)) : Convert.ToString(TimeHour)) + ":" + ((TimeMinute < 10) ? ("0" + Convert.ToString(TimeMinute)) : Convert.ToString(TimeMinute));
            //viewModel.LastRefreshed = "\nList Update \nLast refreshing time: " + ((TimeHour < 10) ? ("0" + Convert.ToString(TimeHour)) : Convert.ToString(TimeHour)) + ":" + ((TimeMinute < 10) ? ("0" + Convert.ToString(TimeMinute)) : Convert.ToString(TimeMinute))+"\n";
            viewModel.LastRefreshed = "Last refreshing time: " + ((TimeHour < 10) ? ("0" + Convert.ToString(TimeHour)) : Convert.ToString(TimeHour)) + ":" + ((TimeMinute < 10) ? ("0" + Convert.ToString(TimeMinute)) : Convert.ToString(TimeMinute)) + "\n";

            viewModel.IsEnabled = true;
        }


    }
}