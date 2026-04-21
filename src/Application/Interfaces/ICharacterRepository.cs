namespace Intergalaxy.Application.Interfaces;

public interface ICharacterRepository
{
    Task<bool> ExistsByExternalId(int externalId);
    Task<List<int>> GetExistingExternalIdsAsync(IEnumerable<int> externalIds);
}
