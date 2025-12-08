using MyApi.DAL.Data;
using MyApi.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.Repository
{

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
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