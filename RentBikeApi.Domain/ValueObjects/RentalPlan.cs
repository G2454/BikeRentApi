
namespace RentBikeApi.Domain.ValueObjects
{
    public sealed record RentalPlan(int Days, decimal DailyRate, decimal EarlyReturnPenaltyRate)
    {
        private static readonly Dictionary<int, RentalPlan> Plans = new()
        {
            [7] = new(7, 30m, 0.20m),
            [15] = new(15, 28m, 0.40m),
            [30] = new(30, 22m, 0m),
            [45] = new(45, 20m, 0m),
            [50] = new(50, 18m, 0m)
        };

        public static RentalPlan? FromDays(int days) => Plans.GetValueOrDefault(days);
    }
}
