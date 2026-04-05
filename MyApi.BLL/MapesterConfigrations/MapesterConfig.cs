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
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
            .Map(dest => dest.CreatedBy, source => source.CreatedBy);

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
            .Map(dest => dest.Name, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
            .Map(dest => dest.MainImage, source =>
                source.MainImage.StartsWith("http")
                    ? source.MainImage
                    : $"http://leena12.runasp.net/images/{source.MainImage}");

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
            .Map(dest => dest.MainImage, source =>
                source.MainImage.StartsWith("http")
                    ? source.MainImage
                    : $"http://leena12.runasp.net/images/{source.MainImage}")
            .Map(dest => dest.Name, source => source.Translations.
            Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
            .Map(dest => dest.MainImage, source =>
                source.MainImage.StartsWith("http")
                    ? source.MainImage
                    : $"http://leena12.runasp.net/images/{source.MainImage}")
            .Map(dest => dest.Name, source => source.Translations
            .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Name).FirstOrDefault())
            .Map(dest => dest.Description, source => source.Translations
            .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
            .Select(t => t.Description).FirstOrDefault())
            .Map(dest => dest.SubImages,
            source => source.SubImages.Select(x =>
                x.ImageName.StartsWith("http")
                    ? x.ImageName
                    : $"http://leena12.runasp.net/images/{x.ImageName}"));


            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
            .Map(dest => dest.userName, source => source.User.UserName);

            TypeAdapterConfig<Reviews, ReviewResponse>.NewConfig()
            .Map(dest => dest.FullName, source => source.User.FullName);
        }
    }
}