using DMIOpenData;
using MathNet.Numerics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using System;
using System.Diagnostics;
using System.Text;
using WebApplication1.Middelware;
using WebApplication1.Services.Analysis;
using static System.Net.WebRequestMethods;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApplication1.Application_Layer.Middelware;

namespace WebApplication1.Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Make postgres use timestamp instead of timestamptz

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            AddAuthentication(builder);

            AddAuthorization(builder);

            AddDatabase(builder, connectionString);

            //Add repository
            builder.Services.AddTransient<ApplicationDbContext>();

            builder.Services.AddControllers();

            builder.Services.AddServices();
            builder.Services.AddRepositories();
            builder.Services.AddCommandHandlers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApiDocument();

            WebApplication app = builder.Build();
            AutoMigrate(app);

            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            AddMiddleware(app);

            app.Run();
        }

        private static void AddDatabase(WebApplicationBuilder builder, string? connectionString)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        private static void AddAuthorization(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
        }

        private static void AddAuthentication(WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "jwt";
                    options.LoginPath = "/Login";
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                }
                );
        }

        private static void AddMiddleware(WebApplication app)
        {
            app.UseMiddleware<UserContextMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

        }

        private static void AutoMigrate(WebApplication app)
        {

            var scope = app.Services.CreateScope(); //Creates scoped lifetime for the service.
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //if (dbContext.Database.CanConnect())
            //{
                dbContext.Database.Migrate();
            //}
            scope.Dispose();
        }
    }

}