using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

namespace ChurchManagementSystem.API.Configuration
{
    public class RewriteConfig
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swaggger"));
        }
    }
}