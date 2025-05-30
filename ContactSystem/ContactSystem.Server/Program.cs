using ContactSystem.Aplication.Interfaces;
using ContactSystem.Aplication.Services;
using ContactSystem.Infrastructure.Persistence.Repositories;
using ContactSystem.Server.ActionHelpers;
using ContactSystem.Server.Configurations;
using ContactSystem.Server.Endpoints;

namespace ContactSystem.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<AppExceptionHendler>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigureDB();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();
            builder.Services.AddScoped<IContactService, ContactService>();


            var app = builder.Build();
            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapContactEndpoints();
            app.Run();
        }
    }
}
