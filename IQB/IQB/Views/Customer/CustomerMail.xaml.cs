using IQB.ViewModel.Customer;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerMail : ContentPage
    {
        public CustomerMailViewModel viewModel = null;

        public CustomerMail(List<ListCustomerID> CustomerID)
        {
            try
            {
                Application.Current.Properties["Viewed"] = "yes";
                InitializeComponent();
                this.viewModel = new CustomerMailViewModel(CustomerID);
                BindingContext = viewModel;

                Emaileditor.Text = "Email Body";
                Emaileditor.TextColor = Color.Gray;

                //this.viewModel.noofcustomer = "You have chosen " + Application.Current.Properties["CustomerCountToSendMail"] + " customer(s)";
                this.viewModel.noofcustomer = $"You have chosen {CustomerID.Count} customer(s)";

                Application.Current.Properties["IsProfileViewed"] = "";
                //Application.Current.Properties["AllCustomerID"] = CustomerID;
            }
            catch (Exception ex)
            {
            }
        }

        private void Send(object sender, EventArgs e)
        {

        }

        private void Emaileditor_Focused(object sender, FocusEventArgs e)
        {
            if (Emaileditor.Text.Equals("Email Body")) //if you have the placeholder showing, erase it and set the text color to black
            {
                Emaileditor.Text = "";
                Emaileditor.TextColor = Color.Black;
            }
        }

        private void Emaileditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (Emaileditor.Text.Equals("")) //if there is text there, leave it, if the user erased everything, put the placeholder Text back and set the TextColor to gray
            {
                Emaileditor.Text = "Email Body";
                Emaileditor.TextColor = Color.Gray;
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
          //  Application.Current.Properties["CustomerToSendMail"] = CustomerID;
        }
    }
}