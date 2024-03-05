namespace Car.Auction.Management.System.Application.UseCases.Bid.Create;

using Car.Auction.Management.System.Application.Core;

public record BidCreatedUseCaseEvent(Guid BidId) : IUseCaseEvent;