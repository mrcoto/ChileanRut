using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrCoto.ChileanRut.Validation;
using Xunit;

namespace MrCoto.ChileanRutTest.Validation
{
    public class ValidRutDvAttributeTest
    {
        class RutDvClass<T, K>
        {
            public T NumberPart { get; set; }

            [ValidRutDv(MinLength = 3, NumberName = "NumberPart")]
            public K Dv { get; set; }
        }

        [Theory]
        [InlineData(123, "6")]
        [InlineData(23971806, "k")]
        [InlineData(23971806, "K")]
        public void Test_Should_Validate_Rut_Dv_With_Int_NumberPart(int numberPart, string Dv)
        {
            var obj = new RutDvClass<int, string>() { NumberPart = numberPart, Dv = Dv };
            AssertValidObject(obj);
        }

        [Theory]
        [InlineData("123", "6")]
        [InlineData("23971806", "k")]
        [InlineData("23971806", "K")]
        public void Test_Should_Validate_Rut_Dv_With_String_NumberPart(string numberPart, string Dv)
        {
             var obj = new RutDvClass<string, string>() { NumberPart = numberPart, Dv = Dv };
            AssertValidObject(obj);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(11, "6")]
        [InlineData(23971806, "7")]
        [InlineData(23971805, "K")]
        public void Test_Should_Invalidate_Rut_Dv_With_Int_NumberPart(int numberPart, string Dv)
        {
             var obj = new RutDvClass<int, string>() { NumberPart = numberPart, Dv = Dv };
            AssertInvalidObject(obj);
        }

        [Theory]
        [InlineData("1", "1")]
        [InlineData("11", "6")]
        [InlineData("23971806", "7")]
        [InlineData("23971805", "K")]
        [InlineData("hola", "mundo")]
        public void Test_Should_Invalidate_Rut_Dv_With_String_NumberPart(string numberPart, string Dv)
        {
             var obj = new RutDvClass<string, string>() { NumberPart = numberPart, Dv = Dv };
            AssertInvalidObject(obj);
        }

        private void AssertValidObject(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            Assert.True(Validator.TryValidateObject(obj, validationContext, results, true));
        }

        private void AssertInvalidObject(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            Assert.False(Validator.TryValidateObject(obj, validationContext, results, true));
        }
    }
}