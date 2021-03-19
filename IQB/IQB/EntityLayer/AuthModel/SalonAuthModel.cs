using System.Collections.Generic;
using XLabs.Data;

namespace IQB.EntityLayer.AuthModel
{
    public class SalonAuthModel
    {
        public int id { get; set; }
        public string SCode { get; set; }

        public string SalonName { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string PostalCode { get; set; }

        public string TelephoneNo { get; set; }

        public string Link { get; set; }

        public string Salon_ImageName { get; set; }

        public string BarberImageFtpFolder { get; set; }
    }

    public class SalonInfoApiResult
    {
        public SalonInfoApiResult()
        {
            Response = new List<SalonInfoAPIModel>();
        }

        public int StatusCode { get; set; }

        public List<SalonInfoAPIModel> Response { get; set; }

        public string StatusMessage { get; set; }
    }

    public class SalonInfoAPIModel
    {
        public int id { get; set; }

        public string SalonAppIcon { get; set; }

        public string SalonLogo { get; set; }

        public string SalonCode { get; set; }

        public string SalonName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }

        public string ContactTel { get; set; }

        public string SalonWebsite { get; set; }
        public string FtpFolderName { get; set; }
       public string FullAddress { get { return Address + ", " + PostCode; } }
    }
    
}