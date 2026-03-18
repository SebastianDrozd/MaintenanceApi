using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;

namespace MaintenanceApi.Service
{
    public class AssetsService
    {
        private readonly AssetsRepo _repo;

        public AssetsService(AssetsRepo repo) 
        {
            _repo = repo;
        }


        public async Task<List<ShortAssetResponse>> GetAllAssets() 
        {
            var assets = await _repo.GetAllAssets();
            return assets;
        }
    }
}
