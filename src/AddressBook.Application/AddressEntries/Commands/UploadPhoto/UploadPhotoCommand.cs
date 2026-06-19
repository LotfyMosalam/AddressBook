using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.UploadPhoto;

public record UploadPhotoCommand : IRequest<string>
{
    public Guid EntryId { get; init; }
    public Stream FileStream { get; init; } = Stream.Null;
    public string FileName { get; init; } = string.Empty;
    public long FileSize { get; init; }
}
