using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace SMS.Frontend.Extensions;

public class ApiClient(HttpClient client, ILocalStorageService storage)
{
    private async Task SetAuthHeader()
    {
        var token = await storage.GetItemAsync<string>("authToken");
        if (token != null)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", token);
    }

    public async Task<T?> GetFromJsonAsync<T>(string url)
    {
        await SetAuthHeader();

        return await client.GetFromJsonAsync<T>(url);
    }

    public async Task<T1?> PostAsync<T1, T2>(string url, T2 data)
    {
        await SetAuthHeader();

        var response = await client.PostAsJsonAsync(url, data);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T1>(content);
    }

    public async Task<T1?> PutAsync<T1, T2>(string url, T2 data)
    {
        await SetAuthHeader();

        var response = await client.PutAsJsonAsync(url, data);

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T1>(content);
    }

    public async Task<T?> DeleteAsync<T>(string url)
    {
        await SetAuthHeader();

        return await client.DeleteFromJsonAsync<T>(url);
    }
}