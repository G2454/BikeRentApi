using RentBikeApi.Domain.Common;
using RentBikeApi.Domain.Entities;

namespace RentBikeApi.Domain.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials =
        new("auth.invalid_credentials", "Usuário ou senha inválidos");

    public static readonly Error WeakPassword =
        new("auth.weak_password", $"A senha deve ter pelo menos {User.PasswordMinLength} caracteres");
}