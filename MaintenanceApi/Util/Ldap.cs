using MaintenanceApi.Exceptions;
using MaintenanceApi.Service;
using System.DirectoryServices.AccountManagement;

namespace MaintenanceApi.Util
{
    public class Ldap
    {
        private readonly IConfiguration _config;

        public Ldap(IConfiguration config) 
        {
            _config = config;
        }

        public async Task<bool> ValidateUser(string username, string password) 
        {
            try
            {
                using var context = new PrincipalContext(ContextType.Domain, "BOBAK.LOCAL");
                return context.ValidateCredentials(username, password);
            }
            catch (PrincipalServerDownException ex) 
            {
                throw new LdapServiceUnavailableException("The ldap service is unreachable");
            }
        }

    }
}
