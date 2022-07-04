using System;
using System.Text;
using System.Threading.Tasks;

using ChurchManagementSystem.Web.Configurations;
using ChurchManagementSystem.Web.Extentions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ChurchManagementSystem.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppsettingsManager.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InjectionExtention.ConfigureServices(services);
            AutoMapperConfig.Configure();
           
            services.AddControllers();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            //Provide a secret key to Encrypt and Decrypt the Token
            var secretKey = Encoding.ASCII.GetBytes("YourKey-2374-OFFKDI940NG7:56753253-tyuw-5769-0921-kfirox29zoxv");

            //Configure JWT Token Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/accounts/login";
                options.LogoutPath = "/accounts/logout";
                options.AccessDeniedPath = "/accounts/login";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);

                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = redirectContext =>
                    {
                        string redirectUri = redirectContext.RedirectUri;

                        UriHelper.FromAbsolute(
                            redirectUri,
                            out string scheme,
                            out HostString host,
                            out PathString path,
                            out QueryString query,
                            out FragmentString fragment);

                        redirectUri = UriHelper.BuildAbsolute(scheme, host, path);

                        redirectContext.Response.Redirect(redirectUri);

                        return Task.CompletedTask;
                    }
                };
            })
             .AddJwtBearer(token =>
             {
                 token.RequireHttpsMetadata = false;
                 token.SaveToken = true;
                 token.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                     ValidateIssuer = true,
                     //Usually this is your application base URL
                     ValidIssuer = "http://localhost:44382/",
                     ValidateAudience = true,
                     //Here we are creating and using JWT within the same application. In this case base URL is fine
                     //If the JWT is created using a web service then this could be the consumer URL
                     ValidAudience = "http://localhost:44382/",
                     RequireExpirationTime = true,
                     ValidateLifetime = true,
                     ClockSkew = TimeSpan.Zero
                 };
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/error/500");

                app.Use(async (context, next) =>
                {
                    await next();

                    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                    {
                        //Re-execute the request so the user gets the error page
                        string originalPath = context.Request.Path.Value;
                        context.Items["originalPath"] = originalPath;
                        context.Request.Path = "/error/404";
                        await next();
                    }
                });

                //app.UseDatabaseErrorPage();

                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseCookiePolicy();
            //Add User session
            app.UseSession();

            //Add JWToken to all incoming HTTP Request Header
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            //are you allowed?

            app.UseHttpsRedirection();

            app.UseRouting();
            //Add JWToken Authentication service
            //who you are?
                                                                         
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}