using System.Net;
using System.Net.Http.Json;
using ADT.Common.Models.Request;
using ADT.Common.Models.Response;
using ADT.UI.Services.Contracts;

namespace ADT.UI.Services;

public class UserProfileService : IUserProfileService
{
    private readonly HttpClient _httpClient;

    public UserProfileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<UserProfileResponseModel>> GetAll()
    {
        try
        {
            var response = await _httpClient.GetAsync("/userProfile");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Enumerable.Empty<UserProfileResponseModel>();

                return await response.Content.ReadFromJsonAsync<IEnumerable<UserProfileResponseModel>>();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
                return Enumerable.Empty<UserProfileResponseModel>();

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http status: {response.StatusCode} Message -{message}");
        }
        catch
        {
            // log
            throw;
        }
    }

    public async Task Add(UserProfileRequestModel requestModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/userProfile", requestModel);

            if (response is not { IsSuccessStatusCode: true, StatusCode: HttpStatusCode.Created })
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }
        catch
        {
            // log
            throw;
        }
    }
}