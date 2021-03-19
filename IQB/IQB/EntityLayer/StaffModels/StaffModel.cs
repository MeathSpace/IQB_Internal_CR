using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQB.EntityLayer.StaffModels
{
    public class StaffModel
    {
        public int BarberId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PreferredName { get; set; }
        public String AvailableForAppointments { get; set; }
        public String EmployeePasscode { get; set; }
        public String EmailAddress { get; set; }
        public int Estimatedwaittime { get; set; }
        public Int32 SalonId { get; set; }
        public Int32 StaffId { get; set; }


        public List<Services> LstService { get; set; }
        public class Services
        {
            public int ServiceId { get; set; }
            public int ServiceTime { get; set; }
        }

    }
   


}
