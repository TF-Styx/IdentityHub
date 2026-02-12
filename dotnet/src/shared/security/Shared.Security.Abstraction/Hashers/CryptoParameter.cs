namespace Shared.Security.Abstraction.Hashers
{
    public sealed record class CryptoParameter(int DegreeOfParallelism, int Iterations, int MemorySizeKB, byte[] Salt);
}
