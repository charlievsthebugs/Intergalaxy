namespace Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Dtos;

public class CharacterPageDto
{
    public required InfoDto Info { get; set; }
    public required List<CharacterDto> Results { get; set; }
}


public class InfoDto
{
    public int Count { get; set; }
    public int Pages { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
}
