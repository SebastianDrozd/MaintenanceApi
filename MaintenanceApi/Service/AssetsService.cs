using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Dto.Pagination;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class AssetsService
    {
        private readonly AssetsRepo _repo;
        private readonly AssetImagesRepo _imagesRepo;

        public AssetsService(AssetsRepo repo, AssetImagesRepo imagesRepo) 
        {
            _repo = repo;
            _imagesRepo = imagesRepo;
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

        public async Task<dynamic> GetAllAssetsQuery(int page,int pageSize,string sortBy, string sortDirection,string searchTerm,string status)
        {
            if (searchTerm.IsWhiteSpace()) 
            {
                searchTerm = string.Empty;
            }
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var assets = await _repo.GetAllAssetsQuery(page, pageSize, sortBy,sortDirection,searchTerm,status);
            var count = await _repo.CountAssets(searchTerm,status);

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

        public async Task<dynamic> CreateNewAsset(CreateAssetRequest asset) 
        {
            Guid id = Guid.NewGuid();

            var res = await _repo.CreateNewAsset(id.ToString(), asset);

            //handle images
            if (res > 0 && asset?.Photos?.Count > 0) 
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "assets");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                foreach (var file in asset.Photos)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await _imagesRepo.CreateAssetPhoto(new SaveAssetImage
                    {
                        Path = uniqueFileName,
                        AssetId = id.ToString()
                    });
                }
            }

            return res;
        }

        public async Task<SingleAssetResponse> GetAssetById(string id) 
        {
            var asset =  await  _repo.GetAssetById(id);
            var images = await _imagesRepo.GetImagesForAsset(id);

            return new SingleAssetResponse
            {
                asset = asset,
                images = images
            };
        }

        public async Task<int> UpdateAsset(string id, UpdateAssetRequest asset) 
        {
            var res = await _repo.UpdateAsset(id, asset);
            return res;
        }
    }
}
