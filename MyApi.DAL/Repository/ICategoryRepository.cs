using MyApi.PLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.DAL.Reository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Create(Category Request);

    }
}
