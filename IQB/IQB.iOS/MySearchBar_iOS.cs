using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using IQB.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(MySearchBar_iOS))]

namespace IQB.iOS
{
   public class MySearchBar_iOS : SearchBarRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            UISearchBar searchView = (base.Control as UISearchBar);
            searchView.SearchResultsButtonSelected = false;
        }
    }
}