﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using IQB.CustomControl;
using Xamarin.Forms;
using IQB.iOS;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace IQB.iOS
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        Version version = new Version(ObjCRuntime.Constants.Version);
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            var view = (CustomSwitch)Element;
            if (!string.IsNullOrEmpty(view.SwitchThumbImage))
            {
                if (version > new Version(6, 0))
                {   //n iOS 6 and earlier, the image displayed when the switch is in the on position.
                    Control.OnImage = UIImage.FromFile(view.SwitchThumbImage.ToString());
                    //n iOS 6 and earlier, the image displayed when the switch is in the off position.
                    Control.OffImage = UIImage.FromFile(view.SwitchThumbImage.ToString());
                }
                else
                {
                    Control.ThumbTintColor = view.SwitchThumbColor.ToUIColor();
                }
            }
            //The color used to tint the appearance of the thumb.
            Control.ThumbTintColor = view.SwitchThumbColor.ToUIColor();
            //Control.BackgroundColor = view.SwitchBGColor.ToUIColor();
            //The color used to tint the appearance of the switch when it is turned on.
            Control.OnTintColor = view.SwitchOnColor.ToUIColor();
            //The color used to tint the outline of the switch when it is turned off.
            Control.TintColor = view.SwitchOffColor.ToUIColor();
        }
    }
}