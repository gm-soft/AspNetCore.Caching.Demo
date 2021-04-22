using System;

namespace AspNetCore.Caching.Demo.Models.Dtos
{
    public class DateTimeDto
    {
        public DateTime Value { get; init; }

        public int TimeZone { get; init; }

        public long Ms { get; init; }

        public DateTimeDto(DateTimeOffset time)
        {
            Value = time.DateTime;
            TimeZone = time.Offset.Hours;
            Ms = time.Millisecond;
        }

        public DateTimeDto()
        {
        }

        public static DateTimeDto Now()
        {
            return new (DateTimeOffset.Now);
        }
    }
}