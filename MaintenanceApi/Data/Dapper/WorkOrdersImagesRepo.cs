using Dapper;
using MaintenanceApi.Dto.WorkOrders;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class WorkOrdersImagesRepo
    {
        private readonly string _mysqlConnectionString;

        public WorkOrdersImagesRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<dynamic> SaveWorkOrderImage(SaveWorkerOrderImage image)  
        {
            string sql = @"Insert into workordersimages(Path,WorkOrderId)
                          values(@Path,@WorkOrderId)";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return await connection.ExecuteAsync(sql, new 
            {
                image.Path,
                image.WorkOrderId
            });

        }

        public async Task<dynamic> GetWorkOrderImages(int id) 
        {
            string sql = @"select * from workordersimages where WorkOrderId = @id";

            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return (await connection.QueryAsync<dynamic>(sql, new { id })).AsList();
        }
    }
}
