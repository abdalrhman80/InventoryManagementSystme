namespace InventoryManagement.Application.UserContextService
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}
