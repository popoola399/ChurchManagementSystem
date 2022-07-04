using ChurchManagementSystem.Web.Services.Access;
//using ChurchManagementSystem.Web.Services.Charities;
//using ChurchManagementSystem.Web.Services.Codes;
//using ChurchManagementSystem.Web.Services.Dashboard;
//using ChurchManagementSystem.Web.Services.Retailers;
//using ChurchManagementSystem.Web.Services.TermsAndCondition;
//using ChurchManagementSystem.Web.Services.Transaction;
using ChurchManagementSystem.Web.Services.UserProfile;
using ChurchManagementSystem.Web.Services.Users;

using Microsoft.Extensions.DependencyInjection;

namespace ChurchManagementSystem.Web.Configurations
{
    public static class InjectionExtention
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAccessManagement, AccessManagement>();
            services.AddTransient<IUserManagement, UserManagement>();
            services.AddTransient<IUserProfileManagement, UserProfileManagement>();
            //services.AddTransient<ITransactionManagement, TransactionManagement>();
            //services.AddTransient<ICharityManagement, CharityManagement>();
            //services.AddTransient<IRetailerManagement, RetailerManagement>();
            //services.AddTransient<ICodeManagement, CodeManagement>();
            //services.AddTransient<IDashboardSummaryManagement, DashboardSummaryManagement>();
            //services.AddTransient<ITermsAndConditionManagement, TermsAndConditionManagement>();
        }
    }
}