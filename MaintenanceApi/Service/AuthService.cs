using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Util;

namespace MaintenanceApi.Service
{
    public class AuthService
    {
        private readonly Ldap _ldap;

        public AuthService(Ldap ldap) 
        {
            _ldap = ldap;
        }

        public async void LoginUser(UserLoginRequest loginRequest) 
        {
            string username = loginRequest.username;
            string password = loginRequest.password;

            try 
            {

            }

        }


    }
}
