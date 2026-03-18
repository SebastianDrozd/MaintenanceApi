using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly AssetsService _service;

        public AssetsController(AssetsService service) 
        {
            _service = service;
        }
       // [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<dynamic>>> GetAllAssets() 
        {
            return Ok(await _service.GetAllAssets());
        }
    }
}
