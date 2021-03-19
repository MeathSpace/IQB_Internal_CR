using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace IQB.ControlBehaviors
{
    public class AlphaNumericBehavior : Behavior<Entry>
    {
        //private const string passwordRegex = @"^[a-zA-Z0-9]*$";
        //private const string passwordRegex = @"^[A-Za-z]{2}[0-9]{2}\z";
        private const string passwordRegex = @"^[A-Z]{3,}.*\d{2}\z";


        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(PasswordValidationBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<PasswordValidationBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);

        public static BindableProperty MessageProperty = BindableProperty.Create<PasswordValidationBehavior, string>(tc => tc.Message, "Required", BindingMode.TwoWay);

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
                IsValid = (Regex.IsMatch(e.NewTextValue, passwordRegex, RegexOptions.None, TimeSpan.FromMilliseconds(250)));
                ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                if (!IsValid)
                {
                Message = "Passcode must be capital and at least 5 characters long followed by 2 numbers";
                    IsShow = true;
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
