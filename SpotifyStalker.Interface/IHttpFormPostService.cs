using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface;

public interface IHttpFormPostService
{
    Task<string> PostFormAsync(
            HttpClient httpClient,
            string url,
            List<KeyValuePair<string, string>> nameValueList);
}
