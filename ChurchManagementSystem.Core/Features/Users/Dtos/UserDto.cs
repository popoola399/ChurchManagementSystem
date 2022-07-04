using ChurchManagementSystem.Core.Features.Users.Utility;

using Newtonsoft.Json;

using System;

namespace ChurchManagementSystem.Core.Features.Users.Dtos
{
    public class BaseUserDto
    {
        /// <summary>
        /// The User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The User Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The User's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The User active flag
        /// </summary>
        public bool Active { get; set; }

    }

    public class GetUserDto : BaseUserDto
    {
        /// <summary>
        /// The parent node identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The User's Role name
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// The User's Role Id
        /// </summary>
        public string RoleId { get; set; }

        public bool NormalLoginEnabled { get; set; }

        /// <summary>
        /// Name to be displayed.
        /// </summary>
        public string Name => $"{FirstName} {LastName}";
    }

    public class GetUsersNameDto
    {
        /// <summary>
        /// The parent node identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The User Last name
        /// </summary>
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Name => $"{FirstName} {LastName}";
    }

    public class EditUserDto : BaseUserDto { }

    public class EditUserByAdminDto : BaseUserDto
    {
        /// <summary>
        /// The parent node identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The level Role of the user
        /// </summary>
        public string RoleType { get; set; }

        /// <summary>
        /// The level Role of the user
        /// </summary>
        public int RoleId { get; set; }
    }

    public class AddUserDto : BaseUserDto
    {
        /// <summary>
        /// The user Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The user Accepted TandC boolean
        /// </summary>
        public bool AcceptedTandC { get; set; }
    }

    public class AddUserWithThirdPartyDto
    {
        /// <summary>
        /// The User's Valid Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The User's Agent of request
        /// </summary>
        [JsonIgnore]
        public string Agent { get; set; }

        /// <summary>
        /// The User active flag
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The user flag to make donations to charity anonymous
        /// </summary>
        public bool SendDonationAsAnonymous { get; set; }

        /// <summary>
        /// The user Accepted TandC boolean
        /// </summary>
        public bool AcceptedTandC { get; set; }

        /// <summary>
        /// The type of third-party to be used
        /// </summary>
        public AccessType AccessType { get; set; }
    }

    public class AddUserByAdminDto : BaseUserDto
    {
        /// <summary>
        /// The user Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The level Role of the user
        /// </summary>
        public RoleType RoleType { get; set; }
    }

    public class SetUserPasswordDto
    {
        /// <summary>
        /// The user's identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user's new password.
        /// </summary>
        public string Password { get; set; }
    }

    public class ProfilePictureUploadResponse
    {
        /// <summary>
        /// The response Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The profile picture URL.
        /// </summary>
        public string ProfilePictureUrl { get; set; }
    }
}