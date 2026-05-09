using MediatR;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.Avatar
{
    public sealed record AvatarCommand(string UserId, Stream File, string FileName, string ContentType) : IRequest<Result>;
}