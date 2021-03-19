using static Android.Provider.Settings;
using IQB.Droid;
using IQB.Interfaces;
using Xamarin.Forms;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfoManagement))]
namespace IQB.Droid
{
    public class DeviceInfoManagement : IDevice
    {
        public string GetIdentifier()
        {
            return Secure.GetString(Forms.Context.ContentResolver, Secure.AndroidId);
        }
    }
}