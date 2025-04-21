using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using SimpleAuthAPI.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using SimpleAuthAPI.Model.Entities;

namespace SimpleAuthAPI;
public sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) { return null; }
        string? str = value.ToString();
        if (string.IsNullOrEmpty(str)) { return null; }

        return Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization(); // Identity Authorizaton Kısmı
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>() // Identity Kısmı
            .AddEntityFrameworkStores<AppDbContext>();

        // Add services to the container.
        
       builder.Services.AddDbContext<AppDbContext>(options => // SQL Bağlantımızı yaptığımız Kısım
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
       
       builder.Services.Configure<IdentityOptions>(options => // Identity ayarları kısmı
       {
           options.SignIn.RequireConfirmedAccount = false;
           options.Password.RequiredLength = 3;
           options.Password.RequireNonAlphanumeric = false;
           options.Password.RequireDigit = false;
           options.Password.RequireUppercase = false;
           options.Password.RequireLowercase = false;
           
       });
       
       builder.Services.AddAutoMapper(typeof(Program).Assembly); // Mapper ekleme Kısmı
       
       builder.Services
           .AddControllers(options =>
           {
               options.Conventions.Add(
                   new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
           })
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
               options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
           });
       
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGroup("/user").MyMapIdentityApi<ApplicationUser>(); // Identity Hazır Enpoinlerini değiştirdik, kendi endponilerimiz ile özelleştirdik.
        app.MapControllers();

        app.Run();
    }
}