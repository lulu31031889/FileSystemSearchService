using System;

namespace FileSystemSearchService.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToElasticSearchNestDateTime(this DateTime x) => DateTime.Parse(x.ToString("s"));
    }
}
