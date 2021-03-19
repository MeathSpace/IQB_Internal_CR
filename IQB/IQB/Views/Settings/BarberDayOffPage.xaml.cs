using IQB.EntityLayer.AuthModel;
using IQB.EntityLayer.Common;
using IQB.IQBServices.AdminServices;
using IQB.ViewModel.AuthViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarberDayOffPage : ContentPage
    {
        ObservableCollection<CheckListModel> objSrc { get; set; }
        public int pageBarberId { get; set; }

        public BarberDayOffPage()
        {
            InitializeComponent();

          
            if (Application.Current.Properties["UserLevel"].ToString() == "2")  //Admin\
            {
                 ProfileImage.Source = CommonEL.SalonImageLocation + "noimage.jpg";
                ReserveStack.IsVisible = false;
                GetBarberListForSelection();
            }
            else
            {
                pageBarberId = Convert.ToInt32(Application.Current.Properties["ProfileBarber_Id"]);
                using (LoginViewModel obj = new LoginViewModel())
                {
                    ProfileImage.Source = new UriImageSource
                    {
                        Uri = new Uri(obj.GetUserProfileImageName()),
                        CachingEnabled = false
                    };
                    BarberNameLabel.Text = obj.GetName();
                }
               
                GetCheckListView();
            }
        }

        private async void GetBarberListForSelection()
        {
            indicator.IsVisible = true;
            ApiResult res = new ApiResult();
            using (BarberSalonService obj = new BarberSalonService())
            {
                res = await obj.GetAppointmentAvlBarberBySalonId((Convert.ToInt32(Application.Current.Properties["_SalonId"])));
            }
            if (res != null && res.StatusCode == 200)
            {
                List<BarberModel.BarberReturnResult> result = UtilityEL.ToModelList<BarberModel.BarberReturnResult>(res.Response);

                BarberListView.ItemsSource = result;
                BarberListView.IsVisible = true;
                CheckListView.IsVisible = false;
            }
            indicator.IsVisible = false;
        }

        private async void GetCheckListView()
        {
            indicator.IsVisible = true;
            BarberListView.IsVisible = false;
            try
            {
                objSrc = new ObservableCollection<CheckListModel>(new[]
                      {
                new CheckListModel {DayName = "Monday", Ischecked = false, DayNum = 1},
                new CheckListModel {DayName = "Tuesday", Ischecked = false, DayNum = 2},
                new CheckListModel {DayName = "Wednesday", Ischecked = false, DayNum = 3},
                new CheckListModel {DayName = "Thrusday", Ischecked = false, DayNum = 4},
                new CheckListModel {DayName = "Friday", Ischecked = false, DayNum = 5},
                new CheckListModel {DayName = "Saturday", Ischecked = false, DayNum = 6},
                new CheckListModel {DayName = "Sunday", Ischecked = false, DayNum = 7},
            });

                ApiResult res = new ApiResult();
                using (BarberSalonService obj = new BarberSalonService())
                {
                    res = await obj.GetSetBarberDayOff((Convert.ToInt32(Application.Current.Properties["_SalonId"])), pageBarberId, null);
                }
                if (res != null && res.StatusCode == 200)
                {
                    if (!String.IsNullOrEmpty(res.Response.ToString()))
                    {
                        JArray arrDayOff = JArray.Parse(JsonConvert.DeserializeObject(res.Response.ToString()).ToString());
                        int[] DayOffNums = arrDayOff.Select(jv => (int)jv).ToArray();
                        foreach (var obj in objSrc)
                        {
                            if (DayOffNums.Contains(obj.DayNum))
                            {
                                obj.Ischecked = true;
                            }
                        }
                    }

                }

                CheckListView.ItemsSource = objSrc;
                ReserveStack.IsVisible = true;
                CheckListView.IsVisible = true;
            }
            catch (Exception ex)
            {

            }
            indicator.IsVisible = false;
        }

        private void BarberSelectListviewTapped(object sender, ItemTappedEventArgs e)
        {
            pageBarberId = (e.Item as BarberModel.BarberReturnResult).BarberId;
            ProfileImage.Source = (e.Item as BarberModel.BarberReturnResult).BarberPic;
            BarberNameLabel.Text = (e.Item as BarberModel.BarberReturnResult).BarberName;
            GetCheckListView();
        }

        private async void ReserveButtonTapped(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            try
            {
                if (objSrc != null)
                {
                    int[] arr = objSrc.Where(x => x.Ischecked == true).Select(y => y.DayNum).ToArray();
                    ApiResult res = new ApiResult();
                    using (BarberSalonService obj = new BarberSalonService())
                    {
                        res = await obj.GetSetBarberDayOff((Convert.ToInt32(Application.Current.Properties["_SalonId"])), pageBarberId, arr);
                    }
                    if (res != null && res.StatusCode == 200)
                    {
                        await DisplayAlert("Success", "Day Off Request Successfully Updated!!", "OK");
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            indicator.IsVisible = false;
        }

        public class CheckListModel
        {
            public string DayName { get; set; }
            public int DayNum { get; set; }
            public bool Ischecked { get; set; }
        }

        private void CheckListSelected(object sender, ItemTappedEventArgs e)
        {
            CheckListView.SelectedItem = null;
        }
    }
}