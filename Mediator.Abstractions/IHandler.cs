namespace Mediator.Abstractions
{
    public interface IHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken=default);
    }
}