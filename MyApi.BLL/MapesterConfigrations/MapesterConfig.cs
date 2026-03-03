using System;
using System.Collections.Generic;
using System.Linq;
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
            .Map(dest => dest.CreatedBy, source => source.CreatedBy);

            TypeAdapterConfig<Category,CategoryUserResponse>.NewConfig()
            .Map(dest => dest.Name, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault());   
          
            TypeAdapterConfig<Product,ProductResponse>.NewConfig()
            .Map(dest => dest.MainImage , source => $"https://localhost:7291/images/{source.MainImage}");
          
            TypeAdapterConfig<Product,ProductUserResponse>.NewConfig()
            .Map(dest => dest.MainImage , source => $"https://localhost:7291/images/{source.MainImage}")
            .Map(dest => dest.Name, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault());  

            TypeAdapterConfig<Product,ProductUserDetails>.NewConfig()
            .Map(dest => dest.MainImage , source => $"https://localhost:7291/images/{source.MainImage}")
            .Map(dest => dest.Name, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault()) 
           .Map(dest => dest.Description, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Description).FirstOrDefault());

            TypeAdapterConfig<Order,OrderResponse>.NewConfig()
            .Map(dest => dest.userName,source => source.User.UserName);
        
        }
    }

}