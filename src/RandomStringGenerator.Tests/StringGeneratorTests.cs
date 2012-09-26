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
        public void Generate_WithoutConfiguration_RetursNotEmptyString()
        {
            new StringGenerator().Generate()
                .Should()
                .NotBeNullOrEmpty();
        }

        [Test]
        public void Generate_WithLengthScpecified_RetursStringWithExactLength()
        {
            const int length = 5;
            new StringGenerator().WithLength(length).Generate()
                .Should()
                .HaveLength(length);
        }

        [Test]
        public void Generate_WithZeroOrNegativeLength_ThrowsArgumetException()
        {
            const int length = 0;

            //act
            Action invalidLengthSpecified = 
                () => new StringGenerator().WithLength(length).Generate();

            //assert
            invalidLengthSpecified.ShouldThrow<ArgumentOutOfRangeException>()
               .And.ActualValue.Should().Be(length);
        }

        [Test]
        public void Generate_WithLengthRangeScpecified_RetursStringWithLengthInThatRange()
        {
            const int min = 17;
            const int max = 21;
            new StringGenerator()
                .WithLengthRange(min, max).Generate().Length
                .Should()
                .BeGreaterOrEqualTo(min).And.BeLessOrEqualTo(max);
        }

        [Test]
        public void Generate_WithZeroOrNegativeRangeBorders_ThrowsArgumetException()
        {
            const int min = -1;
            const int max = 21;

            //act
            Action invalidLengthSpecified = 
                () => new StringGenerator().WithLengthRange(min, max).Generate();

            //assert
            invalidLengthSpecified.ShouldThrow<ArgumentException>()
                .WithMessage("Range elements should be greater then zero");
        }

        [Test]
        public void Generate_WithInvalidRange_ThrowsArgumetException()
        {
            const int min = 45;
            const int max = 42;

            //act
            Action invalidLengthSpecified =
                () => new StringGenerator().WithLengthRange(min, max).Generate();

            //assert
            invalidLengthSpecified.ShouldThrow<ArgumentException>()
                .WithMessage("Lower boundary should be less or equals than higher");
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
        public void Generate_WithNumericCountNegativeSpecified_ShouldThrowArgumentException()
        {
            const int numericCount = -3;

            Action invalidNumericCount =
                () => new StringGenerator().WithNumericCount(numericCount).Generate();

            invalidNumericCount.ShouldThrow<ArgumentOutOfRangeException>()
                .And.ActualValue.Should().Be(numericCount);
        }

        [Test]
        public void Generate_WithNumericCountGreaterThenlenght_ShouldThrowInvalidOperationException()
        {
            const int numericCount = 5;
            const int length = 4;

            Action invalidNumericCount =
                () => new StringGenerator().WithLength(length).WithNumericCount(numericCount).Generate();

            invalidNumericCount.ShouldThrow<InvalidOperationException>()
                .WithMessage("Numeric count should not be greater than length");
        }
    }
}
