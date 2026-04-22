namespace Intergalaxy.Application.Interfaces;

public interface ICharacterRepository
{
    Task<bool> ExistAsync(int id);
    Task<bool> ExistsByExternalId(int externalId);
    Task<List<int>> GetExistingExternalIdsAsync(IEnumerable<int> externalIds);
}
