using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.BLL.Service;
using MyApi.DAL.Repository;
using MyApi.DAL.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using MyApiProject.MyApi.PLL;

namespace MyApi.PLL;

public static class AppConfigration
{
    public static void Config(IServiceCollection Services)
    {
        Services.AddScoped<ICategoryRepository, CategoryRepository>();
        Services.AddScoped<ICategoryService, CategoryService>();
        Services.AddScoped<ISeedData, RoleSeedData>();
        Services.AddScoped<ISeedData, UserSeedData>();
        Services.AddScoped<IAuthenticationService, AuthenticationService>();
        Services.AddTransient<MyApi.BLL.Service.IEmailSender, MyApi.BLL.Service.EmailSender>();
        Services.AddTransient<IFileService, FileService>();
        Services.AddScoped<IProductRepository, ProductRepository>();
        Services.AddScoped<IProductService, ProductService>();
        Services.AddScoped<ICartRepository, CartRepository>();
        Services.AddScoped<ICartService, CartService>();
        Services.AddScoped<ICheckoutService, CheckoutService>();
        Services.AddScoped<IOrderRepository, OrderRepository>();
        Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        Services.AddExceptionHandler<GlobalExceptionHandler>();
        Services.AddProblemDetails();
    }
    
}