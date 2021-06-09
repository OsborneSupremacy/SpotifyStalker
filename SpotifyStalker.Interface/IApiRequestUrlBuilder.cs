using Spotify.Interface;
using System.Collections.Generic;

namespace SpotifyStalker.Interface
{
    public interface IApiRequestUrlBuilder
    {
        string Build<T>(string id) where T : IApiRequestObject, new();

        string Build<T>(string id, int limit) where T : IApiRequestObject, new();

        string Build<T>(IEnumerable<string> ids) where T : IApiRequestObject, new();

        string Build<T>(string id, int limit, int offset) where T : IApiRequestObject, new();

        string Build<T>(
            params KeyValuePair<string, string>[] substitutions) where T : IApiRequestObject, new();

        string BuildBatch<T>(IEnumerable<string> ids) where T : IApiBatchRequestObject, new();
    }
}
