using MaintenanceApi.Dto.Pm;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreventativeMaintenanceController : ControllerBase
    {

        public readonly PreventativeMaintenanceService _service;

        public PreventativeMaintenanceController(PreventativeMaintenanceService service) 
        {
            _service = service;
        }

        [HttpGet("templates")]
        public async Task<ActionResult> GetPmTemplates() 
        {
            var pms = await _service.GetPmTemplates();
            return Ok(pms);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePmTemplateDates([FromRoute] int id, [FromBody] UpdateDatesPmTemplate pm) 
        {
            var res = await _service.UpdatePmDates(pm, id);

            return Ok();
        }

        [HttpPost("templates")]
        public async Task<ActionResult> CreatePmTemplate(CreatePmTemplateRequest pm) 
        {
            var res = await _service.CreateNewPmTemplate(pm);
            return Ok(res);
        }

        [HttpGet("templates/short")]
        public async Task<ActionResult> GetShortPmTemplates() 
        {
            var pmTemplates = await _service.GetShortPmTemplates();
            return Ok(pmTemplates);
        }


    }
}
