using System.Reflection;
using Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Mediator.Extensions
{
    public static class MediatorExtension
    {
        public static IServiceCollection AddMediator(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.AddSingleton<IMediator, Mediator>();

            var handlerType = typeof(IHandler<,>);

            foreach (var assembly in assemblies)
            {
                var handlers = assembly
                    .GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                    .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == handlerType);

                foreach (var handler in handlers)
                    services.AddTransient(handler.Interface, handler.Type);
            }

            return services;
        }
    }
}
