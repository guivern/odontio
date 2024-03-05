using Odontio.Application.Common.Interfaces;

namespace Odontio.Infrastructure.Services;

public class DateTimeProvider: IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateOnly Today => DateOnly.FromDateTime(UtcNow.DateTime);
}