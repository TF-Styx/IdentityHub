namespace Shared.Contracts.Request.SRP
{
    public sealed record SRPVerifyRequest(string Login, string A, string M1);
}
