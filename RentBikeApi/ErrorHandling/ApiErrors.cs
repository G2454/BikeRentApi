using RentBikeApi.Domain.Common;

namespace RentBikeApi.ErrorHandling;

public static class ApiErrors
{
    public const string InternalErrorMessage = "Erro interno do servidor";
    public const string UnauthenticatedMessage = "Não autenticado";
    public const string ForbiddenMessage = "Acesso negado";

    public static Error InvalidBody(string detail) =>
        new("request.invalid_body", detail);

    public static Error Internal(string correlationId) =>
        new("server.internal_error", $"Ocorreu um erro inesperado. Informe o código {correlationId} ao suporte");

    public static readonly Error Unauthenticated =
        new("auth.unauthenticated", "Envie um token válido no cabeçalho Authorization: Bearer <token>");

    public static readonly Error Forbidden =
        new("auth.forbidden", "Seu usuário não tem permissão para esta operação");
}