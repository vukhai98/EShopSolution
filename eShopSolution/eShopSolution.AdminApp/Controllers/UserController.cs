﻿using eShopSolution.AdminApp.Services;
using eShopSolution.AdminApp.Services.IServices;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{

    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;

        private readonly IConfiguration _configuration;


        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var sessions = HttpContext.Session.GetString("Token");
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPagings(request);
            ViewBag.Keyword = keyword;
            return View(data.ResultObj);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new UserDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result != null)
            {
                return View(result.ResultObj);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userApiClient.RegisterRequest(request);

            if (result.IsSuccessed)
                return RedirectToAction("Index");
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            if (user.IsSuccessed)
            {
                var userUpdateRequest = new UserUpdateRequest()
                {
                    Dob = user.ResultObj.Dob,
                    Email = user.ResultObj.Email,
                    FirstName = user.ResultObj.FirstName,
                    LastName = user.ResultObj.LastName,
                    Id = user.ResultObj.Id,
                    PhoneNumber = user.ResultObj.PhoneNumber
                };
                return View(userUpdateRequest);
            }
            return RedirectToAction("Error", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userApiClient.UpdateUser(request.Id, request);

            if (result.IsSuccessed)
                return RedirectToAction("Index");

            return View(request);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetAllRolesAssignRequest(id);
            return View(roleAssignRequest);

        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _userApiClient.RoleAssign(request.Id, request);

            if (result.IsSuccessed)
                return RedirectToAction("Index");

            return View(request);
            ModelState.AddModelError("", result.Message);

            var roleAssignRequest = await GetAllRolesAssignRequest(request.Id);

            return View(roleAssignRequest);

        }


        private async Task<RoleAssignRequest> GetAllRolesAssignRequest(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            var listRoles = await _roleApiClient.GetAllRoles();

            var roleAssginRequests = new RoleAssignRequest();

            foreach (var item in listRoles.ResultObj)
            {
                var roleAssginRequest = new SelectItem()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Selected = user.ResultObj.Roles.Contains(item.Name)

                };
                roleAssginRequests.Roles.Add(roleAssginRequest);
            }

            return roleAssginRequests;
        }
    }
}
