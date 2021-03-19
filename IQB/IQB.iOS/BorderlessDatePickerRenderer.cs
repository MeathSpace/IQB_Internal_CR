﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using IQB.iOS;
using IQB.CustomControl;

[assembly: ExportRenderer(typeof(MyDatePicker), typeof(BorderlessDatePickerRenderer))]
namespace IQB.iOS
{
    

        public class BorderlessDatePickerRenderer : DatePickerRenderer
        {
            public static void Init() { }
            protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                base.OnElementPropertyChanged(sender, e);

                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    
}