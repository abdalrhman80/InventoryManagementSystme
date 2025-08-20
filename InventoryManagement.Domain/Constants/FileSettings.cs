namespace InventoryManagement.Domain.Constants
{
    public static class FileSettings
    {
        public const string ProfilesFolderPath = "profiles";
        public const int MaxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB
        public static readonly List<string> AllowedExtensions = [".jpg", ".jpeg", ".png"];
    }
}
