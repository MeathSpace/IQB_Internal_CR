using Android.Content;
using IQB.Interfaces;
using Xamarin.Forms;
using IQB.Droid;
using Android.Net;
using Application = Android.App.Application;

[assembly: Dependency(typeof(NetworkConnection))]
namespace IQB.Droid
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }

        public void CheckNetworkConnection()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }
    }
}