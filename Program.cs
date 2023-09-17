using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSwag;


namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });


            //Add repository
            builder.Services.AddTransient<ApplicationDbContext>();

            builder.Services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();


            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMvcCore().AddApiExplorer();
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();


        }
    }

}