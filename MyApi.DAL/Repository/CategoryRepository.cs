using MyApi.DAL.Data;
using MyApi.PLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.Reository
{
    public class CategoryRepository : ICategoryRepository
    {
         private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public Category Create(Category Request)
        {
            _context.Categories.Add(Request);
            _context.SaveChanges();
            return Request;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.Include(c=>c.Translations).ToList();
        }
    }
}