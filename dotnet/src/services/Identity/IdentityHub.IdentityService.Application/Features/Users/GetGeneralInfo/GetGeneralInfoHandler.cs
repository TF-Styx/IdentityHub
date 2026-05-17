using MediatR;
using Shared.Kernel.Results;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.User;
using IdentityHub.IdentityService.Domain.Models;
using IdentityHub.IdentityService.Application.Abstracts;

namespace IdentityHub.IdentityService.Application.Features.Users.GetGeneralInfo
{
    public sealed class GetGeneralInfoHandler(IApplicationDbContext context) : IRequestHandler<GetGeneralInfoQuery, Result<ProfileGeneralInfoResponse>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<ProfileGeneralInfoResponse>> Handle(GetGeneralInfoQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null)
                return Result<ProfileGeneralInfoResponse>.Failure(Error.NotFound("Пользователь"));

            var response = new ProfileGeneralInfoResponse(user.Login, user.UserName, user.Email, user.Avatar);

            return Result<ProfileGeneralInfoResponse>.Success(response);
        }
    }
}