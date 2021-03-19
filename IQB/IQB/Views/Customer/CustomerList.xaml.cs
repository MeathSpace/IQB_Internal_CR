using Acr.UserDialogs;
using IQB.ViewModel.Customer;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Customer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerList : ContentPage
    {
        public CustomerListViewModel viewModel = null;
        List<ListCustomerID> _lstCustomerID = new List<ListCustomerID>();
        public List<ListCustomerID> _lstID = new List<ListCustomerID>();
        private bool IsAllSelected = false;
        int CustomerID = 0;
        public CustomerList()
        {
            InitializeComponent();

            this.viewModel = new CustomerListViewModel();
            BindingContext = viewModel;

            MessagingCenter.Subscribe<CustomerListViewModel>(this, "Popped1", (a) => 
            {
                abc();
            });

            MessagingCenter.Subscribe<string>(this, "CustomernameSelected", (names) =>
            {
                if (!string.IsNullOrEmpty(names))
                {
                    var Names = names.Split(',');
                    SelectedListBinding((Names[0]), (Names[1]));
                }

            });

            Application.Current.Properties["MailSent"] = "";
            Application.Current.Properties["IsFilterApplied"] = "";
            Application.Current.Properties["CustomerID"] = "";
            Application.Current.Properties["Viewed"] = "";
            //if(_lstID==null)
            _lstID = new List<ListCustomerID>();
            //selectClient_Changed;
        }

        private void SelectedListBinding(string FirstName, string LastName)
        {
            viewModel._Count = 0;
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
            {
                Application.Current.Properties["CustomerFName"] = "";
                Application.Current.Properties["CustomerLName"] = "";
                viewModel.BindCustomerList("", "");

            }

            else
            {
                Application.Current.Properties["CustomerFName"] = Convert.ToString(FirstName).Trim();
                Application.Current.Properties["CustomerLName"] = Convert.ToString(LastName).Trim();
                Application.Current.Properties["IsFilterApplied"] = "true";

                viewModel.BindCustomerList(FirstName, LastName);
            }
        }

        private async void btnCustomerDetail_Clicked(object sender, EventArgs e)
        {
            _lstCustomerID = new List<ListCustomerID>();
            ListCustomerID _listCustID = new ListCustomerID();
            Button btn = (Button)sender;
            CustomerID = Convert.ToInt16(btn.CommandParameter);
            _listCustID.CustomerID = CustomerID;
            _lstCustomerID.Add(_listCustID);
            await Navigation.PushAsync(new CustomerProfile(_lstCustomerID));
            //Application.Current.Properties["scrollTo"] = lstCustomer.SelectedItem;
            // lstCustomer.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
        }

        private async void OnClosed(object sender, EventArgs e)
        {
            if (viewModel.IsBackIconEnabled)
            {
                viewModel.IsBackIconEnabled = false;
                await Navigation.PopAsync();
            }
        }

        private async void btnBookingHistory_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int CustomerID = Convert.ToInt16(btn.CommandParameter);

            await Navigation.PushAsync(new CustomerBookingHistory(CustomerID));
        }

        private async void btnSendEmail_Clicked(object sender, EventArgs e)
        {
            if (!IsAllSelected)
            {
                //if (Application.Current.Properties["CustomerID"] != "")
                //{
                //    var listitem = Application.Current.Properties["CustomerID"] as List<ListCustomerID>;
                //    //if (Application.Current.Properties["Viewed"] == "yes")
                //    //{ 
                //    //    var allListItem = Application.Current.Properties["AllCustomerID"] as List<ListCustomerID>;
                //    //    if (allListItem != null)
                //    //    {
                //    //        listitem = listitem.Concat(allListItem.ToList()).ToList();
                //    //    }
                //    //}

                //    if (listitem != null)
                //    {
                //        if (listitem.Count > 0)
                //        {
                //            await Navigation.PushAsync(new CustomerMail(listitem));
                //        }
                //        else
                //        {
                //            await App.Current.MainPage.DisplayAlert("Failure", "Select a customer to send mail", "Ok");
                //        }

                //    }
                //    else
                //    {
                //        await App.Current.MainPage.DisplayAlert("Failure", "Select a customer to send mail", "Ok");
                //    }
                //}
                //else
                //{
                //    await App.Current.MainPage.DisplayAlert("Failure", "Select a customer to send mail", "Ok");
                //} 
                if (_lstID?.Count>0)
                    await Navigation.PushAsync(new CustomerMail(_lstID));
                else
                    await App.Current.MainPage.DisplayAlert("Failure", "Select a customer to send mail", "Ok");
            }
            else
            {
                var _msgresult = await App.Current.MainPage.DisplayAlert("Warning", "This may take sometime to send email to all your customers ", "Ok", "Cancel");
                if (_msgresult)
                {
                    await Navigation.PushAsync(new CustomerMail(viewModel.MoreCustomerList.Where(x => x.marketingMail == "1")?.Select(y=>new ListCustomerID {
                        CustomerID=y.CustomerID
                    }).ToList()));
                    Application.Current.Properties["SendMailToAll"] = "true";
                }
            }
        }

        private void CustomerSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                ListCustomerID _listCustomerID = new ListCustomerID();
                var id = (int)((Switch)sender).BindingContext;
                _listCustomerID.CustomerID = id;

                var disableUserList = viewModel.MoreCustomerList.Where(db => db.CustomerID == id).ToList();

                if (((Switch)sender).IsToggled)
                {
                    if (disableUserList[0].marketingMail == "1")
                        _lstID.Add(_listCustomerID);
                    else
                    {
                        DisplayAlert("Alert !", "Sorry, you can not get marketing mail", "OK");
                    }
                }
                else
                {
                    if (_lstID.Count() > 0)
                        _lstID.Remove(_lstID.Single(x => x.CustomerID == id));
                    Application.Current.Properties["CustomerCountToSendMail"] = Convert.ToInt16(Application.Current.Properties["CustomerCountToSendMail"]) - 1;
                }

                if (Application.Current.Properties["Viewed"] == "yes")
                {
                    Application.Current.Properties["CustomerCountToSendMail"] = Convert.ToInt16(Application.Current.Properties["CustomerCountToSendMail"]) + _lstID.Count;
                }
                else
                {
                    Application.Current.Properties["CustomerCountToSendMail"] = _lstID.Count;
                }
                Application.Current.Properties["CustomerID"] = _lstID;
            }
            catch (Exception es)
            {

            }
        }

        private void SelectAllSwitch_Toggled(object sender, ToggledEventArgs e)
        {


            if (viewModel.CustomerList.Count > 0)
            {
                if (!viewModel.IsMainSwitchOn)
                    viewModel.IsMainSwitchOn = true;
                else
                    viewModel.IsMainSwitchOn = false;
                viewModel.BindCustomerList("", "");
            }
        }
        private void abc()
        {
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

            ToggleSelection(IsAllSelected);
        }

        private void MyNotifyPage_Appearing_1(object sender, EventArgs e)
        {

        }

        private async void btnSendEmailToAll_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomerMail(viewModel.IdList));
        }

        private async void lstCustomer_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _lstCustomerID = new List<ListCustomerID>();
            ListCustomerID _listCustID = new ListCustomerID();
            var customerData = lstCustomer.SelectedItem as CustomerListModel;
            _listCustID.CustomerID = customerData.CustomerID;
            _lstCustomerID.Add(_listCustID);
            await Navigation.PushAsync(new CustomerProfile(_lstCustomerID));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var _msgresult = await App.Current.MainPage.DisplayAlert("Warning", "This may take sometime to send email to all your customers ", "Ok", "Cancel");
            if (_msgresult)
            {
                await Navigation.PushAsync(new CustomerMail(viewModel.IdList));
                Application.Current.Properties["SendMailToAll"] = "true";
            }
        }

        private void MyNotifyPage_Appearing(object sender, EventArgs e)
        {

            if (Application.Current.Properties["IsProfileViewed"] != "true")
            {
                //   viewModel.BindCustomerList("", "");

                // if (Application.Current.Properties["MailSent"] == "true")
                //_lstID.Clear();
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
                     //   viewModel._Count = viewModel._Count + 100;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Tapped(object sender, EventArgs e)
        {

        }

        private async void CustomerDetail_Tapped(object sender, EventArgs e)
        {
            _lstCustomerID = new List<ListCustomerID>();
            ListCustomerID _listCustID = new ListCustomerID();
            Grid btn = (Grid)sender;
            int CustomerID = Convert.ToInt16(btn.ClassId);
            _listCustID.CustomerID = CustomerID;
            _lstCustomerID.Add(_listCustID);
            await Navigation.PushAsync(new CustomerProfile(_lstCustomerID));
        }

        private async void ClientFilter_Tapped(object sender, EventArgs e)
        {
            ToggleSelection(false);
            _lstID.Clear();
            await Navigation.PushModalAsync(new CustomerFilter());
            //UserDialogs.Instance.Login(new LoginConfig {
            //    Title="Filter",
            //});
        }

        private void ToggleSelection(bool selectAllCustomers)
        {
            var _lstCustomer = viewModel.MoreCustomerList;


            //foreach (var item in _lstCustomer.Where(x => x.IsSendMailEnabled))
            foreach (var item in _lstCustomer)
            {
                item.IsSwitchToggled = selectAllCustomers;
            }



            viewModel.MoreCustomerList = _lstCustomer;
            IsAllSelected = selectAllCustomers;
            selectAll.IsChecked = IsAllSelected;
        }

        private void selectClient_Changed(object sender, StateChangedEventArgs e)
        {
            if (!IsAllSelected)
            {
                try
                {
                    ListCustomerID _listCustomerID = new ListCustomerID();
                    var id = ((CustomerListModel)((SfCheckBox)sender).Parent.BindingContext).CustomerID;

                    // var id = (int)((SfCheckBox)sender).BindingContext;
                    _listCustomerID.CustomerID = id;

                    var xu = viewModel.MoreCustomerList.Where(x => x.marketingMail == "1").ToList();
                    var disableUserList = viewModel.MoreCustomerList.Where(db => db.CustomerID == id).ToList();

                    //if (((SfCheckBox)sender).IsChecked)
                    if (e.IsChecked.Value)
                    {
                        if (disableUserList[0].marketingMail == "1")
                            _lstID.Add(_listCustomerID);
                        else
                        {
                            //DisplayAlert("Alert !", "Sorry, you can not get marketing mail", "OK");
                        }
                    }
                    else
                    {
                        if (_lstID.Count() > 0)
                            _lstID.Remove(_lstID.Single(x => x.CustomerID == id));
                        Application.Current.Properties["CustomerCountToSendMail"] = Convert.ToInt16(Application.Current.Properties["CustomerCountToSendMail"]) - 1;
                    }

                    //if (Application.Current.Properties["Viewed"] == "yes")
                    //{
                    //    Application.Current.Properties["CustomerCountToSendMail"] = Convert.ToInt16(Application.Current.Properties["CustomerCountToSendMail"]) + _lstID.Count;
                    //}
                    //else
                    //{
                    //    Application.Current.Properties["CustomerCountToSendMail"] = _lstID.Count;
                    //}
                    Application.Current.Properties["CustomerCountToSendMail"] = _lstID.Count;
                    Application.Current.Properties["CustomerID"] = _lstID;
                }
                catch (Exception es)
                {

                } 
            }
        }

        private void selectAll_Changed(object sender, StateChangedEventArgs e)
        {

            if (e.IsChecked != null)
            {
                ToggleSelection(e.IsChecked.Value);
            }
            //lstCustomer.ItemsSource = null;
            //lstCustomer.ItemsSource = new ObservableCollection<CustomerListModel>(_lstCustomer);

        }
    }
}