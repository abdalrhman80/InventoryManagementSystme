namespace InventoryManagement.Application.Account.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ImagePath { get; set; }
        public IList<string> Roles { get; set; } = [];
    }
}
