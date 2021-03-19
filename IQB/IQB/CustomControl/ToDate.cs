using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.CustomControl
{
    public class ToDate : DatePicker
    {
        private string _format = null;
        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create<MyDatePicker, DateTime?>(p => p.NullableDate, null);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        public static readonly BindableProperty IsClearedProperty = BindableProperty.Create<MyDatePicker, bool>(p => p.IsCleared, false);
        public bool IsCleared
        {
            get { return (bool)GetValue(IsClearedProperty); }
            set { SetValue(IsClearedProperty, value); }
        }

        private void UpdateDate()
        {
            if (NullableDate.HasValue)
            {
                if (null != _format)
                {
                    Format = _format;
                }
                Date = NullableDate.Value;
            }
            else
            {
                _format = Format;
                Format = "TO";

            }
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Date")
            {
                NullableDate = Date;
                if (IsCleared)
                {
                    UpdateDate();
                    IsCleared = false;
                }
            }
            else if (propertyName == "IsCleared")
            {
                if (IsCleared)
                {
                    NullableDate = null;
                }
            }
            else if (propertyName == "IsFocused")
            {
                if (!NullableDate.HasValue)
                {
                    NullableDate = DateTime.Now;
                }
            }
        }
    }
}
