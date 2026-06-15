namespace Shared.Contracts.Response.SRP
{
    public sealed record SRPVerifyProofResponse(string M2, string TempAuthToken);
}