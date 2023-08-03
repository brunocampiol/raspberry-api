using Fetchgoods.Text.Json.Extensions;
using System.Text.Json;

namespace RaspberryPi.API.Configuration
{
    public static class AppJsonSerializerOptions
    {
        public static JsonSerializerOptions Default
        {
            get
            {
                return new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                };
            }
        }

        public static void SetDefaultOptions()
        {
            JsonExtensionMethods.DefaultOptions = Default;
        }
    }
}