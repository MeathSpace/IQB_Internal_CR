using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using IQB.CustomControl;
using IQB.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer_ios))]
namespace IQB.iOS
{
   public class BindablePickerRenderer_ios :  PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {

            base.OnElementChanged(e);
            if (Control != null)
            {
                (Control as UITextField).AttributedPlaceholder = new Foundation.NSAttributedString((Control as UITextField).AttributedPlaceholder.Value, foregroundColor: UIColor.White);
                Control.Font = UIFont.FromName("Helvetica-Light", 15f);
            }
        }
    }
}