using AddressBook.Application.Common.Interfaces;
using AddressBook.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace AddressBook.Infrastructure.Services;

public class FileUploadService : IFileUploadService
{
    private readonly FileUploadSettings _settings;
    private readonly IWebHostEnvironment _env;

    public FileUploadService(IOptions<FileUploadSettings> settings, IWebHostEnvironment env)
    {
        _settings = settings.Value;
        _env = env;
    }

    public async Task<string> UploadPhotoAsync(
        Stream fileStream,
        string fileName,
        long fileSize,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        if (!_settings.AllowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                $"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", _settings.AllowedExtensions)}.");

        if (fileSize > _settings.MaxFileSizeBytes)
            throw new InvalidOperationException(
                $"File size exceeds the maximum allowed {_settings.MaxFileSizeBytes / (1024 * 1024)} MB.");

        // Organize by year/month to keep directories manageable
        var now = DateTime.UtcNow;
        var relativeFolderPath = Path.Combine("uploads", _settings.StorageFolderName,
            now.Year.ToString(), now.Month.ToString("D2"));

        var physicalDirectory = Path.Combine(GetWebRootPath(), relativeFolderPath);
        Directory.CreateDirectory(physicalDirectory);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var physicalFilePath = Path.Combine(physicalDirectory, uniqueFileName);

        await using var output = File.Create(physicalFilePath);
        await fileStream.CopyToAsync(output, cancellationToken);

        // Return a URL path relative to the server root (served by UseStaticFiles)
        return $"/{relativeFolderPath.Replace('\\', '/')}/{uniqueFileName}";
    }

    public void DeletePhoto(string? photoUrl)
    {
        if (string.IsNullOrWhiteSpace(photoUrl))
            return;

        // Map URL path back to physical path: /uploads/photos/2026/06/file.jpg
        //   → {WebRoot}/uploads/photos/2026/06/file.jpg
        var relativePath = photoUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var physicalPath = Path.Combine(GetWebRootPath(), relativePath);

        if (File.Exists(physicalPath))
            File.Delete(physicalPath);
    }

    private string GetWebRootPath()
        => string.IsNullOrEmpty(_env.WebRootPath)
            ? Path.Combine(_env.ContentRootPath, "wwwroot")
            : _env.WebRootPath;
}
