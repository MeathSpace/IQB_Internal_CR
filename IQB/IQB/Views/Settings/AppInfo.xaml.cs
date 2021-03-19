using IQB.ViewModel.AdminManagement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace IQB.Views.Settings
{
    public partial class AppInfo : ContentPage
    {
        List<AppInfoViewModel> appInfoList = new List<AppInfoViewModel>();
        
        public AppInfo()
        {
            InitializeComponent();

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Register", Details = "Register form allows you to create an account so you can remotely join the queue to place a position at your local barber shop, once registered you will be sent an activation code which you will need it to activate your account." });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Forgot Password", Details = "When you have forgotten your password, just provide your email address and we will send your password, this information will then be sent back to the email address that you have provided." });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Join Queue", Details = "This screen displays the total number of people queuing, estimated waiting time, total number of barbers on duty and also the next available position! If you join the queue on this screen using the 'Auto Join' option, you will then be automatically assigned to 'Any Barber' that means you don't have any barber preferences or you can select your favourite barber.This screen updates every second so you get instant updates on your position. Once there are any changes on your position you will get a notification with your new position. &#x0a;Note: Always arrive 15 mins before your turn to avoid losing your place in the queue!!!" });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Queuing List", Details = "This screen allows you to view the full queuing list and you can also view where you are in the list when you have joined the queue. If you have either joined the queue by yourself or joined the queue within a group, your names will then be highlighted." });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Cancel Position", Details = "If for any reason you know that you will not make for your appointment, you can cancel your turn, but if you do, you WILL NOT be able to re-join the queue on the same day!" });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Select Barber", Details = "On this screen you can see how many people are queuing for each barber, and you can also choose the barber you prefer to have your appointment with. Once you join the queue through this screen you still get instant notifications on your position!" });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "My Profile", Details = "You can edit your account information as well as adding new fields to your account e.g. profile picture, mobile number and date of birth." });

            appInfoList.Add(new AppInfoViewModel { HeaderTitle = "Settings", Details = "This screen allows you to view app information, delete your account, and reset App back to default." });

            AppInfoView.ItemsSource = appInfoList;

        }
    }
}