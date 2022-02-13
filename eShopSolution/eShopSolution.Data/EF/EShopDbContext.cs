using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.EF
{
    public class EShopDbContext : DbContext
    {
        public DbSet<Product> Products { set; get; }
        public EShopDbContext( DbContextOptions options) : base(options)
        {
        }
    }
}
