using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Pagination;

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

        public async Task<List<AssetResponse>> GetFullAllAssets() 
        {
            var assets = await _repo.GetFullAllAssets();
            return assets;
        }

        public async Task<dynamic> GetAllAssetsQuery(int page,int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var assets = await _repo.GetAllAssetsQuery(page, pageSize);
            var count = await _repo.CountAssets();

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList<AssetResponse>
            {
                Items = assets,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = totalPages
            };
        }
    }
}
