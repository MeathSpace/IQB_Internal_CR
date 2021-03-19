namespace IQB.EntityLayer.HomeModels
{
    using System.Collections.Generic;

    public class HomeEL
    {
        public HomeEL()
        {
            QStatusList = new List<QueueStatusListModel>();
        }

        public string SalonText { get; set; }

        public bool IsJoinedQueue { get; set; }

        public bool IsGroupJoined { get; set; }

        public string gcCode { get; set; }

        public bool SystemStatus { get; set; }

        public string Queuing { get; set; }

        public string EstimatedWaitTime { get; set; }

        public string BarbersOnDuty { get; set; }

        public string NextPositionAvailable { get; set; }

        public int QueueJoinStat { get; set; }

        public bool ShowStatMessag { get; set; }

        public List<QueueStatusListModel> QStatusList { get; set; }
    }

    public class QueueStatusListModel
    {
        public int? SlNo { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Barber { get; set; }

        public string TimeData { get; set; }

        public bool? IsChangedPosition { get; set; }
    }

    public class CheckJoinQueueApiResult
    {
        public int id { get; set; }

        public int Position { get; set; }

        public string Username { get; set; }

        public string FirstLastName { get; set; }

        public string BarberName { get; set; }

        public string QGCode { get; set; }

        public int QPosition { get; set; }
    }

    public class HomeQueueMiscDataApiModel
    {
        public int id { get; set; }

        public int BarbersOnDuty { get; set; }

        public string ExpTime { get; set; }

        public string SystemStatus { get; set; }

        public string SalonText { get; set; }

        public int NAP { get; set; }

        public int TotalQueue { get; set; }

        public string ForceUpdate { get; set; }
    }

    public class SelectBarberApiModel
    {
        public int id { get; set; }

        public int EWT { get; set; }
    }

    public class AdModelAPI
    {
        public int id { get; set; }

        public string AdName { get; set; }

        public int Position { get; set; }

        public string PicName { get; set; }

        public string Url { get; set; }
    }
}
