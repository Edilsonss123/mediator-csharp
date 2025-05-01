namespace Mediator.Abstractions
{
    public interface IMediator
    {
        Task<List<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, 
            CancellationToken cancellationToken = default);
    }
}