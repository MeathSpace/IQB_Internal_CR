using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using IQB.Droid;
using Xamarin.Forms.Platform.Android;
using IQB.CustomControl;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer_Droid))]

namespace IQB.Droid
{
    public class BindablePickerRenderer_Droid : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetHintTextColor(global::Android.Graphics.Color.White);
                Control.SetIncludeFontPadding(false);
                Control.TextSize = 15.0f;
                Control.CanResolveTextAlignment();
            }
        }

    }

}