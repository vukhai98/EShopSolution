﻿using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PagedResult<UserViewModel>> GetUserPaging(GetUserPagingRequest request);

    }
}
