using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Test01.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("WingtipToys")
        {

        }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Product> Products { get; set; }




    }
}