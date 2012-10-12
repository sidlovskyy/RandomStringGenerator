using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace RandomStringGenerator.Tests
{
    [TestFixture]
    class StringGeneratorTests
    {
        [Test]
        public void Generate_WithLengthScpecified_RetursStringWithExactLength()
        {
            const int length = 5;
            new StringGenerator().WithLength(length).Generate()
                .Should()
                .HaveLength(length);
        }

        [Test]
        public void Generate_WithNegativeLength_ThrowsArgumetException()
        {
            const int length = -1;

            //act
            Action invalidLengthSpecified =
                () => new StringGenerator().WithLength(length).Generate();

            //assert
            invalidLengthSpecified.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Generate_WithLengthBetweenScpecified_RetursStringWithLengthInThatRange()
        {
            const int min = 17;
            const int max = 21;
            new StringGenerator()
                .WithLengthBetween(min, max).Generate().Length
                .Should()
                .BeGreaterOrEqualTo(min).And.BeLessOrEqualTo(max);
        }

        [Test]
        public void Generate_WithNumericCountSpecified_ShouldReturnStringWithRequiredNumericCharsCount()
        {
            const int numericCount = 3;

            string generated = new StringGenerator().WithNumericCount(numericCount).Generate();

            var numbersCount = generated.Count(char.IsDigit);
            numbersCount.Should().Be(numericCount);
        }

        [Test]
        public void Generate_WithSpecialCountSpecified_ShouldReturnStringWithRequiredNumericCharsCount()
        {
            const int specialCount = 7;

            string generated = new StringGenerator().WithSpecialCount(specialCount).Generate();

            var resultSpecialCount = generated.Count(IsSpecialChar);
            resultSpecialCount.Should().Be(specialCount);
        }

        [Test]
        public void Generate_WithUppercaseSpecified_ShouldReturnStringWithRequiredNumericCharsCount()
        {
            const int uppercaseCount = 8;

            string generated = new StringGenerator().WithUppercaseCount(uppercaseCount).Generate();

            var resultUppercaseCount = generated.Count(char.IsUpper);
            resultUppercaseCount.Should().Be(uppercaseCount);
        }

        [Test]
        public void Generate_WithLowercaseSpecified_ShouldReturnStringWithRequiredNumericCharsCount()
        {
            const int lowercaseCount = 23;

            string generated = new StringGenerator().WithLowercaseCount(lowercaseCount).Generate();

            var resultLowercaseCount = generated.Count(char.IsLower);
            resultLowercaseCount.Should().Be(lowercaseCount);
        }

        [Test]
        public void Generate_MinCharsCountSpecfiedAreBiggerThenMaxLength_ShouldThrowInvalidOperationException()
        {
            //act
            Action invalidConfiguration =
                () => new StringGenerator()
                    .WithLowercaseCountBetween(12, 13) //12
                    .WithSpecialCount(3) //12 + 3 = 15
                    .WithNumericCount(5) //15 + 5 = 20
                    .WithUppercaseCountBetween(2, 7) //20 + 2 = 22
                    .WithLengthBetween(15, 21)
                    .Generate();

            //assert
            invalidConfiguration.ShouldThrow<InvalidOperationException>()
                .WithMessage("Configuration is invalid. Expected string length is to short to satisfy this criteria");
        }

        [Test]
        public void Generate_ComplexConfiguration_ShouldWorkCorrectly()
        {
            //act
            string generated = new StringGenerator()
                    .WithLowercaseCountBetween(12, 13)
                    .WithSpecialCount(3)
                    .WithNumericCount(5)
                    .WithUppercaseCountBetween(2, 7)
                    .WithLengthBetween(15, 50)
                    .Generate();

            //assert
            var resultLowercaseCount = generated.Count(char.IsLower);
            resultLowercaseCount.Should().BeGreaterOrEqualTo(12).And.BeLessOrEqualTo(13);

            var resultSpeciaCount = generated.Count(IsSpecialChar);
            resultSpeciaCount.Should().Be(3);

            var resultNumericCount = generated.Count(char.IsNumber);
            resultNumericCount.Should().Be(5);

            var resultUppercaseCount = generated.Count(char.IsUpper);
            resultUppercaseCount.Should().BeGreaterOrEqualTo(2).And.BeLessOrEqualTo(7);

            generated.Length.Should().BeGreaterOrEqualTo(15).And.BeLessOrEqualTo(50);
        }

        [Test]
        public void Generate_CalledTwoTimesInAsequence_ShouldNotGenerateTheSameString()
        {
            //arange
            StringGenerator generator = new StringGenerator()
                .WithLowercaseCountBetween(12, 13)
                .WithSpecialCount(3)
                .WithNumericCount(5)
                .WithLengthBetween(15, 50);

            string generatedString1 = generator.Generate();
            string generatedString2 = generator.Generate();

            generatedString1.Should().NotBe(generatedString2);
        }

        [Test]
        public void Generate_WithOvveridenSpecialCharacters_ShoulduseOnlyOverridenSpecialChracters()
        {
            //arange
            string generated = new StringGenerator()
                .WithSpecialCharacters(new[] { '~' })
                .WithSpecialCount(7)
                .WithLength(7)
                .Generate();

            generated.Should().Be("~~~~~~~");
        }

        private bool IsSpecialChar(char ch)
        {
            return SpecialCharacters.Default.Contains(ch);
        }
    }
}
