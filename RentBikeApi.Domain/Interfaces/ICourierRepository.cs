using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Interfaces
{
    public interface ICourierRepository
    {
        Task<Courier?> GetByIdAsync(string id);
        Task<bool> CnpjExistsAsync(string cnpj);
        Task<bool> LicenseNumberExistsAsync(string licenseNumber);
        Task AddAsync(Courier courier);
        Task SaveChangesAsync();

    }
}
