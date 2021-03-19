namespace IQB.EntityLayer
{
    public class SalonEL
    {
        public string SalonLogo { get; set; }
        public string SalonName { get; set; }
    }

    public class FetchSalonByIdReturnResultModel
    {
        public bool IsSuccess { get; set; }
        public SalonEL AddonData { get; set; }
        public object Message { get; set; }
    }
}