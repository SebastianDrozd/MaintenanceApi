namespace MaintenanceApi.Dto.Mechanics
{
    public class FullMechanicResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }    
        public string Shift { get; set; }
        public string Department { get; set; }
        public string Notes { get; set; }
    }
}
