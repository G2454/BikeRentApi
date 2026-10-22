using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface ICourierService
    {
        Task<bool> CreateAsync(Courier courier, string? licenseImageBase64);
        Task<bool> UpdateLicenseImageAsync(string id, string licenseImageBase64);
    }
}
