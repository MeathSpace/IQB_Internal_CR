using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ControlBehaviors
{
    public class SpecialCharacterBehavior : Behavior<Entry>
    {
        private const string specialCharRegex = @"^[a-zA-Z ]+$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(SpecialCharacterBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<SpecialCharacterBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);

        public static BindableProperty MessageProperty = BindableProperty.Create<SpecialCharacterBehavior, string>(tc => tc.Message, "Required", BindingMode.TwoWay);

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
                else if (!(Regex.IsMatch(e.NewTextValue, specialCharRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))))
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
                    ((Entry)sender).TextColor =  Color.Red;
                    IsShow = true;
                }               
                else
                {
                    ((Entry)sender).TextColor = Color.FromHex("#000000");
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
