namespace MaintenanceApi.Dto.WorkOrders
{
    public class UpdateWorkOrderRequest
    {
        public string? Asset { get; set; }
        public int Mechanic { get; set; }
        public string Priority { get; set; }
        public string? DueDate { get; set; }
        public string Description { get; set; }
    }
}
