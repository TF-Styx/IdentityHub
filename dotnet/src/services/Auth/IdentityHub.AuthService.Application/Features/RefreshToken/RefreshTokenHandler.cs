using MediatR;
using Medallion.Threading;
using Shared.Kernel.Results;
using Medallion.Threading.Redis;
using Shared.Contracts.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.Auth;
using IdentityHub.AuthService.Domain.Models;
using IdentityHub.AuthService.Application.Services;
using IdentityHub.AuthService.Application.Abstracts;

namespace IdentityHub.AuthService.Application.Features.RefreshToken
{
    public sealed class RefreshTokenHandler
        (
            IApplicationDbContext context, 
            IJwtTokenGenerator jwtTokenGenerator,
            IIdentityServiceClient identityServiceClient,
            RedisDistributedSynchronizationProvider lockProvider
        ) : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var lockKey = RedisKeys.LockKeyString(request.RefreshToken);

            await using (await lockProvider.TryAcquireLockAsync(lockKey, TimeSpan.FromSeconds(5)))
            {
                try
                {
                    var storageToken = await context.Set<Token>().FirstOrDefaultAsync(x => x.RefrashToken == request.RefreshToken);

                    if (storageToken == null || storageToken.IsUsed || DateTime.UtcNow > storageToken.ExpiryDate)
                        return Result<AuthResponse>.Failure(Error.New(ErrorCode.Unauthorized, "Вы не авторизованы!"));

                    var userMaybe = await identityServiceClient.GetUserByIdAsync(storageToken.UserId.ToString());

                    if (userMaybe.IsNone)
                        return Result<AuthResponse>.Failure(Error.New(ErrorCode.Unauthorized, "Данный пользователь не авторизован!"));

                    var newRefreshToken = jwtTokenGenerator.GenerateRefreshToken();
                    var newAccessToken = jwtTokenGenerator.GenerateAccessToken(userMaybe.Value);

                    var newToken = Token.Create(userMaybe.Value.Id, newRefreshToken, newAccessToken, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), false);

                    await context.Set<Token>().AddAsync(newToken, cancellationToken);
                    context.Set<Token>().Remove(storageToken);

                    await context.SaveChangesAsync(cancellationToken);

                    return Result<AuthResponse>.Success(new AuthResponse(newAccessToken, newRefreshToken));
                }
                catch (Exception ex)
                {
                    return Result<AuthResponse>.Failure(Error.InternalServer($"Произошла серверная ошибка! Ошибка - {ex}"));
                }
            }
        }
    }
}