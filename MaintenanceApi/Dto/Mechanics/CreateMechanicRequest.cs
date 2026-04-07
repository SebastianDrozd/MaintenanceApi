namespace MaintenanceApi.Dto.Mechanics
{
    public class CreateMechanicRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Shift { get; set; }
        public string Department { get; set; }

        public string? Notes { get; set; }
    }
}
