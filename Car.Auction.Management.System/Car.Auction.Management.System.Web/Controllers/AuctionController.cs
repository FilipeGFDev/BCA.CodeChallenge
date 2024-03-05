namespace Car.Auction.Management.System.Web.Controllers;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Auction.Create;
using Car.Auction.Management.System.Application.UseCases.Auction.Update;
using Car.Auction.Management.System.Contracts.Web.Auction;
using Car.Auction.Management.System.Contracts.Web.Auction.Get;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuctionController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetAuctionByIdRequest(id));

        return Ok(response);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] AuctionRequest request)
    {
        var input = _mapper.Map<CreateAuctionInput>(request);

        var response = await _mediator.Send(input);

        return Created($"/auction/{response.Event.AuctionId}", null);
    }

    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] JsonPatchDocument<Auction> request)
    {
        await _mediator.Send(new UpdateAuctionInput(id, request));

        return NoContent();
    }
}