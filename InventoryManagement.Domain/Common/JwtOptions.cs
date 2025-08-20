namespace InventoryManagement.Domain.Common
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public double ExpirationInMinutes { get; set; }
    }
}
