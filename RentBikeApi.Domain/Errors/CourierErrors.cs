using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Errors;

public static class CourierErrors
{
    public static readonly Error IdRequired =
        new("courier.id_required", "O identificador do entregador é obrigatório");

    public static readonly Error NameRequired =
        new("courier.name_required", "O nome do entregador é obrigatório");

    public static readonly Error NameTooLong =
        new("courier.name_too_long", $"O nome do entregador deve ter no máximo {Courier.NameMaxLength} caracteres");

    public static readonly Error InvalidCnpj =
        new("courier.invalid_cnpj", "CNPJ inválido");

    public static readonly Error InvalidLicenseNumber =
        new("courier.invalid_license_number", "O número da CNH deve ter 11 dígitos");

    public static readonly Error InvalidLicenseType =
        new("courier.invalid_license_type", "Tipo de CNH inválido. Valores aceitos: A, B ou A+B");

    public static readonly Error InvalidBirthDate =
        new("courier.invalid_birth_date", "Data de nascimento inválida");

    public static readonly Error InvalidLicenseImage =
        new("courier.invalid_license_image", "A imagem da CNH deve ser um arquivo PNG ou BMP em base64");

    public static readonly Error IdInUse =
        new("courier.id_in_use", "Já existe um entregador com este identificador");

    public static readonly Error CnpjInUse =
        new("courier.cnpj_in_use", "Já existe um entregador com este CNPJ");

    public static readonly Error LicenseNumberInUse =
        new("courier.license_number_in_use", "Já existe um entregador com este número de CNH");

    public static readonly Error NotFound =
        new("courier.not_found", "Entregador não encontrado");
}