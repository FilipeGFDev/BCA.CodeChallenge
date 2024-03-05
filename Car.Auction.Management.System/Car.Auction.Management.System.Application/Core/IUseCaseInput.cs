namespace Car.Auction.Management.System.Application.Core;

using MediatR;

public interface IUseCaseInput<TEvent> : IRequest<UseCaseResult<TEvent>>
    where TEvent : IUseCaseEvent;