using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Common;
using IdentityHub.BFF.Services;
using Shared.Contracts.Response.Auth;

namespace IdentityHub.BFF.Features.Auth.SRPComplete
{
    public class SRPCompleteHandler(RedisService redisService) : IRequestHandler<SRPCompleteCommand, Result<CompleteSrpAuthResponse>>
    {
        public async Task<Result<CompleteSrpAuthResponse>> Handle(SRPCompleteCommand request, CancellationToken cancellationToken)
        {
            var tempKey = $"srp:temp:{request.TempToken}";

            var userSessionResult = await redisService.GetJsonAsync<UserSession>(tempKey);

            if (userSessionResult.IsFailure)
                return Result<CompleteSrpAuthResponse>.Failure(Error.InternalServer($"Произошла непредвиденная ошибка на стороне сервера!"));

            var userSession = userSessionResult.Value;

            var redisResult = await redisService.SetJsonAsync<UserSession>($"sessions:{userSession.SessionId}", userSession, TimeSpan.FromDays(30));

            if (redisResult.IsFailure)
                return Result<CompleteSrpAuthResponse>.Failure(Error.InternalServer("Произошла непредвиденная ошибка на стороне сервера!"));

            await redisService.DeleteAsync(tempKey);

            return Result<CompleteSrpAuthResponse>.Success
            (
                new CompleteSrpAuthResponse
                (
                    userSession.SessionId, 
                    userSession.UserId, 
                    userSession.Login, 
                    userSession.AccessToken, 
                    userSession.RefreshToken, 
                    userSession.AccessTokenExpiresAt
                )
            );
        }
    }
}