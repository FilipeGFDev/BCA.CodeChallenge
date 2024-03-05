namespace Car.Auction.Management.System.Web.Controllers;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Bid.Create;
using Car.Auction.Management.System.Contracts.Web.Bid;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BidController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BidController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] BidRequest request)
    {
        var input = _mapper.Map<CreateBidInput>(request);

        var response = await _mediator.Send(input);

        return Created($"/bid/{response.Event.BidId}", null);
    }
}