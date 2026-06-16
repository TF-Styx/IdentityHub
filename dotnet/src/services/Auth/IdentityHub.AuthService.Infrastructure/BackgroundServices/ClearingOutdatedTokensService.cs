using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using IdentityHub.AuthService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using IdentityHub.AuthService.Infrastructure.Persistence.Contexts;

namespace IdentityHub.AuthService.Infrastructure.BackgroundServices
{
    public class ClearingOutdatedTokensService(IServiceScopeFactory factory, IConfiguration configuration) : BackgroundService
    {
        private readonly TimeSpan _interval = TimeSpan.FromHours(configuration.GetValue<double>("TokenCleanup:IntervalHours", 12));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"[BackgroundServices] [{nameof(ClearingOutdatedTokensService)}] Начал работу!");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = factory.CreateScope();

                    var context = scope.ServiceProvider.GetRequiredService<AuthContext>();

                    var deleteEntityCount = await context.Set<Token>().Where(x => x.ExpiryDate < DateTime.UtcNow).ExecuteDeleteAsync(stoppingToken);

                    Console.WriteLine($"[BackgroundServices] [{nameof(ClearingOutdatedTokensService)}] Количество удаленных записей - {deleteEntityCount}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[BackgroundServices] [{nameof(ClearingOutdatedTokensService)}] Ошибка: `{ex}`");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            Console.WriteLine($"[BackgroundServices] [{nameof(ClearingOutdatedTokensService)}] Завершил работу!");
        }
    }
}