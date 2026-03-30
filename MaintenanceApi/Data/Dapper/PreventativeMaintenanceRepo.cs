using Dapper;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class PreventativeMaintenanceRepo
    {
        private readonly string _mysqlConnectionString;

        public PreventativeMaintenanceRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<dynamic> GetPmTemplates() 
        {
            string sql = @"Select * from pmtemplate";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<dynamic>(sql)).AsList();
        }

        public async Task<dynamic> UpdateTemplateDates(UpdateDatesPmTemplate pm, int id) 
        {
            string sql = @"Update pmtemplate
                           set NextRunDate = @NextRunDate,LastRun = @LastRun
                           where Id = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new
            {
                pm.NextRunDate,
                pm.LastRun,
                id
            });
        }

        public async Task<int> CreatePmTemplate(CreatePmTemplateRequest pm) 
        {
            string sql = @"Insert into pmtemplate(Asset,Description,Mechanic,Priority,NextRunDate,CreatedBy,Frequency)
                           value(@Asset,@Description,@Mechanic,@Priority,@NextRunDate,@CreatedBy,@Frequency);
                           SELECT LAST_INSERT_ID();";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                pm.Asset,
                pm.Description,
                pm.Mechanic,
                pm.Priority,
                pm.NextRunDate,
                pm.CreatedBy,
                pm.Frequency

            });
        }

        public async Task<int> CreatePmTemplateTasks(string task, int id) 
        {
            string sql = @"insert into pmtasks (Description,PmId)
                           values (@task, @id)";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new 
            {
                task, 
                id
            });
        }

        public async Task<List<ShortPmTemplateResponse>> GetShortPmTempaltes() 
        {
            string sql = @"select temp.Description as Description,temp.Frequency as Frequency,temp.LastRun as LastRun,temp.NextRunDate as NextRunDate, m.FirstName, m.LastName, a.comp_Desc as Asset
                            from pmtemplate temp
                            Inner join mechanics m
                            on temp.Mechanic=m.Id
                            Inner Join assets a
                            on temp.Asset=a.compid";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<ShortPmTemplateResponse>(sql)).AsList();
        }

      
    }
}
