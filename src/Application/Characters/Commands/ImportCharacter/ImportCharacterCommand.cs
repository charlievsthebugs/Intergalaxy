using Intergalaxy.Application.Common.Models;
using Intergalaxy.Application.Interfaces;
using Intergalaxy.Application.Persistence;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Characters.Commands.ImportCharacter;

public record ImportCharacterCommand(int? ExternalId, int? Page) : IRequest<(Result, int)>;

public class ImportCharacterCommandHandler : IRequestHandler<ImportCharacterCommand, (Result, int)>
{
    private readonly IWriteRepository<Character> _characterWriteRepository;
    private readonly IIntergalaxyApiAdapter _intergalaxyApi;
    private readonly ICharacterRepository _characterRepository;

    public ImportCharacterCommandHandler(
        IWriteRepository<Character> writeRepository,
        IIntergalaxyApiAdapter intergalaxyApi, 
        ICharacterRepository characterRepository)
    {
        _characterWriteRepository = writeRepository;
        _intergalaxyApi = intergalaxyApi;
        _characterRepository = characterRepository;
    }

    public async Task<(Result, int)> Handle(ImportCharacterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return request.ExternalId.HasValue
                ? await ImportFromExternalId(request.ExternalId.Value)
                : await ImportFromPageId(request.Page ?? 0);
        }
        catch (Exception)
        {
            return (Result.Failure(["An error occurred while importing characters."]), 0);
        }

    }
    // CAN ABSTRACTION BE IMPROVED HERE? MAYBE A SERVICE LAYER
    private async Task<(Result, int)> ImportFromPageId(int pageId)
    {
        var (apiResult, characters) = await _intergalaxyApi.GetCharactersByPageAsync(pageId);

        if (!apiResult.Succeeded || characters == null)
        {
            if(characters == null)
                return (Result.Failure(["Failed to retrieve characters from the API."]), 0);

            return (Result.Failure(apiResult.Errors), 0);
        }

        var externalIds = characters.Select(c => c.ExternalId).ToList();

        var existingIds = await _characterRepository
            .GetExistingExternalIdsAsync(externalIds);

        var newCharacters = characters
            .Where(c => !existingIds.Contains(c.ExternalId))
            .ToList();

        if(newCharacters.Count == 0)
            return (Result.Success(), newCharacters.Count);


        foreach (var character in newCharacters)
        {
            await _characterWriteRepository.AddAsync(character);
        }

        await _characterWriteRepository.SaveChangesAsync();

        return (Result.Success(), newCharacters.Count);
    }
    private async Task<(Result, int)> ImportFromExternalId(int externalId)
    {
        var (result, character) = await _intergalaxyApi.GetCharacterById(externalId);

        if (!result.Succeeded || character is null)
            return (Result.Failure(result.Errors), 0);

        if (await _characterRepository.ExistsByExternalId(character.ExternalId))
            return (Result.Success(), 1);

        await _characterWriteRepository.AddAsync(character);
        await _characterWriteRepository.SaveChangesAsync();

        return (Result.Success(), 1);
    }
}
