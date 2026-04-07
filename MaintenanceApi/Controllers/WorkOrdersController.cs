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

        [HttpGet]
        public async Task<ActionResult> GetTopWorkOrders() 
        {
            var workorders = await _service.GetTopWorkOrders();
            return Ok(workorders);
        }

        [HttpGet("query")]
        public async Task<ActionResult> GetWorkOrdersQuery([FromQuery] int page, int pageSize, string sortBy,string sortDirection,string? searchTerm,string? status,string? priority, string? type) 
        {
            var workOrders = await _service.GetWorkOrdersQuery(page,pageSize,sortBy,sortDirection,searchTerm,status,priority,type);
            return Ok(workOrders);
        } 

        [HttpGet("{id}",Name ="GetWorkOrder")]
        public async Task<ActionResult> GetWorkOrderById(int id) 
        {
            var wo = await _service.GetWorkOrderById(id);
            if (wo == null) 
            {
                return NotFound();
            }
            return Ok(wo);
        }

        [HttpPost(Name ="SaveWorkOrder")]
        public async Task<ActionResult> CreateWorkOrder([FromForm] CreateWorkOrderRequest wo) 
        {

         var id = await _service.CreateWorkOrder(wo);
            return Ok(id);
          
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWorkOrder([FromForm] UpdateWorkOrderRequest wo, [FromRoute] int id) 
        {
            var result = await _service.UpdateWorkOrder(wo, id);
            return Ok(result);
        }


        [HttpPut("close/{id}")]
        public async Task<ActionResult> CloseWorkOrder([FromRoute] int id,[FromForm] CloseWorkOrderRequest wo) 
        {
            Console.WriteLine(id);
            var res = await _service.CloseWorkOrder(wo,id);
            return Ok(res);
        }
        [HttpGet("stats")]
        public async Task<ActionResult> GetDashboardStats() 
        {
            var res = await _service.GetDashboardCardStats();
            return Ok(res);
        }

        [HttpPost("automated")]
        public async Task<ActionResult> CreatedAutomatedWorkOrder(CreateAutomatedWorkOrderRequest wo) 
        {
            var res = await _service.CreatedAutomatedWorkOrder(wo);
            return Ok(res);
        }
       
    }
}
