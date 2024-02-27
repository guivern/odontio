namespace Odontio.API.Common.Helpers;

public class PaginationQueryParams
{
    [FromQuery] public int Page { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
    [FromQuery] public string? Filter { get; set; }
    [FromQuery] public List<string>? OrderBy { get; set; } = ["Id:desc"];
}