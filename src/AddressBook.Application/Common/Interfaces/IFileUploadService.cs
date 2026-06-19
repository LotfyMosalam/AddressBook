namespace AddressBook.Application.Common.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadPhotoAsync(Stream fileStream, string fileName, long fileSize, CancellationToken cancellationToken = default);
    void DeletePhoto(string? photoUrl);
}
