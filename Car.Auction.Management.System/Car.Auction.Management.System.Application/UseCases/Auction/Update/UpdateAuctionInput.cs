namespace Car.Auction.Management.System.Application.UseCases.Auction.Update;

using Microsoft.AspNetCore.JsonPatch;
using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Auction;

public record UpdateAuctionInput(Guid Id, JsonPatchDocument<Auction> Request)
    : IUseCaseInput<AuctionUpdatedUseCaseEvent>;