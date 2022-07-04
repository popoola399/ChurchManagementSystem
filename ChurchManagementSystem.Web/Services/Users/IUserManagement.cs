using System.Collections.Generic;
using System.Threading.Tasks;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Services.Users.Models;

namespace ChurchManagementSystem.Web.Services.Users
{
    public interface IUserManagement
    {
        Task<Response<CreateUserResponse>> CreateUser(CreateUserRequest createUserRequest, string token);

        Task<Response<List<UserRolesRequest>>> GetUserRoles(string token, bool use = true);

        Task<Response<GetUsersResponse>> GetUserById(string token, int id);

        Task<Response<PagedResponse<GetUsersResponse>>> GetAllUsers(GetUsersRequestDto model);

        Task<Response<List<GetUsersNameResponse>>> GetAllUsersName(string token, bool useToken);

        Task<Response> EditUser(EditUserRequest edithUserRequest, string token);

        Task<Response<GetUsersOverviewSummaryResponse>> GetUsersOverviewSummary(string token);

        //Task<Response<GetUsersTransactionSummaryResponse>> GetUsersTransactionSummary(string token);

        Task<Response<GetUserIndividualSummaryResponse>> GetUserIndividualSummary(string token, int userId);
    }
}