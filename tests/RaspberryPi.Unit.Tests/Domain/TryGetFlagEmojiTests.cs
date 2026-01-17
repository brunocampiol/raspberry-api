using RaspberryPi.Domain.Extensions;

namespace RaspberryPi.Unit.Tests.Domain;
public class TryGetFlagEmojiTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\r\n")]
    public void TryGetFlagEmoji_NullOrWhitespace_ReturnsEmptyString(string? countryCode)
    {
        // Arrange
        // (countryCode provided by InlineData)

        // Act
        var result = countryCode!.TryGetFlagEmoji(); // null-forgiving; method handles null/whitespace

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("B")]    // too short
    [InlineData("BRA")]  // too long
    [InlineData("BR ")]  // length 3
    [InlineData(" BR")]  // length 3
    public void TryGetFlagEmoji_LengthNotTwo_ReturnsEmptyString(string countryCode)
    {
        // Arrange

        // Act
        var result = countryCode.TryGetFlagEmoji();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("1R")]
    [InlineData("B1")]
    [InlineData("!R")]
    [InlineData("B!")]
    [InlineData("b-")]
    [InlineData("-r")]
    [InlineData("éR")]   // non-ASCII letter
    [InlineData("Bé")]   // non-ASCII letter
    public void TryGetFlagEmoji_NonLetters_ReturnsEmptyString(string countryCode)
    {
        // Arrange

        // Act
        var result = countryCode.TryGetFlagEmoji();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void TryGetFlagEmoji_LowercaseInput_ReturnsSameAsUppercase()
    {
        // Arrange
        var lower = "br";
        var upper = "BR";

        // Act
        var lowerResult = lower.TryGetFlagEmoji();
        var upperResult = upper.TryGetFlagEmoji();

        // Assert
        Assert.Equal(upperResult, lowerResult);
        Assert.NotEqual(string.Empty, lowerResult);
    }

    [Theory]
    [InlineData("BR", "🇧🇷")]
    [InlineData("US", "🇺🇸")]
    [InlineData("DE", "🇩🇪")]
    [InlineData("JP", "🇯🇵")]
    public void TryGetFlagEmoji_ValidCodes_ReturnsExpectedFlag(string countryCode, string expectedEmoji)
    {
        // Arrange

        // Act
        var result = countryCode.TryGetFlagEmoji();

        // Assert
        Assert.Equal(expectedEmoji, result);
    }

    [Fact]
    public void TryGetFlagEmoji_ValidCode_ReturnsTwoRegionalIndicatorSymbols()
    {
        // Arrange
        var countryCode = "BR";

        // Act
        var result = countryCode.TryGetFlagEmoji();

        // Assert
        // Each regional indicator is a surrogate pair (2 chars), so total should be 4 UTF-16 chars.
        Assert.Equal(4, result.Length);

        // Verify the produced code points match the regional indicators for 'B' and 'R'.
        var firstCodePoint = char.ConvertToUtf32(result, 0);
        var secondCodePoint = char.ConvertToUtf32(result, 2);

        const int RegionalIndicatorOffset = 0x1F1E6;
        Assert.Equal(RegionalIndicatorOffset + ('B' - 'A'), firstCodePoint);
        Assert.Equal(RegionalIndicatorOffset + ('R' - 'A'), secondCodePoint);
    }

    [Theory]
    [InlineData("AA")]
    [InlineData("ZZ")]
    public void TryGetFlagEmoji_BoundaryLetters_ReturnsNonEmptyString(string countryCode)
    {
        // Arrange

        // Act
        var result = countryCode.TryGetFlagEmoji();

        // Assert
        Assert.NotEqual(string.Empty, result);
        Assert.Equal(4, result.Length); // 2 surrogate pairs
    }

    [Fact]
    public void TryGetFlagEmoji_DoesNotThrow_ForArbitraryInput()
    {
        // Arrange
        var inputs = new[]
        {
            null, "", " ", "A", "AAA", "1!", "🇧🇷", "B\uD83C", "ÖZ"
        };

        // Act
        var ex = Record.Exception(() =>
        {
            foreach (var input in inputs)
            {
                _ = input!.TryGetFlagEmoji(); // null-forgiving; method guards null
            }
        });

        // Assert
        Assert.Null(ex);
    }
}

