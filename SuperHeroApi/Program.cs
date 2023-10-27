global using SuperHeroApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperHeroApi.Services.Repository.JWTRepository;
using SuperHeroApi.Services.SuperHeroService;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        
        builder.Services.AddDbContext<SuperHeroDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SuperHeroConnectionString"));
        });
        builder.Services.AddScoped<IDataRepository, DataRepository>();
        builder.Services.AddScoped<IUserInfoRepo, UserInfoRepo>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddCors(options => options.AddPolicy(name: "SuperHeroOrigins",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                policy.WithOrigins("http://localhost:4200/login").AllowAnyMethod().AllowAnyHeader();
            }));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "JWTAuthenticationServer",
                ValidAudience = "JWTServicePostmanClient",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key"))),
                RequireExpirationTime = true
            };
        });

        // builder.Services.AddAuthentication().AddJwtBearer();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(
            options =>
            {
                options.Cookie.Name = ".AuthToken";
                options.IdleTimeout = TimeSpan.FromMinutes(2);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("SuperHeroOrigins");
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseSession();
        //app.Use(async (context, next) =>
        //{
        //    var JwtToken = context.Session.GetString("jwtToken");
        //    if (!string.IsNullOrEmpty(JwtToken)) {
        //        context.Request.Headers.Add("Authorization", "Bearer" + JwtToken);

        //    }
        //    await next();
        //});
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}