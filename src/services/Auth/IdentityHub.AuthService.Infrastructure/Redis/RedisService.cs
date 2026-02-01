using IdentityHub.AuthService.Application.Abstracts;
using Microsoft.Extensions.Options;
using Shared.Kernel.Results;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityHub.AuthService.Infrastructure.Redis
{
    internal sealed class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly RedisOptions _options;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public RedisService(IConnectionMultiplexer multiplexer, IOptions<RedisOptions> options)
        {
            _multiplexer = multiplexer;
            _options = options.Value;
            _database = _multiplexer.GetDatabase(_options.Database);
        }

        public async Task<Result> SetJsonAsync<T>(string key, T value, TimeSpan? time = null)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
                return Result.Failure(Error.New(ErrorCode.Redis, "Для установки данных в Redis, были переданы не все значения!"));

            var json = JsonSerializer.Serialize(value, _serializerOptions);

            var result = await _database.StringSetAsync(key, json, time, false);

            if (!result)
                return Result.Failure(Error.New(ErrorCode.Redis, "Не удалось установить данные в Redis!"));

            return Result.Success();
        }

        public async Task<Result<T>> GetJsonAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return Result<T>.Failure(Error.New(ErrorCode.Redis, "Для получения данных из Redis, требуется ключ!"));

            var result = await _database.StringGetAsync(key);

            if (!result.HasValue)
                return Result<T>.Failure(Error.New(ErrorCode.Redis, "Данные по указанному ключу не найдены в Redis!"));

            var json = JsonSerializer.Deserialize<T>(result.ToString(), _serializerOptions);

            return Result<T>.Success(json!);
        }

        public async Task<Result> DeleteAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return Result.Failure(Error.New(ErrorCode.Redis, "Для получения данных из Redis, требуется ключ!"));

            return await _database.KeyDeleteAsync(key) ? Result.Success() : Result.Failure(Error.New(ErrorCode.Redis, "Не удалось удалить данные из Redis!"));
        }
    }
}
