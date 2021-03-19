using IQB.ViewModel.AuthViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpScreen : ContentPage
    {        
        HelpViewModel viewModel = null;
        public HelpScreen()
        {
            InitializeComponent();
            viewModel = new HelpViewModel();
            BindingContext = viewModel;
        }
    }
}