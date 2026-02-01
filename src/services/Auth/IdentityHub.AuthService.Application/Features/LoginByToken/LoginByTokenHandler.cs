using IdentityHub.AuthService.Application.Abstracts;
using IdentityHub.AuthService.Application.Services;
using IdentityHub.AuthService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.Auth;
using Shared.Kernel.Results;

namespace IdentityHub.AuthService.Application.Features.LoginByToken
{
    public sealed class LoginByTokenHandler
        (
            IIdentityServiceClient client,
            IJwtTokenGenerator jwtTokenGenerator,
            IApplicationDbContext context
        ) : IRequestHandler<LoginByTokenCommand, Result<AuthResponse>>
    {
        private readonly IIdentityServiceClient _client = client;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<AuthResponse>> Handle(LoginByTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _context.Set<Token>().FirstOrDefaultAsync(x => x.RefrashToken == request.RefreshToken, cancellationToken);

                if (token == null || token.IsUsed || DateTime.UtcNow > token.ExpiryDate)
                    return Result<AuthResponse>.Failure(Error.New(ErrorCode.Unauthorized));

                token.MarkAsUsed();

                var userMaybe = await _client.GetUserByIdAsync(token.UserId.ToString(), cancellationToken);

                var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(userMaybe.Value);
                var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                var newToken = Token.Create
                    (
                        userId: userMaybe.Value.Id,
                        refrashToken: newRefreshToken,
                        accessToken: newAccessToken,
                        issueDate: DateTime.UtcNow,
                        expiryDate: DateTime.UtcNow.AddYears(1),
                        false
                    );

                await _context.Set<Token>().AddAsync(newToken, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<AuthResponse>.Success(new AuthResponse(newAccessToken, newRefreshToken));
            }
            catch (Exception)
            {
                return Result<AuthResponse>.Failure(Error.InternalServer());
            }
        }
    }
}
