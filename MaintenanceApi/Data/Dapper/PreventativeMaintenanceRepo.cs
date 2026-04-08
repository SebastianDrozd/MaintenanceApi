using Dapper;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;
using MySqlConnector;
using System.Net.NetworkInformation;

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

        public async Task<List<PmTask>> GetPmTasksByTemplateId(int id) 
        {
            string sql = @"select * from pmtasks where PmId = @id Order by Id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<PmTask>(sql, new
            {
                id = id
            })).AsList();
        }

        public async Task<List<ShortPmTemplateResponse>> GetShortPmTempaltes() 
        {
            string sql = @"select temp.Id, temp.Description as Description,temp.Frequency as Frequency,temp.LastRun as LastRun,temp.NextRunDate as NextRunDate, m.FirstName, m.LastName, a.comp_Desc as Asset
                            from pmtemplate temp
                            Inner join mechanics m
                            on temp.Mechanic=m.Id
                            Inner Join assets a
                            on temp.Asset=a.compid";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<ShortPmTemplateResponse>(sql)).AsList();
        }

        public async Task<List<ShortPmTemplateResponse>> GetShortPmTempaltesQuery(int page, int pageSize,string searchTerm, string frequency)
        {
            int rows = (page - 1) * pageSize;
            string sql = $@"select temp.Id, temp.Description as Description,temp.Frequency as Frequency,temp.LastRun as LastRun,temp.NextRunDate as NextRunDate, m.FirstName, m.LastName, a.comp_Desc as Asset
                            from pmtemplate temp
                            Inner join mechanics m
                            on temp.Mechanic=m.Id
                            Inner Join assets a
                            on temp.Asset=a.compid
                            WHERE temp.Description like '%{searchTerm}%' and temp.Frequency like '%{frequency}%'
                            LIMIT @pageSize OFFSET @rows;";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<ShortPmTemplateResponse>(sql, new {pageSize,rows})).AsList();
        }


        public async Task<int> CountPmTemplates(string searchTerm, string frequency) 
        {
            string sql = $@"SELECT COUNT(Id) FROM pmtemplate Where Description like '%{searchTerm}%' AND Frequency like '%{frequency}%'";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<PmTemplateSqlWrapper> GetPmTemplateById(int id) 
        {
            string sql = @"select temp.Id, temp.Description , temp.Priority, temp.NextRunDate, temp.CreatedBy, temp.LastRun, temp.Frequency, temp.CreatedDate,temp.Status, asset.compid as AssetId, asset.comp_desc as AssetDesc, mech.Id as MechId, mech.FirstName as MechFirstname, mech.LastName as MechLastName
                           from pmtemplate temp
                           Inner Join assets asset on temp.Asset = asset.compid
                           Inner join mechanics mech on temp.Mechanic = mech.Id
                           where temp.Id = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryFirstAsync<PmTemplateSqlWrapper>(sql, new 
            {
                id
            }));
        }

        public async Task<int> UpdatePmTemplateById(UpdatePmTemplateRequest pm, int id) 
        {
            string sql = @"Update pmtemplate
                           set Asset = @Asset, Mechanic = @Mechanic, Priority = @Priority, NextRunDate = @NextRunDate, Frequency = @Frequency, Description = @Description
                            where Id = @id";

            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new 
            {
                pm.Asset,
                pm.Mechanic,
                pm.Priority,
                pm.NextRunDate,
                pm.Frequency,
                pm.Description,
                id
            });
        }

        public async Task<int> DeletePmTemplate(int id) 
        {
            string sql = @"Delete from pmtemplate where Id = @id ";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new 
            {
                id
            });

        }

    }
}
