using IdentityHub.IdentityService.Application.Abstracts;
using IdentityHub.IdentityService.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Results;

namespace IdentityHub.IdentityService.Application.Features.Users.VerifyConfirmCode
{
    public sealed class VerifyConfirmCodeHandler(IApplicationDbContext context, IRedisService redisService) : IRequestHandler<VerifyConfirmCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRedisService _redisService = redisService;

        public async Task<Result> Handle(VerifyConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Login == request.Login);
            var code = await _redisService.GetJsonAsync<ConfirmCodeState>($"ConfirmCode: {user?.Login}");

            if (code.Value.Code != request.Code)
                return Result.Failure(Error.ConfirmCode());

            return Result.Success();
        }
    }
}