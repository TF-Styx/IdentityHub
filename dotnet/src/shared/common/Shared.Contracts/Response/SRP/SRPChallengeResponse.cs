namespace Shared.Contracts.Response.SRP
{
    public sealed record SRPChallengeResponse(string Salt, string B);
}
