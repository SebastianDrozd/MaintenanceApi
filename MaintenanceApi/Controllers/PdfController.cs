using MaintenanceApi.Service;
using MaintenanceApi.Util.PDF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _service;
        public PdfController(PdfService service) 
        {
            _service = service;
        }
       
        [HttpGet("workorder/{id}")]
        public async Task<IActionResult> GetPdf(int id)
        {
            var pdfBytes = await _service.GenerateWorkOrderPdf(id);

            return File(pdfBytes, "application/pdf", "workorder.pdf");
        }
    }
}
