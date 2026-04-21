using Intergalaxy.Application.Common.Models;
using Intergalaxy.Application.Interfaces;
using Intergalaxy.Domain.Entities;
using Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Translators;

namespace Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Adapters;

public class IntergalaxyApiAdapter : IIntergalaxyApiAdapter
{
    private readonly IntergalaxyApiClient _proxy;

    public IntergalaxyApiAdapter(IntergalaxyApiClient proxy)
    {
        _proxy = proxy;
    }
    public async Task<(Result , Character?)> GetCharacterById(int id)
    {
        var (result, characterDto) = await _proxy.GetCharactersByIdAsync(id);

        return (result, characterDto?.ToDomain());
    }

    public async Task<(Result, List<Character>?)> GetCharactersByPageAsync(int id)
    {
        var (result, charactersDto) = await _proxy.GetCharactersByPageAsync(id);

        return (result, charactersDto?.Results.Select(c => c.ToDomain()).ToList());
    }
}
