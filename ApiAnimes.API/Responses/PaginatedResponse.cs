using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAnimes.API.Responses;

public class PaginatedResponse<T>
{
    public IEnumerable<T>? Data { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? NextPageUrl { get; set; }

    public PaginatedResponse
    (
        IEnumerable<T>? data,
        int page,
        int pageSize,
        string? nextPageUrl
    )
    {   
        Data = data;
        Page = page;
        PageSize = pageSize;
        NextPageUrl = nextPageUrl;
    }
    
}
