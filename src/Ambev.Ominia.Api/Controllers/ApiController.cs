using Ambev.Ominia.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.Ominia.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Gets the base URI for the controller.
    /// </summary>
    protected string Uri => $"{ControllerContext.ActionDescriptor.ControllerName.ToLower()}/";

    protected IActionResult Ok<T>(T data) =>
        base.Ok(new Response(data!));

    protected IActionResult OkPaginated<T>(T data) =>
        base.Ok(data);

    protected IActionResult Created<T>(string uri, T data) =>
        base.Created(uri, new Response(data!));

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new Response(false, message));

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new Response(false, message));
}