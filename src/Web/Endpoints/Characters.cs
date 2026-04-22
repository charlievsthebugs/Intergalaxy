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

        groupBuilder.MapGet(GetCharactersByFilter)
            .WithSummary("Get Characters by Filter")
            .WithDescription("Returns a paginated list of characters with optional filters (name, status).");
        
        groupBuilder.MapPost(ImportCharacter)
            .WithSummary("Import Character")
            .WithDescription("Imports a character using the provided details and returns the number of the imported character.");
        
        groupBuilder.MapGet(GetCharacter, "{id}")
            .WithSummary("Get Character by ID")
            .WithDescription("Retrieves a character by their ID.");
    }

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


    public static async Task<Ok<CharacterDto>> GetCharacter(ISender sender, int id)
    {
        var vm = await sender.Send(new GetCharacterByIdQuery() { CharacterId = id });

        return TypedResults.Ok(vm);
    }

    public static async Task<Ok<PaginatedList<CharacterPaginatedDto>>> GetCharactersByFilter(
    ISender sender,
    [AsParameters] GetCharacterByFilterQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }
}
