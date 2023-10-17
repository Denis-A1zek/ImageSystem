using ImageSystem.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ImageSystem.Web.Controllers;

[ApiController]
[Route("api/")]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator =>
       _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
