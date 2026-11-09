using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface ICourierService
    {
        Task<Result> CreateAsync(Courier courier, string? licenseImageBase64);
        Task<Result> UpdateLicenseImageAsync(string id, string licenseImageBase64);
    }
}
