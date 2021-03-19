using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.EntityLayer.AuthModel
{
    public class SettingsModel
    {
        public string username { get; set; }
        public int salonid { get; set; }
        public string imagename { get; set; }
    }
    public class AccountDeleteResultModelAPI
    {
        public int success { get; set; }

        public string message { get; set; }
    }
}
