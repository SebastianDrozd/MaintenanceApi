namespace MaintenanceApi.Dto.Pm
{
    public class CreatePmTemplateRequest
    {
        public string Asset { get; set; }
        public string Description { get; set; }
        public string NextRunDate { get; set; }
        public string CreatedBy{ get; set; }
        public string Priority { get; set; }
        public string Frequency { get; set; }
        public int Mechanic { get; set; }
        public List<string>? Tasks { get; set; }
    }
}
