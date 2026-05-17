using IdentityHub.BFF.Clients.File;
using IdentityHub.BFF.Clients.Identity;
using MediatR;
using Shared.Contracts.Request.Avatar;
using Shared.Contracts.Response.BFF;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Profile.GetGeneralInfo
{
    public sealed class GetGeneralInfoHandler(IIdentityService identityService, IFileService fileService) : IRequestHandler<GetGeneralInfoQuery, Result<ProfileGeneralInfoBFFResponse?>>
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly IFileService _fileService = fileService;

        public async Task<Result<ProfileGeneralInfoBFFResponse?>> Handle(GetGeneralInfoQuery request, CancellationToken cancellationToken)
        {
            var profileResult = await _identityService.GetGeneralInfoAsync(request.UserId);

            if (profileResult.IsFailure)
                return Result<ProfileGeneralInfoBFFResponse?>.Failure(Error.NotFound("Информацию о пользователе"));

            if (profileResult.Value!.Avatar == null)
                return Result<ProfileGeneralInfoBFFResponse?>.Success(
                    new ProfileGeneralInfoBFFResponse(profileResult.Value.Login, profileResult.Value.UserName, profileResult.Value.Email, null));

            var partsProfileInfo = profileResult.Value.Avatar.Split(":");

            var bucketName = partsProfileInfo[0];
            var fileName = partsProfileInfo[1];

            var s3Result = await _fileService.GetPresignedUrlAsync(new PresignedUrlRequest(bucketName, fileName));

            if (s3Result.IsFailure)
                return Result<ProfileGeneralInfoBFFResponse?>.Failure(Error.NotFound("Аватар пользователя не найден!"));

            var response = new ProfileGeneralInfoBFFResponse(profileResult.Value.Login, profileResult.Value.UserName, profileResult.Value.Email, s3Result.Value!.PresignedUrl);
            Console.WriteLine(s3Result.Value!.PresignedUrl);
            return Result<ProfileGeneralInfoBFFResponse?>.Success(response);
        }
    }
}