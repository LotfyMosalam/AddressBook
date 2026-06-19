using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using MediatR;

namespace AddressBook.Application.AddressEntries.Commands.UploadPhoto;

public class UploadPhotoCommandHandler : IRequestHandler<UploadPhotoCommand, string>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UploadPhotoCommandHandler(
        IAddressEntryRepository repository,
        IApplicationDbContext context,
        IFileUploadService fileUploadService)
    {
        _repository = repository;
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<string> Handle(UploadPhotoCommand request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.EntryId, cancellationToken)
            ?? throw new NotFoundException("AddressEntry", request.EntryId);

        _fileUploadService.DeletePhoto(entry.PhotoUrl);

        var photoUrl = await _fileUploadService.UploadPhotoAsync(
            request.FileStream,
            request.FileName,
            request.FileSize,
            cancellationToken);

        entry.SetPhoto(photoUrl);

        await _context.SaveChangesAsync(cancellationToken);

        return photoUrl;
    }
}
