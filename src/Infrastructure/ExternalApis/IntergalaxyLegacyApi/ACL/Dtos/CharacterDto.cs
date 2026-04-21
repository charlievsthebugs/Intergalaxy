namespace Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Dtos;


public class CharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public OriginDto Origin { get; set; } = new();
    public string Image { get; set; } = string.Empty;
}

public class OriginDto
{
    public string Name { get; set; } = string.Empty;
}
