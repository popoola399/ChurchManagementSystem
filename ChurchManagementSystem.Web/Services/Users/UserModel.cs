using System;
using System.Collections.Generic;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Services.Users.Models;

using Newtonsoft.Json;

namespace ChurchManagementSystem.Web.Services.Users
{
    public class CreateUserRequest
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool Active { get; set; }
    }

    public class CreateUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }

    public class UserRolesRequest
    {
        public int RoleId { get; set; }
        public List<UserClaims> Claims { get; set; }
        public bool IsAssignable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CountyRestriction { get; set; }
    }

    public class UserClaims
    {
        public int ClaimId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GetUsersResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("roleName")]
        public string RoleName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("roleId")]
        public int RoleId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

    }

    public class GetUsersNameResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class EditUserRequest
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleType { get; set; }
        public bool Active { get; set; }
    }
}