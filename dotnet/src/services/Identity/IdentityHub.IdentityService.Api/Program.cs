using IdentityHub.IdentityService.Application.Ioc;
using IdentityHub.IdentityService.Infrastructure.Ioc;

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
                    policy.WithOrigins("http://localhost:5012")
                          .WithOrigins("http://localhost:5077")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                }
        );
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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
