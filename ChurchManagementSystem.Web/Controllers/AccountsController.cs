using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using CSharpFunctionalExtensions;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Models;
using ChurchManagementSystem.Web.Services.Access;
using ChurchManagementSystem.Web.Services.UserProfile;
using ChurchManagementSystem.Web.Services.Users;

using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagementSystem.Web.Controllers
{
    [AllowAnonymous]
    public class AccountsController : Controller
    {
        private readonly IAccessManagement _accessManagement;
        private readonly IUserManagement _userManagement;
        private readonly IUserProfileManagement _userProfileManagement;

        public AccountsController(IAccessManagement accessManagement, IUserManagement userManagement,
            IUserProfileManagement userProfileManagement)
        {
            _accessManagement = accessManagement;
            _userManagement = userManagement;
            _userProfileManagement = userProfileManagement;
        }

        public IActionResult Login(string returnUrl = null)
        {                                                                    
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var request = new LoginRequest
                {
                    Login = new Login
                    {
                        ClientId = AppsettingsManager.Fetch("ClientId"),
                        ClientSecret = AppsettingsManager.Fetch("ClientSecret"),
                        Email = model.Email,
                        Password = model.Password,
                        Scope = AppsettingsManager.Fetch("Scope")
                    }
                };

                var rsp = await _accessManagement.Login(request);

                if (rsp != null)
                {
                    if (rsp.Error != null)
                    {
                        ViewBag.Msg = rsp?.Error_description;

                        return View();
                    }

                    // compose identity
                    var identity = SignInUser(rsp, model.Email).Result;
                    Result validateResponse = Result.Combine(identity);
                    if (validateResponse.IsFailure)
                    {
                        ViewBag.Msg = validateResponse.Error;
                        return View();
                    }

                    // sign in user
                    var principal = new ClaimsPrincipal(identity.Value);
                    var authProperties = new AuthenticationProperties() { IsPersistent = false };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity.Value),
                        authProperties);

                    Thread.CurrentPrincipal = principal;

                    //Save token in session object
                    HttpContext.Session.SetString("JWToken", rsp.Access_token);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Msg = "Can't Login At the Moment";

                    return View();
                }
            }

            return View("Index");
        }
                                                                       
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var request = new LogoutRequest
            {
                ClientSecret = "",
                ClientId = "",
                Token = HttpContext.Session.GetString("JWToken")
            };

            var rsp = await _accessManagement.Logout(request);

            if (rsp != null && !rsp.HasErrors)
            {
                HttpContext.Session.Clear();

                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return RedirectToAction("Index", "Home");
            }

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Register(string test)
        //{
        //    ViewBag.Msg = "Can't Register at the Moment...";

        //    return View();
        //}

        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        [HttpGet("ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Msg = "The email field is required";
                return View();
            }

            var request = new ForgotPasswordRequest
            {
                RecoveryEmail = email
            };

            var rsp = await _userProfileManagement.ForgotPassword(request);

            if (rsp != null && !rsp.HasErrors)
            {
                ViewBag.Info = $"We have sent a mail to {email} successfully";

                ViewBag.Email = email;

                return View();
            }
            else
            {
                ViewBag.Msg = rsp?.Errors[0].ErrorMessage ?? "Mail Can't be Sent At the Moment";

                return View();
            }
        }

        public IActionResult ResetForgotPassword(string email)
        {
            var model = new ResetForgotPasswordViewModel
            {
                Email = email
            };

            return View(model);
        }

        [HttpGet("ResetForgotPassword/{email}")]
        public async Task<IActionResult> ResetForgotPassword(ResetForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new ResetForgotPasswordRequest
                {
                    NewPassword = model.Password,
                    RecoveryEmail = model.Email,
                    Token = model.Token
                };

                var rsp = await _userProfileManagement.ResetForgotPassword(request);

                if (rsp != null && !rsp.HasErrors)
                {
                    return RedirectToAction("ResetForgotPasswordSuccess");
                }
                else
                {
                    ViewBag.Msg = rsp?.Errors[0].ErrorMessage ?? "Password Can't be changed At the Moment";

                    return View();
                }
            }

            return View();
        }

        public IActionResult ResetForgotPasswordSuccess()
        {
            return View();
        }

        private async Task<Result<ClaimsIdentity>> SignInUser(LoginResponse userProfile, string email)
        {
            var stream = userProfile.Access_token;
            var handler = new JwtSecurityTokenHandler();
            var tokens = handler.ReadToken(stream) as JwtSecurityToken;

            var roleId = tokens.Claims.First(claim => claim.Type == "RoleId").Value;
            if (roleId != "1")
            {
                return Result.Failure<ClaimsIdentity>($"Your profile does not have the permission to access to this portal.");
            }

            //var GetRoles = await _userManagement.GetUserRoles(stream);
            //if (GetRoles == null || GetRoles.HasErrors)
            //    return Result.Failure<ClaimsIdentity>($"Can't Access Roles in the system at the moment");

           // var roles = GetRoles.Data;

            var claims = new List<Claim>
            {
                new Claim(ChurchManagementSystemClaimTypes.UserName, email),
                new Claim(ChurchManagementSystemClaimTypes.FirstName, tokens.Claims.First(claim => claim.Type == "FirstName").Value),
                new Claim(ChurchManagementSystemClaimTypes.LastName, tokens.Claims.First(claim => claim.Type == "LastName").Value),
                new Claim(ChurchManagementSystemClaimTypes.FullName, $"{tokens.Claims.First(claim => claim.Type == "LastName").Value} {tokens.Claims.First(claim => claim.Type == "FirstName").Value}"),
                new Claim(ChurchManagementSystemClaimTypes.Token_type, userProfile.Token_type),
                new Claim(ChurchManagementSystemClaimTypes.Access_token, userProfile.Access_token),
                new Claim(ChurchManagementSystemClaimTypes.Expires_in, userProfile.Expires_in.ToString()),
                new Claim(ChurchManagementSystemClaimTypes.RoleId, tokens.Claims.First(claim => claim.Type == "RoleId").Value),
               // new Claim(ChurchManagementSystemClaimTypes.RoleName, roles.FirstOrDefault(a => a.RoleId == int.Parse(tokens.Claims.First(claim => claim.Type == "RoleId").Value))?.Name),
                new Claim(ChurchManagementSystemClaimTypes.UserId, tokens.Claims.First(claim => claim.Type == "UserId").Value),
                new Claim(ChurchManagementSystemClaimTypes.HasWebPortalAccess, tokens.Claims.First(claim => claim.Type == "HasWebPortalAccess").Value),

               // new Claim(ClaimTypes.Role, roles.FirstOrDefault(a => a.RoleId == int.Parse(tokens.Claims.First(claim => claim.Type == "RoleId").Value))?.Name)
            };

            // compose identity
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaims(claims);

            return Result.Success(identity);

            //return await Task.FromResult(identity);
        }
    }
}