// Library för JSON-serialisering: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview
using System.Text.Json;

namespace Sprakcoach
{
    public static class JsonLagring
    {
        public static List<OrdPost> Ladda(string filnamn)
        {
            if (!File.Exists(filnamn)) return new List<OrdPost>();
            string json = File.ReadAllText(filnamn);
            // Deserialisera JSON till en lista av OrdPoster
            List<OrdPost>? data = JsonSerializer.Deserialize<List<OrdPost>>(json, JsonOptions());
            return data ?? new List<OrdPost>();
        }

        public static void Spara(string filnamn, List<OrdPost> poster)
        {
            string json = JsonSerializer.Serialize(poster, JsonOptions());
            File.WriteAllText(filnamn, json);
        }


        // Serialisering av JSON med indentering och camelCase för egenskapsnamn
        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            // Gör JSON lättläst med indentering
            WriteIndented = true,
            // Använd camelCase för egenskapsnamn
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

}