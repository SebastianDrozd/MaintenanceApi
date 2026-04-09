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
        public async Task<ActionResult> GetAssetsQuery([FromQuery] int page, int pageSize, string sortBy,string sortDirection,string searchTerm ="",string status = "") 
        {
            Console.WriteLine($"page={page}, pageSize={pageSize}, sortBy={sortBy}, sortDirection={sortDirection}");
            var res = await _service.GetAllAssetsQuery(page, pageSize,sortBy,sortDirection,searchTerm,status);

            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewAsset([FromForm] CreateAssetRequest asset) 
        {
            Console.WriteLine("endpoint hit");
            var res = await _service.CreateNewAsset(asset);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAssetById(string id) 
        {
            Console.WriteLine($"Endpoint hit with ID : {id}");
            var res = await _service.GetAssetById(id);
            return Ok(res);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsset([FromRoute]string id, [FromForm] UpdateAssetRequest asset) 
        {
            var res = await _service.UpdateAsset(id, asset);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(string id) 
        {
            await _service.DeleteAssetById(id);
            return NoContent();
        }
    }
}
