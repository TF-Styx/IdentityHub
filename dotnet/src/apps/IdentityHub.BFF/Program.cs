using System.Reflection;
using IdentityHub.BFF.Extensions.ServiceCollections;

var builder = WebApplication.CreateBuilder(args);

var assembly = Assembly.GetExecutingAssembly();
var configuration = builder.Configuration;
var env = builder.Environment;

builder.Services.AddOpenApi().AddAuthorization()
    .AddServices(configuration)
    .AddHttpClients(configuration)
    .AddDistributedLock()
    .UseCors()
    .AddSharedCryptoKeyASPNET(configuration)
    .UseCookie(env);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.AddEndpoints(assembly);

app.Run();
