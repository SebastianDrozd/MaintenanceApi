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

        public async Task<int> UpdateWorkOrder(UpdateWorkOrderRequest wo, int id) 
        {
            string sql = @"update workorders
                            set Description = @description,Mechanic = @mechanic, Asset = @asset, DueDate = @dueDate, Priority = @priority
                            where Id = @id";
            await using var connection = new MySqlConnection(_myslConnectionString);

            var test =  await connection.ExecuteAsync(sql, new 
            {
                wo.Description,
                wo.Mechanic,
                wo.Asset,
                wo.DueDate,
                wo.Priority,
                id
            });
            return test;
        }

        public async Task<int> CloseWorkOrder(CloseWorkOrderSave wo) 
        {
            string sql = "UPDATE workorders " +
                          "set Status = 'Completed', ClosedDescription = @ClosedDescription, ClosedHours = @ClosedHours, ClosedMinutes = @ClosedMinutes,ClosedDate= CURDATE(), ClosedBy = @ClosedBy WHERE Id = @Id";
                          
            await using var connection = new MySqlConnection(_myslConnectionString);

           var id = await connection.ExecuteAsync(sql, new 
           {
               wo.ClosedDescription,
               wo.ClosedHours,
               wo.ClosedMinutes,
               wo.ClosedBy,
               wo.Id
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

        public async Task<List<dynamic>> GetTopWorkOrders() 
        {
            string sql = @"Select wo.Id , wo.Priority, wo.Type, wo.status,wo.Date, wo.Requestor, wo.Description, wo.Status,mech.FirstName, mech.LastName, a.comp_desc from workorders wo
                           inner join mechanics mech
                            on mech.id = wo.mechanic
                            inner join assets a
                            on wo.asset = a.compid
                            where status = 'open'
                            Order by wo.date DESC
                            limit 10
                            ";
            await using var connection = new MySqlConnection(_myslConnectionString);

            return (await connection.QueryAsync<dynamic>(sql)).AsList();
        }

        public async Task<List<dynamic>> GetWorkOrdersQuery(string sortBy)
        {
            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Id",
        "Priority",
        "Type",
        "Status",
        "Date",
        "Requestor"
    };

            // Default fallback
            var column = allowedColumns.Contains(sortBy) ? sortBy : "Date";

            string sql = $@"
             SELECT 
            wo.Id, 
            wo.Priority, 
            wo.Type, 
            wo.Status,
            wo.Date, 
            wo.Requestor, 
            wo.Description, 
            mech.FirstName, 
            mech.LastName, 
            a.comp_desc 
        FROM workorders wo
        INNER JOIN mechanics mech ON mech.id = wo.mechanic
        INNER JOIN assets a ON wo.asset = a.compid
        WHERE wo.status = 'open'
        ORDER BY wo.{column} ASC
    ";

            await using var connection = new MySqlConnection(_myslConnectionString);

            return (await connection.QueryAsync<dynamic>(sql)).AsList();
        }
    }
}
