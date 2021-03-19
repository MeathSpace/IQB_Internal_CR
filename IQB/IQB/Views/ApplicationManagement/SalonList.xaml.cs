using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.LoginManagement;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.ApplicationManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalonList : ContentPage
    {
        private List<SalonInfoAPIModel> _salonList = null;
        private SalonService obj = new SalonService();
        public SalonList(List<SalonInfoAPIModel> _salonList)
        {
            InitializeComponent();
            this._salonList = _salonList;
            //salonList.ItemsSource = _salonList;
            Title = _salonList.FirstOrDefault().City + " Salon's List";
            salonList.RefreshCommand = new Command<string>((s) => GetSalonList(_salonList.FirstOrDefault().City));
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Application.Current.Properties["IsAdminUserSalonRegistered"] = false;
        }
        private async void GetSalonList(string city)
        {
            ApiResult res = new ApiResult();
            res = await obj.GetSalonListbyCity(city);
            if (res != null && res.StatusCode == 200)
            {
                List<SalonInfoAPIModel> data = UtilityEL.ToModelList<SalonInfoAPIModel>(res.Response);
                if (data?.Count() > 0)
                {
                    _salonList = data;
                    salonList.ItemsSource = null;
                    salonList.ItemsSource = data;
                    salonList.EndRefresh();
                }
            }
        }

        private async void salonList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            indicator.IsVisible = true;
            var item = (SalonInfoAPIModel)e.Item;
            salonList.IsEnabled = false;
            if (NetworkConnectionMgmt.IsConnectedToNetwork())
            {
                ApiResult res = new ApiResult();

                using (SalonService obj = new SalonService())
                {
                    res = await obj.GetSalonInfoNew(item.SalonCode);
                }

                if (res != null && res.StatusCode == 200)
                {
                    List<SalonInfoAPIModel> data = UtilityEL.ToModelList<SalonInfoAPIModel>(res.Response);

                    if (data != null && data.Count() > 0)
                    {
                        SalonInfoAPIModel result = data.FirstOrDefault();

                        if (result != null && result.id > 0)
                        {
                            SalonViewModel salonViewModel = new SalonViewModel();

                            SalonAuthModel salonInfo = new SalonAuthModel()
                            {
                                id = result.id,
                                SCode = item.SalonCode,
                                address = result.Address,
                                city = result.City,
                                state = result.County,
                                PostalCode = result.PostCode,
                                TelephoneNo = result.ContactTel,
                                Link = result.SalonWebsite,
                                SalonName = result.SalonName,
                                Salon_ImageName = result.SalonAppIcon,
                                BarberImageFtpFolder = result.FtpFolderName
                            };

                            salonViewModel.SetSalonInfo(salonInfo, true);

                            await Navigation.PushAsync(new SalonInfo(salonViewModel));
                        }
                        else
                        {
                            await DisplayAlert("Error!", "Something is not right!", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error!", "Something is not right!", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error!", "Something is not right!", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Error!", CommonEL.NoNetworkErrorMsg, "Ok");
            }
            salonList.IsEnabled = true;
            indicator.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            await Navigation.PushAsync(new RegisterAdminSalon(_salonList.FirstOrDefault().City));
            indicator.IsVisible = false;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            salonList.BeginRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            indicator.IsVisible = true;
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                salonList.ItemsSource = _salonList;
            }

            else
            {
                // salonList.ItemsSource = _salonList.Where(x => x.SalonName.StartsWith(e.NewTextValue));
                salonList.ItemsSource = _salonList.Where(x => x.SalonName.Contains(e.NewTextValue));
            }
            indicator.IsVisible = false;
        }
    }
}