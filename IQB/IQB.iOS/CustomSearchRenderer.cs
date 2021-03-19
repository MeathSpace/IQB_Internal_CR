
using Xamarin.Forms;
using IQB.CustomControl.GooglePlacesSearch;
using IQB.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(PlacesBar), typeof(CustomSearchRenderer))]
namespace IQB.iOS
{
    public class CustomSearchRenderer : BorderlessEntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName == "Text")
            //{
            //    Control.ShowsCancelButton = false;

            //}



          //  Control.SetImageforSearchBarIcon(UIImage.FromBundle("blank"), UISearchBarIcon.Search, UIControlState.Normal);
           

        }
    }
}