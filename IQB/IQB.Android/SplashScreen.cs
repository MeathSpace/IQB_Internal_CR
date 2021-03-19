using Android.App;
using Android.Content;
using Android.OS;

namespace IQB.Droid
{
    [Activity(Theme = "@style/SplashTheme", //Indicates the theme to use for this activity
              MainLauncher = true, //Set it as boot activity
              NoHistory = true)] //Doesn't place it in back stack
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //System.Threading.Thread.Sleep(1500); //Let's wait awhile...
            //this.StartActivity(typeof(MainActivity));

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
        }
    }
}