using System;
using System.Collections.Generic;
using System.Text;

namespace IQB.EntityLayer.AppointmentModels
{
    public class MappedBarberModel
    {
        public int StatusCode { get; set; }
        public Mappedbarber[] Response { get; set; }
        public string StatusMessage { get; set; }
    }

   

    public class Mappedbarber
    {
        public string BarberId { get; set; }
        public string BarberName { get; set; }
    }

}
