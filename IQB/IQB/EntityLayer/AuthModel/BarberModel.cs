namespace IQB.EntityLayer.AuthModel
{
    public class BarberModel
    {
        public class BarberReturnResult
        {
            public int id { get; set; }
            public string BarberPic { get; set; }
            public string BarberName { get; set; }
            public string TotalQueue { get; set; }
            public string EWT { get; set; }
            public string Position { get; set; }
            public string BarberOnline { get; set; }
            public string SalonBarberFolder { get; set; }
            public string BarberStatus { get; set; }
            public int BarberId { get; set; }
            public int BarberOnDutyCnt { get; set; }
        }

        public class CheckUserReturnResult
        {
            public int id { get; set; }
            public int SalonId { get; set; }
            public string Position { get; set; }
            public string UserName { get; set; }
            public string DateJoinedQ { get; set; }
            public string ForceUpdateCheck { get; set; }
        }

        public class UserJoinQueueResultModelAPI
        {
            public int success { get; set; }

            public string message { get; set; }
        }


        public class UserQueueJoinInsertModel
        {
            public int position { get; set; }
            public string username { get; set; }
            public string firstlastname { get; set; }
            public string rdatejoinedq { get; set; }
            public string timejoinedq { get; set; }
            public string barbername { get; set; }
            public int salonid { get; set; }
            public string qgcode { get; set; }
            public string is_single_join { get; set; }
            public int ServiceId { get; set; }
            public int BarberId { get; set; }
        }
    }
}