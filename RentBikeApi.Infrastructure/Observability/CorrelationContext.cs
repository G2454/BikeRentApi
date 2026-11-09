using System.Net.NetworkInformation;

namespace RentBikeApi.Infrastructure.Observability
{
    public static class CorrelationContext
    {
        private static readonly AsyncLocal<string?> CurrentId = new();

        public static string? Current
        {
            get => CurrentId.Value;
            set => CurrentId.Value = value;
        }
    }
}
