using IQB.ViewModel.AuthViewModel;
using IQB.Views.MenuItems;
using Xamarin.Forms;

namespace IQB.Views.LoginManagement
{
    public partial class Logout : ContentPage
    {
        public Logout()
        {
            //LoginViewModel.DestroyLoginCredData();
            MenuMaster.HomeTimerStat = false;
            MenuMaster.QueueListTimerStat = false;
            App.GoToLoginRootSecondary();
            //App.GoToLoginRoot();
        }
    }
}