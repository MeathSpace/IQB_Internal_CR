using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace IQB.ControlBehaviors
{
    public class EmptyFieldSpecialCharacterBehavior : Behavior<Entry>
    {
        private const string specialEmptyCharRegex = @"^[a-zA-Z ]+$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmptyFieldSpecialCharacterBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<EmptyFieldSpecialCharacterBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);

        public static BindableProperty MessageProperty = BindableProperty.Create<EmptyFieldSpecialCharacterBehavior, string>(tc => tc.Message, "Required", BindingMode.TwoWay);

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public bool IsShow
        {
            get
            {
                return (bool)GetValue(IsShowProperty);
            }
            set
            {
                SetValue(IsShowProperty, value);
            }
        }

        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsShow = false;
            Message = string.Empty;

            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                if (e.NewTextValue.Length > 25)
                {
                    IsValid = false;
                    Message = "Maximum character length is 25";

                }
                else if (!(Regex.IsMatch(e.NewTextValue, specialEmptyCharRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))))
                {
                    IsValid = false;
                    Message = "Invalid entry";
                }
                else
                {
                    IsValid = true;
                }

                if (!IsValid)
                {
                    //((Entry)sender).TextColor = Color.Red;
                    IsShow = true;
                }
                else
                {
                   // ((Entry)sender).TextColor =Color.FromHex(App.Current.Resources["AppForeground"].ToString());
                    IsShow = false;
                }
            }
            else
            {
                IsValid = false;
                Message = "Required";
                IsShow = true;
            }
        }
    }
}
