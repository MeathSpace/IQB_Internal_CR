namespace IQB.EntityLayer.Common
{
    public class ApiResult
    {
        public int StatusCode { get; set; }

        public object Response { get; set; }

        public string StatusMessage { get; set; }

        public int BarberID { get; set; }

        public int Cnt { get; set; }
    }
}
