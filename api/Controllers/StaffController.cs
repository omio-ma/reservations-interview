using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Route("staff")]
    public class StaffController : Controller
    {
        private IConfiguration Config { get; set; }

        public StaffController(IConfiguration config)
        {
            Config = config;
        }

        /// <summary>
        /// Checks if the request is from a staff member, if not returns true and a 403 result
        /// </summary>
        /// <param name="request"></param>
        private bool IsNotStaff(HttpRequest request, out IActionResult? result)
        {
            // TODO explore UseAuthentication
            request.Cookies.TryGetValue("access", out string? accessValue);

            if (accessValue == null || accessValue == "0")
            {
                result = StatusCode(403);
                return true;
            }

            result = null;
            return false;
        }

        [HttpGet, Route("login")]
        public async Task<IActionResult> CheckCode([FromHeader(Name = "X-Staff-Code")] string accessCode)
        {
            var configuredSecret = Config.GetValue<string>("staffAccessCode");
            if (configuredSecret != accessCode)
            {
                // don't set cookie, don't indicate anything
                return NoContent();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Staff")
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


            return Ok();
        }

        [HttpGet, Route("check")]
        [Authorize]
        public IActionResult CheckCookie()
        {
            return Ok("Authorized");
        }
    }
}
