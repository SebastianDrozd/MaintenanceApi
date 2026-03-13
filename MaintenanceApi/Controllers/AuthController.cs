using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly LdapService _ldapService;
        private readonly AuthService _authservice;

        public AuthController(LdapService lservice,AuthService authService) 
        {
            _ldapService = lservice;
            _authservice = authService;
        }


        [HttpPost]
        public ActionResult LoginUser(UserLoginRequest loginRequest) 
        {
           var result = _authservice.LoginUser(loginRequest);

            if (!result.Success) 
            {
                return Unauthorized();
            }
          
     
            return Ok(result.User);
        }
    }
}
