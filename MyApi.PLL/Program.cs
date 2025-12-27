using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using MyApi.BLL.Service;
using MyApi.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MyApi.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyApi.DAL.Utils;
using MyApi.DAL.Repository;
using Microsoft.CodeAnalysis.Options;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddLocalization(options => options.ResourcesPath = "");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
        {
            Options.User.RequireUniqueEmail = true;
            Options.Password.RequireDigit = true;
            Options.Password.RequireLowercase = true;
            Options.Password.RequireUppercase = true;
            Options.Password.RequireNonAlphanumeric = false;
            Options.Password.RequiredLength = 6;
            Options.SignIn.RequireConfirmedEmail = true;

            Options.Lockout.MaxFailedAccessAttempts = 5;
            Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            Options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
            };
        });

        const string defaultCulture = "en";
        var supportedCultures = new[] { new CultureInfo(defaultCulture), new CultureInfo("ar") };
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
            {
                QueryStringKey = "lang"
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }

        );

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ISeedData, RoleSeedData>();
        builder.Services.AddScoped<ISeedData, UserSeedData>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        var app = builder.Build();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var seedDatas = services.GetServices<ISeedData>();
            foreach (var seedData in seedDatas)
            {
                seedData.DataSeed().Wait();
            }
        }

        app.MapControllers();
        app.Run();
    }
}

