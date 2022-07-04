namespace ChurchManagementSystem.Core.Features.Access.Dtos
{
    public class TokenDto
    {
        /// <summary>
        /// The access token
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// The type of token returned
        /// </summary>
        public string Token_type { get; set; }

        /// <summary>
        /// The expiry time of token in seconds
        /// </summary>
        public int Expires_in { get; set; }

        /// <summary>
        /// The OAuth error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// The OAuth error description
        /// </summary>
        public string Error_Description { get; set; }
    }
}