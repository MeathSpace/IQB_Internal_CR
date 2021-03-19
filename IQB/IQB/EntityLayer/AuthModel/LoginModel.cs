using System;

namespace IQB.EntityLayer.AuthModel
{
    public class LoginModel
    {
        public class LoginReturnResult
        {
            public int id { get; set; }
            public int SalonId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string SecurityQ { get; set; }
            public string SecurityA { get; set; }
            public string Email { get; set; }
            public string DateofBirth { get; set; }
            public string Mob { get; set; }
            public string ActivationCode { get; set; }
            public string Activated { get; set; }
            public string LoggedIn { get; set; }
            public string RegisterDate { get; set; }
            public string RateSystem { get; set; }
            public string RatingScore { get; set; }
            public string AppOSchecked { get; set; }
            public string AppOS { get; set; }
            public string LastLogin { get; set; }
            public string userIcon { get; set; }
            public string IsBarberRegistered { get; set; }
            public string ModuleAvailability { get; set; }
            public string Currency { get; set; }
            public DateTime AccountExpiryDate { get; set; }
            public int MaketingEmails { get; set; }
        }

        public class UserAuthenticationResult
        {
            public int UserLevel { get; set; }

            public int id { get; set; }

            public string StaffBarberId { get; set; }

            public bool IsBarber { get; set; }

            public string Email { get; set; }
        }
    }
}