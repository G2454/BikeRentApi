using System.Text.Json;
using System.Text.Json.Serialization;
using RentBikeApi.Domain.Enums;

namespace RentBikeApi.Converters
{
    public sealed class LicenseTypeJsonConverter : JsonConverter<LicenseType>
    {
        public override LicenseType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetString()?.Trim().ToUpperInvariant() switch
            {
                "A" => LicenseType.A,
                "B" => LicenseType.B,
                "A+B" => LicenseType.AB,
                _ => throw new JsonException("Invalid license type")
            };

        public override void Write(Utf8JsonWriter writer, LicenseType value, JsonSerializerOptions options)
            => writer.WriteStringValue(value switch
            {
                LicenseType.A => "A",
                LicenseType.B => "B",
                LicenseType.AB => "A+B",
                _ => throw new JsonException("Invalid license type")
            });
    }


}
