using MediatR;
using Shared.Kernel.Results;
using IdentityHub.IdentityService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using IdentityHub.IdentityService.Application.Abstracts;

namespace IdentityHub.IdentityService.Application.Features.Users.GenerateConfirmCode
{
    public sealed class GenerateConfirmCodeHandler(IApplicationDbContext context, IRedisService redisService) : IRequestHandler<GenerateConfirmCodeCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRedisService _redisService = redisService;

        public async Task<Result> Handle(GenerateConfirmCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Login == request.Login, cancellationToken);

            if (user == null)
                return Result.Failure(Error.NotFound("Пользователь не найден!"));

            var code = GenerateRandomNumber();

            var codeState = new ConfirmCodeState {Code = int.Parse(code), Type = "Email"};

            var redisResult = await _redisService.SetJsonAsync($"ConfirmCode: {request.Login}", codeState, TimeSpan.FromMinutes(5));

            if (redisResult.IsFailure)
                return Result.Failure(Error.InternalServer());

            // TODO : Добавить код в сервис уведомления

            return Result.Success();
        }

        private string GenerateRandomNumber()
        {
            var random = new Random();
            List<int> number = [];

            for (int i = 0; i < 6; i++)
                number.Add(random.Next(0,9));

            return string.Join("", number);
        }
    }
}