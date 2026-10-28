namespace RentBikeApi.Mapping
{
    public static class DateMappings
    {
        public static DateOnly? ToDateOnly(this DateTime? value)
            => value is null ? null : DateOnly.FromDateTime(value.Value);
    }
}
