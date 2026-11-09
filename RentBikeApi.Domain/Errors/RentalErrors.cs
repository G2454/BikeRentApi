using RentBikeApi.Domain.Common;

namespace RentBikeApi.Domain.Errors;

public static class RentalErrors
{
    public static readonly Error InvalidPlan =
        new("rental.invalid_plan", "Plano inválido. Planos disponíveis: 7, 15, 30, 45 ou 50 dias");

    public static readonly Error CourierNotEligible =
        new("rental.courier_not_eligible", "Somente entregadores habilitados na categoria A podem alugar motos");

    // Methods instead of fields: the message includes the value the client should have sent
    public static Error InvalidStartDate(DateOnly expected) =>
        new("rental.invalid_start_date", $"A data de início deve ser {expected:yyyy-MM-dd}, o primeiro dia após a criação da locação");

    public static Error InvalidEndDate(DateOnly expected) =>
        new("rental.invalid_end_date", $"A data de término deve ser {expected:yyyy-MM-dd} para o plano escolhido");

    public static Error InvalidExpectedEndDate(DateOnly expected) =>
        new("rental.invalid_expected_end_date", $"A data de previsão de término deve ser {expected:yyyy-MM-dd} para o plano escolhido");

    public static readonly Error MotorcycleUnavailable =
        new("rental.motorcycle_unavailable", "A moto já está alugada");

    public static readonly Error CourierUnavailable =
        new("rental.courier_unavailable", "O entregador já possui uma locação ativa");

    public static readonly Error NotFound =
        new("rental.not_found", "Locação não encontrada");

    public static readonly Error AlreadyReturned =
        new("rental.already_returned", "A devolução desta locação já foi informada");

    public static readonly Error ReturnBeforeStart =
        new("rental.return_before_start", "A data de devolução não pode ser anterior à data de início");
}