using IQB.CustomControl;
using Xamarin.Forms;
using IQB.Droid;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using System;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace IQB.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class BorderlessEntryRenderer: EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
                SetColors();
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);
            if (args.PropertyName == nameof(Entry.IsEnabled)) SetColors();
        }

        private void SetColors()
        {
            Control.SetTextColor(Element.IsEnabled ? Element.TextColor.ToAndroid() : Android.Graphics.Color.White);
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}