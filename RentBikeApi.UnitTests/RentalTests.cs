using RentBikeApi.Domain.Entities;
using RentBikeApi.Domain.Enums;
using RentBikeApi.Domain.ValueObjects;

namespace RentBikeApi.UnitTests;

public class RentalTests
{
    private static readonly DateOnly Start = new(2026, 1, 1);

    private static Rental CreateRental(int planDays)
    {
        var plan = RentalPlan.FromDays(planDays)!;
        var expectedEnd = Start.AddDays(plan.Days - 1);

        return new Rental
        {
            PlanDays = plan.Days,
            DailyRate = plan.DailyRate,
            StartDate = Start,
            EndDate = expectedEnd,
            ExpectedEndDate = expectedEnd
        };
    }

    // ----- Cost calculation -----

    [Theory]
    [InlineData(7, 6, 210)]    // returned on the expected end date
    [InlineData(7, 4, 162)]    // 5 days used + 20% penalty on 2 unused days
    [InlineData(7, 8, 310)]    // 2 days late: 210 + 2 × 50
    [InlineData(15, 9, 336)]   // 10 days used + 40% penalty on 5 unused days
    [InlineData(15, 14, 420)]  // 15-day plan on time
    [InlineData(30, 19, 440)]  // early on a 30-day plan: 20 days used, no penalty
    [InlineData(50, 50, 950)]  // 1 day late on a 50-day plan: 900 + 50
    public void CalculateTotalCost_ReturnsExpectedValue(int planDays, int daysAfterStart, int expectedTotal)
    {
        var rental = CreateRental(planDays);

        var total = rental.CalculateTotalCost(Start.AddDays(daysAfterStart));

        Assert.Equal(expectedTotal, total);
    }

    // ----- Registering the return -----

    [Fact]
    public void RegisterReturn_SetsDateAndTotal()
    {
        var rental = CreateRental(7);

        var registered = rental.RegisterReturn(Start.AddDays(4));

        Assert.True(registered);
        Assert.Equal(Start.AddDays(4), rental.ReturnDate);
        Assert.Equal(162m, rental.TotalCost);
    }

    [Fact]
    public void RegisterReturn_Fails_WhenAlreadyReturned()
    {
        var rental = CreateRental(7);
        rental.RegisterReturn(Start.AddDays(4));

        var registeredAgain = rental.RegisterReturn(Start.AddDays(6));

        Assert.False(registeredAgain);
        Assert.Equal(Start.AddDays(4), rental.ReturnDate);   // unchanged
        Assert.Equal(162m, rental.TotalCost);                // unchanged
    }

    [Fact]
    public void RegisterReturn_Fails_BeforeStartDate()
    {
        var rental = CreateRental(7);

        var registered = rental.RegisterReturn(Start.AddDays(-1));

        Assert.False(registered);
        Assert.Null(rental.ReturnDate);
        Assert.Null(rental.TotalCost);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(60)]
    public void FromDays_ReturnsNull_ForInvalidPlans(int days)
    {
        Assert.Null(RentalPlan.FromDays(days));
    }


    [Theory]
    [InlineData(LicenseType.A, true)]
    [InlineData(LicenseType.AB, true)]
    [InlineData(LicenseType.B, false)]
    public void CanRentMotorcycle_OnlyForCategoryA(LicenseType licenseType, bool expected)
    {
        var courier = new Courier { LicenseType = licenseType };

        Assert.Equal(expected, courier.CanRentMotorcycle());
    }
}