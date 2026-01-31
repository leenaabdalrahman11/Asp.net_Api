using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MyApi.DAL.DTO.Response;
using MyApi.DAL.Models;

namespace MyApi.BLL.MapesterConfigurations
{
    public static class MapesterConfig
{
    public static void MapesterConfRegister()
        {
            //TypeAdapterConfig<Category,CategoryResponse>.NewConfig()
            //.Map(dest => dest.,source => source.Id).TwoWays();
            TypeAdapterConfig<Category,CategoryResponse>.NewConfig()
            .Map(dest=>dest.UserCreated,source => source.User.UserName);
        }
    
}
    
}

