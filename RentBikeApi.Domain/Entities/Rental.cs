using RentBikeApi.Domain.ValueObjects;

namespace RentBikeApi.Domain.Entities;

public class Rental
{
    public const decimal LateFeePerDay = 50m;

    public string Id { get; set; } = string.Empty;
    public string CourierId { get; set; } = string.Empty;
    public string MotorcycleId { get; set; } = string.Empty;
    public int PlanDays { get; set; }
    public decimal DailyRate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly ExpectedEndDate { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public decimal? TotalCost { get; set; }

    public decimal CalculateTotalCost(DateOnly returnDate)
    {
        var plan = RentalPlan.FromDays(PlanDays)
            ?? throw new InvalidOperationException($"Rental {Id} has an invalid plan: {PlanDays} days.");

        if (returnDate < ExpectedEndDate)
        {
            var usedDays = returnDate.DayNumber - StartDate.DayNumber + 1;
            var unusedDays = plan.Days - usedDays;
            var penalty = unusedDays * DailyRate * plan.EarlyReturnPenaltyRate;

            return usedDays * DailyRate + penalty;
        }

        if (returnDate > ExpectedEndDate)
        {
            var extraDays = returnDate.DayNumber - ExpectedEndDate.DayNumber;

            return plan.Days * DailyRate + extraDays * LateFeePerDay;
        }

        return plan.Days * DailyRate;
    }

    public bool RegisterReturn(DateOnly returnDate)
    {
        if (ReturnDate is not null)
            return false;

        if (returnDate < StartDate)
            return false;

        ReturnDate = returnDate;
        TotalCost = CalculateTotalCost(returnDate);
        return true;
    }
}