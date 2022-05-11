using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak_Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        private readonly ILogger<InfoController> _logger;

        public InfoController(ILogger<InfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("authenticated")]
        [Authorize]
        public IActionResult Authenticated()
        {
            return Ok(new { data = "User has authenticated.", user = GetSubject() });
        }

        [HttpGet("claims")]
        [Authorize]
        public IActionResult Claims()
        {
            return Ok(new { data = "User claims.", user = GetSubject(), claims = User.Claims });
        }

        [HttpGet("roles")]
        [Authorize]
        public IActionResult Roles()
        {
            return Ok(new { data = "User claims.", user = GetSubject(), roles = User.Claims.Where(c => c.Type == "user_roles").Select(x => x?.Value) });
        }

        [HttpGet("adminOnly")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminOnly()
        {
            return Ok(new { data = "User has admin policy", user = GetSubject() });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Ok(new { data = "User has admin role", user = GetSubject() });
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Manager")]
        public IActionResult Manager()
        {
            return Ok(new { data = "User has manager role", user = GetSubject() });
        }

        [HttpGet("employer")]
        [Authorize(Roles = "Employer")]
        public IActionResult Employer()
        {
            return Ok(new { data = "User has employer role", user = GetSubject() });
        }

        private string GetSubject()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        private string GetClaim(string type)
        {
            Claim c = User.Claims.FirstOrDefault(c => c.Type == type);
            return c?.Value;
        }
    }
}
