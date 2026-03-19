using Dapper;
using MaintenanceApi.Dto.WorkOrders;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class WorkOrdersRepo
    {
        private readonly string _myslConnectionString;

        public WorkOrdersRepo(IConfiguration config) 
        {
            _myslConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<int> SaveWorkOrder(SaveWorkOrder wo)
        {
            string sql = @"
        INSERT INTO workorders(DueDate,Requestor,Description,Priority,Mechanic,Asset)
        VALUES (@DueDate,@Requestor,@Description,@Priority,@Mechanic,@Asset);
        SELECT LAST_INSERT_ID();";
            await using var connection = new MySqlConnection(_myslConnectionString);
            var id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                wo.DueDate,
                wo.Requestor,
                wo.Description,
                wo.Priority,
                wo.Mechanic,
                wo.Asset
            });

            return id;
        }

        public async Task<dynamic> GetWorkOrderById(int id) 
        {
            string sql  = @"Select * from workorders wo
                            Inner Join mechanics mech
                            on mech.Id = wo.Mechanic 
                             where wo.Id = @id";

            await using var connection = new MySqlConnection(_myslConnectionString);

            return (await connection.QueryAsync<dynamic>(sql, new
            {
                id
            })).AsList();
        }
    }
}
