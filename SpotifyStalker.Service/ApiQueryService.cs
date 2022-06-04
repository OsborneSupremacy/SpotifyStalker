using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spotify.Interface;
using SpotifyStalker.Interface;
using SpotifyStalker.Model;

namespace SpotifyStalker.Service;

public class ApiQueryService : IApiQueryService
{
    private readonly IApiRequestUrlBuilder _apiRequestUrlBuilder;

    private readonly IApiRequestService _apiRequestService;

    public ApiQueryService(
        IApiRequestUrlBuilder apiRequestUrlBuilder,
        IApiRequestService apiRequestService
    )
    {
        _apiRequestUrlBuilder = apiRequestUrlBuilder ?? throw new ArgumentNullException(nameof(apiRequestUrlBuilder));
        _apiRequestService = apiRequestService ?? throw new ArgumentNullException(nameof(apiRequestService));
    }

    public async Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string id) where T : IApiRequestObject, new()
    {
        var url = _apiRequestUrlBuilder.Build<T>(id);
        return await _apiRequestService.GetAsync<T>(url);
    }

    public async Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string id, int limit) where T : IApiRequestObject, new()
    {
        var url = _apiRequestUrlBuilder.Build<T>(id, limit);
        return await _apiRequestService.GetAsync<T>(url);
    }

    public async Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(string id, int limit, int offset) where T : IApiRequestObject, new()
    {
        var url = _apiRequestUrlBuilder.Build<T>(id, limit, offset);
        return await _apiRequestService.GetAsync<T>(url);
    }

    public async Task<(RequestStatus RequestStatus, T Data)> QueryAsync<T>(IEnumerable<string> ids) where T : IApiRequestObject, new()
    {
        var url = _apiRequestUrlBuilder.Build<T>(ids);
        return await _apiRequestService.GetAsync<T>(url);
    }
}
