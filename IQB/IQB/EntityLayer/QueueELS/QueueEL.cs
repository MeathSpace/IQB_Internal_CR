namespace IQB.EntityLayer.QueueELS
{
    public class QueueEL
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string BarberName { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class QueueListApiModel
    {
        public int id { get; set; }

        public int SalonId { get; set; }

        public int QPosition { get; set; }

        public string Username { get; set; }

        public string FirstLastName { get; set; }

        public string BarberName { get; set; }

        public string QGCode { get; set; }
    }
}