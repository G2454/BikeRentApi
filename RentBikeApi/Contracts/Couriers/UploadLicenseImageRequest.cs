using System.Text.Json.Serialization;

namespace RentBikeApi.Contracts.Couriers
{
    public record UploadLicenseImageRequest(
        [property: JsonPropertyName("imagem_cnh")] string LicenseImage
        );
}
