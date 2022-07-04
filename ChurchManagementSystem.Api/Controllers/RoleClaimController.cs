using System.Collections.Generic;
using System.Threading.Tasks;
using ChurchManagementSystem.API.Security.Authorization;
using ChurchManagementSystem.Core.Features.RolesClaims;
using ChurchManagementSystem.Core.Features.RolesClaims.Dto;
using ChurchManagementSystem.Core.Mediator;
using ChurchManagementSystem.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagementSystem.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/roleclaim")]
    
    public class RoleClaimController : BaseApiController
    {
        /// <summary>
        /// Gets a list of available claims
        /// </summary>
        // GET: api/RoleClaim/GetAllClaims
        [HttpGet("GetAllClaims")]
        [Produces(typeof(Response<IEnumerable<ClaimDto>>))]
        public Task<IActionResult> GetAllClaims()
        {
            return HandleAsync(new GetAllClaimsRequest());
        }

        /// <summary>
        /// Gets a list of available roles
        /// </summary>
        // GET: api/RoleClaim/GetAllRoles
        [HttpGet("GetAllRoles")]
        [Produces(typeof(Response<IEnumerable<GetRoleDto>>))]
        public Task<IActionResult> GetAllRoles()
        {
            return HandleAsync(new GetAllRolesRequest());
        }

        /// <summary>
        /// Gets a role and its claims by roleId.
        /// </summary>
        /// <param name="roleId">The id of the Role.</param>
        // GET: api/RoleClaim/GetRoleClaimsByRoleId/roleId
        [HttpGet("GetRoleClaimsByRoleId/{roleId:int}")]
        [Produces(typeof(Response<GetRoleDto>))]
        public Task<IActionResult> GetRole(int roleId)
        {
            return HandleAsync(new GetRoleByRoleIdRequest(roleId));
        }

        /// <summary>
        /// Add a custom role and assign Claims to it.
        /// </summary>
        [RequiresClaims(Claims.AddRole, Claims.WebPortal)]
        [HttpPost("CreateCustomRole")]
        [Produces(typeof(Response<AddResponse>))]
        public Task<IActionResult> Post([FromBody]AddRoleRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Updates a Role and its claims based on the data in the request.
        /// </summary>
        [RequiresClaims(Claims.EditRole, Claims.WebPortal)]
        [HttpPut("EditCustomRole")]
        [Produces(typeof(Response))]
        public Task<IActionResult> Edit([FromBody]EditRoleRequest request)
        {
            return HandleAsync(request);
        }

        /// <summary>
        /// Delete roles
        /// </summary>
        /// <param name="roleId">The id of the Role.</param>
        [RequiresClaims(Claims.RemoveRole, Claims.WebPortal)]
        [HttpDelete("DeleteCustomRoles/{roleId:int}")]
        [Produces(typeof(Response))]
        public Task<IActionResult> DeleteRoles(int roleId)
        {
            return HandleAsync(new DeleteRoleRequest(roleId));
        }
    }
}