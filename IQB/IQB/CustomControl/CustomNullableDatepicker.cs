namespace IQB.CustomControl
{
    using System;
    using Xamarin.Forms;
    public class CustomNullableDatepicker : Label
    {
        private DatePicker _Picker;
        private DateTime? _OldDate;
        private IViewContainer<View> _ParentLayout; 

        public static BindableProperty DateProperty = BindableProperty.Create<CustomNullableDatepicker, DateTime?>(p => p.Date, null);
        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly BindableProperty IsEnabledProperty = BindableProperty.Create<CustomNullableDatepicker, bool>(p => p.IsEnabled, true);
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly BindableProperty IsClearedProperty = BindableProperty.Create<CustomNullableDatepicker, bool>(p => p.IsCleared, false);
        public bool IsCleared
        {
            get { return (bool)GetValue(IsClearedProperty); }
            set { SetValue(IsClearedProperty, value); }
        }

        new private static readonly BindableProperty TextProperty = BindableProperty.Create<CustomNullableDatepicker, string>(p => p.Text, "...");
        new public string Text
        {
            get { return (string)GetValue(TextProperty); }
            private set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty DefaultTextProperty = BindableProperty.Create<CustomNullableDatepicker, string>(p => p.DefaultText, "Pick Date...");
        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }

        public static readonly BindableProperty TextFormatProperty = BindableProperty.Create<CustomNullableDatepicker, string>(p => p.TextFormat, "MM'/'dd'/'yyyy");
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set { SetValue(TextFormatProperty, value); }
        }

        public event EventHandler<DateChangedEventArgs> DateSelected;

        public CustomNullableDatepicker()
        {
            //create the datepicker, make it invisible on the form.
            _Picker = new DatePicker
            {
                IsVisible = false
            };

            //handle the focus/unfocus or rather the showing and hiding of the dateipicker
            _Picker.Focused += _Picker_Focused;
            _Picker.Unfocused += _Picker_Unfocused;
            //DateSelected += CustomNullableDatepicker_DateSelected;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                // handle the tap

                //try to get the parent layout and add the datepicker
                if (_ParentLayout == null)
                {
                    _ParentLayout = _GetParentLayout(Parent);
                    if (_ParentLayout != null)
                    {
                        //add the picker to the closest layout up the tree
                        _ParentLayout.Children.Add(_Picker);
                    }
                    else
                    {
                        throw new InvalidOperationException("The CustomNullableDatepicker needs to be inside an Layout type control that can have other children");
                    }
                }
                //show the picker modal
                if (IsEnabled)
                {
                    _Picker.Focus();
                }
            };
            base.GestureRecognizers.Add(tapGestureRecognizer);

            base.VerticalTextAlignment = TextAlignment.Center;
            base.FontSize = 18;

            _UpdateText();
        }

        private IViewContainer<View> _GetParentLayout(Element ParentView)
        {
            //StackLayout, RelativeLayout, Grid, and AbsoluteLayout all implement IViewContainer,
            //it would be very rare that this method would return null.
            IViewContainer<View> parent = ParentView as IViewContainer<View>;
            if (ParentView == null)
            {
                return null;
            }
            else if (parent != null)
            {
                return parent;
            }
            else
            {
                return _GetParentLayout(ParentView.Parent);
            }
        }

        void _Picker_Focused(object sender, FocusEventArgs e)
        {
            //default the date to now if Date is empty
            //_Picker.IsVisible = true;
            _Picker.Date = Date ?? DateTime.Now;
        }

        void _Picker_Unfocused(object sender, FocusEventArgs e)
        {
            //this always sets.. can't cancel the dialog.
            Date = _Picker.Date;
            _UpdateText();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _UpdateText();
        }

        private void _UpdateText()
        {
            if (IsCleared)
            {
                base.Text = DefaultText;
            }
            else
            {
                //the button has a default text, use that the first time.
                if (Date.HasValue)
                {
                    //default formatting is in the FormatProperty BindableProperty 
                    base.Text = Date.Value.ToString(TextFormat);
                }
                else
                {
                    base.Text = DefaultText;
                }
            }
        }

        protected override void OnPropertyChanging(string propertyName = null)
        {
            //set this so there is an old date for the DateChangedEventArgs
            base.OnPropertyChanging(propertyName);
            //if (propertyName == DateProperty.PropertyName)
            //{
            //    _OldDate = Date;
            //    if (IsCleared)
            //    {
            //        Date = null;
            //        _UpdateText();
            //        IsCleared = false;
            //    }
            //}
            _OldDate = Date;
            if (IsCleared)
            {
                Date = null;
                _UpdateText();
                IsCleared = false;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == DateProperty.PropertyName)
            {
                //if the event isn't null, and the old date isn't null, and the date isn't null ... EVENT!
                if (DateSelected != null && _OldDate != null && Date != null)
                {
                    DateSelected(this, new DateChangedEventArgs((DateTime)_OldDate, (DateTime)Date));
                }
                //if the event isn't null, and the date isn't null ... EVENT!
                else if (DateSelected != null && Date != null)
                {
                    DateSelected(this, new DateChangedEventArgs((DateTime)Date, (DateTime)Date));
                }
                _OldDate = null;
            }
        }
    }
}
