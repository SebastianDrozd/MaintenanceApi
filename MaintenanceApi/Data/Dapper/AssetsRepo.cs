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
        public async Task<List<AssetResponse>> GetAllAssetsQuery(int page, int pageSize, string sortBy, string sortDirection)
        {
            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "compid", "comp_desc", "department", "line_no", "status", "manufacturer" };
            var column = allowedColumns.Contains(sortBy) ? sortBy : "Date";
            int rows = (page - 1) * pageSize;
            string sql = $@"SELECT * 
                           FROM assets
                           ORDER BY {column} {sortDirection}
                           LIMIT @pageSize OFFSET @rows;";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<AssetResponse>(sql, new 
            {
                sortBy,
                sortDirection,
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
