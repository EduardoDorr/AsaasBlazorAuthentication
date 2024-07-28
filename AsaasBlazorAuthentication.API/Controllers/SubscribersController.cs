using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MediatR;

using AsaasBlazorAuthentication.Common.Results;
using AsaasBlazorAuthentication.API.Extensions;
using AsaasBlazorAuthentication.Application.Subscribers.Models;
using AsaasBlazorAuthentication.Application.Subscribers.Enroll;
using AsaasBlazorAuthentication.Application.Subscribers.GetSubscriber;
using AsaasBlazorAuthentication.Application.Subscribers.CreateSubscriber;
using AsaasBlazorAuthentication.Application.Subscribers.UpdateSubscriber;
using AsaasBlazorAuthentication.Application.Subscribers.DeleteSubscriber;
using AsaasBlazorAuthentication.Application.Subscribers.GetSubscriberById;

namespace AsaasBlazorAuthentication.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SubscribersController : ControllerBase
{
    private readonly ISender _sender;

    public SubscribersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriberViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] GetSubscribersQuery query)
    {
        var result = await _sender.Send(query);

        return result.Match(
        onSuccess: Ok,
        onFailure: value => value.ToProblemDetails());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubscriberDetailsViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetSubscriberByIdQuery(id);

        var result = await _sender.Send(query);

        return result.Match(
        onSuccess: Ok,
        onFailure: value => value.ToProblemDetails());
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateSubscriberCommand command)
    {
        var result = await _sender.Send(command);

        return result.Match(
        onSuccess: (value) => CreatedAtAction(nameof(GetById), new { id = value }, command),
        onFailure: value => value.ToProblemDetails());
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubscriberInputModel model)
    {
        var command =
            new UpdateSubscriberCommand(
                id,
                model.Name,
                model.PhoneNumber);

        var result = await _sender.Send(command);

        return result.Match(
        onSuccess: NoContent,
        onFailure: value => value.ToProblemDetails());
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _sender.Send(new DeleteSubscriberCommand(id));

        return result.Match(
        onSuccess: NoContent,
        onFailure: value => value.ToProblemDetails());
    }

    [HttpPut("{id}/enroll")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Enroll(Guid id, [FromBody] EnrollInputModel model)
    {
        var command =
            new EnrollCommand(
                id,
                model.SubscriptionId,
                model.Value);

        var result = await _sender.Send(command);

        return result.Match(
        onSuccess: value => Accepted(value),
        onFailure: value => value.ToProblemDetails());
    }
}