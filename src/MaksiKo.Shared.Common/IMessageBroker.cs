using MaksiKo.Shared.Common.Models;

namespace MaksiKo.Shared.Common;

public interface IMessageBroker
{
    Task PublishAsync<TMessage>(TMessage message) where TMessage : BaseMessage;
    Task SubscribeAsync<TMessage>(string subscriptionId, Func<TMessage, Task> handler) where TMessage : BaseMessage;
}