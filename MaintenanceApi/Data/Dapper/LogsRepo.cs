using Dapper;
using MaintenanceApi.Dto.Logs;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class LogsRepo
    {
        private readonly string _mysqlConnectionString;

        public LogsRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<int> CreateNewEvent(CreateNewLogRequest log) 
        {
            var sql = @"Insert into appevents(event_type,event_action,entity_id,description,performed_by)
                        values(@event_type,@event_action,@entity_id,@description,@performed_by)";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new 
            {
                log.event_type,
                log.event_action,
                log.entity_id,
                log.description,
                log.performed_by
            });
        }

        public async Task<List<LogResponse>> GetLogs() 
        {
            var sql = "SELECT * from appevents";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<LogResponse>(sql)).AsList();
        }

    }
}
