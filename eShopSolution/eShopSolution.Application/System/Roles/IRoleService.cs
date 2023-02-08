﻿using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System;
using eShopSolution.ViewModels.System.Roles;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAllRoles();
    }
}
