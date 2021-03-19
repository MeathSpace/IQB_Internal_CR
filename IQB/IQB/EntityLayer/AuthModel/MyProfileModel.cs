namespace IQB.EntityLayer.AuthModel
{
    public class MyProfileModel
    {
        public int id { get; set; }
        public int SalonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string DOB { get; set; }
        public string ProfileImage { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
        public int IsMailEnabled { get; set; }
    }

    public class MyProfileUpdateResultModelAPI
    {
        public int success { get; set; }

        public string message { get; set; }
    }
}