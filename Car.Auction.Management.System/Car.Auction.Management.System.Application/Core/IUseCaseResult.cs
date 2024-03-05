namespace Car.Auction.Management.System.Application.Core;

public interface IUseCaseResult
{
    IUseCaseEvent Event { get; }
}