using MaintenanceApi.Exceptions;
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
                throw new LdapServiceUnavailableException("Error connecting to the ldap service");
            }




        }

        public List<string> GetGroups(string username, string password) 
        {
            List<string> groups = new List<string>();
            Console.WriteLine(username);
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "BOBAK.LOCAL","administrator","!Dcm3604454")) 
            {
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, username);
                if (user == null) 
                {
                    Console.WriteLine("user not found");
                }
                if (user != null) 
                {
                    var principalGroups = user.GetGroups();
                    Console.WriteLine(principalGroups);
                    foreach (var group in principalGroups) 
                    {
                        Console.WriteLine(group.Name);
                        groups.Add(group.Name);
                    }
                }
            }
            return groups;
        }
    }
}
