using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using ChurchManagementSystem.API.Security.Authorization;
using ChurchManagementSystem.Core.Features.Users;
using ChurchManagementSystem.Core.Features.Users.Dtos;
using ChurchManagementSystem.Core.Logic.Queries;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Security;
using System;
using ChurchManagementSystem.API.Controllers;

namespace ChurchManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Gets a paged list of all Users.
        /// </summary>
        // GET: api/User/GetAllUsers
        [HttpGet("GetAllUsers")]
        [AllowAnonymous]
        [Produces(typeof(Response<PagedQueryResult<GetUserDto>>))]
        public Task<IActionResult> GetAllUsers([FromQuery] PagedQueryRequest pageQuery,
            [FromQuery] string Search,
            [FromQuery] string SortBy,
            [FromQuery] int? RoleId,
            [FromQuery] bool SortAscending = true)
        {
            return HandleAsync(new GetAllUsersRequest()
            {
                Query = pageQuery,
                Search = Search ?? "",
                SortBy = SortBy ?? "",
                SortAscending = SortAscending,
                RoleId = RoleId
            });
        }

   
        /// <summary>                
        /// Adds a User object based on the data in the request.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        [Produces(typeof(Response<AddResponse>))]
        public Task<IActionResult> Post([FromBody] AddUserRequest request)
        {
            return HandleAsync(request);
        }


        /// <summary>
        /// Gets a list of all Users Name.
        /// </summary>
        // GET: api/User/GetAllUsersName
        [HttpGet("GetAllUsersName")]
        [AllowAnonymous]
        [Produces(typeof(Response<List<GetUsersNameDto>>))]
        public Task<IActionResult> GetAllUsersName()
        {
            return HandleAsync(new GetAllUsersNameRequest());
        }

        /// <summary>
        /// Gets a User permit by id.
        /// </summary>
        /// <param name="id">The id of the User.</param>
        // GET: api/User/GetUserById/id
        [HttpGet("GetUserById/{id:int}")]
        [AllowAnonymous]
        [Produces(typeof(Response<GetUserDto>))]
        public Task<IActionResult> GetUser(int id)
        {
            return HandleAsync(new GetUserByIdRequest(id));
        }


        /// <summary>
        /// Updates a User based on the data in the request.
        /// </summary>
        [HttpPut("EditUser")]
        [AllowAnonymous]
        [Produces(typeof(Response))]
        public Task<IActionResult> Edit([FromBody] EditUserRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Updates a User based on the data in the request.
        /// </summary>
        [HttpPut("EditUserByAdmin")]
        [AllowAnonymous]
        [Produces(typeof(Response))]
        public Task<IActionResult> EditUserByAdmin([FromBody] EditUserByAdminRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Remove User
        /// </summary>
        /// <param name="userId">The unique identifier of the User</param>
        [HttpDelete("DeleteUser/{userId:int}")]
        [AllowAnonymous]
        [Produces(typeof(Response))]
        public Task<IActionResult> DeleteUser(int userId)
        {
            return HandleAsync(new DeleteUserRequest(userId));
        }
    }


}
