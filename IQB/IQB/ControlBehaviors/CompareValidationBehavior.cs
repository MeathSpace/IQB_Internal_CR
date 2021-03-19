namespace IQB.ControlBehaviors
{
    using Xamarin.Forms;

    public class CompareValidationBehavior : Behavior<Entry>
    {
        public static BindableProperty TextProperty = BindableProperty.Create<CompareValidationBehavior, string>(tc => tc.Text, string.Empty, BindingMode.TwoWay);

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = e.NewTextValue == Text;

            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }

    #region Calling

    //<Entry IsPassword = "True" Placeholder="Enter same as above" >
    //  <Entry.Behaviors>
    //    <local:PasswordValidationBehavior />
    //    <local:CompareValidationBehavior BindingContext = "{x:Reference txtpassword}" Text="{Binding Text}"/>
    //  </Entry.Behaviors>
    //</Entry>

    #endregion Calling
}