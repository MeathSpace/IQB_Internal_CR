using System;
using Xamarin.Forms;

namespace IQB.EntityLayer.Common
{
    public class CommonEL
    {
        public const string SalonAppIconBaseName = "SalonAppIcon_";
        public const string SelonServiceName = "SelonService";
        public const string LoginServiceName = "LoginService";
        public const string baseurl = "https://server.iqueuebarbers.com/"; //"http://108.163.174.242/";
        public const string ApiUser = "~iqueue";
        //public const string PayUrl = "https://server.iqueuebarbers.com/~iqueue/IQBphpFiles-Dev/paymentGateway/iqueue_payment.php";
        public const string PayUrl = "https://server.iqueuebarbers.com/~iqueue/IQBphpFiles-Dev/paymentGateway/iqueue_payment1.php";
        //public const string ApiUser = "~iqbdev";
        public const string ApiContainFolder = "IQBphpFiles-Dev";//IQBPhpFiles-Dev-backup,IQBphpFiles-Dev
    //    public const string ApiContainFolder = "IQBphpFiles-Prod";
        public const string SalonImageLocation = "https://server.iqueuebarbers.com/~iqueue/SalonAppIcons/"; //"http://108.163.174.242/~iqueue/SalonAppIcons/";
        public const string ProfileImageLocation = "https://server.iqueuebarbers.com/~iqueue/userspics/"; //"http://108.163.174.242/~iqueue/userspics/";
        public const string BarberImageLocation = "https://server.iqueuebarbers.com/~iqueue/barberpics/"; //"http://108.163.174.242/~iqueue/barberpics/";
        public const string AdLocation = "https://server.iqueuebarbers.com/~iqueue/IQBAds/"; //"http://108.163.174.242/~iqueue/IQBAds/";

        //Background colors
        public static Color MainPageBackgroundColor = Color.FromHex("020203");

        public static Color QueueGridListBackgroundColor = Color.FromHex("D7DEEE");
        //public static Color QueueGridListBackgroundColor = Color.FromHex("000000");
        public static Color ListCellBackgroundColor = Color.FromHex("252931");
        public static Color ListDataCellBackgroundColor = Color.FromHex("ffffff");
        //public static Color ListHeaderCellBackgroundColor = Color.FromHex("758094");
        public static Color ListHeaderCellBackgroundColor = Color.FromHex("ffffff");
        public static Color DarkBlackBackgroundColor = Color.FromHex("020203");
        public static Color LightBlackBackgroundColor = Color.FromHex("797e86");
        public static Color LightBackgroundColor = Color.FromHex("ffffff");

        //Text Colors
        public static Color WhiteTextColor = Color.FromHex("ffffff");
        public static Color BlackTextColor = Color.FromHex("000000");
        public static Color DarkGrayColor = Color.FromHex("a9a9a9");

        public static Color LightBlackTextColor = Color.FromHex("797e86");
        public static Color RedTextColor = Color.FromHex("ef3430");
        public static Color GreenTextColor = Color.FromHex("1ababa");

        //Vertical seperator colors
        public static Color LightBlackVerticalSeperatorColor = Color.FromHex("797e86");

        public static Color DarkBlackVerticalSeperatorColor = Color.FromHex("020203");
        public static Color RedVerticalSeperatorColor = Color.FromHex("ef3430");

        //Horizontal seperator colors
        public static Color LightHorizontalSeperatorColor = Color.FromHex("e2ebfa");
        public static Color LightBlackHorizontalSeperatorColor = Color.FromHex("797e86");
        public static Color WhiteHorizontalSeperatorColor = Color.FromHex("D7DEEE");
        //public static Color WhiteHorizontalSeperatorColor = Color.FromHex("000000");
        public static Color DarkBlackHorizontalSeperatorColor = Color.FromHex("020203");

        public const string ErrorMessage = "Some error occured! Please try again later.";

        public const string GroupJoinServiceName = "GroupJoinService";

        public const string NoNetworkErrorMsg = "No network connection";

        public const string NoUserSelectedGroupJoin = "No user is selected";


        public const string TimeSlotStartTime = "10:00 AM";
        //public const string TimeSlotEndTime = "7:00 PM";
        public const string TimeSlotEndTime = "09:00 PM";

        public const string DayTimeEndTime = "11:59 PM";

        public const string TimeSlotInterVal = "00:15:00";

        public const string TimeSlotInterValInMinute = "15";

        public const string DefaultServiceDeuarion = "01:00:00";

        public static string GetCallUrl(string serviceFileName)
        {
            return (ApiUser + "/" + ApiContainFolder + "/" + serviceFileName);
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}