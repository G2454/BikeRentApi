namespace RentBikeApi.Domain.Common
{
    public sealed class DomainException(Error error, Exception? innerException = null) : Exception(error.Description, innerException)
    {
        public Error Error { get; } = error;
    }
}
