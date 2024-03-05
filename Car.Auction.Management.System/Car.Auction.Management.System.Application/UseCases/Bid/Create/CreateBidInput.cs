namespace Car.Auction.Management.System.Application.UseCases.Bid.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Bid;

public record CreateBidInput(BidProposal Proposal) : IUseCaseInput<BidCreatedUseCaseEvent>;