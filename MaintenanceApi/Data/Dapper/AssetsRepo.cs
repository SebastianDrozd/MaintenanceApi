using Dapper;
using MaintenanceApi.Dto.Assets;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class AssetsRepo
    {
        private readonly string _mysqlConnectionString;

        public AssetsRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }

        public async Task<List<ShortAssetResponse>> GetAllAssets() 
        {
            string sql = @"Select compid,comp_desc from assets order by comp_desc ASC";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return (await connection.QueryAsync<ShortAssetResponse>(sql)).AsList();
        }
    }
}
