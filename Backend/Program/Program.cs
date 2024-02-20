using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Formatters;
using System.Text;
using WebApplication1.Middelware;

namespace WebApplication1.Program
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            AddAuthentication(builder);

            AddAuthorization(builder);

            AddDatabase(builder);

            //Make JSON serializer be able to use inheritance.
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                    options.SerializerSettings.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
                });

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

        private static void AddDatabase(WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //Make postgres use timestamp instead of 'timestamptz'-datatype.
            string connectionString = "Host=localhost; Database=EstablishmentProject; Username=postgres; password=postgres";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                //options.UseLazyLoadingProxies(true);
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
        }

        private static void AutoMigrate(WebApplication app)
        {
            var scope = app.Services.CreateScope(); //Creates scoped lifetime for the service.
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            scope.Dispose();
        }
    }

}