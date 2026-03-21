namespace MaintenanceApi.Dto.WorkOrders
{
    public class CloseWorkOrderSave
    {
        public int Id { get; set; }
        public string? ClosedDescription { get; set; }
        public string? ClosedHours { get; set; }
        public string? ClosedMinutes { get; set; }
        public string ClosedBy { get; set; }
        public IFormFile? ClosedPhoto { get; set; }
    }
}
