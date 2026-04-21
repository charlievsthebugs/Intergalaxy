using Intergalaxy.Application.Common.Models;
using Intergalaxy.Domain.Entities;

namespace Intergalaxy.Application.Interfaces;

public interface IIntergalaxyApiAdapter
{
    Task<(Result, Character?)> GetCharacterById(int id);
    Task<(Result, List<Character>?)> GetCharactersByPageAsync(int id);
}
