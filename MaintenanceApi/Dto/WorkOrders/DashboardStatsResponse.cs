namespace MaintenanceApi.Dto.WorkOrders
{
    public class DashboardStatsResponse
    {
        public int ActiveOrders { get; set; }
        public int PastDue { get; set; }
        public int NewThisWeek { get; set; }
        public int ActivePms { get; set; }
    }
}
