using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System;
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

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly IConfiguration _config;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }
        public async Task<string> Authencate(LoginRequest request)
        {
            // Tìm kiếm xem có tồn tại User như Client truyền vào ko ?
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user== null)
            {
                return null;
            }
            // Kiểm tra xem nếu 
            //var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, true);
            //if (!result.Succeeded)
            //{
            //    return null;
            //}
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role,string.Join(";",roles)),
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request)
        {
            // Láy ra tất cả các user
            var query = _userManager.Users;

            // key word truyền vào khác rỗng
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword.ToLower()) 
                                 || x.PhoneNumber.Contains(request.Keyword.ToLower()));
            }
            //3.Paging
            //Đếm số bản ghi
            int totalRow = await query.CountAsync();

            //Todo Khaivm: Fix cứng nếu không tryền pagesize và page index
            if (request.PageIndex == null && request.PageSize == null)
            {
                request.PageIndex = 1;
                request.PageSize = 25;
            }
          
            // Phân trang và map model trả ra cho clientk
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
                            .Select(x => new UserViewModel
                            {
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                UserName = x.UserName,
                                FirstName =x.FirstName,
                                Id = x.Id,
                                LastName =x.LastName
                            }).ToListAsync();

            //4. Select and projection
            var pageResult = new PagedResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pageResult;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
