using MaintenanceApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogService _service;
        public LogsController(LogService service) 
        {
           _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetLogs() 
        {
            var logs = await _service.GetLogs();
            return Ok(logs);
        } 
    }
}
