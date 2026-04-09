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
            var sql = "SELECT * from appevents ORDER BY created_at desc";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<LogResponse>(sql)).AsList();
        }

        public async Task<List<LogResponse>> GetLogsQuery(int page, int pageSize, string searchTerm,string type,string action) 
        {
            int rows = (page - 1) * pageSize;
            string sql = $@"SELECT * 
                           from appevents
                           where (description like '%{searchTerm}%' || performed_by like '%{searchTerm}%') and event_type like '%{type}%' and event_action like '%{action}%'
                           limit @pageSize OFFSET @rows";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return (await connection.QueryAsync<LogResponse>(sql, new { pageSize, rows })).AsList();
        }
        public async Task<int> CountLogs(string searchTerm, string type, string action)
        {
            string sql = $@"SELECT COUNT(event_id) FROM appevents where (description like '%{searchTerm}%' || performed_by like '%{searchTerm}%' ) and event_type like '%{type}%' and event_action like '%{action}%'";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
