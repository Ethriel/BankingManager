
using BankingManager.Database;
using BankingManager.Server.Extensions;
using BankingManager.Server.Middlewares;
using BankingManager.Services.Model.Mapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace BankingManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConsole();
            builder.Services.AddSingleton<IConfiguration>(provider => builder.Configuration);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddValidators()
                            .AddCors()
                            .AddEndpointsApiExplorer()
                            .AddSwaggerGen()
                            .AddDbContext<BankingManagerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")))
                            .AddAutoMapper(typeof(BankAccountMapperProfile))
                            .AddAppServices()
                            .AddFluentValidationAutoValidation(config =>
                            {
                                config.DisableBuiltInModelValidation = true;
                                config.ValidationStrategy = SharpGrip.FluentValidation.AutoValidation.Mvc.Enums.ValidationStrategy.Annotations;
                                config.EnableBodyBindingSourceAutomaticValidation = true;
                                config.EnableFormBindingSourceAutomaticValidation = true;
                                config.EnableQueryBindingSourceAutomaticValidation = true;
                            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(
                opt => opt.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var result = JsonConvert.SerializeObject(new { error = exception?.Message });
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(result);
            }));

            app.UseMiddleware<AssignNewAccountNumberMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
