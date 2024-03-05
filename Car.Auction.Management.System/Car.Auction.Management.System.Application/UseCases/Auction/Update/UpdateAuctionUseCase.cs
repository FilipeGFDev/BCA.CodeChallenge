namespace Car.Auction.Management.System.Application.UseCases.Auction.Update;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Application.Helpers;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.Exceptions;
using Microsoft.AspNetCore.JsonPatch;

public class UpdateAuctionUseCase : IUseCase<UpdateAuctionInput, AuctionUpdatedUseCaseEvent>
{
    private readonly IRepository<Auction> _auctionRepository;

    public UpdateAuctionUseCase(IRepository<Auction> auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    public async Task<UseCaseResult<AuctionUpdatedUseCaseEvent>> Handle(
        UpdateAuctionInput request,
        CancellationToken cancellationToken)
    {
        var auction = await _auctionRepository.Get(request.Id, cancellationToken);

        if (auction is null)
        {
            throw new NotFoundException(nameof(auction), request.Id);
        }

        UpdateAuction(auction, request.Request);

        await _auctionRepository.Update(auction, cancellationToken);

        return this.Result(new());
    }

    private static void UpdateAuction(Auction auction, JsonPatchDocument<Auction> requestRequest)
    {
        foreach (var operation in requestRequest.Operations)
        {
            if (AuctionHelper.IsIsActiveFieldOperation(operation.path))
            {
                auction.Close();
            }
        }
    }
}