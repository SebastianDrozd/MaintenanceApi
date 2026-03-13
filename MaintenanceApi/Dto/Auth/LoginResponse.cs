namespace MaintenanceApi.Dto.Auth
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public UserResponse? User { get; set; }
    }
}
