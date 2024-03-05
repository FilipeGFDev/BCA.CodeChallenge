namespace Car.Auction.Management.System.Application.UseCases.Auction.Update;

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Car.Auction.Management.System.Application.Helpers;
using Car.Auction.Management.System.Models.Aggregates.Auction;
using Car.Auction.Management.System.Models.ErrorCodes;

public class UpdateAuctionInputValidator : AbstractValidator<UpdateAuctionInput>
{
    public UpdateAuctionInputValidator()
    {
        RuleFor(x => x.Request)
            .Custom(ValidateAuctionUpdateDocument);
    }

    private static void ValidateAuctionUpdateDocument(
        JsonPatchDocument<Auction> patchDocument,
        ValidationContext<UpdateAuctionInput> context)
    {
        if (patchDocument.Operations.Any(x => x.OperationType != OperationType.Replace))
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Application.InvalidJsonPatchOperationType.ErrorMessage,
                    null)
                {
                    ErrorCode = ErrorCodes.Application.InvalidJsonPatchOperationType.Code,
                    CustomState = $"Resource: {nameof(Auction)}",
                });

            return;
        }

        ValidateIsActiveUpdateOperation(patchDocument, context);
    }

    private static void ValidateIsActiveUpdateOperation(
        JsonPatchDocument<Auction> patchDocument,
        ValidationContext<UpdateAuctionInput> context)
    {
        if (patchDocument.Operations.Any(x => !AuctionHelper.IsIsActiveFieldOperation(x.path)))
        {
            context.AddFailure(
                new ValidationFailure(
                    string.Empty,
                    ErrorCodes.Auction.InvalidAuctionFieldUpdate.ErrorMessage,
                    null)
                {
                    ErrorCode = ErrorCodes.Auction.InvalidAuctionFieldUpdate.Code,
                });

            return;
        }

        var isActiveFieldOperations = patchDocument.Operations
            .Where(x => AuctionHelper.IsIsActiveFieldOperation(x.path))
            .ToList();

        if (!isActiveFieldOperations.Any())
        {
            return;
        }

        foreach (var isActiveFieldOperation in isActiveFieldOperations)
        {
            if (bool.TryParse(isActiveFieldOperation.value.ToString(), out var isActiveUpdatedValue) && isActiveUpdatedValue)
            {
                context.AddFailure(
                    new ValidationFailure(
                        string.Empty,
                        ErrorCodes.Auction.InvalidAuctionIsActiveFieldUpdate.ErrorMessage,
                        null)
                    {
                        ErrorCode = ErrorCodes.Auction.InvalidAuctionIsActiveFieldUpdate.Code,
                        CustomState = $"Value: \"{isActiveFieldOperation.value}\"",
                    });
            }
        }
    }
}