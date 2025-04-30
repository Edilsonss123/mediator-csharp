using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<List<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var handlerInterfaceType = typeof(IHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handlers = _serviceProvider.GetServices(handlerInterfaceType).ToArray();
            if (handlers.Length == 0)
                throw new InvalidOperationException($"Handler not found for {requestType}");

            List<Task<TResponse>> tasks = new();

            foreach (var handler in handlers)
            {
                var method = handlerInterfaceType.GetMethod("HandleAsync");
                if (method is null)
                    throw new InvalidOperationException($"HandleAsync method not found for {handler.GetType()}");

                var result = method.Invoke(handler, new object[] { request, cancellationToken });
                if (result is not Task<TResponse> task)
                    throw new InvalidOperationException($"HandleAsync method must return Task<{typeof(TResponse)}>");

                tasks.Add(task);
            }

            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }
    }
}
