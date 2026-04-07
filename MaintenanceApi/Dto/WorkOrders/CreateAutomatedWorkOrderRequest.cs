namespace MaintenanceApi.Dto.WorkOrders
{
    public class CreateAutomatedWorkOrderRequest
    {
        public string Asset { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public int Mechanic { get; set; }
        public string Requestor { get; set; }

        public int PmTemplatedId { get; set; }
    }
}
