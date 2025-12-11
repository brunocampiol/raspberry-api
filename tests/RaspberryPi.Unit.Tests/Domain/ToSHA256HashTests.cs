using RaspberryPi.Domain.Extensions;

namespace RaspberryPi.Unit.Tests.Domain;

public class ToSHA256HashTests
{
    [Fact]
    public void ToSHA256Hash_WhenInputIsValid_ReturnsCorrectSHA256Hash()
    {
        // Arrange
        string input = "Hello World";
        string expectedHash = "a591a6d40bf420404a011733cfb7b190d62c65bf0bcda32b57b277d9ad9f146e";

        // Act
        string actualHash = input.ToSHA256Hash();

        // Assert
        Assert.Equal(64, actualHash.Length);
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ToSHA256Hash_WhenInputIsEmptyString_ReturnsCorrectSHA256Hash()
    {
        // Arrange
        string input = "";
        string expectedHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        // Act
        string actualHash = input.ToSHA256Hash();

        // Assert
        Assert.Equal(64, actualHash.Length);
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ToSHA256Hash_WhenInputIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => input.ToSHA256Hash());
    }

    [Fact]
    public void ToSHA256Hash_ReturnsConsistentHashForSameInput()
    {
        // Arrange
        string input = "Test Input";

        // Act
        string hash1 = input.ToSHA256Hash();
        string hash2 = input.ToSHA256Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ToSHA256Hash_ReturnsDifferentHashForDifferentInput()
    {
        // Arrange
        string input1 = "Hello";
        string input2 = "World";

        // Act
        string hash1 = input1.ToSHA256Hash();
        string hash2 = input2.ToSHA256Hash();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ToSHA256Hash_WhenInputHasSpecialCharacters_ReturnsCorrectHash()
    {
        // Arrange
        string input = "Test@123#Special$";
        string expectedHash = "b520c372754d40bced2182e4d9881e0dfe6a28ca67265486fecef75743b01f6e";

        // Act
        string actualHash = input.ToSHA256Hash();

        // Assert
        Assert.Equal(64, actualHash.Length);
        Assert.Equal(expectedHash, actualHash);
    }
}