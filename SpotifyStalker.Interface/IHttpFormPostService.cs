using System.Net.Http;

namespace SpotifyStalker.Interface;

public interface IHttpFormPostService
{
    Task<string> PostFormAsync(
            HttpClient httpClient,
            string url,
            List<KeyValuePair<string, string>> nameValueList);
}
