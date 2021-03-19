using System;

namespace IQB.EntityLayer.ServiceModels
{
    public class ServiceModel
    {

        public int ServiceId { get; set; }
        public int BarberId { get; set; }
        public string ServiceName { get; set; }
        public string ServicePrice { get; set; }
        public Int32 SalonId { get; set; }
        public string Estimated_wait_time { get; set; }
        public string BarberLName { get; set; }
        public string BarberFName { get; set; }
        public string BarberNName { get; set; }
        public string least_EST_wait_time { get; set; }

    }

}
