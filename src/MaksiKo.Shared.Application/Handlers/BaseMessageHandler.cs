using AutoMapper;
using MaksiKo.Shared.Common.Models;
using MaksiKo.Shared.Mongo;
using Microsoft.Extensions.Logging;

namespace MaksiKo.Shared.Application.Handlers;

public abstract class BaseMessageHandler<TMessage> where TMessage : BaseMessage
{
    protected IMapper Mapper { get; }
    protected IUnitOfWork Work { get; }
    protected ILogger<BaseMessageHandler<TMessage>> Logger { get; }

    public BaseMessageHandler(IUnitOfWork work, ILogger<BaseMessageHandler<TMessage>> logger, IMapper mapper)
    {
        Work = work;
        Mapper = mapper;
        Logger = logger;
    }

    public abstract Task HandleAsync(TMessage msg);
}