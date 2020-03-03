using System;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(2, 2);
        }

        [Theory]
        [InlineData(2,2,4)]
        [InlineData(3, 7, 10)]
        [InlineData(2, 3, 5)]
        public void CanAdd(int a, int b, int expectedAnswer)
        {
            var answer = a + b;
            Assert.Equal(expectedAnswer, answer);
        }
    }
}
