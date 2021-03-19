using IQB.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace IQB.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {

                if (value != null)
                {
                    string s = value.ToString();
                    switch (s)
                    {
                        //case "unselected":
                        //    return Color.Red;
                        //case "selected":
                        //    return Color.Green;

                        case "unselected":
                            return string.Format("{0}", GetImage("grey_tick.png", "grey_tick_big.png"));
                        case "selected":
                            return string.Format("{0}", GetImage("green_tick@2x.png", "green_tick_big.png"));
                    }

                }
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Color.Black;
        }

        private string GetImage(string oniOS, string onAndroid)
        {
            string img = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS: img = oniOS; break;
                case Device.Android: img = onAndroid; break;
            }
            return img;
        }
    }
}
