using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using IQB.Utility;



namespace IQB.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            WriteErrorLog errLog = new WriteErrorLog();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }
            catch (Exception ex)
            {
                errLog.WinrtErrLogTest(ex);

            }
        }
    }
}
