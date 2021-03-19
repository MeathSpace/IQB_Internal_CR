using Acr.UserDialogs;
using IQB.EntityLayer.Common;
using IQB.Utility;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IQB.Views.PhotoGallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoGalleryPage : ContentPage
    {
        bool isbusy = false;
        int _salonID;
        byte[] upfilebytes;
        string fileName = string.Empty;
        string fName = string.Empty;
        bool isViewOnly = false;
        public PhotoGalleryPage(int salonID, bool isView = false)
        {
            InitializeComponent();
            PhotosListView.ItemSize = Application.Current.MainPage.Width / 3;
            _salonID = salonID;
            isViewOnly = isView;
            if (Device.RuntimePlatform == Device.iOS)
            {
                GalleryBtn.CornerRadius = 20;
                CameraBtn.CornerRadius = 20;
            }
            if (isViewOnly)
            {
                AddImageFrame.IsVisible = false;
            }
            else
            {
                if (Application.Current.Properties["UserLevel"].ToString() == "2")  //Admin\
                {
                    AddImageFrame.IsVisible = true;
                    isViewOnly = false;
                }
                else
                {
                    AddImageFrame.IsVisible = false;
                    isViewOnly = true;
                }
            }
            LoadSalonImages();
        }

        private async void LoadSalonImages()
        {
            indicator.IsVisible = true;
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("GetSalonImageList.php");
            var Fullurl = url + "?SalonId=" + _salonID + "";
            try
            {
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(Fullurl);
                SalonImageListModel resultObject = new SalonImageListModel();
                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responsestr))
                {
                    resultObject = JsonConvert.DeserializeObject<SalonImageListModel>(responsestr);
                }
                if (resultObject != null && resultObject.StatusCode == 200)
                {
                    resultObject.Response.ForEach(x => x.IsDeleteVisible = !isViewOnly);
                    PhotosListView.ItemsSource = resultObject.Response;
                    this.Title = "Photo Gallery" + "  ( " + resultObject.Response.Count.ToString() + " )";
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error!", Convert.ToString(resultObject.Response), "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert!", "No images Found!", "Ok");
                indicator.IsVisible = false;
                return;
            }
            indicator.IsVisible = false;
        }

        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    status = await StorageUtil.CheckPermissions(Permission.Storage);
                }

                if (status == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Oops", "Pick photo not supported", "OK");
                        return;
                    }

                    MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = "2.jpg",
                        PhotoSize = PhotoSize.Small,
                    });
                    if (photo == null)
                    {
                        return;
                    }
                    string path = photo.Path;

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string[] partPaths = path.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if (partPaths != null && partPaths.Length > 0)
                        {
                            fileName = partPaths[partPaths.Length - 1];
                        }

                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            upfilebytes = await ReadFully(photo.GetStream());
                            int length = upfilebytes.Length;
                        }
                    }
                    await SalonUploadImageToServer();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void OnUploadGalleryClicked(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    status = await StorageUtil.CheckPermissions(Permission.Storage);
                }

                if (status == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Oops", "Pick photo not supported", "OK");
                        return;
                    }

                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        CustomPhotoSize = 50
                    });
                    if (photo == null)
                    {
                        return;
                    }
                    string path = photo.Path;


                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        string[] partPaths = path.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if (partPaths != null && partPaths.Length > 0)
                        {
                            fileName = partPaths[partPaths.Length - 1];
                        }

                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            upfilebytes = await ReadFully(photo.GetStream());
                            int length = upfilebytes.Length;
                        }
                    }
                    await SalonUploadImageToServer();
                }
            }
            catch (Exception)
            {


            }
        }

        private async void OpenPhotoOptionsClicked(object sender, EventArgs e)
        {
            if (!isbusy)
            {
                isbusy = true;
                if (SelectGalleryStack.Opacity == 0)
                {
                    UploadPhotoStack.IsVisible = true;
                    PhotosListView.Opacity = 0.1;
                    SelectPhotoFrame.Source = "resource://IQB.Resources.Image.cross.svg";
                    await SelectCameraStack.FadeTo(1, 300, Easing.CubicIn);
                    await SelectGalleryStack.FadeTo(1, 300, Easing.CubicIn);
                }

                else
                {
                    SelectPhotoFrame.Source = "resource://IQB.Resources.Image.TakePhoto.svg";
                    await SelectGalleryStack.FadeTo(0, 300, Easing.CubicOut);
                    await SelectCameraStack.FadeTo(0, 300, Easing.CubicOut);
                    UploadPhotoStack.IsVisible = false;
                    PhotosListView.Opacity = 1;
                }
                isbusy = false;
            }
        }

        public async Task<byte[]> ReadFully(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task SalonUploadImageToServer()
        {
            //variable
            var url = CommonEL.baseurl + CommonEL.GetCallUrl("Salon_Upload_Pic.php");
            var Fullurl = url + "?SalonId=" + _salonID + "";
            try
            {
                HttpClient client = new HttpClient();

                string boundary = "---8393774hhy37373773";
                MultipartFormDataContent content = new MultipartFormDataContent(boundary);

                var ImageContent = new ByteArrayContent(upfilebytes);

                ImageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

                string uploadedImageName = fileName;
                int index = uploadedImageName.LastIndexOf('.');
                var ext = uploadedImageName.Substring(index + 1);
                fName = _salonID + "." + ext; //UserId + "." + ext;
                ImageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = _salonID + "." + ext,  //UserId + "." + ext,
                    Name = "file",

                };
                content.Add(ImageContent);
                //content.Add(userIdContent, "userid");

                //upload MultipartFormDataContent content async and store response in response var
                //  var response = await client.PostAsync(url, content);

                App.toastConfig.Message = "Picture is being uploaded in the background!!";
                UserDialogs.Instance.Toast(App.toastConfig);

                var response = await client.PostAsync(Fullurl, content);
                ApiResult resultObject = new ApiResult();
                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrWhiteSpace(responsestr))
                {
                    resultObject = UtilityEL.ToApiModel(responsestr);
                }
                if (resultObject != null && resultObject.StatusCode == 200)
                {
                    await App.Current.MainPage.DisplayAlert("Success!", Convert.ToString(resultObject.Response), "Ok");
                    LoadSalonImages();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error!", Convert.ToString(resultObject.Response), "Ok");
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                await App.Current.MainPage.DisplayAlert("Error!", "Couldn't upload the image", "Ok");
                return;
            }
        }

        private async void DeleteImageClicked(object sender, EventArgs e)
        {
            var ans = await App.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to delete this image?", "Yes", "No");
            if (ans)
            {
                indicator.IsVisible = true;
                var url = CommonEL.baseurl + CommonEL.GetCallUrl("DeleteSalonImage.php");
                var Fullurl = url + "?SalonId=" + _salonID + "" + "&ImageId=" + (((sender as FFImageLoading.Svg.Forms.SvgCachedImage).Parent as Grid).Children[0] as Label).Text.ToString() + "";
                try
                {

                    HttpClient client = new HttpClient();

                    var response = await client.GetAsync(Fullurl);
                    ApiResult resultObject = new ApiResult();
                    //read response result as a string async into json var
                    var responsestr = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responsestr))
                    {
                        resultObject = JsonConvert.DeserializeObject<ApiResult>(responsestr);
                    }
                    indicator.IsVisible = false;
                    if (resultObject != null && resultObject.StatusCode == 200)
                    {
                        LoadSalonImages();
                        await App.Current.MainPage.DisplayAlert("Success!", Convert.ToString(resultObject.StatusMessage), "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error!", Convert.ToString(resultObject.StatusMessage), "Ok");
                    }
                    
                }
                catch (Exception ex)
                {
                    indicator.IsVisible = false;
                    App.toastConfig.Message = "Gallery is Empty!!";
                    UserDialogs.Instance.Toast(App.toastConfig);
                }
            }
        }

        private async void ZoomImageClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ZoomImagePage(((sender as Image).Source as UriImageSource).Uri.AbsoluteUri.ToString()));
        }
    }

    public class SalonImageListModel
    {
        public int StatusCode { get; set; }

        public List<SalonImageListResponse> Response { get; set; }

        public string StatusMessage { get; set; }
    }

    public class SalonImageListResponse
    {
        public string ImageID { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleteVisible { get; set; } = true;
    }

}