using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Dto.Logs;
using MaintenanceApi.Dto.Pagination;
using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class AssetsService
    {
        private readonly AssetsRepo _repo;
        private readonly AssetImagesRepo _imagesRepo;
        private readonly LogService _logsService;

        public AssetsService(AssetsRepo repo, AssetImagesRepo imagesRepo,LogService logService) 
        {
            _repo = repo;
            _imagesRepo = imagesRepo;
            _logsService = logService;
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
            string id = Guid.NewGuid().ToString("N").Substring(0, 8);

            var res = await _repo.CreateNewAsset(id, asset);
            await _logsService.CreateNewEvent(new CreateNewLogRequest
            {
                event_type = "asset",
                event_action = "Created",
                description = $"New asset created: {asset.comp_desc}",
                performed_by = asset.CreatedBy,
                entity_id = id
                
            });
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

            Console.WriteLine($"Length of new Images {asset.newImages.Count}");
            var res = await _repo.UpdateAsset(id, asset);

            await _logsService.CreateNewEvent(new CreateNewLogRequest
            {
                event_type = "asset",
                event_action = "Modified",
                description = $"Asset was modified",
                performed_by = asset.UpdatedBy,
                entity_id = id

            });

            if (asset.removedImageIds.Count > 0) 
            {
                foreach (var imageId in asset.removedImageIds) 
                {
                    AssetImage image = await _imagesRepo.GetImageById(imageId);
                    Console.WriteLine($"THis is image that will be deleted {image.photo_path}");
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "assets");
                   string filePath = $"{uploadsFolder}/{image.photo_path}";
                    Console.WriteLine($"This is stringpath: {filePath}");
                    if (File.Exists(filePath)) 
                    {
                        File.Delete(filePath);
                        await _imagesRepo.DeleteAssetImageById(imageId);
                    }                              
                }

            }



            if (asset.newImages.Count > 0) 
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "assets");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                foreach (var file in asset.newImages)
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

        public async Task<dynamic> DeleteAssetById(string id) 
        {
            var res = _repo.DeleteAssetById(id);
            await _logsService.CreateNewEvent(new Dto.Logs.CreateNewLogRequest
            {
                event_type = "asset",
                event_action = "Deleted",
                description = $"Asset was Deleted : ${id}",
                performed_by = "maintenance",
                entity_id = id

            });
            return res;
        }
    }
}
