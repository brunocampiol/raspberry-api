using RaspberryPi.Domain.Extensions;
using System.Text.Json;

namespace RaspberryPi.Unit.Tests.Domain;

public class JsonExtensionsTests
{
    [Fact]
    public void ToJson_NullValue_ReturnsLiteralNull()
    {
        // Arrange
        object? value = null;

        // Act
        var json = value.ToJson();

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void ToJson_StringValue_SerializesAsJsonString()
    {
        // Arrange
        object value = "hello";

        // Act
        var json = value.ToJson();

        // Assert
        Assert.Equal("\"hello\"", json);
    }

    [Fact]
    public void ToJson_Dictionary_UsesCamelCaseKeys_WhenDictionaryKeyPolicyIsCamelCase()
    {
        // Arrange
        var dict = new Dictionary<string, int>
        {
            ["TotalCount"] = 3,
            ["HTTPStatus"] = 200
        };

        // Act
        var json = dict.ToJson();

        // Assert
        // Keys should be camelCased due to DictionaryKeyPolicy = CamelCase
        Assert.Contains("\"totalCount\":3", json);
        Assert.Contains("\"httpStatus\":200", json);
        Assert.DoesNotContain("\"TotalCount\":", json);
        Assert.DoesNotContain("\"HTTPStatus\":", json);
    }

    [Fact]
    public void ToJson_DateTime_Utc_IncludesZSuffix()
    {
        // Arrange
        var utc = new DateTime(2026, 1, 16, 12, 34, 56, DateTimeKind.Utc);

        // Act
        var json = utc.ToJson();

        // Assert
        // Example: "2026-01-16T12:34:56Z"
        Assert.StartsWith("\"2026-01-16T12:34:56", json);
        Assert.EndsWith("Z\"", json);
    }

    [Fact]
    public void FromJson_StringJson_ReturnsString()
    {
        // Arrange
        var json = "\"hello\"";

        // Act
        var value = json.FromJson<string>();

        // Assert
        Assert.Equal("hello", value);
    }

    [Fact]
    public void FromJson_NullLiteral_ReturnsNull_ForReferenceType()
    {
        // Arrange
        var json = "null";

        // Act
        var value = json.FromJson<string>();

        // Assert
        Assert.Null(value);
    }

    [Fact]
    public void FromJson_ObjectJson_IsCaseInsensitive_ForProperties()
    {
        // Arrange
        // System.Drawing.Point has public settable properties: X and Y
        var json = "{\"x\":10,\"Y\":20}";

        // Act
        var point = json.FromJson<System.Drawing.Point>();

        // Assert
        Assert.NotNull(point);
        Assert.Equal(10, point!.X);
        Assert.Equal(20, point.Y);
    }

    [Fact]
    public void FromJson_Dictionary_KeysAreNotAutomaticallyNormalized_AndAreReadAsProvided()
    {
        // Arrange
        // DictionaryKeyPolicy affects serialization, not deserialization normalization.
        var json = "{\"TotalCount\":3,\"httpStatus\":200}";

        // Act
        var dict = json.FromJson<Dictionary<string, int>>();

        // Assert
        Assert.NotNull(dict);

        // Expect exact keys as present in JSON payload
        Assert.True(dict!.ContainsKey("TotalCount"));
        Assert.True(dict.ContainsKey("httpStatus"));

        // Also ensure camelCased version wasn't auto-created for "TotalCount"
        Assert.False(dict.ContainsKey("totalCount"));
    }

    [Fact]
    public void FromJson_InvalidJson_ThrowsJsonException()
    {
        // Arrange
        var json = "{ this is not valid json }";

        // Act & Assert
        Assert.Throws<JsonException>(() => json.FromJson<Dictionary<string, int>>());
    }

    [Fact]
    public void FromJson_EmptyString_ThrowsJsonException()
    {
        // Arrange
        var json = "";

        // Act & Assert
        Assert.Throws<JsonException>(() => json.FromJson<int>());
    }

    [Fact]
    public void FromJson_WhitespaceOnly_ThrowsJsonException()
    {
        // Arrange
        var json = "   \n\t  ";

        // Act & Assert
        Assert.Throws<JsonException>(() => json.FromJson<int>());
    }
}