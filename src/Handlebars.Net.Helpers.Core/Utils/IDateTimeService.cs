using System;

namespace HandlebarsDotNet.Helpers.Utils
{
    public interface IDateTimeService
    {
        DateTime Now();

        DateTime UtcNow();        
    }
}