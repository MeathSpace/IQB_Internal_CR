using System;
using System.Threading.Tasks;

namespace IQB.Utility
{
    public class WriteErrorLog
    {
        public async Task WinrtErrLogTest(string msg="", string pagename = "", string methodname = "")
        {
            //if (ex != null)
            //{
            var objErrorLogger = new ErrorLogger("1FAIpQLScNzUMWWO7Had8T2E_kjDIDqYOqMvGeoYXyDR-nPjwfOkPDmw");

                objErrorLogger.AddEntry("1067832160" ,msg);
                objErrorLogger.AddEntry("1932671687", msg);
                objErrorLogger.AddEntry("1747538657", msg);
                objErrorLogger.AddEntry("1549180302", msg + "\n PageName = " + pagename + "\n MethodName = " + methodname);

                var objHttpResponse = await objErrorLogger.UploadAsync();
                if (objHttpResponse.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Exception logged successfully.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine
                    ("Error while exception logging. HTTP status : " + objHttpResponse.StatusCode);
                }
            }
        // }

        public async Task WinrtErrLogTest(Exception ex, string pagename = "", string methodname = "")
        {
            WriteErrorLog errLog = new WriteErrorLog();
            try
            {
                if (ex != null)
                {
                    var objErrorLogger = new ErrorLogger("1FAIpQLScNzUMWWO7Had8T2E_kjDIDqYOqMvGeoYXyDR-nPjwfOkPDmw");

                    objErrorLogger.AddEntry("1067832160", ex.GetType().Name);
                    objErrorLogger.AddEntry("1932671687", ex.Message);
                    objErrorLogger.AddEntry("1747538657", ex.StackTrace);
                    objErrorLogger.AddEntry("1549180302", ex.Source + "\n PageName = " + pagename + "\n MethodName = " + methodname);

                    var objHttpResponse = await objErrorLogger.UploadAsync();
                    if (objHttpResponse.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception logged successfully.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine
                        ("Error while exception logging. HTTP status : " + objHttpResponse.StatusCode);
                    }
                }
            }
            catch (Exception es)
            {
                errLog.WinrtErrLogTest(es);
            }
        }

    }
}
