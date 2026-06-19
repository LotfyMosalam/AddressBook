using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.ExportEntries;

public record ExportEntriesQuery : IRequest<byte[]>;
