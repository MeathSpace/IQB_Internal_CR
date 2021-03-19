
using Android.Widget;
using Android.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using IQB.Droid;
using Android.Graphics;

[assembly: ExportRenderer(typeof(SearchBar), typeof(MySearchBar_Droid))]
namespace IQB.Droid
{
    public class MySearchBar_Droid : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            // Get native control (background set in shared code, but can use SetBackgroundColor here)
            SearchView searchView = (base.Control as SearchView);
            searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

            // Access search textview within control
            int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);


           
            //Removing frame underline

            int searchPlateId = searchView.Resources.GetIdentifier("android:id/search_plate", null, null);
            if(searchPlateId>0)
            {
                var searchPlate = (searchView.FindViewById(searchPlateId));
                searchPlate.SetBackgroundColor(Android.Graphics.Color.White);
            }
           

            // to disable the search icon
            //var searchIconId = searchView.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            //if (searchIconId > 0)
            //{
            //    var searchPlateIcon = searchView.FindViewById(searchIconId);
            //    searchPlateIcon.RemoveFromParent();
            //}
        }
    }
}