namespace AddressBook.Infrastructure.Settings;

public class FileUploadSettings
{
    public string StorageFolderName { get; set; } = "photos";
    public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024; // 5 MB
    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp"];
}
