namespace RentBikeApi.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}