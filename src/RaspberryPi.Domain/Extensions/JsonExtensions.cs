using RaspberryPi.Domain.Helpers;
using System.Text.Json;

namespace RaspberryPi.Domain.Extensions;

public static class JsonExtensions
{
    public static string ToJson(this object? value)
        => JsonSerializer.Serialize(value, JsonDefaults.Options);

    public static T? FromJson<T>(this string json)
        => JsonSerializer.Deserialize<T>(json, JsonDefaults.Options);

    public static object? FromJson(this string json, Type type)
        => JsonSerializer.Deserialize(json, type, JsonDefaults.Options);

    //public static async Task<T?> FromJsonAsync<T>(this Stream utf8Json, CancellationToken ct = default)
    //=> await JsonSerializer.DeserializeAsync<T>(utf8Json, JsonDefaults.Options, ct);

    //public static async Task ToJsonAsync(this Stream target, object? value, CancellationToken ct = default)
    //    => await JsonSerializer.SerializeAsync(target, value, JsonDefaults.Options, ct);
}