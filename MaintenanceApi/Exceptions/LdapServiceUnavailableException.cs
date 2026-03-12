namespace MaintenanceApi.Exceptions
{
    public class LdapServiceUnavailableException : Exception
    {
        public LdapServiceUnavailableException(string message) : base(message) { }
    }
}
