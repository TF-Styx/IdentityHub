namespace IdentityHub.AuthService.Application.Features.SRPChallenge
{
    internal sealed record SessionState
        (
            string Login,
            string ServerPrivateKeyB,
            string VerifierV,
            string ServerPublicKeyB
        );
}
