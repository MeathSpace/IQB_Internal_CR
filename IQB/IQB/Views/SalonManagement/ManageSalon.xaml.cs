using Acr.UserDialogs;
using ContextMenu;
using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.EntityLayer.SalonModels;
using IQB.IQBServices;
using IQB.IQBServices.AuthServices;
using IQB.IQBServices.SalonServices;
using IQB.ViewModel.AuthViewModel;
using IQB.Views.ApplicationManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.SalonManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageSalon : ContentPage
    {
        private SalonServices objSalonService = new SalonServices();
        private List<MappedSalon> salonList = null;
        public ManageSalon()
        {
            InitializeComponent();
            SampleList.RefreshCommand = new Command<string>((s) => GetSalonList(Convert.ToString(Application.Current.Properties["UserName"])));
        }

        private async void GetSalonList(string userName)
        {
            salonList = new List<MappedSalon>();
            //var salonListResponse = await objSalonService.GetSalonList(userName);
            var salonListResponse = await objSalonService.GetUserSalonMapList(userName);
            if (salonListResponse?.StatusCode == 200)
            {
                if (salonListResponse?.Response?.Count() > 0)
                    salonList.AddRange(salonListResponse.Response);
                SampleList.ItemsSource = null;
                SampleList.ItemsSource = salonList;
                SampleList.EndRefresh();
            }
        }

        private async void AddSalon_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateSalon("0", true));
        }

        private async void EditSalon_Tapped(object sender, EventArgs e)
        {
            var salonID = (e as TappedEventArgs).Parameter?.ToString();
            await Navigation.PushAsync(new CreateSalon(salonID, true));
        }
        private async void DeleteSalon_Tapped(object sender, EventArgs e)
        {
            bool x = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Confirm",
                Message = "Do you want to delete this Salon?",
                OkText = "Yes",
                CancelText = "No"
            });
            if (x)
            {
                var salonID = (e as TappedEventArgs).Parameter?.ToString();
                var globalSalonID = Application.Current.Properties["_SalonId"].ToString();
                if (!globalSalonID.Equals(salonID))
                {
                    var result = await objSalonService.DeleteSalon(salonID);
                    if (result != null && result.StatusCode == 200)
                    {
                        var uName = Convert.ToString(Application.Current.Properties["UserName"]);
                        var removeMapResult = await objSalonService.RemoveMapUserSalon(new MapUserSalon
                        {
                            UserId = uName,
                            SalonId = salonID,
                            UpdatedBy = uName
                        });
                        if (removeMapResult != null && removeMapResult.StatusCode == 200)
                        {
                            UserDialogs.Instance.Toast(new ToastConfig("Salon Deleted Successfully!")
                            {
                                BackgroundColor = Color.FromHex("#0DA5B7"),
                                Duration = TimeSpan.FromSeconds(3),
                                Position = ToastPosition.Top
                            });
                            SampleList.BeginRefresh();
                        }
                    }
                }
                else
                {
                    UserDialogs.Instance.Toast(new ToastConfig("Salon can't be deleted!")
                    {
                        BackgroundColor = (Color)App.CurrentApp.Resources["ValidationBackground"],
                        Duration = TimeSpan.FromSeconds(3),
                        Position = ToastPosition.Top
                    });
                }
            }

        }

        private async void ViewSalon_Tapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            var salonID = (e as TappedEventArgs).Parameter?.ToString();
            var salonDetailsResponse = await objSalonService.GetSalonDetailsByID($"{salonID}");
            if (salonDetailsResponse?.StatusCode == 200)
            {
                if (salonDetailsResponse.Response != null)
                {
                    //await Navigation.PushAsync(new CreateSalon(salonID));
                    if (NetworkConnectionMgmt.IsConnectedToNetwork())
                    {
                        ApiResult res = new ApiResult();

                        using (SalonService obj = new SalonService())
                        {
                            res = await obj.GetSalonInfoNew(salonDetailsResponse.Response.SalonCode);
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
                                        SCode = salonDetailsResponse.Response.SalonCode,
                                        address = result.Address,
                                        city = result.City,
                                        state = result.County,
                                        PostalCode = result.PostCode,
                                        TelephoneNo = result.ContactTel,
                                        Link = result.SalonWebsite,
                                        SalonName = result.SalonName,
                                        Salon_ImageName = result.SalonAppIcon,
                                        BarberImageFtpFolder = result.FtpFolderName,
                                    };

                                    salonViewModel.SetSalonInfo(salonInfo, true);
                                    salonViewModel.IsConnectShowed = false;
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
                }
            }
            indicator.IsVisible = false;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            SampleList.BeginRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                SampleList.ItemsSource = salonList;
            }

            else
            {
                SampleList.ItemsSource = salonList.Where(x => x.SalonName.StartsWith(e.NewTextValue));
            }
        }

        private void imgCloseHint_Tapped(object sender, EventArgs e)
        {
            frmHintList.IsVisible = false;
            frmHintList.HeightRequest = 0;
            frmHintList.Margin = new Thickness(0);
        }
    }
    public class Song
    {
        public string title { get; set; }
    }
}