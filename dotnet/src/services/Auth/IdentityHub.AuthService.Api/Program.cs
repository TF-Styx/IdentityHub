using IdentityHub.AuthService.Application.Ioc;
using IdentityHub.AuthService.Application.Services;
using IdentityHub.AuthService.Infrastructure.Ioc;
using IdentityHub.AuthService.Infrastructure.Services;

namespace IdentityHub.AuthService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy
                    (
                        name: "AllowSpecificOrigin",
                            policy =>
                            {
                                policy.WithOrigins("http://localhost:5077")
                                      .WithOrigins("http://localhost:5005")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                            }
                    );
            });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client => client.BaseAddress = new Uri(builder.Configuration["IdentityService"]!));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
