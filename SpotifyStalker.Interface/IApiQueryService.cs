﻿using Spotify.Interface;
using SpotifyStalker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpotifyStalker.Interface
{
    public interface IApiQueryService
    {
        Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string userName) where T : IApiRequestObject, new();

        Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string id, int limit) where T : IApiRequestObject, new();

        Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(IEnumerable<string> ids) where T : IApiRequestObject, new();

        Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string id, int limit, int offset) where T : IApiRequestObject, new();
    }
}
