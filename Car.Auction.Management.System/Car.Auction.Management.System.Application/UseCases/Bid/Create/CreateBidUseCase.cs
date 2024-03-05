namespace Car.Auction.Management.System.Application.UseCases.Bid.Create;

using Car.Auction.Management.System.Application.Core;
using Car.Auction.Management.System.Models.Aggregates.Bid;
using Car.Auction.Management.System.Models.Core;

public class CreateBidUseCase : IUseCase<CreateBidInput, BidCreatedUseCaseEvent>
{
    private readonly IRepository<Bid> _repository;

    public CreateBidUseCase(IRepository<Bid> repository)
    {
        _repository = repository;
    }

    public async Task<UseCaseResult<BidCreatedUseCaseEvent>> Handle(
        CreateBidInput request,
        CancellationToken cancellationToken)
    {
        var bid = await _repository.Create(new(request.Proposal), cancellationToken);

        return this.Result(new(bid!.Id));
    }
}