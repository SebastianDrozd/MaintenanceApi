namespace MaintenanceApi.Dto.WorkOrders
{
    public class ShortPmTemplateResponse
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Asset { get; set; }
        public string Frequency { get; set; }
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public DateTime LastRun { get; set; }
        public DateTime NextRunDate { get; set; }
    }
}
