using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class PersonDBContext :DbContext
    {
        public PersonDBContext(DbContextOptions<PersonDBContext> options) : base(options) { 
        }

        public DbSet<PersonModel> PersonModel { get; set; }
        public DbSet<ProductModel> ProductModel { get; set; }
        
    }
}
