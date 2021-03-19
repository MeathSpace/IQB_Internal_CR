namespace IQB.ControlBehaviors
{
    using Xamarin.Forms;

    public class EmptyFieldValidatorBehavior : Behavior<Entry>
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmptyFieldValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<EmptyFieldValidatorBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);
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
                if (e.NewTextValue.Length > 450)
                {
                    IsValid = false;
                    Message = "Maximum character length is 450";
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
                    //((Entry)sender).TextColor = Color.Default;
                    IsShow = false;
                }
            }
            else
            {
                IsValid = false;
                Message = "Required";
                IsShow = true;
            }
            //IsValid = (!string.IsNullOrWhiteSpace(e.NewTextValue)) ? true : false;
            //((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

            //if (!IsValid)
            //{
            //    IsShow = true;
            //    Message = "Required";
            //}
        }
    }
    public class EmptyFieldAddressValidatorBehavior : Behavior<Entry>
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmptyFieldValidatorBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static BindableProperty IsShowProperty = BindableProperty.Create<EmptyFieldValidatorBehavior, bool>(tc => tc.IsShow, false, BindingMode.TwoWay);
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
                if (e.NewTextValue.Length > 40)
                {
                    IsValid = false;
                    Message = "Maximum character length is 40";
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
                    //((Entry)sender).TextColor = Color.Default;
                    IsShow = false;
                }
            }
            else
            {
                IsValid = false;
                Message = "Required";
                IsShow = true;
            }
            //IsValid = (!string.IsNullOrWhiteSpace(e.NewTextValue)) ? true : false;
            //((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;

            //if (!IsValid)
            //{
            //    IsShow = true;
            //    Message = "Required";
            //}
        }
    }
}