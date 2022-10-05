namespace SpotifyStalker.Service;


[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IHttpFormPostService))]
public class HttpFormPostService : IHttpFormPostService
{
    public async Task<string> PostFormAsync(
        HttpClient httpClient,
        string url,
        List<KeyValuePair<string, string>> nameValueList)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new FormUrlEncodedContent(nameValueList)
        };
        using var response = await httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
