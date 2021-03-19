namespace IQB.EntityLayer.GenericModels
{
    public class RegistrationEmailEL
    {
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string activationcode { get; set; }

        public string email { get; set; }

        public string vSalonname { get; set; }

        public string vSalonTel { get; set; }

        public string vSalonWeb { get; set; }
    }

    public class ServiceEmail
    {
            public string SendToAdmin { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string SalonId { get; set; }
       

    }
}