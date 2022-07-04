using System.Security;

namespace ChurchManagementSystem.Core.Exceptions
{
    public class AuthorizationFailedException : SecurityException
    {
        public AuthorizationFailedException()
        {
        }

        public AuthorizationFailedException(string message) : base(message)
        {
        }
    }
}