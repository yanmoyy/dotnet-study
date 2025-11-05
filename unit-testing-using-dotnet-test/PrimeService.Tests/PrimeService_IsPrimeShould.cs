using System.Runtime.InteropServices;
using Prime.Services;
using Xunit;

namespace Prime.UnitTests.Services
{
    public class PrimeService_IsPrimeShould
    {
        private readonly PrimeService _primeservice;

        public PrimeService_IsPrimeShould()
        {
            _primeservice = new PrimeService();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void IsPrime_ValueLessThan2_ReturnFalse(int value)
        {
            var result = _primeservice.IsPrime(value);

            Assert.False(result, $"{value} should not be prime");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        public void IsPrime_PrimeLessThan10_ReturnTrue(int value)
        {
            var result = _primeservice.IsPrime(value);

            Assert.True(result, $"{value} should be prime");
        }

        [Theory]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(9)]
        public void IsPrime_PrimeLessThan10_ReturnFalse(int value)
        {
            var result = _primeservice.IsPrime(value);

            Assert.False(result, $"{value} should not be prime");
        }
    }
}
