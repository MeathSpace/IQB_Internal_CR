﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.TabPagers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SamplePage : ContentPage
	{
		public SamplePage (string param)
		{
			InitializeComponent ();
            ExceptionLabel.Text = param;
		}
	}
}