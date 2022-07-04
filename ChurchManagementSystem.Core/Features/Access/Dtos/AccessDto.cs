using ChurchManagementSystem.Core.Features.Users.Utility;

using Newtonsoft.Json;

namespace ChurchManagementSystem.Core.Features.Access.Dtos
{
    public class BaseAccessDto
    {
        /// <summary>
        /// The application client Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The application client secret
        /// </summary>
        public string ClientSecret { get; set; }
    }

    public class LoginDto : BaseAccessDto
    {
        /// <summary>
        /// The User's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The User's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The application's scope
        /// </summary>
        public string Scope { get; set; }
    }

    public class LogOutDto : BaseAccessDto
    {
        /// <summary>
        /// The application's token to be revoked
        /// </summary>
        public string Token { get; set; }
    }

    public class LoginWithThirdPartyDto : BaseAccessDto
    {
        /// <summary>
        /// The User's Valid Third-party Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The User's Agent of request
        /// </summary>
        [JsonIgnore]
        public string Agent { get; set; }

        /// <summary>
        /// The application's scope
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The type of third-party to be used
        /// </summary>
        public AccessType AccessType { get; set; }
    }
}