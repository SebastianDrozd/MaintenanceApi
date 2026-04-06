using Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Auth;
using MySqlConnector;

namespace MaintenanceApi.Data.Dapper
{
    public class AssetImagesRepo
    {
        private readonly string _mysqlConnectionString;

        public AssetImagesRepo(IConfiguration config) 
        {
            _mysqlConnectionString = config.GetConnectionString("Mysql");
        }


        public async Task<int> CreateAssetPhoto(SaveAssetImage image) 
        {
            string sql = @"INSERT INTO assetphotos(photo_asset_id,photo_path) values(@assetId,@path)";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new
            {
                image.AssetId,
                image.Path
            });
        }

        public async Task<List<AssetImage>> GetImagesForAsset(string id) 
        {
            string sql = @"select * from assetphotos where photo_asset_id = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return (await connection.QueryAsync<AssetImage>(sql, new
            {
                id
            })).AsList();
        }

        public async Task<int> DeleteAssetImageById(string id) 
        {
            string sql = @"DELETE from assetphotos where photo_id = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.ExecuteAsync(sql, new
            {
                id
            });
        }

        public async Task<AssetImage> GetImageById(string id) 
        {
            string sql = @"select * from assetphotos where photo_id = @id";
            await using var connection = new MySqlConnection(_mysqlConnectionString);
            return await connection.QueryFirstAsync<AssetImage>(sql, new 
            {
                id
            });
           
        }
       
    }
}
