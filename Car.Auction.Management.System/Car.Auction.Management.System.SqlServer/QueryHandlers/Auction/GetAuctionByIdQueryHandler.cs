namespace Car.Auction.Management.System.SqlServer.QueryHandlers.Auction;

using AutoMapper;
using Car.Auction.Management.System.Contracts.Web.Auction.Get;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.Core;
using Car.Auction.Management.System.Models.Exceptions;
using MediatR;

public class GetAuctionByIdQueryHandler
    : IRequestHandler<GetAuctionByIdRequest, GetAuctionResponse>
{
    private readonly IRepository<Auction> _repository;
    private readonly IMapper _mapper;

    public GetAuctionByIdQueryHandler(
        IRepository<Auction> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetAuctionResponse> Handle(GetAuctionByIdRequest request, CancellationToken cancellationToken)
    {
        var auction = await _repository.Get(request.AuctionId, cancellationToken);

        if (auction is null)
        {
            throw new NotFoundException(nameof(Vehicle), request.AuctionId);
        }

        return _mapper.Map<GetAuctionResponse>(auction);
    }
}