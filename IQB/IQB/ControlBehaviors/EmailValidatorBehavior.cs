namespace IQB.ControlBehaviors
{
    using System;
    using System.Text.RegularExpressions;
    using Xamarin.Forms;

    public class EmailValidatorBehavior : Behavior<Entry>
    {
        private const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmailValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<EmailValidatorBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);

        public static BindableProperty MessageProperty = BindableProperty.Create<EmailValidatorBehavior, string>(tc => tc.Message, "Required", BindingMode.TwoWay);

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
                IsValid = (Regex.IsMatch(e.NewTextValue.Trim(), emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                //((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

                if (!IsValid)
                {
                    Message = "Invalid email id.";
                    IsShow = true;
                }
            }
            else
            {
                IsValid = true;
                Message = "Required";
                IsShow = false;
            }
        }
    }
}