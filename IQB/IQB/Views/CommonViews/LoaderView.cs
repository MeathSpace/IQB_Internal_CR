namespace IQB.Views.CommonViews
{
    using Xamarin.Forms;

    public class LoaderView : ContentView
    {
        public LoaderView()
        {
            ActivityIndicator loader = new ActivityIndicator();
            loader.IsRunning = true;

            Content = loader;
        }
    }
}