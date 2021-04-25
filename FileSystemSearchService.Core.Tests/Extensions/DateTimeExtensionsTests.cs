using FileSystemSearchService.Core.Extensions;
using System;
using Xunit;

namespace FileSystemSearchService.Core.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void When_using_the_ToElasticSearchNestDateTime_extension_method()
        {
            //Arrange
            var currentDateTime = DateTime.Now;

            //Act
            var result = currentDateTime.ToElasticSearchNestDateTime();

            //Assert
            Assert.Equal(currentDateTime.Year, result.Year);
            Assert.Equal(currentDateTime.Month, result.Month);
            Assert.Equal(currentDateTime.Day, result.Day);
            Assert.Equal(currentDateTime.Hour, result.Hour);
            Assert.Equal(currentDateTime.Minute, result.Minute);
            Assert.Equal(currentDateTime.Second, result.Second);

            Assert.Equal(0, result.Millisecond);
        }
    }
}
