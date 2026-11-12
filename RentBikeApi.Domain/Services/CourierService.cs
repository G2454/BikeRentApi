using Microsoft.Extensions.Logging;
using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Enums;
using RentBikeApi.Domain.Errors;
using RentBikeApi.Domain.Interfaces;
using RentBikeApi.Domain.Validation;

namespace RentBikeApi.Domain.Services
{
    public class CourierService(
        ICourierRepository repository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IFileStorage fileStorage,
        ILogger<CourierService> logger) : ICourierService
    {
        public async Task<Result> CreateAsync(Courier courier, string password, string? licenseImageBase64)
        {
            courier.Cnpj = Cnpj.Normalize(courier.Cnpj);
            courier.LicenseNumber = new string((courier.LicenseNumber ?? string.Empty).Where(char.IsAsciiDigit).ToArray());

            if (string.IsNullOrWhiteSpace(courier.Id))
                return CourierErrors.IdRequired;

            if (string.IsNullOrWhiteSpace(courier.Name))
                return CourierErrors.NameRequired;

            if (courier.Name.Length > Courier.NameMaxLength)
                return CourierErrors.NameTooLong;

            if (!Cnpj.IsValid(courier.Cnpj))
                return CourierErrors.InvalidCnpj;

            if (courier.LicenseNumber.Length != 11)
                return CourierErrors.InvalidLicenseNumber;

            if (!Enum.IsDefined(courier.LicenseType))
                return CourierErrors.InvalidLicenseType;

            if (courier.BirthDate == default ||
                courier.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
                return CourierErrors.InvalidBirthDate;

            if (string.IsNullOrEmpty(password) || password.Length < User.PasswordMinLength)
                return AuthErrors.WeakPassword;

            byte[]? imageContent = null;
            var imageExtension = string.Empty;
            if (!string.IsNullOrWhiteSpace(licenseImageBase64) &&
                !LicenseImageParser.TryParse(licenseImageBase64, out imageContent, out imageExtension))
                return CourierErrors.InvalidLicenseImage;

      
            if (await repository.GetByIdAsync(courier.Id) is not null ||
                await userRepository.UsernameExistsAsync(courier.Id))
                return CourierErrors.IdInUse;

            if (await repository.CnpjExistsAsync(courier.Cnpj))
                return CourierErrors.CnpjInUse;

            if (await repository.LicenseNumberExistsAsync(courier.LicenseNumber))
                return CourierErrors.LicenseNumberInUse;

            if (imageContent is not null)
                courier.LicenseImagePath = await fileStorage.SaveAsync(imageContent, $"{Guid.NewGuid()}{imageExtension}");

            try
            {
                await repository.AddAsync(courier);
                await userRepository.AddAsync(new User
                {
                    Id = Guid.NewGuid(),
                    Username = courier.Id,
                    PasswordHash = passwordHasher.Hash(password),
                    Role = UserRole.Courier,
                    CourierId = courier.Id
                });


                await repository.SaveChangesAsync();
            }
            catch
            {

                if (courier.LicenseImagePath is not null)
                    await fileStorage.DeleteAsync(courier.LicenseImagePath);
                throw;
            }

            logger.LogInformation("Courier {CourierId} registered with license type {LicenseType}", courier.Id, courier.LicenseType);
            return Result.Success();
        }

        public async Task<Result> UpdateLicenseImageAsync(string id, string licenseImageBase64)
        {
            var courier = await repository.GetByIdAsync(id);
            if (courier is null)
                return CourierErrors.NotFound;

            if (!LicenseImageParser.TryParse(licenseImageBase64, out var content, out var extension))
                return CourierErrors.InvalidLicenseImage;

            var previousPath = courier.LicenseImagePath;
            var newPath = await fileStorage.SaveAsync(content, $"{Guid.NewGuid()}{extension}");
            courier.LicenseImagePath = newPath;

            try
            {
                await repository.SaveChangesAsync();
            }
            catch
            {
                await fileStorage.DeleteAsync(newPath);
                throw;
            }

            if (previousPath is not null)
                await fileStorage.DeleteAsync(previousPath);

            logger.LogInformation("License image of courier {CourierId} updated ({Extension})", id, extension);
            return Result.Success();
        }
    }
}