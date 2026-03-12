using Microsoft.AspNetCore.Authentication;
using System.DirectoryServices.AccountManagement;

namespace MaintenanceApi.Service
{
    public class LdapService
    {
        private readonly IConfiguration _config;

        public LdapService(IConfiguration config) 
        {
            _config = config;
        }

        public bool ValidateUser(string username, string password) 
        {
            try
            {
                using var context = new PrincipalContext(ContextType.Domain, "BOBAK.LOCAL");
                return context.ValidateCredentials(username, password);
            }
            catch (PrincipalServerDownException ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }




        }
    }
}
