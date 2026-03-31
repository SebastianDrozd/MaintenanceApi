namespace MaintenanceApi.Dto.Pm
{
    public class PmTemplateSqlWrapper
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
        public string AssetId { get; set; }
        public string AssetDesc { get; set; }
        public int MechId { get; set; }
        public string MechFirstname { get; set; }
        public string MechLastname { get;set; }
    }
}
