using System.Threading.Tasks;
using ChurchManagementSystem.Web.Extentions;
namespace ChurchManagementSystem.Web.Services.UserProfile
{
    public interface IUserProfileManagement
    {
        Task<Response> ForgotPassword(ForgotPasswordRequest forgotPasswordRequest);
        Task<Response> ResetForgotPassword(ResetForgotPasswordRequest resetForgotPasswordRequest);
    }
}