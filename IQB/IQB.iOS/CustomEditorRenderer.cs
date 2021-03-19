using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(IQB.iOS.CustomEditorRenderer))]
namespace IQB.iOS
{
    public class CustomEditorRenderer:EditorRenderer
    {
        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.BorderColor =
                              UIColor.FromRGB(204, 204, 204).CGColor;
                Control.Layer.BorderWidth = 0.8f;
                Control.Layer.CornerRadius = 10f;
            }
        }
    }
}