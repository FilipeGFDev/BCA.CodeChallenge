namespace Car.Auction.Management.System.Web.Controllers;

using AutoMapper;
using Car.Auction.Management.System.Application.UseCases.Vehicle.Create;
using Car.Auction.Management.System.Contracts.Web.Vehicle;
using Car.Auction.Management.System.Contracts.Web.Vehicle.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public VehicleController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetVehicleByIdRequest(id));

        return Ok(response);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetVehiclesRequest.RequestFilter requestFilter)
    {
        var response = await _mediator.Send(new GetVehiclesRequest(requestFilter));

        return Ok(response);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> CreateAsync([FromBody] VehicleRequest request)
    {
        var input = _mapper.Map<CreateVehicleInput>(request);

        var response = await _mediator.Send(input);

        return Created($"/vehicle/{response.Event.VehicleId}", null);
    }
}