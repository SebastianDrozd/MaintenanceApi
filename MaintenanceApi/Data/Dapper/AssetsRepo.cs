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

        public async Task<List<AssetResponse>> GetFullAllAssets() 
        {
            string sql = @"select * from assets";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<AssetResponse>(sql)).AsList();
        }
        public async Task<List<AssetResponse>> GetAllAssetsQuery(int page, int pageSize)
        {
            int rows = (page - 1) * pageSize;
            string sql = @"SELECT * 
                           FROM assets
                           LIMIT @pageSize OFFSET @rows;";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<AssetResponse>(sql, new 
            {
                pageSize,
                rows
            })).AsList();
        }

        public async Task<int> CountAssets()
        {
            string sql = @"SELECT COUNT(compid) FROM assets";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
