namespace IdentityHub.AuthService.Infrastructure.Redis
{
    public sealed record RedisOptions
    {
        public const string SectionName = "Redis";
        public string ConnectionString { get; set; }
        public int Database { get; set; } = 0;
    }
}
