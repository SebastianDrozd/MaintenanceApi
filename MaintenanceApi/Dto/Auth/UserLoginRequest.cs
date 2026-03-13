using System.ComponentModel.DataAnnotations;

namespace MaintenanceApi.Dto.Auth
{
    public class UserLoginRequest
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
