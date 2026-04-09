using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
using MaintenanceApi.Dto.Logs;
using MaintenanceApi.Dto.Pagination;
using MaintenanceApi.Dto.WorkOrders;
using MaintenanceApi.Util.Email;

namespace MaintenanceApi.Service
{
    public class WorkOrdersService
    {
        private readonly WorkOrdersRepo _repo;
        private readonly WorkOrdersImagesRepo _imagesRepo;
        private readonly EmailService _emailService;
        private readonly LogService _logsService;
        
        public WorkOrdersService(WorkOrdersRepo repo, WorkOrdersImagesRepo imagesRepo, EmailService emailService, LogService logService)
        {
            _repo = repo;
            _imagesRepo = imagesRepo;
            _emailService = emailService;
            _logsService = logService;
        }
            

        public async Task<int> CreateWorkOrder(CreateWorkOrderRequest wo)
        {
            Console.WriteLine(wo.Asset);
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(wo.DueDate))
                dueDate = DateTime.Parse(wo.DueDate);
            var newId = await _repo.SaveWorkOrder(new SaveWorkOrder
            {
                Type = wo.Type,
                Asset = wo.Asset,
                Mechanic = wo.Mechanic,
                Requestor = wo.Requestor,
                Description = wo.Description,
                Priority = wo.Priority,
                DueDate = dueDate

            });

            if (newId > 0) 
            {
                await _logsService.CreateNewEvent(new CreateNewLogRequest 
                {
                    event_type = "work_order",
                    event_action = "Created",
                    entity_id = newId.ToString(),
                    description = $"New work order created : {wo.Description}",
                    performed_by = wo.Requestor
                });
            }

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

            try
            {
              //  var message = $"View your work order at http://sebastian.bobak.local:3000/dashboard/workorders/{newId}";
                //await _emailService.SendEmail("sdrozd","New Work Order Created",message);
            }
            catch (Exception ex) 
            {

            }
            return newId;

        }


        public async Task<int> UpdateWorkOrder(UpdateWorkOrderRequest wo,int id) 
        {
            var result = await _repo.UpdateWorkOrder(wo, id);
            await _logsService.CreateNewEvent(new CreateNewLogRequest
            {
                event_type = "work_order",
                event_action = "Modified",
                entity_id = id.ToString(),
                description = $"Work was modified : {wo.Description}",
                performed_by = wo.UpdatedBy
            });
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
            await _logsService.CreateNewEvent(new CreateNewLogRequest
            {
                event_type = "work_order",
                event_action = "Completed",
                entity_id = id.ToString(),
                description = $"Closed work order : {wo.ClosedDescription}",
                performed_by = wo.ClosedBy
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

        public async Task<dynamic> GetWorkOrdersQuery(int page,int pageSize,string sortBy,string sortDirection,string searchTerm,string status,string priority,string type) 

        {
            if (searchTerm.IsWhiteSpace()) 
            {
                Console.WriteLine("String is whitespace");
                searchTerm = string.Empty;
            }
            if (type.IsWhiteSpace())
            {
                type = "Regular";
            }
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var workOrders = await _repo.GetWorkOrdersQuery(page,pageSize,sortBy,sortDirection,searchTerm,status,priority,type);
            var count = await _repo.CountWorkOrders(searchTerm, status, priority, type);
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            return new PaginatedList<dynamic>
            {
                Items = workOrders,
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = totalPages
            };
        }

        public async Task<DashboardStatsResponse> GetDashboardCardStats() 
        {
            var res = await _repo.GetDashboardStats();
            return res;
        }

        public async Task<int> CreatedAutomatedWorkOrder(CreateAutomatedWorkOrderRequest wo) 
        {
            var res = await _repo.CreateAuomatedWorkOrder(wo);
            return res;
        }
    }
}
