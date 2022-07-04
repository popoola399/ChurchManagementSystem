using System.Threading.Tasks;

using ChurchManagementSystem.Core.Domain;
using ChurchManagementSystem.Core.Features.UserProfile;
using ChurchManagementSystem.Core.Features.Utilities;
using ChurchManagementSystem.Core.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagementSystem.API.Controllers
{
    [AllowAnonymous]
    [Route("api/userprofile")]
    [Produces("application/json")]
    public class UserProfileController : BaseApiController
    {
        /// <summary>
        /// Changes the user's password based on the data in the request.
        /// </summary>
        [HttpPut("UserResetPassword")]
        [Produces(typeof(Response))]
        public Task<IActionResult> UserResetPassword([FromBody]UserResetPasswordRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Send TOken to Email for Forgotten password reset
        /// </summary>
        [AllowAnonymous]
        [HttpPost("UserForgotPassword")]
        [Produces(typeof(Response))]
        public Task<IActionResult> UserForgotPassword([FromBody]UserForgotPasswordRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Reset user forgotten password with sent Email token which is part of the data in the request.
        /// </summary>
        [HttpPut("UserResetForgotPassword")]
        [Produces(typeof(Response))]
        public Task<IActionResult> UserResetForgotPassword([FromBody]UserResetForgotPasswordRequest request)
        {
            return HandleAsync(request);
        } 
        
        /// <summary>
        /// Reset user forgotten password with sent Email token which is part of the data in the request.
        /// </summary>
        [HttpPost("validate-token")]
        [Produces(typeof(Response))]
        public Task<IActionResult> ValidateToken([FromQuery]string token)
        {
            var mainToken = "";
            var email = "";

            if (token.Contains("xzyzyx"))
            {
                mainToken = token.Split("xzyzyx")[0];
                email = token.Split("xzyzyx")[1];
            }
            else
            {
                return HandleAsync("Invalid token provided.", System.Net.HttpStatusCode.BadRequest);
            }
            return HandleAsync(new ValidateTokenRequest(email, mainToken, UseCase.ValidateToken));
        }
    }
}