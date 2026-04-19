using IdentityHub.AuthService.Application.Abstracts;
using Microsoft.Extensions.Options;
using Shared.Redis;
using StackExchange.Redis;

namespace IdentityHub.AuthService.Infrastructure.Redis
{
    internal class RedisService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options) 
        : AbstractRedisService(connectionMultiplexer, options), IRedisService
    {
        
    }
}
