

using System;
using System.Collections.Generic;

namespace IQB.EntityLayer.AuthModel
{
    public class BarberSaloonModel
    {
        public class StaffModelReturn
        {

            //public Int32 SalonId { get; set; }
            //public Int32 StaffId { get; set; }

            public int StatusCode { get; set; }
            public Response _response { get; set; }
            public string StatusMessage { get; set; }


        }

        public class Response
        {
            public int BarberId { get; set; }
            public String FirstName { get; set; }
            public String LastName { get; set; }
            public String PreferredName { get; set; }
            public String EmployeePasscode { get; set; }
            public String EmailAddress { get; set; }
            public String Estimatedwaittime { get; set; }
            public List<Services1> listservice { get; set; }
        }

        public class Services1
        {
            public int ServiceId { get; set; }
            public string ServiceIdTime { get; set; }
        }


    }


    public class Rootobject
    {
        public int BarberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string AvailableForAppointments { get; set; }
        public string EmployeePasscode { get; set; }
        public string EmailAddress { get; set; }
        public string Estimated_wait_time { get; set; }
        public string BarberPic { get; set; }
        public Service[] Services { get; set; }
    }

    public class Service
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTime { get; set; }
        public string ServicePrice { get; set; }
    }

}
