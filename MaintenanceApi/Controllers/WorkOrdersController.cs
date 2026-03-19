using MaintenanceApi.Dto.WorkOrders;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly WorkOrdersService _service;

        public WorkOrdersController(WorkOrdersService service) 
        {
            _service = service;
        }

        [HttpGet("{id}",Name ="GetWorkOrder")]
        public async Task<ActionResult> GetWorkOrderById(int id) 
        {
            var wo = await _service.GetWorkOrderById(id);
            return Ok(wo);
        }

        [HttpPost(Name ="SaveWorkOrder")]
        public async Task<ActionResult> CreateWorkOrder([FromForm] CreateWorkOrderRequest wo) 
        {
         var id = await _service.CreateWorkOrder(wo);
            return CreatedAtRoute("GetWorkOrder", new
            {
                id
            });
        }

       
    }
}
