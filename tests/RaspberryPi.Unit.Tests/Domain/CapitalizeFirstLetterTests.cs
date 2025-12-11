using RaspberryPi.Domain.Extensions;

namespace RaspberryPi.Unit.Tests.Domain;

public class CapitalizeFirstLetterTests
{
    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("world", "World")]
    [InlineData("test string", "Test string")]
    public void CapitalizeFirstLetter_WhenFirstLetterIsLowercase_CapitalizesFirstLetter(string input, string expected)
    {
        // Arrange
        // Input provided via InlineData

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("WORLD")]
    [InlineData("Test String")]
    public void CapitalizeFirstLetter_WhenFirstLetterIsAlreadyUppercase_ReturnsOriginalString(string input)
    {
        // Arrange
        // Input provided via InlineData

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CapitalizeFirstLetter_WhenInputIsNullOrWhiteSpace_ReturnsOriginalInput(string input)
    {
        // Arrange
        // Input provided via InlineData

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(input, result);
    }

    [Theory]
    [InlineData("123hello", "123hello")]
    [InlineData("@test", "@test")]
    [InlineData("_underscore", "_underscore")]
    public void CapitalizeFirstLetter_WhenFirstCharacterIsNotALetter_ReturnsOriginalString(string input, string expected)
    {
        // Arrange
        // Input provided via InlineData

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CapitalizeFirstLetter_WhenInputHasOnlyOneCharacter_CapitalizesCorrectly()
    {
        // Arrange
        string input = "a";
        string expected = "A";

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CapitalizeFirstLetter_WhenFirstCharacterIsLowercaseLetter_OnlyCapitalizesFirstCharacter()
    {
        // Arrange
        string input = "hello world";
        string expected = "Hello world";

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal('H', result[0]);
        Assert.Equal('e', result[1]); // Second character remains lowercase
    }

    [Fact]
    public void CapitalizeFirstLetter_PreservesAllOtherCharacters()
    {
        // Arrange
        string input = "hello WORLD 123!@#";
        string expected = "Hello WORLD 123!@#";

        // Act
        string result = input.CapitalizeFirstLetter();

        // Assert
        Assert.Equal(expected, result);
        // Verify all characters except first are preserved
        Assert.Equal(input.Substring(1), result.Substring(1));
    }
}