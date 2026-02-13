using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityHub.BFF.Clients.Identity;
using MediatR;
using Shared.Contracts.Request.User;
using Shared.Kernel.Results;

namespace IdentityHub.BFF.Features.Registration
{
    public class RegisterUserHandler(IIdentityService identityService) : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            => await _identityService.RegistrationAsync
                    (
                        new RegisterUserRequest
                            (
                                request.Login, 
                                request.UserName, 
                                request.Verifier, 
                                request.ClientSalt, 
                                request.EncryptedDek, 
                                request.EncryptionAlgorithm, 
                                request.Iterations, 
                                request.KdfType, 
                                request.Email, 
                                request.Phone
                            )
                    );
    }
}