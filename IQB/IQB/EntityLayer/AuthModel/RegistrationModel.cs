namespace IQB.EntityLayer.AuthModel
{
    public class RegistrationModel
    {
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public string activationcode { get; set; }

        public string activated { get; set; }

        public string loggedin { get; set; }

        public string registerdate { get; set; }

        public int salonid { get; set; }

        public int canSendEmail { get; set; }
        public int UserLevel { get; set; }
        public bool IsBarber { get; set; }
    }

    public class RegistrationResultModelAPI
    {
        public int success { get; set; }

        public string message { get; set; }
    }

    public class EmailExistCheckModel
    {
        public int id { get; set; }

        public int SalonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class RegisterAdminResponseModel
    {
        public int StatusCode { get; set; }
        public AdminUserDetails Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class AdminUserDetails
    {
        public int USerName { get; set; }
        public string UserLevel { get; set; }
        public string SalonId { get; set; }
    }


    public class DeleteRegisteredUserResponseModel
    {
        public int StatusCode { get; set; }
        public DeleteRegisteredUserResponse Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class DeleteRegisteredUserResponse
    {
        public string UserId { get; set; }
    }
}