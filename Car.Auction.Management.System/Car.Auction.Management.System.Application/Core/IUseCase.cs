namespace Car.Auction.Management.System.Application.Core;

using MediatR;

public interface IUseCase<in TRequest, TEvent> : IRequestHandler<TRequest, UseCaseResult<TEvent>>
    where TRequest : IRequest<UseCaseResult<TEvent>>
    where TEvent : IUseCaseEvent;