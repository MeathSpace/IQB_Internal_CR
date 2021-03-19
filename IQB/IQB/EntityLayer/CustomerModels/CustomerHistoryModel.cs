using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.EntityLayer.CustomerModels
{
    public class CustomerHistoryModel
    {
        public class History
        {
            public string BarberName { get; set; }
            public string ServiceName { get; set; }
            public string DateOfBooking { get; set; }
        }

        public class RootObject
        {
            public string CustomerId { get; set; }
            public string CustomerFName { get; set; }
            public string CustomerLName { get; set; }
            public List<History> History { get; set; }
        }
    }
}
