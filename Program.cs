using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSwag;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;



namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie(options => options.Cookie.Name = "token")
                .AddJwtBearer(
                options =>
                { options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "issuer",
                    ValidAudience = "audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
                    options.Events = new JwtBearerEvents {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["token"];
                            return Task.CompletedTask;
                        }
                    }; 
                }
                );
                

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            //builder.Services.AddCors(options =>
            //options.AddDefaultPolicy(
            //    policy => policy
            //    .WithOrigins("https://localhost:44331,https://localhost:4200")
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(_ => true)

            //    .AllowAnyMethod()
            //));



            //Add repository
            builder.Services.AddTransient<ApplicationDbContext>();

            builder.Services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();


            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApiDocument(options => {
                options.PostProcess = document =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "ToDo API",
                        Description = "An ASP.NET Core Web API for managing ToDo items",
                        TermsOfService = "https://example.com/terms",
                        Contact = new OpenApiContact
                        {
                            Name = "Example Contact",
                            Url = "https://example.com/contact"
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Example License",
                            Url = "https://example.com/license"
                        }
                    };
                };
            });

            var app = builder.Build();

            //app.UseCors();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();


            app.Run();


        }
    }

}