using System.Security.Cryptography;
using System.Text;

namespace RaspberryPi.Domain.Extensions;

public static class StringExtensions
{
    public static string ToSHA256Hash(this string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        char firstChar = input[0];
        if (char.IsUpper(firstChar) || !char.IsLetter(firstChar))
        {
            return input;
        }

        return string.Create(input.Length, (input, char.ToUpperInvariant(firstChar)),
            static (chars, state) =>
            {
                var (inputValue, newFirstChar) = state;
                inputValue.AsSpan().CopyTo(chars);
                chars[0] = newFirstChar;
            });
    }
}