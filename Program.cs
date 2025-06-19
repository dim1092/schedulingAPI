using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchedulinAPI;
using SchedulingAPI.Models;
using System.Text;

public partial class Program
{
    public static async Task Main(string[] args) // Explicit Main method with async support
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var connectionString = builder.Configuration.GetConnectionString("DbConnectionString")
            ?? throw new InvalidOperationException("Connection string 'DbConnectionString' not found.");

        builder.Services.AddDbContext<ScheduleContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure()));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ScheduleContext>()
            .AddDefaultTokenProviders();

        //builder.Services.AddIdentityCore<User>(options =>
        //{
        //    options.Password.RequireDigit = false;
        //    options.Password.RequiredLength = 6;
        //}).AddEntityFrameworkStores<ScheduleContext>()
        //    .AddSignInManager()
        //    .AddDefaultTokenProviders();


        // JWT configuration
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        //if (app.Environment.IsDevelopment())
        //{
        try
        {
            using (var scope = app.Services.CreateScope())
            {
                var scheduleContext = scope.ServiceProvider.GetRequiredService<ScheduleContext>();
                scheduleContext.Database.Migrate();
                scheduleContext.Seed();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        //}
    }
}
