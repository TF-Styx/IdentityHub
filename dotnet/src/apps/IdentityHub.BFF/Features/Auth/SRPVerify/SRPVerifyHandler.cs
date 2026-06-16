using MediatR;
using Shared.Kernel.Results;
using Shared.Contracts.Common;
using IdentityHub.BFF.Services;
using Shared.Contracts.CacheKeys;
using IdentityHub.BFF.Clients.Auth;
using Shared.Contracts.Request.SRP;
using System.Security.Cryptography;
using Shared.Contracts.Response.SRP;

namespace IdentityHub.BFF.Features.Auth.SRPVerify
{
    public class SRPVerifyHandler(IAuthService authService, RedisService redisService, JwtReader jwtReader) : IRequestHandler<SRPVerifyCommand, Result<SRPVerifyProofResponse>>
    {
        private readonly IAuthService _authService = authService;

        public async Task<Result<SRPVerifyProofResponse>> Handle(SRPVerifyCommand request, CancellationToken cancellationToken)
        {
            var authResponseResult = await _authService.SRPVerify(new SRPVerifyRequest(request.Login, request.A, request.M1));

            if (authResponseResult.IsFailure)
                return Result<SRPVerifyProofResponse>.Failure(authResponseResult.Errors);

            var authResponse = authResponseResult.Value;

            var accessTokenData = jwtReader.Extract(authResponse.AccessToken);

            var userSession = new UserSession
            (
                Guid.NewGuid().ToString(), 
                authResponse.AccessToken, 
                authResponse.RefreshToken, 
                accessTokenData.ExpiredTime, 
                accessTokenData.UserId, 
                accessTokenData.Login
            );

            Span<byte> tempToken = stackalloc byte[32];
            RandomNumberGenerator.Fill(tempToken);

            string tempAuthToken = Convert.ToBase64String(tempToken).Replace("+", "-").Replace("/", "_").Replace("=", "");

            var redisResult = await redisService.SetJsonAsync(RedisKeys.SRPTempTokenString(tempAuthToken), userSession, TimeSpan.FromMinutes(2));

            if (redisResult.IsFailure)
                return Result<SRPVerifyProofResponse>.Failure(Error.InternalServer("Произошла ошибка на стороне сервера!"));

            return Result<SRPVerifyProofResponse>.Success(new SRPVerifyProofResponse(authResponse.M2!, tempAuthToken));
        }
    }
}