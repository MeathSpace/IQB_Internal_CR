using IQB.CustomControl.GooglePlacesSearch;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.Utility;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.Settings;

using System;
using System.Linq;

using Xamarin.Forms;

namespace IQB.Views.ApplicationManagement
{
    public partial class WelcomeScreen : ContentPage
    {
        string GooglePlacesApiKey = "AIzaSyCc0rrgXw7WkdvKOqS5YeD6IWHKvl1OJa0";
        public bool IsPlaceSelected = false;
        WriteErrorLog errLog = new WriteErrorLog();
        public WelcomeScreen()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                //entrySalonCode.BackgroundColor = new Color(1, 1, 1, 0.1);
                //lblMessage.BackgroundColor = new Color(255, 0, 0, 0.5);
                //search_bar.BackgroundColor = new Color(1, 1, 1, 0.1);
                //results_list.BackgroundColor = new Color(1, 1, 1, 0.1);
                search_bar.ApiKey = GooglePlacesApiKey;
                search_bar.Type = PlaceType.Cities;
                search_bar.PlacesRetrieved += Search_Bar_PlacesRetrieved;
                search_bar.TextChanged += Search_Bar_TextChanged;
                search_bar.MinimumSearchText = 2;
                results_list.ItemSelected += Results_List_ItemSelected;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);
            }


        }

        private async void btnSearch_Clicked(object sender, EventArgs args)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                indicator.IsVisible = true;
                btnSearch.IsEnabled = false;

                string city = search_bar.Text;

                if (!string.IsNullOrWhiteSpace(city))
                {
                    //lblMailLink.IsVisible = false;
                    lblMessage.IsVisible = false;
                    lblMessage.Text = "";
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        ApiResult res = new ApiResult();

                        using (SalonService obj = new SalonService())
                        {
                            res = await obj.GetSalonListbyCity(city);
                        }

                        if (res != null && res.StatusCode == 200)
                        {
                            System.Collections.Generic.List<SalonInfoAPIModel> data = UtilityEL.ToModelList<SalonInfoAPIModel>(res.Response);

                            if (data != null && data.Count() > 0)
                            {
                                await Navigation.PushAsync(new SalonList(data), true);
                            }
                            else
                            {
                                var option = await DisplayAlert("Alert!", "Sorry couldn't find any salons within this area! Would you like register your interest?", "Yes", "No");
                                if (option)
                                {
                                    await Navigation.PushAsync(new SalonRequest(), true);
                                }
                                //lblMessage.Text = "Sorry couldn't find any salons within this area!";
                                //lblMessage.IsVisible = true;
                                //lblMailLink.IsVisible = true;
                            }
                        }
                        else if (res != null && res.StatusCode == 201)
                        {
                            var option = await DisplayAlert("Alert!", "Sorry couldn't find any salons within this area! Would you like register your interest?", "Yes", "No");
                            if (option)
                            {
                                await Navigation.PushAsync(new SalonRequest(), true);
                            }
                            //lblMessage.Text = "Sorry couldn't find any salons within this area!";
                            //lblMessage.IsVisible = true;
                            // lblMailLink.IsVisible = true;
                        }
                        else
                        {
                            lblMessage.Text = "Something went wrong! Please try again";
                            lblMessage.IsVisible = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
                    }
                }
                else
                {
                    lblMessage.Text = "Please enter the city!";
                    lblMessage.IsVisible = true;
                }

                btnSearch.IsEnabled = true;
                indicator.IsVisible = false;
            }
            catch (Exception ex)
            {

                indicator.IsVisible = false;
                errLog.WinrtErrLogTest(ex);
            }
        }

        private async void OnHelpClicked(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new Help());
        }

        void Search_Bar_PlacesRetrieved(object sender, AutoCompleteResult result)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                results_list.ItemsSource = result.AutoCompletePlaces;
                spinner.IsRunning = false;
                spinner.IsVisible = false;

                if (result.AutoCompletePlaces != null && result.AutoCompletePlaces.Count > 0 && IsPlaceSelected == false)
                {
                    results_list.IsVisible = true;
                    SearchResult.IsVisible = true;
                }
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex);
            }
        }

        void Search_Bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (!string.IsNullOrEmpty(e.NewTextValue))
                {
                    results_list.IsVisible = false;
                    spinner.IsVisible = true;
                    spinner.IsRunning = true;
                    SearchResult.IsVisible = false;
                    lblMessage.IsVisible = false;
                    lblMessage.Text = "";
                    //lblMailLink.IsVisible = false;
                }
                else
                {
                    results_list.IsVisible = true;
                    spinner.IsRunning = false;
                    spinner.IsVisible = false;
                    SearchResult.IsVisible = true;
                }
                if (e.NewTextValue == string.Empty)
                {
                    results_list.IsVisible = false;
                    SearchResult.IsVisible = false;
                    lblMessage.IsVisible = false;
                    lblMessage.Text = "";
                    // lblMailLink.IsVisible = false;
                    IsPlaceSelected = false;
                }
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex);
            }
        }

        async void Results_List_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (e.SelectedItem == null)
                    return;

                var prediction = (AutoCompletePrediction)e.SelectedItem;
                results_list.SelectedItem = null;

                var place = await Places.GetPlace(prediction.Place_ID, GooglePlacesApiKey);

                if (place != null)
                {
                    search_bar.Text = place.Name;
                    SearchResult.IsVisible = false;
                    results_list.IsVisible = false;
                    lblMessage.IsVisible = false;
                    lblMessage.Text = "";
                    // lblMailLink.IsVisible = false;
                    IsPlaceSelected = true;
                }
            }
            catch (Exception ex)
            {

                errLog.WinrtErrLogTest(ex);
            }
        }

        //private async void SalonRequest_Tapped(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new SalonRequest(), true);
        //}

    }
}