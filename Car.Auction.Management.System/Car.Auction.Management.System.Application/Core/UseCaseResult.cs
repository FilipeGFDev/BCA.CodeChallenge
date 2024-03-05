namespace Car.Auction.Management.System.Application.Core;

public class UseCaseResult<TEvent> : IUseCaseResult
    where TEvent : IUseCaseEvent
{
    public UseCaseResult(TEvent evt)
    {
        this.Event = evt;
    }

    public TEvent Event { get; }

    IUseCaseEvent IUseCaseResult.Event => this.Event;
}