using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AdminManagement;
using System;
using IQB.Views.Home;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IQB.Views.MenuItems;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IQB.Views.AdminManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStaffMember : ContentPage
    {
        public AddStaffMemberViewModel viewModel = null;
        public ManageStaffViewModel _ManageStaffViewModel = null;
        public double height = 0;
        bool IsImageUploaded = false;
        StaffServices staffservice = new StaffServices();

        public AddStaffMember(int barberid)
        {

            this.viewModel = new AddStaffMemberViewModel(barberid);
            this._ManageStaffViewModel = new ManageStaffViewModel();
            BindingContext = viewModel;

            InitializeComponent();

            if(barberid == 0)
            {
                ProfileHeader.Text = "Add Profile";
                ServicesHeader.Text = "Add Services";
                ButtonText.Text = "ADD";
                HeaderText.Text = "Create Staff";
            }
            else
            {
                ButtonText.Text = "UPDATE";
                HeaderText.Text = "Manage Staff";
            }
            if(Device.RuntimePlatform == Device.iOS)
            {
                ServiceStack.HeightRequest = Application.Current.MainPage.Height - 250;
            }

        }

        private void DoSometjing(object sender, EventArgs e)
        {
            
        }

        private void SfAccordion_Expanded(object sender, Syncfusion.XForms.Accordion.ExpandedAndCollapsedEventArgs e)
        {
            

       //     if (e.Index == 1)
       //     {
            //    ServicesListView.HeightRequest = viewModel.BrResult.Count * 100;
                //SampleList.ItemsSource = null;
                //SampleList.ItemsSource = viewModel.BrResult;
       //     }
        }

        private void ServiceListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void ServicesListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //   ServicesListView.HeightRequest = (viewModel.BrResult.Count * 110) + 50;
           //    ListServiceStack.HeightRequest = (viewModel.BrResult.Count * 110) + 50;
        }

        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    Label lbl = (Label)sender;

        //    if (lbl.Text == "Profile")
        //    {
        //        lblProfile.BackgroundColor = Color.Black;
        //        lblServices.BackgroundColor = Color.LightGray;
        //        lblProfile.TextColor = Color.White;
        //        lblServices.TextColor = Color.Black;
        //        gdProfile.IsVisible = true;
        //        gdServices.IsVisible = false;
        //    }
        //    else if (lbl.Text == "Services")
        //    {
        //        lblProfile.BackgroundColor = Color.LightGray;
        //        lblServices.BackgroundColor = Color.Black;
        //        lblProfile.TextColor = Color.Black;
        //        lblServices.TextColor = Color.White;
        //        gdServices.IsVisible = true;
        //        gdProfile.IsVisible = false;

        //    }
        //}

        //private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        //{

        //}

        //private async void TapGestureRecognizer_Tapped_CanceButton(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (viewModel.IsBackIconEnabled)
        //        {
        //            viewModel.IsBackIconEnabled = false;
        //            if (Convert.ToString(Application.Current.Properties["IsGridEnabled"]) == "true")
        //            {
        //                await Navigation.PushAsync(new MenuMaster(false));
        //            }
        //            else
        //            {
        //                //_ManageStaffViewModel.BindStaffList();
        //                //_ManageStaffViewModel = new ManageStaffViewModel();
        //                //await Navigation.PushModalAsync(new ManageStaff()); 
        //                MessagingCenter.Send(this, "Popped");
        //                await Navigation.PopAsync();
        //            }
        //            // await Navigation.PushAsync(new ManageStaff());
        //            //await Application.Current.MainPage.Navigation.PushModalAsync(new ManageStaff());
        //        }
        //    }
        //    catch (Exception es)
        //    {

        //    }
        //}

        ////private void FirstName_Completed(object sender, EventArgs e)
        ////{
        ////    if (txtFirstName.Text != null)
        ////        txtFirstName.Text= UppercaseFirst(txtFirstName.Text);
        ////}

        //string UppercaseFirst(string str)
        //{
        //    if (string.IsNullOrEmpty(str))
        //        return string.Empty;
        //    return char.ToUpper(str[0]) + str.Substring(1).ToLower();
        //}

        //private void Entry_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (txtFirstName.Text != null)
        //        txtFirstName.Text = UppercaseFirst(txtFirstName.Text);
        //    if (txtLastName.Text != null)
        //        txtLastName.Text = UppercaseFirst(txtLastName.Text);
        //    if (txtPreferredName.Text != null)
        //        txtPreferredName.Text = UppercaseFirst(txtPreferredName.Text);
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight;
        //}

        //private void txtLastName_Unfocused(object sender, FocusEventArgs e)
        //{

        //}

        //private void txtPreferredName_Unfocused(object sender, FocusEventArgs e)
        //{

        //}

        //private void Entry_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 200;
        //    Application.Current.Properties["IsUpdateEnabled"] = "true";
        //}

        //private void txtPreferredName_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 150;
        //}

        //private void BorderlessEntry_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 130;
        //}

        //private void EmpPasscode_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 110;
        //}

        //private void Email_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 80;
        //}

        //private void EWT_Focused(object sender, FocusEventArgs e)
        //{
        //    gdAddStaff.HeightRequest = viewModel.deviceHeight + 100;
        //}
    }
}