using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Forms;
using IQB.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using IQB.CustomControl;

[assembly: ExportRenderer(typeof(WebViewer), typeof(WebViewRender))]
namespace IQB.iOS
{
    public class WebViewRender : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var webView = e.NewElement as WebViewer;
            if (webView != null)
                webView.EvaluateJavascript = (js) =>
                {
                    return Task.FromResult(this.EvaluateJavascript(js));
                };
        }
    }
}