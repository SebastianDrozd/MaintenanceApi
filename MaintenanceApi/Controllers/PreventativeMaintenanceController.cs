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
            return Created();
        }

        [HttpGet("templates/short")]
        public async Task<ActionResult> GetShortPmTemplates() 
        {
            var pmTemplates = await _service.GetShortPmTemplates();
            return Ok(pmTemplates);
        }
        [HttpGet("templates/short/query")]
        public async Task<ActionResult> GetShortPmTemplatesQuery([FromQuery] int page, int pageSize, string? searchTerm, string? frequency) 
        {
            var templates = await _service.GetShortPmTemplatesQuery(page, pageSize, searchTerm, frequency);
            return Ok(templates);
        }

        [HttpGet("templates/{id}",Name ="GetTemplateById")]
        public async Task<ActionResult<FullPmTemplateResponse>> GetPmTemplateById(int id) 
        {
            var pmTemplates = await _service.GetPmTemplateById(id);
            return Ok(pmTemplates);
        }

        [HttpPut("templates/{id}")]
        public async Task<ActionResult> UpdatePmTemplate([FromRoute] int id, [FromBody] UpdatePmTemplateRequest pm) 
        {
            var res = await _service.UpdatePmTemplateById(pm, id);
            return NoContent();
        }

        [HttpDelete("templates/{id}")]
        public async Task<ActionResult> DeletePmTemplate(int id) 
        {
            var res = await _service.DeletePmTemplate(id);
            return NoContent() ;
        }

    }
}
