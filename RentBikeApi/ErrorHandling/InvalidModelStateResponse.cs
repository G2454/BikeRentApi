using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Contracts;

namespace RentBikeApi.ErrorHandling;

public static class InvalidModelStateResponse
{
    public static IActionResult Create(ActionContext context)
    {
        var parameterNames = context.ActionDescriptor.Parameters.Select(p => p.Name).ToHashSet();

        var fields = context.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .Select(entry => entry.Key.StartsWith("$.") ? entry.Key[2..] : entry.Key)   
            .Where(key => key.Length > 0 && key != "$" && !parameterNames.Contains(key))
            .Distinct()
            .Order()
            .ToList();

        var detail = fields.Count > 0
            ? $"Campos ausentes ou com formato inválido: {string.Join(", ", fields)}"
            : "O corpo da requisição está ausente, incompleto ou não é um JSON válido";

        return new BadRequestObjectResult(ErrorResponse.InvalidData(ApiErrors.InvalidBody(detail)));
    }
}