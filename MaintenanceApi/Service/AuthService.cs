using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Util;
using Microsoft.AspNetCore.Identity;
using System.DirectoryServices.AccountManagement;
using System.Security.Cryptography.X509Certificates;

namespace MaintenanceApi.Service
{
    public class AuthService
    {
        private readonly LdapService _ldap;

        public AuthService(LdapService ldap) 
        {
            _ldap = ldap;
        }

        public  LoginResponse LoginUser(UserLoginRequest loginRequest)
        {
            string username = loginRequest.username;
            string password = loginRequest.password;
            var result = _ldap.ValidateUser(username, password);
            if (!result)
            {
                return new LoginResponse
                {
                    Success = false
                };
            }
            var groups = _ldap.GetGroups(username, password);
            var roles = new List<string>();
            if (groups.Contains("Maintenance_Admin"))
                roles.Add("Admin");
            if (roles.Count == 0)
                roles.Add("User");
            return new LoginResponse
            {
                Success = true,
                User = new UserResponse
                {
                    Username = username,
                    Roles = roles
                }
            };
        }
    }
}
