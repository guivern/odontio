﻿using Newtonsoft.Json;

namespace Odontio.API.Common.Helpers;

public static class Extensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        var header = new { currentPage, itemsPerPage, totalItems, totalPages };
            
        response.Headers["X-Pagination"] = JsonConvert.SerializeObject(header);
        response.Headers["Access-Control-Expose-Headers"] = "X-Pagination";
    }
    
    public static void AddContentDispositionHeader(this HttpResponse response, string filename)
    {
        response.Headers["Content-Disposition"] = $"attachment; filename={filename}";
        response.Headers["Access-Control-Expose-Headers"] = "Content-Disposition";
    }
}