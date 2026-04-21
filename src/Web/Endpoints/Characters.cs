using Intergalaxy.Application.Characters.Commands.ImportCharacter;
using Intergalaxy.Application.Characters.Queries.GetCharacterByFilter;
using Intergalaxy.Application.Characters.Queries.GetCharacterById;
using Intergalaxy.Application.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Intergalaxy.Web.Endpoints;

public class Characters : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        //groupBuilder.RequireAuthorization();

        groupBuilder.MapGet(GetCharactersByFilter);
        groupBuilder.MapPost(ImportCharacter);
        groupBuilder.MapGet(GetCharacter, "{id}");
    }

    [EndpointSummary("Import Character")]
    [EndpointDescription("Imports a character using the provided details and returns the number of the imported character.")]
    public static async Task<Results<Created, NoContent, BadRequest>> ImportCharacter(ISender sender, ImportCharacterCommand command)
    {
        var (result, count) = await sender.Send(command);

        if (!result.Succeeded)
        {
            return TypedResults.BadRequest();
        }

        return count == 0 
            ? (Results<Created, NoContent, BadRequest>)TypedResults.NoContent() 
            : (Results<Created, NoContent, BadRequest>)TypedResults.Created();
    }


    [EndpointSummary("Get Character by ID")]
    [EndpointDescription("Retrieves a character by their ID.")]
    public static async Task<Ok<CharacterDto>> GetCharacter(ISender sender, int id)
    {
        var vm = await sender.Send(new GetCharacterByIdQuery() { CharacterId = id });

        return TypedResults.Ok(vm);
    }

    [EndpointSummary("Get Characters")]
    [EndpointDescription("Returns a paginated list of characters with optional filters (name, status).")]
    public static async Task<Ok<PaginatedList<CharacterPaginatedDto>>> GetCharactersByFilter(
    ISender sender,
    [AsParameters] GetCharacterByFilterQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
