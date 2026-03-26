using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Util.PDF;
using QuestPDF.Fluent;
using System.Net.NetworkInformation;

namespace MaintenanceApi.Service
{
    public class PdfService
    {

        private readonly WorkOrdersService _woService;
        private readonly WorkOrdersImagesRepo _imagesRepo;
        private readonly WorkOrdersRepo _woRepo;

        public PdfService(WorkOrdersService woService,WorkOrdersImagesRepo repo, WorkOrdersRepo woRepo)
        {
            _woService = woService;
            _imagesRepo = repo;
            _woRepo = woRepo;
        }

        public async Task<byte[]> GenerateWorkOrderPdf(int id)
        {
            var workOrder = await _woRepo.GetWorkOrderById(id);
            var workOrderImages = await _imagesRepo.GetWorkOrderImages(id);
            var closedWorkOrderImages = await _imagesRepo.GetWorkOrderImagesClosed(id);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            var openImageBytes = new List<byte[]>();
            var closedImageBytes = new List<byte[]>();

            if (workOrderImages != null)
            {
                foreach (var image in workOrderImages)
                {
                    if (!string.IsNullOrWhiteSpace(image.Path))
                    {
                        var fullPath = Path.Combine(uploadsFolder, image.Path);

                        if (File.Exists(fullPath))
                            openImageBytes.Add(await File.ReadAllBytesAsync(fullPath));
                    }
                }
            }

            if (closedWorkOrderImages != null)
            {
                foreach (var image in closedWorkOrderImages)
                {
                    if (!string.IsNullOrWhiteSpace(image.Path))
                    {
                        var fullPath = Path.Combine(uploadsFolder, image.Path);

                        if (File.Exists(fullPath))
                            closedImageBytes.Add(await File.ReadAllBytesAsync(fullPath));
                    }
                }
            }

            var doc = new WorkOrderPdf
            {
                Title = $"Work Order #{workOrder.ElementAt(0).Id}",
                Description = workOrder.ElementAt(0).Description ?? "",
                Requestor = workOrder.ElementAt(0).Requestor ?? "",
                Status = workOrder.ElementAt(0).Status ?? "",
                Mechanic = workOrder.ElementAt(0).Mechanic ?? "",
                Asset = workOrder.ElementAt(0).Asset ?? "",
                CreatedDate = workOrder.ElementAt(0).Date,
                DueDate = workOrder.ElementAt(0).DueDate,

                ClosedDescription = workOrder.ElementAt(0).ClosedDescription ?? "",
                ClosedHours = workOrder.ElementAt(0).ClosedHours,
                ClosedMinutes = workOrder.ElementAt(0).ClosedMinutes,
                ClosedDate = workOrder.ElementAt(0).ClosedDate,
                ClosedBy = workOrder.ElementAt(0).ClosedBy ?? "",

                WorkOrderImages = openImageBytes,
                ClosedWorkOrderImages = closedImageBytes
            };

            return doc.GeneratePdf();
        }
    }
}
