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
        INSERT INTO workorders(Type,DueDate,Requestor,Description,Priority,Mechanic,Asset)
        VALUES (@Type,@DueDate,@Requestor,@Description,@Priority,@Mechanic,@Asset);
        SELECT LAST_INSERT_ID();";
            await using var connection = new MySqlConnection(_myslConnectionString);
            var id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                wo.Type,
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

        public async Task<List<dynamic>> GetWorkOrderById(int id) 
        {
            string sql  = @"Select * from workorders wo
                            Inner Join mechanics mech
                            on mech.Id = wo.Mechanic 
                            Inner join assets a
                            on a.compid = wo.Asset
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
                            where wo.status = 'open'
                            Order by wo.date DESC
                            limit 10
                            ";
            await using var connection = new MySqlConnection(_myslConnectionString);

            return (await connection.QueryAsync<dynamic>(sql)).AsList();
        }

        public async Task<List<dynamic>> GetWorkOrdersQuery(int page,int pageSize,string sortBy,string sortDirection, string searchTerm,string status,string priority,string type)
        {
            Console.WriteLine($"This is sortby : {sortBy}, this is direction : {sortDirection}. This is term : {searchTerm}, THis is TYpe : {type}");
            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase){"Id","Priority","Type","Status","Date","Requestor"};

            // Default fallback
            var column = allowedColumns.Contains(sortBy) ? sortBy : "Date";
            int rows = (page - 1) * pageSize;
            string sql = $@"SELECT wo.Id,wo.Priority,wo.Type,wo.Status,wo.Date,wo.Requestor,wo.Description, mech.FirstName,mech.LastName,a.comp_desc 
                            FROM workorders wo
                            INNER JOIN mechanics mech ON mech.id = wo.mechanic
                            INNER JOIN assets a ON wo.asset = a.compid
                            Where wo.Description like '%{searchTerm}%' AND wo.Status like '%{status}%' AND wo.Priority like '%{priority}%%' and wo.Type like '%{type}%'
                            ORDER BY wo.{column} {sortDirection}
                            LIMIT @pageSize OFFSET @rows;";

            await using var connection = new MySqlConnection(_myslConnectionString);

            return (await connection.QueryAsync<dynamic>(sql, new { pageSize,rows})).AsList();
        }

        public async Task<DashboardStatsResponse> GetDashboardStats()
        {
            string sql = @"
        SELECT
            (SELECT COUNT(*) 
             FROM workorders 
             WHERE status = 'Open') AS ActiveOrders,

            (SELECT COUNT(*) 
             FROM workorders 
             WHERE status = 'Open'
               AND DueDate < NOW()) AS PastDue,

            (SELECT COUNT(*) 
             FROM workorders 
             WHERE Date >= DATE_SUB(CURDATE(), INTERVAL 7 DAY)) AS NewThisWeek,

            (SELECT COUNT(*) 
             FROM workorders 
             WHERE type = 'Pm') AS OpenPms;
    ";

            await using var connection = new MySqlConnection(_myslConnectionString);
            return await connection.QueryFirstOrDefaultAsync<DashboardStatsResponse>(sql);
        }

        public async Task<int> CreateAuomatedWorkOrder(CreateAutomatedWorkOrderRequest wo) 
        {
            string sql = @"Insert into workorders (Asset,Type,Description,Priority,Mechanic,Requestor,PmTemplateId)
                            values (@Asset,@Type,@Description,@Priority,@Mechanic,@Requestor,@PmTemplatedId)";
            await using var connection = new MySqlConnection(_myslConnectionString);

            return await connection.ExecuteAsync(sql, new 
            {
                wo.Asset,
                wo.Type,
                wo.Description,
                wo.Priority,
                wo.Mechanic,
                wo.Requestor,
                wo.PmTemplatedId
            });
        }

        public async Task<int> CountWorkOrders(string searchTerm, string status, string priority, string type) 
        {
            string sql = $@"SELECT COUNT(Id) FROM workorders Where Description like '%{searchTerm}%' AND Status like '%{status}%' AND Priority like '%{priority}%%' and Type like '%{type}%'";
            await using var connection = new MySqlConnection(_myslConnectionString);
            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
