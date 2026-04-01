using MaintenanceApi.Data.Dapper;
using MaintenanceApi.Dto.Assets;
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
        [HttpGet("full")]
        public async Task<ActionResult<List<AssetResponse>>> GetAssets() 
        {
            return Ok(await _service.GetFullAllAssets());
        }

        [HttpGet("query")]
        public async Task<ActionResult> GetAssetsQuery([FromQuery] int page, int pageSize, string sortBy,string sortDirection) 
        {
            Console.WriteLine($"page={page}, pageSize={pageSize}, sortBy={sortBy}, sortDirection={sortDirection}");
            var res = await _service.GetAllAssetsQuery(page, pageSize,sortBy,sortDirection);

            return Ok(res);
        }
    }
}
