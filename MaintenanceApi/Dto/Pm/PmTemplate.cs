namespace MaintenanceApi.Dto.Pm
{
    public class PmTemplate
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime NextRunDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastRun { get; set; }
        public string Frequency { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
