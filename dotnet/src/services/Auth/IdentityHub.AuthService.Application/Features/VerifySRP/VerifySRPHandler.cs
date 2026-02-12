using IdentityHub.AuthService.Application.Abstracts;
using IdentityHub.AuthService.Application.Features.SRPChallenge;
using IdentityHub.AuthService.Application.Services;
using IdentityHub.AuthService.Domain.Models;
using MediatR;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace IdentityHub.AuthService.Application.Features.VerifySRP
{
    public sealed class VerifySRPHandler
        (
            IApplicationDbContext context, 
            IRedisService redisService,
            IJwtTokenGenerator jwtTokenGenerator,
            IIdentityServiceClient identityServiceClient
        ) : IRequestHandler<VerifySRPCommand, Result<AuthResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRedisService _redisService = redisService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IIdentityServiceClient _identityServiceClient = identityServiceClient;

        private readonly BigInteger N = BigInteger.Parse("00B3F8C9A7D2E4F1C5A6B9D0E3F2C1A4B8D7E6F5A2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3", NumberStyles.HexNumber);

        public async Task<Result<AuthResponse>> Handle(VerifySRPCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<Error>();

            if (string.IsNullOrWhiteSpace(request.M1) || string.IsNullOrWhiteSpace(request.A))
                return Result<AuthResponse>.Failure(Error.Validation("Параметры валидации не могут быть пустыми!"));

            var sessionResult = await _redisService.GetJsonAsync<SessionState?>($"SRP: {request.Login}");

            if (sessionResult.IsFailure)
                return Result<AuthResponse>.Failure(Error.New(ErrorCode.NotFound, "Срок действия сессии истек!"));

            var sessionState = sessionResult.Value;

            BigInteger A = new(Convert.FromBase64String(request.A), isUnsigned: true, isBigEndian: true);
            BigInteger M1Client = new(Convert.FromBase64String(request.M1), isUnsigned: true, isBigEndian: true);
            BigInteger b = new(Convert.FromBase64String(sessionState!.ServerPrivateKeyB), isUnsigned: true, isBigEndian: true);
            BigInteger B = new(Convert.FromBase64String(sessionState!.ServerPublicKeyB), isUnsigned: true, isBigEndian: true);
            BigInteger v = new(Convert.FromBase64String(sessionState!.VerifierV), isUnsigned: true, isBigEndian: true);

            if (v <= 0)
                errors.Add(Error.InternalServer("Внутренняя ошибка данных: верификатор поврежден!"));

            if (A % N == 0)
                errors.Add(Error.InternalServer("Не верное значение А!"));

            if (A <= 0 || A >= N)
                errors.Add(Error.InternalServer("Некорректное значение A (out of range)!"));

            BigInteger u = CalculateSRP(A, B);

            if (u == 0)
                errors.Add(Error.InternalServer("Оишбка вычисления сервера!"));

            BigInteger vU = BigInteger.ModPow(v, u, N);
            BigInteger S = BigInteger.ModPow((A * vU) % N, b, N);
            BigInteger M1Server = CalculateSRP(A, B, S);

            // TODO : Исправть текст ошибки!!!

            if (M1Client != M1Server)
                return Result<AuthResponse>.Failure(Error.New(ErrorCode.Server, "Не верынй логин или пароль!"));

            if (errors.Count > 0)
                return Result<AuthResponse>.Failure(errors);

            BigInteger M2Server = CalculateSRP(A, M1Client, S);

            var userMaybe = await _identityServiceClient.GetUserByLoginAsync(request.Login, cancellationToken);

            if (!userMaybe.HasValue)
                return Result<AuthResponse>.Failure(Error.NotFound("Пользователь не найден!"));

            var user = userMaybe.Value;

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var token = Token.Create
                (
                    userId: user.Id,
                    refrashToken: refreshToken,
                    accessToken: accessToken,
                    issueDate: DateTime.UtcNow,
                    expiryDate: DateTime.UtcNow.AddYears(1),
                    false
                );

            await _context.Set<Token>().AddAsync(token, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<AuthResponse>.Success(new AuthResponse(accessToken, refreshToken, Convert.ToBase64String(M2Server.ToByteArray(true, true))));
        }

        private BigInteger CalculateSRP(params BigInteger[] values)
        {
            using var sha256 = SHA256.Create();
            var combinedBytes = new List<byte>();

            foreach (var v in values)
            {
                byte[] b = v.ToByteArray(isUnsigned: true, isBigEndian: true);
                int targetLen = b.Length > 32 ? 384 : 32;

                byte[] padded = new byte[targetLen];
                Buffer.BlockCopy(b, 0, padded, targetLen - b.Length, b.Length);
                combinedBytes.AddRange(padded);
            }

            byte[] hash = sha256.ComputeHash(combinedBytes.ToArray());

            return new BigInteger(hash, isUnsigned: true, isBigEndian: true);
        }
    }
}
