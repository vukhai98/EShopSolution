using eShopSolution.Application.System.Roles;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System;
using eShopSolution.ViewModels.System.Roles;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly IConfiguration _config;
        public RoleService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<List<RoleViewModel>> GetAllRoles()
        {
            var listRoleViewModels = new List<RoleViewModel>();

            var roles = await _roleManager.Roles.ToListAsync();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    var roleVm = new RoleViewModel()
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Discription = role.Description
                    };
                    listRoleViewModels.Add(roleVm);
                }
            }
           return listRoleViewModels;
        }
    }
}
