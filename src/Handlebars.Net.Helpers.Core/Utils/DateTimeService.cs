namespace HandlebarsDotNet.Helpers.Utils;

public class DateTimeService : IDateTimeService
{
    public DateTime Now()
    {
        return DateTime.Now;
    }

    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}