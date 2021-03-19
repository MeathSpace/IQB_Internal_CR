using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using IQB.iOS;

[assembly: ExportRenderer(typeof(ViewCell), typeof(CustomAllViewCellRendereriOS))]
namespace IQB.iOS
{
    public class CustomAllViewCellRendereriOS : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            if (cell != null)
            {
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
            }
            return cell;
        }
    }
}