using System.Net;
using System.Threading.Tasks;
using ChurchManagementSystem.Core.Features.Access;
using ChurchManagementSystem.Core.Features.Access.Dtos;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [Produces("application/json")]
    public class AccessController : BaseApiController
    {
        private readonly PasswordPolicy _passwordPolicy;

        public AccessController(PasswordPolicy passwordPolicy)
        {
            _passwordPolicy = passwordPolicy;
        }

        /// <summary>
        /// Endpoint that returns token object to login user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("Login")]
        [Produces(typeof(Response<TokenDto>))]
        public Task<IActionResult> SignIn([FromBody] UserLoginRequest request)
        {
            return HandleAsyncAuthorized(request);
        }

     
        /// <summary>
        /// Endpoint to sign out user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("LogOut")]
        [Produces(typeof(Response))]
        public Task<IActionResult> SignOut([FromBody] UserLogOutRequest request)
        {
            return HandleAsync(request);
        }

        [AllowAnonymous]
        [HttpGet("Password-Policy")]
        [Produces(typeof(string[]))]
        public IActionResult GetPasswordPolicy()
        {
            var requirements = _passwordPolicy.FormatRequirements();

            return StatusCode((int)HttpStatusCode.OK, requirements);
        }
    }
}