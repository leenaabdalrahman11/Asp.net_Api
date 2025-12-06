using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.PLL.Models;
using Microsoft.EntityFrameworkCore;


namespace MyApi.DAL.Data
{
    public class ApplicationDbContext :DbContext
    {
        
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


        }

    }
}
