namespace IQB.IQBServices
{
    using Interfaces;
    using Xamarin.Forms;

    public class NetworkConnectionMgmt
    {
        public static bool IsConnectedToNetwork()
        {
            bool IsConnected = false;
            INetworkConnection networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();

            IsConnected = networkConnection.IsConnected;

            return IsConnected;
        }
    }
}
