namespace Shared.Contracts.CacheKeys
{
    public static class RedisKeys
    {
        private const string TERMINEX = "terminex";

        public static string SessionString(string sessionId)
            => $"{TERMINEX}:sessions:{sessionId}";

        public static string DataProtectionString()
            => $"{TERMINEX}:shared:bff:data-protection-keys";

        public static string SRPTempTokenString(string tempToken)
            => $"{TERMINEX}:srp:temp:{tempToken}";

        public static string SRPSessionStateString(string login)
            => $"{TERMINEX}:srp:session-state:{login}";
    }
}