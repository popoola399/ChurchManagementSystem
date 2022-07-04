using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DataTables.AspNetCore.Mvc.Binder;

using ChurchManagementSystem.Web.Extentions;
using ChurchManagementSystem.Web.Models;
using ChurchManagementSystem.Web.Services.Users;
using ChurchManagementSystem.Web.Services.Users.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManagementSystem.Web.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [AllowAnonymous]
    public class UsersController : Controller
    {
        private readonly IUserManagement _userManagement;

        public UsersController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }

        public async Task<IActionResult> Test()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");
            var SummaryViewModel = new UserIndividualSummaryViewModel();
            var users = await _userManagement.GetAllUsersName(token, true);

            var userNames = users.Data;
            userNames.Insert(0, new GetUsersNameResponse() { Id = 0, Name = "Select User" });
            ViewBag.ListOfUsers = userNames;
            ViewBag.ActiveOptions = new List<string> { "Active", "Inactive" };
            return View(SummaryViewModel);
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");

            var summary = await _userManagement.GetUsersOverviewSummary(token);

            var overviewSummaryModel = Mapper.Map<UsersOverviewSummaryViewModel>(summary.Data);

            return View(overviewSummaryModel);
        }

        public async Task<IActionResult> Individual(int userId = 0, string info = "")
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");
            var SummaryViewModel = new UserIndividualSummaryViewModel();
            var users = await _userManagement.GetAllUsersName(token, true);

            var userNames = users.Data;
            userNames.Insert(0, new GetUsersNameResponse() { Id = 0, Name = "Select User" });
            ViewBag.ListOfUsers = userNames;
            ViewBag.ActiveOptions = new List<string> { "Active", "Inactive" };

            if (userId == 0)
            {
                SummaryViewModel.HasData = false;
            }
            else
            {
                var userSummary = await _userManagement.GetUserIndividualSummary(token, userId);

                SummaryViewModel = Mapper.Map<UserIndividualSummaryViewModel>(userSummary.Data);
                SummaryViewModel.HasData = true;
            }

            if (!string.IsNullOrEmpty(info))
            {
                ViewBag.Msg = info;
            }

            return View(SummaryViewModel);
        }

        //public async Task<IActionResult> CreateUser()
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
           
        //    if (token == null)
        //        return RedirectToAction("Index", "Home");

        //    var roles = await _userManagement.GetUserRoles(token);

        //    if (roles == null || roles.HasErrors)
        //    {
        //        ViewBag.Msg = "Can't Access Roles at the moment";

        //        return View();
        //    }

        //    var displayRoles = new List<RolesViewModel>();

        //    foreach (var role in roles.Data)
        //    {
        //        displayRoles.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
        //    }

        //    displayRoles.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "Assign Role to user" });

        //    ViewBag.ListofRoles = displayRoles;

        //    return View(roles);
        //}

        //[HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDto createUser)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token != null)
                return RedirectToAction("Index", "Home");

            var createUserRequest = Mapper.Map<CreateUserRequestDto, CreateUserRequest>(createUser);

            var user = await _userManagement.CreateUser(createUserRequest, token);

            if (user != null && !user.HasErrors)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Msg = string.IsNullOrEmpty(user?.Errors[0].ErrorMessage) ? "User Can't be created at the moment" : user?.Errors[0].ErrorMessage;

            return View("CreateUser");
        }

        public IActionResult ViewUsers()
        {
            return View();
        }

        public async Task<IActionResult> ViewProfile(string userId)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");

            var user = await _userManagement.GetUserById(token, Convert.ToInt32(userId));
            if (user == null || user.HasErrors)
            {
                ViewBag.Msg = "Can't Get Users at the moment";

                return View("Index");
            }
            var model = Mapper.Map<GetUsersResponse, UserProfileViewModel>(user.Data);

            return View(model);
        }

        //public async Task<IActionResult> EditProfile(int id)
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    if (token == null)
        //        return RedirectToAction("Index", "Home");

        //    var user = await _userManagement.GetUserById(token, id);

        //    var model = new EditUserProfileViewModel
        //    {
        //        FirstName = user.Data.FirstName,
        //        LastName = user.Data.LastName,
        //        Email = user.Data.Email,
        //        Id = user.Data.Id,
        //        //RoleId = user.Data.RoleId,
        //        //Active = user.Data.Active
        //    };

        //    var roles = await _userManagement.GetUserRoles(token, false);

        //    if (roles == null || roles.HasErrors)
        //    {
        //        ViewBag.Msg = "Can't Access Roles at the moment";

        //        return View();
        //    }

        //    var displayRoles = new List<RolesViewModel>();

        //    foreach (var role in roles.Data)
        //    {
        //        displayRoles.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
        //    }

        //    displayRoles.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "User Role" });

        //    ViewBag.ListofRoles = displayRoles;

        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");

            if (model.RoleId == "0" || model.Active == "0")
            {
                ViewBag.Msg = "All Fields are required";

                var user = await _userManagement.GetUserById(token, model.Id);

                if (user != null)
                {
                    var request = new EditUserRequest
                    {
                        Active = true,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        RoleId = Convert.ToInt32(model.RoleId),
                        UserId = model.Id,
                    };

                    var edithUser = await _userManagement.EditUser(request, token);

                    if (edithUser.HasErrors)
                    {
                        ViewBag.Msg = "Can't Edit Profile At the moment";

                        var roles1 = await _userManagement.GetUserRoles(token, false);

                        if (roles1 == null || roles1.HasErrors)
                        {
                            ViewBag.Msg = "Can't Access Roles at the moment";

                            return View();
                        }

                        var displayRoles1 = new List<RolesViewModel>();

                        foreach (var role in roles1.Data)
                        {
                            displayRoles1.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
                        }

                        displayRoles1.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "User Role" });

                        ViewBag.ListofRoles = displayRoles1;

                        return View(model);
                    }
                }

                var roles = await _userManagement.GetUserRoles(token, false);

                if (roles == null || roles.HasErrors)
                {
                    ViewBag.Msg = "Can't Access Roles at the moment";

                    return View();
                }

                var displayRoles = new List<RolesViewModel>();

                foreach (var role in roles.Data)
                {
                    displayRoles.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
                }

                displayRoles.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "User Role" });

                ViewBag.ListofRoles = displayRoles;

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var request = new EditUserRequest
                {
                    Active = (model.Active == "Active"),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    UserId = model.Id,
                    LastName = model.LastName,
                    RoleId = Convert.ToInt32(model.RoleId)
                };

                var editUser = await _userManagement.EditUser(request, token);

                if (!editUser.HasErrors)
                {
                    ModelState.AddModelError("Info", "User update successful");
                    //return RedirectToAction("Index", "Dashboard");
                    return RedirectToAction("Individual", "Users", new { userId = model.Id, info = "User update successful" });
                }
                else
                {
                    //ViewBag.Msg = editUser.Errors[0].ErrorMessage;
                    ModelState.AddModelError("Error", editUser.Errors[0].ErrorMessage);
                    return RedirectToAction("Individual", "Users", new { userId = model.Id, info = editUser.Errors[0].ErrorMessage });
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile2(EditUserProfileViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return RedirectToAction("Index", "Home");

            if (model.RoleId == "0" || model.Active == "0")
            {
                ViewBag.Msg = "All Fields are required";

                var user = await _userManagement.GetUserById(token, model.Id);

                if (user != null)
                {
                    var request = new EditUserRequest
                    {
                        Active = true,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        RoleId = Convert.ToInt32(model.RoleId),
                        UserId = model.Id,
                    };

                    var edithUser = await _userManagement.EditUser(request, token);

                    if (edithUser.HasErrors)
                    {
                        ViewBag.Msg = "Can't Edit Profile At the moment";

                        var roles1 = await _userManagement.GetUserRoles(token, false);

                        if (roles1 == null || roles1.HasErrors)
                        {
                            ViewBag.Msg = "Can't Access Roles at the moment";

                            return View();
                        }

                        var displayRoles1 = new List<RolesViewModel>();

                        foreach (var role in roles1.Data)
                        {
                            displayRoles1.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
                        }

                        displayRoles1.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "User Role" });

                        ViewBag.ListofRoles = displayRoles1;

                        return View(model);
                    }
                }

                var roles = await _userManagement.GetUserRoles(token, false);

                if (roles == null || roles.HasErrors)
                {
                    ViewBag.Msg = "Can't Access Roles at the moment";

                    return View();
                }

                var displayRoles = new List<RolesViewModel>();

                foreach (var role in roles.Data)
                {
                    displayRoles.Add(new RolesViewModel { RoleId = role.RoleId, RoleName = role.Name });
                }

                displayRoles.Insert(0, new RolesViewModel { RoleId = 0, RoleName = "User Role" });

                ViewBag.ListofRoles = displayRoles;

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var request = new EditUserRequest
                {
                    Active = (model.Active == "Active"),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    UserId = model.Id,
                    LastName = model.LastName,
                    RoleId = Convert.ToInt32(model.RoleId),
                };

                var editUser = await _userManagement.EditUser(request, token);

                if (!editUser.HasErrors)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return View();
        }

        public async Task<IActionResult> ResetPassword(int userId)
        {

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult GetOverviewData([DataTablesRequest] DataTablesRequest dataRequest)
        {
            var usersRequest = new GetUsersRequestDto
            {
                PageSize = dataRequest.Length,
                PageNumber = (dataRequest.Start / dataRequest.Length) + 1,
                Search = dataRequest.Search?.Value,
                Token = HttpContext.Session.GetString("JWToken")
            };

            var users = _userManagement.GetAllUsers(usersRequest).Result;

            if (users == null || users.HasErrors)
            {
                ViewBag.Msg = "Can't Get Users at the moment";

                return View("Index");
            }

            List<GetUsersResponse> usersItem = users.Data.Items;

            int usersTotal = users.Data.TotalItemCount;

            int usersFiltered = usersTotal;

            var overviewData = Mapper.Map<IEnumerable<UsersOverviewViewModel>>(usersItem);
            foreach (var over in overviewData)
            {
                over.Name = $"<a style =\"color:black;font-weight:bold\" href='{Url.Action("Individual", "Users", new { userId = over.Id })}'>{over.Name}</a>";
            }
            return Json(overviewData.ToDataTablesResponse(dataRequest, usersTotal, usersFiltered));
        }

    }  
}
