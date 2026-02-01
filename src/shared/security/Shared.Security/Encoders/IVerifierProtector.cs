namespace Shared.Security.Encoders
{
    public interface IVerifierProtector
    {
        string Protect(string verifier);
        string Unprotect(string protectedVerifier);
    }
}
