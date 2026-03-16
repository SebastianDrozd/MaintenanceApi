using MaintenanceApi.Dto.Auth;
using MaintenanceApi.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<ActionResult> LoginUser(UserLoginRequest loginRequest) 
        {
           var result = _authservice.LoginUser(loginRequest);

            if (!result.Success) 
            {
                return Unauthorized();
            }

            bool isADmin = false;
            foreach (string group in result.User.Roles) 
            {
                if(group.Contains("Admin"))
                    isADmin = true;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name , result.User.Username),
                new Claim(ClaimTypes.Role , isADmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            return Ok(result.User);
        }
        [Authorize]
        [HttpGet("me")]
        public ActionResult Me() 
        {
            var username = User.Identity?.Name;
            var role = "Admin";

            return Ok(new
            {
                username,
                role
            });
        }
    }
}
