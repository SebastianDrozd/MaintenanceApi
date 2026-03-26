using MaintenanceApi.Dto.Events;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class EventsRepo
    {
        private readonly string _mysqlConnectionString;

        public EventsRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }
    }
}
