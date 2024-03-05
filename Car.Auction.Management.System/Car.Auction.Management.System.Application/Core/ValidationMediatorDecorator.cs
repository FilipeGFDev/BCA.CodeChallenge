namespace Car.Auction.Management.System.Application.Core;

using FluentValidation;
using MediatR;

public class ValidationMediatorDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IRequestHandler<TRequest, TResponse> _nextHandler;

    public ValidationMediatorDecorator(
        IEnumerable<IValidator<TRequest>> validators,
        IRequestHandler<TRequest, TResponse> nextHandler)
    {
        _validators = validators;
        _nextHandler = nextHandler;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await _nextHandler.Handle(request, cancellationToken);
        }

        var validation = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(validation, cancellationToken)));

        var validationFailures = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .ToList();

        if (validationFailures.Count != 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await _nextHandler.Handle(request, cancellationToken);
    }
}
