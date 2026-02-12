using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Abstracts
{
    public interface IRedisService
    {
        Task<Result> DeleteAsync(string key);
        Task<Result<T>> GetJsonAsync<T>(string key);
        Task<Result> SetJsonAsync<T>(string key, T value, TimeSpan? time = null);
    }
}