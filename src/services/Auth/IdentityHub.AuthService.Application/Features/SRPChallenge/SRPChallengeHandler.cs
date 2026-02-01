using IdentityHub.AuthService.Application.Abstracts;
using IdentityHub.AuthService.Application.Services;
using MediatR;
using Shared.Contracts.Response.SRP;
using Shared.Kernel.Results;
using Shared.Security.Encoders;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace IdentityHub.AuthService.Application.Features.SRPChallenge
{
    public sealed class SRPChallengeHandler
        (
            IIdentityServiceClient identityServiceClient, 
            IRedisService redisService,
            IVerifierProtector verifierProtector
        ) : IRequestHandler<SRPChallengeCommand, Result<SRPChallengeResponse>>
    {
        private readonly IIdentityServiceClient _identityServiceClient = identityServiceClient;
        private readonly IRedisService _redisService = redisService;
        private readonly IVerifierProtector _verifierProtector = verifierProtector;

        private readonly BigInteger N = BigInteger.Parse("00B3F8C9A7D2E4F1C5A6B9D0E3F2C1A4B8D7E6F5A2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3", NumberStyles.HexNumber);
        private readonly BigInteger g = new(2);
        private readonly BigInteger k = new(3);

        public async Task<Result<SRPChallengeResponse>> Handle(SRPChallengeCommand request, CancellationToken cancellationToken)
        {
            var userMaybe = await _identityServiceClient.GetUserByLoginAsync(request.Login);

            if (userMaybe.HasValue)
                return Result<SRPChallengeResponse>.Failure(Error.NotFound("Пользователь не найден!"));
            
            var user = userMaybe.Value;

            var decryptedVerifier = _verifierProtector.Unprotect(user.Password);
            var vBytes = Convert.FromBase64String(decryptedVerifier);

            BigInteger v = new(vBytes, isUnsigned: true, isBigEndian: true);

            var bBytes = new byte[32];
            RandomNumberGenerator.Fill(bBytes);

            BigInteger b = new(bBytes, isUnsigned: true, isBigEndian: true);
            BigInteger gB = BigInteger.ModPow(g, b, N);
            BigInteger B = (k * v + gB) % N;

            var sessionState = new SessionState
                (
                    request.Login, 
                    Convert.ToBase64String(bBytes), 
                    Convert.ToBase64String(vBytes), 
                    Convert.ToBase64String(B.ToByteArray(isUnsigned: true, isBigEndian: true))
                );

            await _redisService.SetJsonAsync($"SRP: {request.Login}", sessionState, TimeSpan.FromMinutes(2));

            return Result<SRPChallengeResponse>.Success(new SRPChallengeResponse(user.ClientSalt, sessionState.ServerPublicKeyB));
        }
    }
}
