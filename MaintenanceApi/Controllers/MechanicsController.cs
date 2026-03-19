using MaintenanceApi.Dto.Mechanics;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MechanicsController : ControllerBase
    {
        private readonly MechanicService _service;

        public MechanicsController(MechanicService service) 
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MechanicResponse>>> GetAllMechanics() 
        {
            return Ok(await _service.GetAllMechanics());
        }


    }
}
