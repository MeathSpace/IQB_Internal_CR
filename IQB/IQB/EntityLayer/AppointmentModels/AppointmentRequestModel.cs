using System;
using System.Collections.Generic;
using System.Text;

namespace IQB.EntityLayer.AppointmentModels
{
    public class AppointmentRequestModel
    {
        public string ID { get; set; }
        public string SalonID { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string IsReserved { get; set; }

        public string ReserveReason { get; set; }
        public string WalkInCustomerName { get; set; }

        public string StatusChangeReason { get; set; }

        public string pId { get; set; } = "0";
        public string TotalAmount { get; set; } = "0";

        
        // public ServiceRequest[] Services { get; set; }

        public List<ServiceRequest> Services { get; set; }
    }



    public class ServiceRequest
    {
        public string ServiceId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string BarberId { get; set; }
    }

    public class AppoinmentSettingsRequestModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string SalonId { get; set; }
        //public string NumberOfDaysBeforeAppdate { get; set; }
        public string NumberOfDaysAfterAppdate { get; set; }

        public string ReScheduleEnableTime { get; set; }
        public string AdvanceBookingTime { get; set; }

        public string TimeSlotGap { get; set; }

        public string SalonStartTime { get; set; }
        public string SalonEndTime { get; set; }
        public string AdvancePaymentPercentage { get; set; }
    }


}
