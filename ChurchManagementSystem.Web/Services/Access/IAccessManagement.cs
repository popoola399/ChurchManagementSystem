using System.Threading.Tasks;

using ChurchManagementSystem.Web.Extentions;

namespace ChurchManagementSystem.Web.Services.Access
{
    public interface IAccessManagement
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);

        Task<Response> Logout(LogoutRequest logoutRequest);
    }
}