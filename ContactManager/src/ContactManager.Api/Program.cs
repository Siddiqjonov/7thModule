using ContactManager.Api.Configurations;
using ContactManager.Api.Endpoints;
using ContactManager.Api.ActionHelpers;

namespace ContactManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<AppExceptionHandler>();

            // Add services to the container.
            builder.ConfigureDatabase();
            builder.RegisterServices();
            builder.ConfigurationJwtAuth();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            // Minimal apis
            app.MapAuthEndpoints();
            app.MapContactEndpoints();
            app.MapAdminEndpoints();
            app.MapRoleEndpoints();

            app.Run();
        }
    }
}
