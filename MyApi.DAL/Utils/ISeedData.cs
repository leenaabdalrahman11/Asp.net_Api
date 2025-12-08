using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace MyApi.DAL.Utils
{
    public interface ISeedData 
    {
        Task DataSeed();
        
    }
}