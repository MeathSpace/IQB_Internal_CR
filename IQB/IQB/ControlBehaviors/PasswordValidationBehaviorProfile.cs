
namespace IQB.ControlBehaviors
{
    using System;
    using System.Text.RegularExpressions;
    using Xamarin.Forms;
    public class PasswordValidationBehaviorProfile:Behavior<Entry>
    {
        private const string passwordProfRegex = @"^.{6,}$";

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
                IsValid = (Regex.IsMatch(e.NewTextValue, passwordProfRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                ((Entry)sender).TextColor = IsValid ? Color.FromHex("#000000") : Color.Red;

                if (!IsValid)
                {
                    Message = "Password must have at least 6 characters";
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
