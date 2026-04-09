namespace MaintenanceApi.Dto.Pm
{
    public class UpdatePmTemplateRequest
    {
        public string? Asset { get; set; }
        public int Mechanic { get; set; }
        public string Priority { get; set; }
        public DateTime NextRunDate { get; set; }
        public string Frequency { get; set; }
        public string UpdatedBy { get; set; }
        public string Description { get; set; }
    }
}
