namespace MaintenanceApi.Dto.Logs
{
    public class LogResponse
    {
        public int event_id { get; set; }
        public string event_type { get; set; }
        public string event_action { get; set; }
        public string entity_id { get; set; }
        public string description { get; set; }
        public string performed_by { get; set; }
        public DateTime created_at { get; set; }
    }
}
