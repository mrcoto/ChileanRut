using System;
using MrCoto.ChileanRut;
using MrCoto.ChileanRut.Exceptions;
using Xunit;

namespace MrCoto.ChileanRutTest
{
    public class RutTest
    {
        [Theory]
        [InlineData("1", "9")]
        [InlineData("21198663", "8")]
        [InlineData("21313774", "3")]
        [InlineData("13239959", "k")]
        [InlineData("21770960", "1")]
        [InlineData("19963722", "3")]
        [InlineData("11765793", "0")]
        [InlineData("14713193", "3")]
        [InlineData("14449209", "9")]
        [InlineData("15605286", "8")]
        [InlineData("5942232", "4")]
        public void Test_Should_Give_Valid_Rut_With_Constructor(string number, string dv)
        {
            var rut = new Rut(number, dv);
            Assert.True(rut.IsValid());
        }

        [Theory]
        [InlineData("19253299-k")]
        [InlineData("19253299-K")]
        [InlineData("19253299k")]
        [InlineData("19253299K")]
        [InlineData("19.253.299-k")]
        [InlineData("19.253.299-K")]
        [InlineData("19.253.299k")]
        [InlineData("19.253.299K")]
        public void Test_Should_Parse_Rut_String_Value(string rutValue)
        {
            var rut = Rut.Parse(rutValue);
            Assert.True(rut.IsValid());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        [InlineData(123)]
        [InlineData(1234)]
        [InlineData(12345)]
        [InlineData(123456)]
        [InlineData(1234567)]
        [InlineData(12345678)]
        public void Test_Should_Give_Parse_Number_Part(int numberPart)
        {
            var rut = Rut.Parse(numberPart);
            Assert.True(rut.IsValid());
        }

        [Theory]
        [InlineData("1", "0")]
        [InlineData("21198663", "7")]
        [InlineData("21313774", "4")]
        [InlineData("13239959", "1")]
        [InlineData("21770960", "2")]
        [InlineData("19963722", "6")]
        [InlineData("11765793", "7")]
        [InlineData("14713193", "9")]
        [InlineData("14449209", "0")]
        [InlineData("15605286", "k")]
        [InlineData("5942232", "1")]
        public void Test_Should_Give_InValid_Rut_With_Constructor(string number, string dv)
        {
            var rut = new Rut(number, dv);
            Assert.False(rut.IsValid());
        }

        [Fact]
        public void Test_Should_Throw_InvalidRutFormatException_When_Rut_Is_Invalid()
        {
            Assert.Throws<InvalidRutFormatException>(() => Rut.Parse("123-k").Check());
        }

        [Fact]
        public void Test_Should_Throw_Exception_When_Rut_Is_Invalid()
        {
            Assert.Throws<Exception>(() => Rut.Parse("123-k").Check<Exception>());
        }

        [Fact]
        public void Test_Should_Throw_ArgumentException_When_Rut_Is_Invalid()
        {
            Assert.Throws<ArgumentException>(() => Rut.Parse("123-k").Check<ArgumentException>("my message"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("1.2k")]
        [InlineData("12.345.67ck")]
        [InlineData("1a.345.678k")]
        [InlineData("7.6543.21k")]
        [InlineData("123.456.78k")]
        [InlineData("12,345,678k")]
        [InlineData("0k")]
        [InlineData("123456789k")]
        [InlineData("12-A")]
        public void Test_Should_Throw_ArgumentException_On_Invalid_Rut(string rutValue)
        {
            Assert.Throws<ArgumentException>(() => Rut.Parse(rutValue));
        }

        [Theory]
        [InlineData("1", "9", RutFormat.FULL, "1-9")]
        [InlineData("15605286", "8", RutFormat.FULL, "15.605.286-8")]
        [InlineData("13239959", "k", RutFormat.FULL, "13.239.959-k")]
        [InlineData("1", "9", RutFormat.ONLY_DASH, "1-9")]
        [InlineData("15605286", "8", RutFormat.ONLY_DASH, "15605286-8")]
        [InlineData("13239959", "k", RutFormat.ONLY_DASH, "13239959-k")]
        [InlineData("1", "9", RutFormat.ESCAPED, "19")]
        [InlineData("15605286", "8", RutFormat.ESCAPED, "156052868")]
        [InlineData("13239959", "k", RutFormat.ESCAPED, "13239959k")]
        public void Test_Should_Format_Rut(string number, string dv, RutFormat format, string expected)
        {
            var rut = new Rut(number, dv);
            Assert.Equal(expected, rut.Format(format));
        }

        [Theory]
        [InlineData("1", "9")]
        [InlineData("12345678", "k")]
        [InlineData("12345678", "K")]
        public void Test_Should_Destructure_Rut(string number, string dv)
        {
            var rut = new Rut(number, dv);
            var (desNum, desDv) = rut;
            Assert.Equal(int.Parse(number), desNum);
            Assert.Equal(dv.ToLower(), desDv);
        }

        [Fact]
        public void Test_Should_Give_Random_Rut()
        {
            var rut = Rut.Random();
            Assert.True(rut.IsValid());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(1)]
        [InlineData(0)]
        public void Test_Should_Give_Random_Ruts(int length)
        {
            var rutList = Rut.Randoms(n: length);
            Assert.Equal(length, rutList.Count);
            rutList.ForEach(rut => Assert.True(rut.IsValid()));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(1)]
        [InlineData(0)]
        public void Test_Should_Give_Unique_Random_Ruts(int length)
        {
            var rutList = Rut.Uniques(n: length);
            Assert.Equal(length, rutList.Count);
            rutList.ForEach(rut => Assert.True(rut.IsValid()));
        }

        [Fact]
        public void Test_Should_Compare_Ruts()
        {
            var rut1 = new Rut("1234", "3");
            var rut2 = new Rut("1345", "5");
            Assert.True(rut1 < rut2);
            Assert.False(rut1 > rut2);
        }

        [Fact]
        public void Test_Should_Give_Same_Rut_Equality()
        {
            var rut1 = new Rut("1234", "3");
            var rut2 = new Rut("1345", "5");
            var rut3 = new Rut("1234", "3");
            Assert.True(rut1 == rut3);
            Assert.False(rut1 == rut2);
        }

        [Fact]
        public void Test_Should_Sort_Rut_List()
        {
            var rutList = Rut.Randoms(n: 42);
            rutList.Sort();
            for(int i = 0; i < rutList.Count - 2; i++)
            {
                Assert.True(rutList[i] < rutList[i + 1]);
            }
        }

    }
}
