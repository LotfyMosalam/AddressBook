using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.Common.Exceptions;
using AddressBook.Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace AddressBook.Application.AddressEntries.Queries.GetEntryById;

public class GetEntryByIdQueryHandler : IRequestHandler<GetEntryByIdQuery, AddressEntryDto>
{
    private readonly IAddressEntryRepository _repository;
    private readonly IMapper _mapper;

    public GetEntryByIdQueryHandler(IAddressEntryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AddressEntryDto> Handle(GetEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entry is null)
            throw new NotFoundException("AddressEntry", request.Id);

        return _mapper.Map<AddressEntryDto>(entry);
    }
}
