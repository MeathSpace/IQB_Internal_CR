using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.EntityLayer.CustomerModels
{
    public class CustomerModel
    {
        public Int32 CustomerId { get; set; }
        public string CustomerFName { get; set; }
        public string CustomerLName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerDOB { get; set; }
        public string CustomerImage { get; set; }
        public string MaketingEmails { get; set; }
    }

    public class CustomerMailModel
    {
        public int SalonId { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public List<CustomerIdModel> CustomerId { get; set; }
    }
    public class CustomerIdModel
    {
        public int CustomerID { get; set; }
    }
}
