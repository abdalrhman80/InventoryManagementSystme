namespace InventoryManagement.Infrastructure.DatabaseInitializer
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
