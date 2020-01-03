using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrCoto.ChileanRut.Validation;
using MrCoto.ChileanRut;
using Xunit;

namespace MrCoto.ChileanRutTest.Validation
{
    public class ValidRutAttributeTest
    {

        class RutClass<T>
        {
            [ValidRut]
            public T RutObject { get; set; }
        }

        class RutStringClass
        {
            [ValidRut(MinLength = 2)]
            public string RutString { get; set; }
        }

        class RutStringWithFormatClass
        {
            [ValidRut(MinLength = 2, RutFormat = RutFormat.ONLY_DASH)]
            public string RutString { get; set; }
        }

        [Theory]
        [InlineData("1-9")]
        [InlineData("123-6")]
        [InlineData("23971806-k")]
        [InlineData("23971806-K")]
        public void Test_Should_Validate_Rut_Object(string rut)
        {
            var obj = new RutClass<Rut>() { RutObject = Rut.Parse(rut) };
            AssertValidObject(obj);
        }

        [Theory]
        [InlineData("1-1")]
        [InlineData("123-k")]
        [InlineData("123-K")]
        [InlineData("23971806-9")]
        [InlineData("23971806-8")]
        public void Test_Should_Invalidate_Rut_Object(string rut)
        {
            var obj = new RutClass<Rut>() { RutObject = Rut.Parse(rut) };
            AssertInvalidObject(obj);
        }

        [Theory]
        [InlineData("11-6")]
        [InlineData("116")]
        [InlineData("123-6")]
        [InlineData("1236")]
        [InlineData("23971806-k")]
        [InlineData("23971806-K")]
        [InlineData("23971806k")]
        [InlineData("23971806K")]
        [InlineData("23.971.806-k")]
        [InlineData("23.971.806-K")]
        public void Test_Should_Validate_Rut_String(string rut)
        {
            var obj = new RutStringClass() { RutString = rut };
            AssertValidObject(obj);
        }

        [Theory]
        [InlineData("1-9")] // MinLength is 2
        [InlineData("123-5")]
        [InlineData("hola-mundo")]
        [InlineData("a")]
        [InlineData(" a ")]
        [InlineData("a ")]
        [InlineData(" a")]
        [InlineData("u")]
        [InlineData("-K")]
        [InlineData("23971807-k")]
        [InlineData("23971807-K")]
        public void Test_Should_Invalidate_Rut_String(string rut)
        {
            var obj = new RutStringClass() { RutString = rut };
            AssertInvalidObject(obj);
        }

        [Theory]
        [InlineData("1-9")] // MinLength is 2
        [InlineData("123-5")]
        [InlineData("116")]
        [InlineData("1236")]
        [InlineData("hola-mundo")]
        [InlineData("23971807-k")]
        [InlineData("23971807-K")]
        [InlineData("23971806k")]
        [InlineData("23971806K")]
        [InlineData("23.971.806-k")]
        [InlineData("23.971.806-K")]
        public void Test_Should_Invalidate_Rut_String_With_Format(string rut)
        {
            var obj = new RutStringWithFormatClass() { RutString = rut };
            AssertInvalidObject(obj);
        }

        [Fact]
        public void Test_Should_Invalidate_Rut_String_With_Other_Types()
        {   
            AssertInvalidObject(new RutClass<bool>());
            AssertInvalidObject(new RutClass<long>());
            AssertInvalidObject(new RutClass<int>());
            AssertInvalidObject(new RutClass<byte>());
            AssertInvalidObject(new RutClass<List<string>>());
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