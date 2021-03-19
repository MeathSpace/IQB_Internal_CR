using IQB.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using IQB.EntityLayer.Common;
using IQB.Views.Appointment.Customer;
using IQB.Views.Customer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;

namespace IQB.Views.Appointment.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clients : ContentPage
    {
        public CustomerListViewModel viewModel = null;
        List<ListCustomerID> _lstCustomerID = new List<ListCustomerID>();
        public List<ListCustomerID> _lstID = new List<ListCustomerID>();
        int CustomerID = 0;
        public string WalkInClientName { get; set; }
        public Clients()
        {
            InitializeComponent();

            this.viewModel = new CustomerListViewModel();
            BindingContext = viewModel;
            MessagingCenter.Subscribe<CustomerListViewModel>(this, "Popped1", (a) => { abc(); });
            Application.Current.Properties["MailSent"] = "";
            Application.Current.Properties["IsFilterApplied"] = "";
            Application.Current.Properties["CustomerID"] = "";
            Application.Current.Properties["Viewed"] = "";
            //if(_lstID==null)
            //_lstID = new List<ListCustomerID>();
        }
        private void abc()
        {
            lstCustomer.ItemsSource = null;
            lstCustomer.ItemsSource = viewModel.MoreCustomerList;
            if (CustomerID != 0)
            {

                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                {

                    CustomerListModel item = lstCustomer.ItemsSource
                             .Cast<CustomerListModel>()
                             .FirstOrDefault(x => x.CustomerID == CustomerID);
                    lstCustomer.ScrollTo(item, ScrollToPosition.MakeVisible, false);
                    return false;
                });

            }
        }


        private async void lstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //_lstCustomerID = new List<ListCustomerID>();
            //ListCustomerID _listCustID = new ListCustomerID();
            var customerData = lstCustomer.SelectedItem as CustomerListModel;
            //_listCustID.CustomerID = customerData.CustomerID;
            //_lstCustomerID.Add(_listCustID);
            await Navigation.PushAsync(new SelectService(false, customerData.CustomerID));
        }

        private void MyNotifyPage_Appearing(object sender, EventArgs e)
        {
            if (Application.Current.Properties["IsProfileViewed"] != "true")
            {
                //viewModel.BindCustomerList("", "");

                // if (Application.Current.Properties["MailSent"] == "true")
                _lstID.Clear();
            }
        }

        private void lstCustomer_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                var q = e.Item as CustomerListModel;
                if (Application.Current.Properties["IsFilterApplied"] != "true")
                {
                    if (q.CustomerID == viewModel.MoreCustomerList[viewModel.MoreCustomerList.Count - 1].CustomerID)
                    {

                        if (viewModel._Count < viewModel.Count)
                        {
                            viewModel.BindCustomerList("", "");
                            //viewModel._Count = viewModel._Count + 10;
                        }
                        viewModel._Count = viewModel._Count + 100;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                Application.Current.Properties["CustomerFName"] = "";
                Application.Current.Properties["CustomerLName"] = "";
                viewModel.BindCustomerList("", "");
                
            }

            else
            {
                var _lstCustomer = viewModel.MoreCustomerList;
                //viewModel.MoreCustomerList = viewModel.MoreCustomerList.Where(x =>
                //    /*x.CustomerFirstName.Contains(e.NewTextValue) || x.CustomerLastName.Contains(e.NewTextValue)*/
                //    (x.CustomerFirstName + " " + x.CustomerLastName).Contains(e.NewTextValue,StringComparison.OrdinalIgnoreCase)).ToList();
                _lstCustomer = _lstCustomer.Where(x =>
               /*x.CustomerFirstName.Contains(e.NewTextValue) || x.CustomerLastName.Contains(e.NewTextValue)*/
               (x.CustomerFirstName + " " + x.CustomerLastName).Contains(e.NewTextValue, StringComparison.OrdinalIgnoreCase)).ToList();
               lstCustomer.ItemsSource = null;
                lstCustomer.ItemsSource = _lstCustomer;
            }
        }

        private async void RegisterClient_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterClient());
        }

        private async void WalkInClientTapped(object sender, EventArgs e)
        {

            UserDialogs.Instance.Prompt(new PromptConfig
            {
                Title = "Please enter the customer name",
                Text = WalkInClientName,
                OnAction =  ChangeLINK
                
            });

            //await Navigation.PushAsync(new SelectService());//don't open as popup and customer ID will be  for anonymous(Walk-In) client
        }

        private async void ChangeLINK(PromptResult res)
        {
            if (res.Ok)
            {
                string walkincustomername = $"{$"{res.Value.First()}".ToUpper()}{res.Value.Substring(1)}";
                if (res.Value.Contains(" "))
                    walkincustomername = string.Join(" ", res.Value.Split(' ').Select(x => $"{$"{x.First()}".ToUpper()}{x.Substring(1)}"));
                Application.Current.Properties["_WalKinClientName"] = walkincustomername;
                await Navigation.PushAsync(new SelectService());
            }
        }
    }
}