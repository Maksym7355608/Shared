using MaksiKo.Shared.Application.Handlers;
using MaksiKo.Shared.Common.Models;
using MaksiKo.Shared.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Events;
using SerilogTimings;

namespace MaksiKo.Shared.Application.Infrastructure;

#nullable disable
public abstract class BaseBusBackgroundService<TEvent, THandler> : BackgroundService where TEvent :
    BaseMessage where THandler : BaseMessageHandler<TEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected virtual string SubscriptionId => this.GetType().Name;

    protected IUnitOfWork Work { get; }

    protected ILogger<THandler> Logger { get; }

    protected BaseBusBackgroundService(
        IUnitOfWork work,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<THandler> logger)
    {
        Work = work;
        _serviceScopeFactory = serviceScopeFactory;
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Work.MessageBroker.SubscribeAsync<TEvent>(SubscriptionId, HandleAsync);
    }

    protected virtual async Task HandleAsync(TEvent e)
    {
        using var op = Operation.At(LogEventLevel.Debug).Begin("event {0}", typeof(TEvent).Name);
        using IServiceScope scope = CreateScope();
        try
        {
            LogEvent(e);
            await scope.ServiceProvider.GetRequiredService<THandler>().HandleAsync(e);
            op.Complete();
        }
        catch (Exception ex)
        {
            LogEventError(e, ex);
            throw;
        }
    }

    protected IServiceScope CreateScope() => _serviceScopeFactory.CreateScope();

    protected virtual void LogEvent(TEvent msg)
    {
        if (Logger.IsEnabled(LogLevel.Trace))
        {
            Logger.LogTrace("Handled event {0}: {1}", typeof(TEvent).Name,
                JsonConvert.SerializeObject(msg));
        }
        else
        {
            if (!Logger.IsEnabled(LogLevel.Debug))
                return;
            Logger.LogDebug("Handled event {0}", typeof(TEvent).Name);
        }
    }

    protected virtual void LogEventError(TEvent msg, Exception error) => LogEventError(msg, error.ToString());

    protected virtual void LogEventError(TEvent msg, string error) => Logger.LogError(
        "Error event {0}: msg = {1}, error = {2}", typeof(TEvent).Name,
        JsonConvert.SerializeObject(msg), error);
}

public class BusBackgroundService<TEvent, THandler>(
    IUnitOfWork work,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<THandler> logger)
    : BaseBusBackgroundService<TEvent, THandler>(work, serviceScopeFactory, logger)
    where TEvent : BaseMessage
    where THandler : BaseMessageHandler<TEvent>;