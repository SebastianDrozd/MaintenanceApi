namespace MaintenanceApi.Dto.Events
{
    public class SaveNewEvent
    {
        public int event_wo { get; set; }
        public string event_desc { get; set;}
        public string event_user { get; set; }
    }
}
