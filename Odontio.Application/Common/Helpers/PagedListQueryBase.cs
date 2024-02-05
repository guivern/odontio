using Microsoft.AspNetCore.Mvc;

namespace Odontio.Application.Common.Helpers;

public record PagedListQueryBase
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Filter { get; set; }
    public List<string>? OrderBy { get; set; } = ["Id:desc"];
}