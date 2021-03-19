using Xamarin.Forms;
using IQB.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer_Droid))]
namespace IQB.Droid
{
    public class CustomEntryRenderer_Droid : EntryRenderer
    {
        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);
        //    Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //}
    }
}