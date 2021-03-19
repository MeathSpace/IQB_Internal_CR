using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IQB.ControlBehaviors
{
    public class GroupJoinNameValidatorBehavior : Behavior<Entry>
    {
        private const string groupCharRegex = @"^[a-zA-Z ].{0,52}$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(GroupJoinNameValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<GroupJoinNameValidatorBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);

        public static BindableProperty MessageProperty = BindableProperty.Create<GroupJoinNameValidatorBehavior, string>(tc => tc.Message, "Required", BindingMode.TwoWay);

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
                IsValid = (Regex.IsMatch(e.NewTextValue, groupCharRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                ((Entry)sender).TextColor = IsValid ? Color.White : Color.Red;

                if (!IsValid)
                {
                    Message = "Invalid Name";
                    IsShow = true;
                }
            }
            //else
            //{
            //    IsValid = false;
            //    Message = "Required";
            //    IsShow = true;
            //}
        }
    }
}
