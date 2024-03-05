namespace Car.Auction.Management.System.Application.Core;

using MediatR;

public static class UseCaseExtensions
{
    public static UseCaseResult<TEvent> Result<TInput, TEvent>(
        this IUseCase<TInput, TEvent> handler,
        TEvent evt)
        where TInput : IRequest<UseCaseResult<TEvent>>
        where TEvent : IUseCaseEvent
    {
        return new UseCaseResult<TEvent>(evt);
    }
}