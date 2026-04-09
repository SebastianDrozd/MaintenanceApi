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

        [HttpGet("query")]
        public async Task<ActionResult> GetLogs([FromQuery] int page, int pageSize,string? searchTerm, string? type, string? action) 
        {
            var logs = await _service.GetLogs(page,pageSize,searchTerm,type,action);
            return Ok(logs);
        } 
    }
}
