using RentBikeApi.Domain.Common;

namespace RentBikeApi.ErrorHandling;

public static class ApiErrors
{
    public const string InternalErrorMessage = "Erro interno do servidor";

    public static Error InvalidBody(string detail) =>
        new("request.invalid_body", detail);

    public static Error Internal(string correlationId) =>
        new("server.internal_error", $"Ocorreu um erro inesperado. Informe o código {correlationId} ao suporte");
}