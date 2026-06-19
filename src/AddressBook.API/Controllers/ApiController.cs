using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly IMediator Mediator;

    protected ApiController(IMediator mediator) => Mediator = mediator;
}
