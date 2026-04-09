namespace MaintenanceApi.Dto.Logs
{
    public class CreateNewLogRequest
    {
        public string event_type { get; set; }
        public string event_action { get; set; }
        public string entity_id { get; set; }
        public string description { get; set; }
        public string performed_by { get; set; }
    }
}
