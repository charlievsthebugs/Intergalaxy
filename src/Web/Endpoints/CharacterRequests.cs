using Intergalaxy.Application.CharacterRequests.Commands.CreateCharacterRequest;
using Intergalaxy.Application.CharacterRequests.Commands.UpdateCharacterRequestStatus;
using Intergalaxy.Application.CharacterRequests.Queries.GetCharacterRequestsByFilter;
using Intergalaxy.Application.CharacterRequests.Queries.GetCharacterRequestsById;
using Intergalaxy.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Intergalaxy.Web.Endpoints;

public class CharacterRequests : IEndpointGroup
{
    public static void Map(RouteGroupBuilder group)
    {
        // POST /api/requests
        group.MapPost("/", CreateRequest)
            .WithName("CreateRequest")
            .WithSummary("Create a new request")
            .WithDescription("Creates a request with character, requester, event, and event date");

        // GET /api/requests
        group.MapGet("/", GetCharacterRequests)
            .WithName("GetRequests")
            .WithSummary("Get requests list")
            .WithDescription("Returns a list of requests with optional filters (status, client)");

        // GET /api/requests/{id}
        group.MapGet("/{id:int}", GetRequestById)
            .WithName("GetRequestById")
            .WithSummary("Get request details")
            .WithDescription("Returns details of a specific request");

        // PATCH /api/requests/{id}/status
        group.MapPatch("/{id:int}/status", UpdateStatus)
            .WithName("UpdateRequestStatus")
            .WithSummary("Update request status")
            .WithDescription("Updates request status with transition validation");
    }

    private static async Task<IResult> CreateRequest(
        ISender sender,
            CreateCharacterRequestCommand command)
    {
        var id = await sender.Send(command);
        return Results.Created($"/api/requests/{id}", new { id });
    }

    public static async Task<Ok<PaginatedList<CharacterRequestsPaginatedDto>>> GetCharacterRequests(
     ISender sender,
     [AsParameters] GetCharacterRequestsByFilterQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public static async Task<Ok<CharacterRequestDto>> GetRequestById(ISender sender, int id)
    {
        var vm = await sender.Send(new GetCharacterRequestsByIdQuery() { Id = id });

        return TypedResults.Ok(vm);
    }


    private static async Task<IResult> UpdateStatus(
        int id,
        UpdateCharacterRequestStatusCommand body,
        IMediator mediator)
    {
        var command = new UpdateCharacterRequestStatusCommand
        {
            RequestId = id,
            NewStatus = body.NewStatus
        };

        await mediator.Send(command);

        return Results.Ok();
    }
}
