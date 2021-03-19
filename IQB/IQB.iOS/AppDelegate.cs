using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfSchedule.XForms.iOS;
using System;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.Media;
using UserNotifications;
using IQB.Utility;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Transformations;
using Xam.Plugin.TabView;
using ContextMenu.iOS;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Syncfusion.XForms.iOS.Accordion;
using Xamarin.Forms;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using System.Collections.Generic;

namespace IQB.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                string FIRST_RUN = "hasRunBefore";
                var userDefaults = NSUserDefaults.StandardUserDefaults;
                if (userDefaults.BoolForKey("NotFirstLaunch"))
                {
                    //// Key is populated - this is not the first launch
                    //var securityRecords = new[] { SecKind.GenericPassword,
                    //                    SecKind.Certificate,
                    //                    SecKind.Identity,
                    //                    SecKind.InternetPassword,
                    //                    SecKind.Key
                    //                };
                    //foreach (var recordKind in securityRecords)
                    //{
                    //    SecRecord query = new SecRecord(recordKind);
                    //    SecKeyChain.Remove(query);
                    //}
                }
                else
                {
                    // Key is not populated - this is the first launch

                    // Set the key so we can check on the next launch
                    userDefaults.SetBool(true, "NotFirstLaunch");
                    // userDefaults.BeginInvokeOnMainThread(() => {
                    userDefaults.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
                    userDefaults.Synchronize();

                    //});
                }

                global::Xamarin.Forms.Forms.Init();
                SfAccordionRenderer.Init();
                Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
                Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
                SfListViewRenderer.Init();
                SfPickerRenderer.Init();
                SfCalendarRenderer.Init();
                SfScheduleRenderer.Init();
                Syncfusion.SfRating.XForms.iOS.SfRatingRenderer.Init();
                Syncfusion.SfNumericUpDown.XForms.iOS.SfNumericUpDownRenderer.Init();
                Syncfusion.XForms.iOS.Buttons.SfCheckBoxRenderer.Init();
                Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
                new SfBusyIndicatorRenderer();
                CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
                ImageCircleRenderer.Init();
                CrossMedia.Current.Initialize();
                XFGloss.iOS.Library.Init();
                CachedImageRenderer.Init();
                var ignore = typeof(SvgCachedImage);
                var transformation = new TintTransformation();
                var tv = new TabViewControl();
                ContextMenuScrollViewRenderer.Initialize();
                Xamarin.FormsMaps.Init();

                LoadApplication(new App());

                //DependencyService.Register<ClientService>();
                // DependencyService.Register<HttpClientHelper>();

                // Required if using Apple Pay
                // DependencyService.Register<ApplePayService>();


                OneSignal.Current.SetLogLevel(LOG_LEVEL.VERBOSE, LOG_LEVEL.NONE);

                //OneSignal.Current.StartInit("62bf335c-1418-47be-9252-ceccce2774e3")
                OneSignal.Current.StartInit("6bef1d4d-b34f-4c94-8995-909bdb91201b")
                .Settings(new Dictionary<string, bool>() {
    { IOSSettings.kOSSettingsKeyAutoPrompt, false },
    { IOSSettings.kOSSettingsKeyInAppLaunchURL, false } })
                .InFocusDisplaying(OSInFocusDisplayOption.Notification)
                .EndInit();

                // The promptForPushNotificationsWithUserResponse function will show the iOS push notification prompt. We recommend removing the following code and instead using an In-App Message to prompt for notification permission (See step 7)
                OneSignal.Current.RegisterForPushNotifications();


                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    // iOS 10
                    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                    {
                        Console.WriteLine(granted);
                    });

                    // For iOS 10 display notification (sent via APNS)
                    UNUserNotificationCenter.Current.Delegate = this;
                }
                else
                {

                    var settings = UIUserNotificationSettings.GetSettingsForTypes(
                               UIUserNotificationType.Alert
                               | UIUserNotificationType.Badge
                               | UIUserNotificationType.Sound,
                               new NSSet());
                    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                }


                UIApplication.SharedApplication.RegisterForRemoteNotifications();

                UINavigationBar.Appearance.BarTintColor = Xamarin.Forms.Color.FromHex("#ffffff").ToUIColor();
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
            return base.FinishedLaunching(app, options);
        }

    }
}
