using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    // Send - For command/query operations
    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default
    )
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(
            requestType,
            typeof(TResponse)
        );

        var handler = serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"No handler registered for {requestType.Name}");

        // Get pipeline behaviors
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(
            requestType,
            typeof(TResponse)
        );

        var behaviors = serviceProvider.GetServices(behaviorType).Cast<object>().ToList();

        RequestHandlerDelegate<TResponse> pipeline = () =>
        {
            var method = handlerType.GetMethod("Handle");
            return (Task<TResponse>)method.Invoke(handler, [request, cancellationToken]);
        };

        // Apply behaviors in reverse order
        for (var i = behaviors.Count - 1; i >= 0; i--)
        {
            var behavior = behaviors[i];
            var current = pipeline;

            pipeline = () =>
            {
                var method = behavior.GetType().GetMethod("Handle");
                return (Task<TResponse>)
                    method.Invoke(behavior, [request, current, cancellationToken]);
            };
        }

        return await pipeline();
    }

    // Publish - For notification operations
    public async Task Publish<TNotification>(
        TNotification notification,
        CancellationToken cancellationToken = default
    )
        where TNotification : INotification
    {
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = serviceProvider.GetServices(handlerType);

        var tasks = new List<Task>();
        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("Handle");
            tasks.Add((Task)method.Invoke(handler, [notification, cancellationToken]));
        }

        await Task.WhenAll(tasks);
    }
}