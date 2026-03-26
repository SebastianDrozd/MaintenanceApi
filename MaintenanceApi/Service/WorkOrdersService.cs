using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.WorkOrders;

namespace MaintenanceApi.Service
{
    public class WorkOrdersService
    {
        private readonly WorkOrdersRepo _repo;
        private readonly WorkOrdersImagesRepo _imagesRepo;
        
        public WorkOrdersService(WorkOrdersRepo repo, WorkOrdersImagesRepo imagesRepo)
        {
            _repo = repo;
            _imagesRepo = imagesRepo;
        }
            

        public async Task<int> CreateWorkOrder(CreateWorkOrderRequest wo)
        {
            Console.WriteLine(wo.Asset);
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(wo.DueDate))
                dueDate = DateTime.Parse(wo.DueDate);
            var newId = await _repo.SaveWorkOrder(new SaveWorkOrder
            {
                Asset = wo.Asset,
                Mechanic = wo.Mechanic,
                Requestor = wo.Requestor,
                Description = wo.Description,
                Priority = wo.Priority,
                DueDate = dueDate

            });
            //handle images
            if (newId > 0 && wo?.Photos?.Count > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                foreach (var file in wo.Photos)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await _imagesRepo.SaveWorkOrderImage(new SaveWorkerOrderImage
                    {
                        Path = uniqueFileName,
                        WorkOrderId = newId
                    });
                }
            }
            return newId;

        }


        public async Task<int> UpdateWorkOrder(UpdateWorkOrderRequest wo,int id) 
        {
            var result = await _repo.UpdateWorkOrder(wo, id);
            return result;
        }

        public async Task<int> CloseWorkOrder(CloseWorkOrderRequest wo, int id) 
        {
            var rowsAffected = await _repo.CloseWorkOrder(new CloseWorkOrderSave 
            {
                Id = id,
                ClosedDescription = wo.ClosedDescription,
                ClosedHours = wo.ClosedHours,
                ClosedMinutes = wo.ClosedMinutes,
                ClosedBy = wo.ClosedBy,

            });
         
            if (rowsAffected > 0 && wo.ClosedPhoto != null) 
            {
                Console.WriteLine("phot is not null.activing save");
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                    var file = wo.ClosedPhoto;
              
                    var extension = Path.GetExtension(file.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await _imagesRepo.SaveWorkOrderImageClosed(new SaveWorkerOrderImage
                    {
                        Path = uniqueFileName,
                        WorkOrderId = id
                    });
              

            }
            return 1;
        }


        public async Task<dynamic> GetWorkOrderById(int id)
        {
            var workorder = await _repo.GetWorkOrderById(id);
           
            var photos = await _imagesRepo.GetWorkOrderImages(id);
            var closedPhoto = await _imagesRepo.GetWorkOrderImagesClosed(id);
            return new
            {
                workorder,
                photos,
                closedPhoto
            };
        }

        public async Task<dynamic> GetTopWorkOrders() 
        {
            var workOrders = await _repo.GetTopWorkOrders();
            return workOrders;
        }

        public async Task<dynamic> GetWorkOrdersQuery(string sortBy,string sortDirection) 
        {
            var workOrders = await _repo.GetWorkOrdersQuery(sortBy,sortDirection);
            return workOrders;
        }
    }
}
