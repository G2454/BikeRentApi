using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.Validation;

namespace RentBikeApi.Domain.Services
{
    public class CourierService(ICourierRepository repository, IFileStorage fileStorage): ICourierService
    {
        public async Task<bool> CreateAsync(Courier courier, string? licenseImageBase64)
        {
            courier.Cnpj = Cnpj.Normalize(courier.Cnpj);
            courier.LicenseNumber = new string((courier.LicenseNumber ?? string.Empty).Where(char.IsAsciiDigit).ToArray());

            if (string.IsNullOrWhiteSpace(courier.Id) ||
                string.IsNullOrWhiteSpace(courier.Name))
                return false;

            if (!Cnpj.IsValid(courier.Cnpj))
                return false;

            if (courier.LicenseNumber.Length != 11)
                return false;

            if (!Enum.IsDefined(courier.LicenseType))
                return false;

            if (courier.BirthDate == default ||
                courier.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
                return false;

            if (await repository.GetByIdAsync(courier.Id) is not null)
                return false;

            if (await repository.CnpjExistsAsync(courier.Cnpj))
                return false;

            if (await repository.LicenseNumberExistsAsync(courier.LicenseNumber))
                return false;

            if (!string.IsNullOrWhiteSpace(licenseImageBase64))
            {
                if (!LicenseImageParser.TryParse(licenseImageBase64, out var content, out var extension))
                    return false;
                courier.LicenseImagePath = await fileStorage.SaveAsync(content, $"{Guid.NewGuid()}{extension}");
            }

            await repository.AddAsync(courier);
            await repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateLicenseImageAsync(string id, string licenseImageBase64)
        {
            var courier = await repository.GetByIdAsync(id);
            if (courier is null)
                return false;
            if (!LicenseImageParser.TryParse(licenseImageBase64, out var content, out var extension))
                return false;

            var previousPath = courier.LicenseImagePath;

            courier.LicenseImagePath = await fileStorage.SaveAsync(content, $"{Guid.NewGuid()}{extension}");
            await repository.SaveChangesAsync();

            if (previousPath is not null)
                await fileStorage.DeleteAsync(previousPath);

            return true;
        }

    }
}
