﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.System.Users
{
    public class LoginRequest
    {
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool RememberMe { set; get; }
    }
}
