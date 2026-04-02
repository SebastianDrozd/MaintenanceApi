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
        public async Task<List<AssetResponse>> GetAllAssetsQuery(int page, int pageSize, string sortBy, string sortDirection,string searchTerm,string status)
        {
            var allowedColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "compid", "comp_desc", "department", "line_no", "status", "manufacturer" };
            var column = allowedColumns.Contains(sortBy) ? sortBy : "Date";
            int rows = (page - 1) * pageSize;
            string sql = $@"SELECT * 
                           FROM assets
                           WHERE comp_desc like '%{searchTerm}%' AND status like '%{status}%'
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

        public async Task<int> CountAssets(string searchTerm,string status)
        {
            string sql = $@"SELECT COUNT(compid) FROM assets where comp_desc like '%{searchTerm}%' AND status like '%{status}%'";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> CreateNewAsset(string id,CreateAssetRequest asset) 
        {
            string sql = @"Insert into assets(compid,comp_desc,line_no,department,manufacturer,model_no,serial_no,status)
                           values(@id,@comp_desc,@line_no,@department,@manufacturer,@model_no,@serial_no,@status)";
            await using var connection = new MySqlConnection(_mysqlConnectionString);

            return await connection.ExecuteAsync(sql, new 
            {
                id,
                asset.comp_desc,
                asset.line_no,
                asset.department,
                asset.manufacturer,
                asset.model_no,
                asset.serial_no,
                asset.status
            });
        }

        public async Task<FullAsset> GetAssetById(string id) 
        {
            string sql = @"select * from assets where compid = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryFirstAsync<FullAsset>(sql, new
            {
                id
            }));
        }

    
    }
}
