using RentBikeApi.Domain.Validation;

namespace RentBikeApi.UnitTests
{
    public class CnpjTests
    {
        [Theory]
        [InlineData("11222333000181")]       // numeric
        [InlineData("11.222.333/0001-81")]   // formatted
        [InlineData("12.ABC.345/01DE-35")]   // alphanumeric (new format)
        [InlineData("12abc34501de35")]       // lowercase is accepted
        public void IsValid_ReturnsTrue_ForValidCnpj(string input)
        {
            Assert.True(Cnpj.IsValid(Cnpj.Normalize(input)));
        }

        [Theory]
        [InlineData("11222333000182")]       // wrong check digit
        [InlineData("1122233300018")]        // 13 characters
        [InlineData("11111111111111")]       // all the same digit
        [InlineData("12ABC34501DEAB")]       // letters in the check digits
        [InlineData("")]
        public void IsValid_RetunsFalse_ForInvalidCnpj(string input)
        {
            Assert.False(Cnpj.IsValid(Cnpj.Normalize(input)));
        }

        [Fact]
        public void Normalize_RemovesFormatting()
        {
            Assert.Equal("11222333000181", Cnpj.Normalize("11.222.333/0001-81"));
        }
    }

}
