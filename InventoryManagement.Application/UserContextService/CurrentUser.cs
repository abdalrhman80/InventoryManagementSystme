namespace InventoryManagement.Application.UserContextService
{
    public record CurrentUser(string Id, string UserName, string Email, IEnumerable<string> Roles);
}
