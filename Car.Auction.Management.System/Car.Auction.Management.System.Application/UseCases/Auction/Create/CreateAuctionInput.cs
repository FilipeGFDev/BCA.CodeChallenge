namespace Car.Auction.Management.System.Application.UseCases.Auction.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Auction;

public record CreateAuctionInput(AuctionProposal Proposal) : IUseCaseInput<AuctionCreatedUseCaseEvent>;