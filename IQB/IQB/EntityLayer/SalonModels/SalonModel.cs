using System;
using System.Collections.Generic;
using System.Text;

namespace IQB.EntityLayer.SalonModels
{
    public class SalonModel
    {
        public string SalonId { get; set; }
        public string SalonAppIcon { get; set; }
        public string SalonName { get; set; }
        public string SalonCode { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public string ContactTel { get; set; }
        public string SalonWebsite { get; set; }
        public string SalonTwitter { get; set; }
        public string SalonFacebook { get; set; }
        public string SalonInstagram { get; set; }
        public string SalonOnStop { get; set; }
        public string FtpFolderName { get; set; }
        public string AccountExpiryDate { get; set; }
        public string CountryCode { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string Currency { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
       
        public List<PaymentGatewayData> PaymentGatewaySettings { get; set; }
    }

    public class SalonResponseModel
    {
        public int StatusCode { get; set; }
        public SalonResponse Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class SalonResponse
    {
        public int SalonId { get; set; }
    }

    public class SalonListResponseModel
    {
        public int StatusCode { get; set; }
        public SalonListResponse Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class SalonListResponse
    {
        public string SalonId { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }

    public class SalonDetailsResponse
    {
  //      {
  //"PaymentGatewaySettings": {
  //  "merchant_id": "z46jdh5h8dkmhmnp",
  //  "public_key": "ncxywydb7c2x4mby",
  //  "private_key": "8cb22d04464a7799f79cf7b31224d83d",
  //  "tokenization_key": "sandbox_bnf3qq52_z46jdh5h8dkmhmnp"
  //},
        public PaymentGatewayData PaymentGatewaySettings { get; set; }
        public int StatusCode { get; set; }
        public SalonModel Response { get; set; }
        public string StatusMessage { get; set; }
    }


    public class PaymentGatewayData
    {
        public string merchant_email { get; set; }
        //public string merchant_id { get; set; }
        //public string public_key { get; set; }
        //public string private_key { get; set; }
        //public string tokenization_key { get; set; }

    }

    public class MapUserSalon
    {
        public string UserId { get; set; }
        public string SalonId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class MapUserSalonResponseModel
    {
        public int StatusCode { get; set; }
        public int Response { get; set; }
        public string StatusMessage { get; set; }
    }


    public class MappedSalonListResponseModel
    {
        public int StatusCode { get; set; }
        public MappedSalon[] Response { get; set; }
        public string StatusMessage { get; set; }
    }

    public class MappedSalon
    {
        public string SalonId { get; set; }
        public string SalonName { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string SalonAppIcon { get; set; }
    }


    public class Countries
    {
        public int StatusCode { get; set; }
        public Country[] Response { get; set; }
        public string StatusMessage { get; set; }
    }


    
    public class Country
    {
        public string Id { get; set; }
        public string CountryName { get; set; }
        public string Currency { get; set; }
        public string CountryCode { get; set; }
    }

    public class Currencies
    {
        public int StatusCode { get; set; }
        public Currency[] Response { get; set; }
        public string StatusMessage { get; set; }
    }



    public class Currency
    {
        public string currency_string { get; set; }
        public string symbol { get; set; }
    }


}
