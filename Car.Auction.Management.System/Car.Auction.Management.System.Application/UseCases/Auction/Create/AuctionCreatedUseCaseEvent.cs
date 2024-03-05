namespace Car.Auction.Management.System.Application.UseCases.Auction.Create;

using Car.Auction.Management.System.Application.Core;

public record AuctionCreatedUseCaseEvent(Guid AuctionId) : IUseCaseEvent;