using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.Converters
{
    public class DateToVisibilityConverter : IValueConverter
    {
        //Plugin
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BarberOnlineToVisibilityConverter : IValueConverter
    {
        //Plugin
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ($"{value}")?.Replace(" ","").ToLower()=="yes"?true:false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmountToCurrencyConverter : IValueConverter
    {
        //Plugin
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Application.Current.Properties["_Currency"].ToString() + value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
