namespace MaintenanceApi.Dto.WorkOrders
{
    public class SaveWorkOrder
    {
        public string? Asset { get; set; }
        public int Mechanic { get; set; }

        public string Type { get; set; }
        public string Requestor { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
