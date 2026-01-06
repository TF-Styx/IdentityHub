namespace Shared.Security.Hashers
{
    public sealed record class CryptoParameter(int DegreeOfParallelism, int Iterations, int MemorySizeKB, byte[] Salt);
}
