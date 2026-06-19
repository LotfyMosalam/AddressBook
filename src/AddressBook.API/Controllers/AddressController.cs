using AddressBook.Application.AddressEntries.Commands.CreateEntry;
using AddressBook.Application.AddressEntries.Commands.DeleteEntry;
using AddressBook.Application.AddressEntries.Commands.UpdateEntry;
using AddressBook.Application.AddressEntries.Commands.UploadPhoto;
using AddressBook.Application.AddressEntries.DTOs;
using AddressBook.Application.AddressEntries.Queries.ExportEntries;
using AddressBook.Application.AddressEntries.Queries.GetEntries;
using AddressBook.Application.AddressEntries.Queries.GetEntryById;
using AddressBook.Application.AddressEntries.Queries.SearchEntries;
using AddressBook.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers;

public sealed class PhotoUploadRequest
{
    public IFormFile Photo { get; set; } = null!;
}

[Authorize]
[Route("api/addresses")]
public class AddressController : ApiController
{
    public AddressController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<AddressEntryListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetEntriesQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedResult<AddressEntryListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchEntriesQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("export")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> Export(CancellationToken cancellationToken)
    {
        var bytes = await Mediator.Send(new ExportEntriesQuery(), cancellationToken);
        return File(bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "address-book.xlsx");
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddressEntryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEntryByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateEntryCommand command, CancellationToken cancellationToken)
    {
        var id = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEntryCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteEntryCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/photo")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadPhoto(Guid id, [FromForm] PhotoUploadRequest request, CancellationToken cancellationToken)
    {
        var command = new UploadPhotoCommand
        {
            EntryId = id,
            FileStream = request.Photo.OpenReadStream(),
            FileName = request.Photo.FileName,
            FileSize = request.Photo.Length
        };

        var photoUrl = await Mediator.Send(command, cancellationToken);
        return Ok(new { photoUrl });
    }
}
