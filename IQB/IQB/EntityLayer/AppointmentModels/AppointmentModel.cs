using IQB.EntityLayer.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IQB.EntityLayer.AppointmentModels
{
    public class AppointmentModel
    {
        // "Date": "",
        //"SalonID": "",
        //"CustomerID": "",
        //"AppoinmentID": "",
        //"AppointmentStatus":"",
        public string AppointmentId { get; set; }
        public string AppointmentStatus { get; set; } = "Booked";

        public string AppointmentUpdatedBy { get; set; }
        public string AppointmentUpdatedTime { get; set; }

        public DateTime AppointmentStartTime { get; set; }
        public DateTime AppointmentEndTime { get; set; }
        public string AppointmentSubject { get; set; }
        public bool IsReservedAppointment { get; set; }
        public Color AppointmentColor { get; set; }
        public string AppointmentLocation { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();
    }


    public class AppointmentResponseBaseClass
    {
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
    }

    public class MappedAppointmentListResponseModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public Response Response { get; set; }
        //public string StatusMessage { get; set; }
    }

    public class Response
    {
        // public Appointment[] Appointment { get; set; }

        public List<Appointment> Appointment { get; set; }
    }

    public class Appointment
    {
        //public  Appointment()
        // {
        //     List<Service> Services = new List<Service>();
        // }
        public string AppointmentID { get; set; }
        public string SalonID { get; set; }
        public string Date { get; set; }
        public DateTime AppoDate
        {
            get
            {
                return UtilityEL.GetExactData(Date, "dd/MM/yyyy");
            }
        }

        public string UserName { get; set; }
        public string Status { get; set; }
        public string IsReserved { get; set; }
        public string CustomerName { get; set; }
        public List<Service> Services { get; set; }

        public string ReserveReason { get; set; }
        public string WalkInCustomerName { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDateTime { get; set; }

    }

    public class Service
    {
        public string ServiceId { get; set; }
        public string Price { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string BarberId { get; set; }

        public string BarberName { get; set; }

        public string EstWaitTime { get; set; }
        


        // public string ServiceName { get; set; } = "Reservation";
        private string _ServiceName;
        public string ServiceName {


            get
            {
                if (ServiceId == "0")
                {
                    return "Reservation";
                }
                else
                {
                    return _ServiceName;
                }
            }
            set
            {
                _ServiceName = value;


            }



        }
    }





    public class SetAppointmentResponseModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public SetAppointmentResponse Response { get; set; }
        // public string StatusMessage { get; set; }
    }

    public class SetAppointmentResponse
    {
        public string AppoinmentID { get; set; }
    }





    public class GetAppointmentByIdResponseModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public GetAppointmentByIdResponse Response { get; set; }
        //public string StatusMessage { get; set; }
    }

    public class GetAppointmentByIdResponse
    {
        public string AppointmentID { get; set; }
        public string SalonID { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string IsReserved { get; set; }

        public string ReserveReason { get; set; }
        public string WalkInCustomerName { get; set; }

        public string CustomerName { get; set; }

        public string StatusChangeReason { get; set; }
        public Service[] Services { get; set; }
    }

    //public class Service
    //{
    //    public string ServiceId { get; set; }
    //    public string Price { get; set; }
    //    public string From { get; set; }
    //    public string To { get; set; }
    //    public string BarberId { get; set; }
    //}

    public class SetAppoinmentStatusResponseModel : AppointmentResponseBaseClass
    {
        // public int StatusCode { get; set; }
        public SetAppoinmentStatusResponse Response { get; set; }
        // public string StatusMessage { get; set; }
    }

    public class SetAppoinmentStatusResponse
    {
        public string AppoinmentID { get; set; }
    }




    public class GetEngageBarberTimeSlotResponseModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public GetEngageBarberTimeSlotResponse[] Response { get; set; }
        //public string StatusMessage { get; set; }
    }

    public class GetEngageBarberTimeSlotResponse
    {
        public string AppointmentId { get; set; }

        public int AppointmentServiceId { get; set; }
        public string BarberId { get; set; }
        public string BarberName { get; set; }

        public string UserName { get; set; } = "0";
        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public DateTime AppoDate
        {
            get
            {
                return UtilityEL.GetExactData(Date + " " + From, "dd/MM/yyyy hh:mm tt");
            }
        }
    }


    public class GetEngageReserveTimeBySalonidModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public GetEngageReserveTimeBySalonidResponse[] Response { get; set; }
        // public string StatusMessage { get; set; }
    }



    public class GetEngageReserveTimeBySalonidResponse
    {
        public string AppointmentId { get; set; } = "0";
        public int AppointmentServiceId { get; set; }
        public string BarberId { get; set; }

        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }


        public DateTime AppoDate
        {
            get
            {
                return UtilityEL.GetExactData(Date + " " + From, "dd/MM/yyyy hh:mm tt");
            }
        }
    }

    public  class TimeCheckList
    {
        public string BarberId { get; set; }

        public string Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public DateTime AppoDate { get; set; }
    }





    public class AppointmentSettingsResponseModel : AppointmentResponseBaseClass
    {
        //public int StatusCode { get; set; }
        public AppointmentSettingsResponse Response { get; set; }
        // public string StatusMessage { get; set; }
    }

    public class AppointmentSettingsResponse
    {
        public int AppoinmentSettingsID { get; set; }
        //public string NumberOfDaysBeforeAppdate { get; set; }
        public string NumberOfDaysAfterAppdate { get; set; }
        public string ReScheduleEnableTime { get; set; }
        public string AdvanceBookingTime { get; set; }

        public string TimeSlotGap { get; set; }


        public string SalonStartTime { get; set; }
        public string SalonEndTime { get; set; }
        public string AdvancePaymentPercentage { get; set; }
    }


    public class SetAppoinmentSettingsResponseModel : AppointmentResponseBaseClass
    {
        // public int StatusCode { get; set; }
        public SetAppoinmentSettingsResponse Response { get; set; }
        // public string StatusMessage { get; set; }
    }

    public class SetAppoinmentSettingsResponse
    {
        public int AppoinmentSettingsID { get; set; }
    }




    public class DeleteAppointmentResponseModel
    {
        public int StatusCode { get; set; }
        public Response Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class DeleteAppointmentResponse
    {
        public int AppointmentID { get; set; }
    }



    public class BarberIDResponseModel
    {
        public int StatusCode { get; set; }
        public BarberIDResponse Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class BarberIDResponse
    {
        public string BarberId { get; set; }
    }



}
