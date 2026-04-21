using System.Net.Http.Json;
using Intergalaxy.Application.Common.Models;
using Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi.ACL.Dtos;

namespace Intergalaxy.Infrastructure.ExternalApis.IntergalaxyLegacyApi;

public class IntergalaxyApiClient
{
    private readonly HttpClient _client;

    public IntergalaxyApiClient(HttpClient client)
    {
        _client = client;
    }

    public Task<(Result, CharacterDto?)> GetCharactersByIdAsync(int id)
    {
        return SendAsync<CharacterDto>($"character/{id}");
    }

    public Task<(Result, CharacterPageDto?)> GetCharactersByPageAsync(int pageId)
    {
        return SendAsync<CharacterPageDto>($"character?page={pageId}");
    }


    //abstracted method to handle API calls and error handling in one place
    private async Task<(Result, T?)> SendAsync<T>(string url)
    {
        try
        {
            var response = await _client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return (Result.Failure(["Resource not found"]), default);

            if (!response.IsSuccessStatusCode)
                return (Result.Failure([$"API error: {response.StatusCode}"]), default);

            var data = await response.Content.ReadFromJsonAsync<T>();

            if (data == null)
                return (Result.Failure(["Empty response"]), default);

            return (Result.Success(), data);
        }
        catch (TaskCanceledException)
        {
            return (Result.Failure(["Timeout calling API"]), default);
        }
        catch (HttpRequestException ex)
        {
            return (Result.Failure([ex.Message]), default);
        }
    }


}
