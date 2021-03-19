using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Gcm.Client;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.Media;
using System;
using FFImageLoading.Svg.Forms;
using CarouselView.FormsPlugin.Android;
using Acr.UserDialogs;
using Plugin.Permissions;
using Android.Runtime;
using Android;
using Android.Content;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Xamarin.Forms.Maps.Android;

namespace IQB.Droid
{
    [Activity(Label = "IQB",Name ="com.iqbarbers.iqb.MainActivity", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static MainActivity instance = null;
       


        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override async void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            //{
            //    Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            //    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            //    Window.SetStatusBarColor(Android.Graphics.Color.White);
            //}
            //Window.SetSoftInputMode(SoftInput.AdjustResize);
            //Window.DecorView.SystemUiVisibility = 0;
            //// Fix the keyboard so it doesn't overlap the grid icons above keyboard etc
            //if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            //{
            //    // Bug in Android 5+, this is an adequate workaround
            //    AndroidBug5497WorkaroundForXamarinAndroid.assistActivity(this, WindowManager);
            //}
            this.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            //this.Window.SetStatusBarColor(Android.Graphics.Color.Rgb(240, 52, 50));
            await CrossMedia.Current.Initialize();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CarouselViewRenderer.Init();
            ImageCircleRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            UserDialogs.Init(() => this);
            LoadApplication(new App());
            XFGloss.Droid.Library.UsingSwitchCompatCell = false;
            XFGloss.Droid.Library.Init(this, bundle);
            //var config = new PayPalConfiguration(PayPalEnvironment.Sandbox, "ATST04krm1C57Wu1U85gvajA1yNCBGwliyvISOtmLx0sEWjCHeUF7NSTuL2T8h51EZToUiACBuboq8nr")
            //{
            //    //If you want to accept credit cards
            //    AcceptCreditCards = true,
            //    //Your business name
            //    MerchantName = "Test Store",
            //    //Your privacy policy Url
            //    MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
            //    //Your user agreement Url
            //    //MerchantUserAgreementUri = "https://www.example.com/legal",
            //    //// OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
            //    //ShippingAddressOption = ShippingAddressOption.Both,
            //    //// OPTIONAL - Language: Default languege for PayPal Plug-In
            //    //Language = "es",
            //    //// OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
            //    //PhoneCountryCode = "52",
            //};
            //CrossPayPalManager.Init(config);

            Xamarin.FormsMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps

            try
            {
                // Check to ensure everything's set up right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }
        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        //    PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        //    PayPalManagerImplementation.Manager.Destroy();
        }

        public class AndroidBug5497WorkaroundForXamarinAndroid
        {

            // For more information, see https://code.google.com/p/android/issues/detail?id=5497
            // To use this class, simply invoke assistActivity() on an Activity that already has its content view set.

            // CREDIT TO Joseph Johnson (http://stackoverflow.com/users/341631/joseph-johnson) for publishing the original Android solution on stackoverflow.com

            private Android.Views.View mChildOfContent;
            private int usableHeightPrevious;
            private FrameLayout.LayoutParams frameLayoutParams;

            public static void assistActivity(Activity activity, IWindowManager windowManager)
            {
                new AndroidBug5497WorkaroundForXamarinAndroid(activity, windowManager);
            }

            private AndroidBug5497WorkaroundForXamarinAndroid(Activity activity, IWindowManager windowManager)
            {

                var softButtonsHeight = getSoftbuttonsbarHeight(windowManager);

                var content = (FrameLayout)activity.FindViewById(Android.Resource.Id.Content);
                mChildOfContent = content.GetChildAt(0);
                var vto = mChildOfContent.ViewTreeObserver;
                vto.GlobalLayout += (sender, e) => possiblyResizeChildOfContent(softButtonsHeight);
                frameLayoutParams = (FrameLayout.LayoutParams)mChildOfContent.LayoutParameters;
            }

            private void possiblyResizeChildOfContent(int softButtonsHeight)
            {
                var usableHeightNow = computeUsableHeight();
                if (usableHeightNow != usableHeightPrevious)
                {
                    var usableHeightSansKeyboard = mChildOfContent.RootView.Height - softButtonsHeight;
                    var extraDifference = usableHeightSansKeyboard - usableHeightPrevious; // This might be an issue on some devices with no soft buttons
                    var heightDifference = usableHeightSansKeyboard - usableHeightNow;
                    if (heightDifference > (usableHeightSansKeyboard / 4))
                    {
                        // keyboard probably just became visible
                        if (softButtonsHeight == 0)
                        {
                            // If there is no softbutton height, there might be a gap, so add some calculated extra difference
                            frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference + extraDifference;
                        }
                        else
                            frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference + (softButtonsHeight / 2);
                    }
                    else
                    {
                        // keyboard probably just became hidden
                        frameLayoutParams.Height = usableHeightSansKeyboard;
                    }
                    mChildOfContent.RequestLayout();
                    usableHeightPrevious = usableHeightNow;
                }
            }

            private int computeUsableHeight()
            {
                var r = new Rect();
                mChildOfContent.GetWindowVisibleDisplayFrame(r);
                return (r.Bottom - r.Top);
            }

            private int getSoftbuttonsbarHeight(IWindowManager windowManager)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    var metrics = new DisplayMetrics();
                    windowManager.DefaultDisplay.GetMetrics(metrics);
                    int usableHeight = metrics.HeightPixels;
                    windowManager.DefaultDisplay.GetRealMetrics(metrics);
                    int realHeight = metrics.HeightPixels;
                    if (realHeight > usableHeight)
                        return realHeight - usableHeight;
                    else
                        return 0;
                }
                return 0;
            }

        }
    }
}